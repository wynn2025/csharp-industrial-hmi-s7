using GaugeDemo300.Models;

namespace GaugeDemo300.Services;

/// <summary>
/// 过站防错状态机（核心业务逻辑）
/// 流程：待扫码 → 扫码 → MES 校验中 → 校验通过（允许检测）/ 校验拦截（禁止启动+声光报警）
///       → 检测中 → 检测完成（判定+存库+过站同步）→ 复位
/// 防错规则：
///   1. 未扫码/未过站校验 → 不允许启动检测
///   2. MES 校验 code≠200 → 禁止启动，显示 msg，声光报警
///   3. 检测完成前 → 不允许换件（状态机锁定）
/// 判定：环控点（200 穿孔 Int 结果==OK 值）+ 位移点（60 REAL 公差判定/PLC 结果优先）；
/// 任一启用点位 NG（含无数据）→ 总结果 NG。
/// 型号切换：应用参数组到点位表，可选下发公差到 PLC（MAX_TOL/MIN_TOL）。
/// </summary>
public class StationFlowService
{
    /// <summary>过站状态</summary>
    public enum FlowState
    {
        /// <summary>就绪，等待扫码</summary>
        Idle,
        /// <summary>已扫码，MES 校验中</summary>
        MesChecking,
        /// <summary>校验通过，允许启动检测</summary>
        Passed,
        /// <summary>校验拦截，禁止启动</summary>
        Blocked,
        /// <summary>检测中</summary>
        Measuring,
        /// <summary>检测完成（结果显示后复位）</summary>
        Completed
    }

    /// <summary>状态变化事件（状态 + 提示消息）</summary>
    public event Action<FlowState, string> StateChanged;

    /// <summary>检测完成事件（携带完整记录）</summary>
    public event Action<MeasurementRecord> MeasurementCompleted;

    /// <summary>校验拦截事件（界面弹窗提示）</summary>
    public event Action<string> CheckBlocked;

    private readonly SystemConfig _cfg;
    private readonly ConfigService _configService;
    private readonly PlcService _plc;
    private readonly MesService _mes;
    private readonly DatabaseService _db;
    private readonly LogService _log;

    private string _currentProductCode = "";
    private MeasurementRecord _currentRecord;
    private bool _measuring;
    private DateTime _measureStartTime = DateTime.MinValue;  // 测量启动时刻（假完成判别：测量刚开始就来的完成=残留假上升沿）
    private int _completing;           // CompleteMeasure 并发防护（上升沿+状态迁移兜底可能双触发）
    private System.Threading.Timer _measureWatchdog;          // 测量超时看门狗：任何路径卡死都能自动归位

    /// <summary>标记测量开始：记录起始时刻 + 启动 120 秒超时看门狗</summary>
    private void MarkMeasureStart()
    {
        _measureStartTime = DateTime.Now;
        _plc.SetFastPoll(true);   // 测量中 PLC 轮询全速（完成信号/状态需要 200ms 响应）
        _measureWatchdog?.Dispose();
        _measureWatchdog = new System.Threading.Timer(_ => OnMeasureTimeout(), null, 120_000, System.Threading.Timeout.Infinite);
    }

    /// <summary>测量超时：写 code_ack=0 并复位（防止任何未预见路径造成 PLC/HMI 死锁）</summary>
    private void OnMeasureTimeout()
    {
        if (!_measuring) return;
        _log.Warn("检测超时（120 秒未收到完成信号）：自动写 code_ack=0 并复位，请重新扫码");
        _measuring = false;
        _currentRecord = null;
        _plc.SetFastPoll(false);
        _plc.WriteCodeAck(0);
        SetState(FlowState.Idle, "检测超时已自动复位（code_ack=0），请重新扫码");
    }

    public FlowState State { get; private set; } = FlowState.Idle;

    /// <summary>运行模式：自动（流程检测）/ 手动（气缸操作）</summary>
    public bool ManualMode { get; private set; }

    /// <summary>当前产品码</summary>
    public string CurrentProductCode => _currentProductCode;

    /// <summary>MES point1/point2 字段点位列表安全上限（300 点全量列表可能超长，按数量摘要）</summary>
    private const int MaxListPoints = 60;

    public StationFlowService(SystemConfig config, ConfigService configService, PlcService plc, MesService mes,
        DatabaseService db, LogService log)
    {
        _cfg = config;
        _configService = configService;
        _plc = plc;
        _mes = mes;
        _db = db;
        _log = log;
    }

    #region 扫码与校验

    /// <summary>扫码输入（扫码枪回车触发）</summary>
    public void OnScan(string productCode)
    {
        string code = productCode.Trim();
        if (code.Length == 0) return;

        // 防错：检测中不允许换产品
        if (State == FlowState.Measuring)
        {
            _log.Warn($"检测中不允许扫码换件，当前产品：{_currentProductCode}");
            SetState(FlowState.Measuring, "检测中，请等待当前产品检测完成");
            return;
        }

        _currentProductCode = code;
        _log.Info($"扫码：{code}");

        // 离线模式：跳过 MES 校验直接通过，写 code_ack=1 通知 PLC
        if (!_plc.PcOnlineState)
        {
            _log.Info("离线模式：跳过 MES 校验，直接通过");
            _plc.WriteCodeAckPulse(1);
            _measuring = true;
            MarkMeasureStart();
            _currentRecord = new MeasurementRecord
            {
                ProductCode = code,
                DeviceCode = _cfg.Mes.DeviceCode,
                Operator = _cfg.Mes.Operator,
                ProductName = _cfg.Mes.ProductName,
                ProductModel = _cfg.CurrentModel,
                Note = _cfg.Mes.Note,
                StationTime = DateTime.Now
            };
            _plc.ResetDoneBitMonitor();
            SetState(FlowState.Measuring, "离线模式：跳过MES，等待PLC检测...");
            return;
        }

        if (!_cfg.AutoStartCheck)
        {
            // 手动校验模式：扫码后直接放行，由操作员点击"过站校验"触发 MES 校验
            SetState(FlowState.Passed, $"扫码完成：{code}（请点击过站校验）");
            return;
        }

        SetState(FlowState.MesChecking, $"正在校验：{code} ...");
        _ = CheckAsync(code);
    }

    /// <summary>调用 MES 过站校验接口</summary>
    public async Task CheckAsync(string productCode)
    {
        var (code, msg) = await _mes.CheckProcessLeakAsync(productCode);
        bool passed = code == "200";

        _db.InsertStationCheck(new StationCheckRecord
        {
            ProductCode = productCode,
            CheckTime = DateTime.Now,
            Code = code,
            Msg = msg,
            Passed = passed
        });

        if (passed)
        {
            // MES 过站 OK → 立即写 code_ack = 1 通知 PLC 可以开始检测
            _plc.WriteCodeAckPulse(1);
            _measuring = true;
            MarkMeasureStart();
            _currentRecord = new MeasurementRecord
            {
                ProductCode = productCode,
                DeviceCode = _cfg.Mes.DeviceCode,
                Operator = _cfg.Mes.Operator,
                ProductName = _cfg.Mes.ProductName,
                ProductModel = _cfg.CurrentModel,
                Note = _cfg.Mes.Note,
                StationTime = DateTime.Now
            };
            _plc.ResetDoneBitMonitor();
            SetState(FlowState.Measuring, "code_ack=1，等待 PLC 自动启动检测...");
        }
        else
        {
            SetState(FlowState.Blocked, msg.Length > 0 ? msg : "校验未通过，禁止启动");
            TriggerAlarm();
            CheckBlocked?.Invoke(msg);
        }
    }

    /// <summary>手动触发过站校验（校验失败后重试 / 手动模式）</summary>
    public void ManualCheck()
    {
        if (_currentProductCode.Length == 0)
        {
            SetState(FlowState.Idle, "请先扫码");
            return;
        }
        SetState(FlowState.MesChecking, $"正在重新校验：{_currentProductCode} ...");
        _ = CheckAsync(_currentProductCode);
    }

    #endregion

    #region 模式切换

    /// <summary>切换手动/自动模式（手动模式允许气缸操作；自动模式走检测流程）</summary>
    public void SetMode(bool manual)
    {
        if (ManualMode == manual) return;
        ManualMode = manual;
        if (!string.IsNullOrWhiteSpace(_cfg.Plc.ManualModeAddress))
        {
            _plc.WriteBool(_cfg.Plc.ManualModeAddress, manual);
        }
        _log.Info(manual ? "切换到手动模式（气缸操作允许）" : "切换到自动模式");
        SetState(State, manual ? "手动模式" : "自动模式");
    }

    #endregion

    #region 检测流程

    /// <summary>
    /// 尝试启动检测。只有"校验通过"状态允许启动（过站防错核心）
    /// </summary>
    public bool TryStartMeasure()
    {
        if (ManualMode)
        {
            SetState(State, "手动模式下不允许启动自动检测，请切换自动模式");
            return false;
        }
        if (State != FlowState.Passed)
        {
            string reason = State switch
            {
                FlowState.Idle => "请先扫码",
                FlowState.MesChecking => "MES 校验中，请稍候",
                FlowState.Blocked => "过站校验未通过，禁止启动",
                FlowState.Measuring => "检测中",
                FlowState.Completed => "请复位后扫码下一件",
                _ => "当前状态不允许启动"
            };
            _log.Warn($"启动被拦截：{reason}");
            SetState(State, reason);
            return false;
        }

        _currentRecord = new MeasurementRecord
        {
            ProductCode = _currentProductCode,
            DeviceCode = _cfg.Mes.DeviceCode,
            Operator = _cfg.Mes.Operator,
            ProductName = _cfg.Mes.ProductName,
            ProductModel = _cfg.CurrentModel,
            Note = _cfg.Mes.Note,
            StationTime = DateTime.Now
        };
        _measuring = true;
        MarkMeasureStart();
        SetState(FlowState.Measuring, "检测中，请等待检测完成...");

        // MES 过站 OK → 写 code_ack = 1 通知 PLC 可以开始检测
        _plc.WriteCodeAckPulse(1);

        // 同时写启动信号（兼容两种 PLC 程序流程）
        if (!string.IsNullOrWhiteSpace(_cfg.Plc.StartAddress))
        {
            _plc.WriteBool(_cfg.Plc.StartAddress, true);
        }
        _plc.SimulateMeasureStarted();
        _log.Info($"检测开始（code_ack=1）：{_currentProductCode}");
        return true;
    }

    /// <summary>停止检测（写停止信号，状态复位）</summary>
    public void StopMeasure()
    {
        if (!string.IsNullOrWhiteSpace(_cfg.Plc.StopAddress))
        {
            _plc.WriteBool(_cfg.Plc.StopAddress, true);
        }
        // 启动信号撤销
        if (!string.IsNullOrWhiteSpace(_cfg.Plc.StartAddress))
        {
            _plc.WriteBool(_cfg.Plc.StartAddress, false);
        }
        bool wasMeasuring = _measuring;
        _measuring = false;
        _currentRecord = null;
        _log.Warn(wasMeasuring ? "检测被手动停止" : "停止");
        SetState(FlowState.Idle, wasMeasuring ? "检测已停止，请扫码" : "已停止");
    }

    /// <summary>
    /// 检测完成：采集当前全部点位 → 环控点按 OK 值、位移点按公差/PLC 结果判定 → 存库 → MES 过站同步
    /// 由 PLC 完成位上升沿自动触发，或手动按钮触发
    /// </summary>
    public void CompleteMeasure()
    {
        if (!_measuring || _currentRecord == null) return;
        // test_success 上升沿与 Equipment 运行→就绪兜底可能先后双触发，防止并发重复完成
        if (Interlocked.CompareExchange(ref _completing, 1, 0) != 0) return;
        try { CompleteMeasureCore(); }
        finally { Interlocked.Exchange(ref _completing, 0); }
    }

    private void CompleteMeasureCore()
    {
        // 假完成信号判别：残留 test_success 造成的假上升沿总在扫码后 1 秒内出现（实测 0.25s），
        // 真实检测至少十几秒（实测 19~21s）。测量开始不足 2 秒的完成信号一律拒绝，
        // code_ack 保持 1，PLC 仍可正常执行真实检测。
        // 注意不能用 Equipment 运行态做完成见证——该 PLC 的 Equipment 滞后 test_success 39 秒以上（20:03 实测）
        if ((DateTime.Now - _measureStartTime).TotalSeconds < 2.0)
        {
            _log.Warn("完成信号忽略：测量开始不足 2 秒（假上升沿），继续等待真实检测...");
            return;
        }

        var rec = _currentRecord;
        var points = _cfg.Points;
        var values = _plc.GetValuesSnapshot();
        var hasData = _plc.GetHasDataSnapshot();

        // 逐点位判定
        var okList = new List<string>();
        var ngList = new List<string>();
        var ngNoData = new List<string>();
        var details = new List<object>();
        var measureValues = new List<string>();
        int enabledCount = 0;

        for (int i = 0; i < points.Count; i++)
        {
            var p = points[i];
            int idx = p.Index - 1;
            bool has = idx >= 0 && idx < hasData.Length && hasData[idx];
            double v = has && idx < values.Length ? values[idx] : double.NaN;
            string result = p.FinalJudge();
            details.Add(new
            {
                name = p.Name,
                description = p.Description,
                zone = p.Zone,
                type = p.Type == PointType.Analog ? "位移" : "环控",
                value = p.Type == PointType.Analog
                    ? (has ? v.ToString("F3") : "-")
                    : (has ? v.ToString("F0") : "-"),
                result
            });
            if (!p.Enabled) continue;
            enabledCount++;
            if (result == "OK")
            {
                okList.Add(p.Name);
            }
            else
            {
                ngList.Add(p.Name);
                if (!has && p.PlcResult.Length == 0) ngNoData.Add(p.Name);
            }
            // point3-10 取位移点测量值
            if (p.Type == PointType.Analog && measureValues.Count < 8)
            {
                measureValues.Add(has ? v.ToString("F3") : "");
            }
        }
        // 位移点不足 8 个时用环控点结果补齐
        for (int i = 0; measureValues.Count < 8 && i < points.Count; i++)
        {
            if (points[i].Type == PointType.Result && points[i].Enabled)
            {
                int idx = points[i].Index - 1;
                measureValues.Add(idx < values.Length && !double.IsNaN(values[idx]) ? values[idx].ToString("F0") : "");
            }
        }

        rec.OverallResult = ngList.Count == 0 ? "OK" : "NG";
        rec.OkPoints = BuildPointList(okList, enabledCount);
        rec.NgPoints = BuildNgList(ngList, ngNoData);
        while (measureValues.Count < 8) measureValues.Add("");
        rec.MeasurePoints = measureValues.ToArray();
        rec.ExtraResults = new[]
        {
            rec.OverallResult,
            $"型号:{_cfg.CurrentModel} 环控{_cfg.Points.Count(x => x.Type == PointType.Result)}/位移{_cfg.Points.Count(x => x.Type == PointType.Analog)}",
            "", ""
        };
        var perfVals = new List<string>();
            var dispVals = new List<string>();
            foreach (var p in points)
            {
                if (p.Type == PointType.Result)
                    perfVals.Add(p.HasData && Math.Abs(p.Value - p.OkResultValue) < 0.5 ? "1" : "0");
                else if (p.Type == PointType.Analog)
                    dispVals.Add(p.HasData && !double.IsNaN(p.Value) ? p.Value.ToString("F4") : "");
            }
            rec.PerforationResults = string.Join(",", perfVals);
            rec.DisplacementValues = string.Join(",", dispVals);
            var limitVals = new List<string>();
            foreach (var p in points.Where(p => p.Type == PointType.Analog))
            {
                limitVals.Add(p.UpperLimit.ToString("F3"));
                limitVals.Add(p.LowerLimit.ToString("F3"));
            }
            rec.DisplacementLimits = string.Join(",", limitVals);
            rec.ChannelValuesJson = Newtonsoft.Json.JsonConvert.SerializeObject(details);

        _measuring = false;
        _currentRecord = null;
        _plc.SetFastPoll(false);  // 完成后回到空闲降载轮询
        _measureWatchdog?.Dispose();
        _measureWatchdog = null;
        string ngTip = ngNoData.Count > 0 ? $"（其中无数据 {ngNoData.Count} 点）" : "";

        // 先刷 UI（立即响应，不等 PLC 写/数据库）
        SetState(FlowState.Completed, $"检测完成：{rec.OverallResult}（OK {okList.Count} / NG {ngList.Count}{ngTip}）");
        MeasurementCompleted?.Invoke(rec);

        // PLC 复位 + 存库 + MES 同步全部放后台线程
        // （PLC 断线重连时 WriteCodeAck 会在锁上等待数十秒，绝不能阻塞 UI 线程）
        Task.Run(() =>
        {
            try
            {
                _plc.WriteCodeAck(0);
                if (!string.IsNullOrWhiteSpace(_cfg.Plc.StartAddress))
                {
                    _plc.WriteBool(_cfg.Plc.StartAddress, false);
                }
                _db.InsertMeasurement(rec);
                _ = _mes.SyncStationAsync(rec);
            }
            catch (Exception ex) { _log.Error("检测收尾（PLC复位/存库/MES同步）后台异常", ex); }
        });

        if (_cfg.AutoResetAfterDone)
        {
            Reset();
        }
    }

    /// <summary>OK 点位列表：点数多时摘要（MES point1 防超长）</summary>
    private static string BuildPointList(List<string> okList, int enabledCount)
    {
        if (okList.Count == 0) return "";
        if (okList.Count > MaxListPoints) return $"OK {okList.Count}/{enabledCount}（明细见上位机数据库）";
        return string.Join(",", okList);
    }

    /// <summary>NG 点位列表：优先保留明细（NG 是追溯关键），超长截断</summary>
    private static string BuildNgList(List<string> ngList, List<string> ngNoData)
    {
        if (ngList.Count == 0) return "";
        if (ngList.Count <= MaxListPoints) return string.Join(",", ngList);
        string head = string.Join(",", ngList.Take(MaxListPoints));
        string tail = $"...等{ngList.Count}点";
        if (ngNoData.Count > 0) tail += $"（含无数据{ngNoData.Count}点）";
        return head + tail;
    }

    /// <summary>复位：清除当前状态回到待扫码（拦截后可复位重新扫码），写复位脉冲</summary>
    public void Reset()
    {
        _measuring = false;
        _currentRecord = null;
        if (!string.IsNullOrWhiteSpace(_cfg.Plc.ResetAddress))
        {
            _plc.WriteBool(_cfg.Plc.ResetAddress, true);
            _ = Task.Run(async () =>
            {
                await Task.Delay(500);
                _plc.WriteBool(_cfg.Plc.ResetAddress, false);
            });
        }
        ClearAlarm();
        _plc.SetFastPoll(false);
        _plc.WriteCodeAck(0);
        SetState(FlowState.Idle, "就绪，请扫码");
    }

    #endregion

    #region 型号切换

    /// <summary>
    /// 切换产品型号：应用参数组 → 可选下发公差到 PLC → 记录型号
    /// </summary>
    public bool SwitchModel(string modelName)
    {
        var model = _configService.LoadModel(modelName);
        if (model == null)
        {
            _log.Warn($"型号参数文件不存在：{modelName}");
            return false;
        }
        _configService.ApplyModel(model);
        if (_cfg.Plc.WriteToleranceOnModelChange && !_cfg.Plc.UseSimulator && _plc.IsConnected)
        {
            var analogs = _cfg.Points.Where(p => p.Type == PointType.Analog).ToList();
            Task.Run(() => _plc.WriteTolerances(analogs));
        }
        return true;
    }

    /// <summary>把当前点位参数保存为指定型号（含公差下发）</summary>
    public void SaveCurrentAsModel(string modelName)
    {
        var model = _configService.BuildModelFromPoints(modelName);
        _configService.SaveModel(model);
        if (_cfg.Plc.WriteToleranceOnModelChange && !_cfg.Plc.UseSimulator && _plc.IsConnected)
        {
            var analogs = _cfg.Points.Where(p => p.Type == PointType.Analog).ToList();
            Task.Run(() => _plc.WriteTolerances(analogs));
        }
    }

    #endregion

    #region 报警

    /// <summary>过站拦截时声光报警（PLC 输出 + 软件提示）</summary>
    private void TriggerAlarm()
    {
        if (!_cfg.AlarmOnBlock) return;
        if (!string.IsNullOrWhiteSpace(_cfg.Plc.AlarmAddress))
        {
            _plc.WriteBool(_cfg.Plc.AlarmAddress, true);
            // 3 秒后自动解除报警输出
            _ = Task.Run(async () =>
            {
                await Task.Delay(3000);
                _plc.WriteBool(_cfg.Plc.AlarmAddress, false);
            });
        }
    }

    /// <summary>手动解除报警</summary>
    public void ClearAlarm()
    {
        if (!string.IsNullOrWhiteSpace(_cfg.Plc.AlarmAddress))
        {
            _plc.WriteBool(_cfg.Plc.AlarmAddress, false);
        }
    }

    #endregion

    private void SetState(FlowState state, string message)
    {
        State = state;
        _log.Info($"[过站] {state} | {message}");
        StateChanged?.Invoke(state, message);
    }
}
