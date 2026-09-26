using GaugeDemo300.Models;
using S7.Net;

namespace GaugeDemo300.Services;

/// <summary>
/// PLC 通讯服务（西门子 S7 以太网，S7netplus 库）—— 300 点位批量采集版
/// - 启动时按点位地址构建"读取计划"：同一 DB 内的连续/邻近地址合并为区域，按 ≤160 字节分块
///   （200 环控 Int = 400 字节 + 60 位移 REAL = 240 字节 + 60 结果 Int = 120 字节，每周期约 6 次报文）
/// - 环控点：Int 结果（POINT_RESULT[1..200]，值==OK值 判定）
/// - 位移点：REAL 测量值（Measured_value[1..60]）+ 可选 Int 判定结果（M_RESULT）
/// - 气缸：swich 开关位（写 1 动作 / 0 复位）+ 可选到位反馈（纳入批量读取）
/// - 断线自动重连 + 心跳监测 + 检测完成位上升沿 + 公差下发（MAX_TOL/MIN_TOL）
/// - 无硬件时模拟数据降级（调试用，模拟模式下启动检测 3 秒后自动触发完成）
/// </summary>
public class PlcService : IDisposable
{
    /// <summary>点位值更新事件（每次轮询周期触发一次）</summary>
    public event Action ValuesUpdated;

    /// <summary>连接状态变化事件</summary>
    public event Action<bool> ConnectionChanged;

    /// <summary>PLC 心跳丢失事件</summary>
    public event Action HeartbeatLost;

    /// <summary>PLC 检测完成位上升沿触发（test_success→读结果+复位code_ack）</summary>
    public event Action MeasureDoneTriggered;

    /// <summary>PLC 模式变化事件（true=自动，false=手动）</summary>
    public event Action<bool> ModeChanged;

    /// <summary>PLC 数据清除信号上升沿（delete_data→清除所有测量结果和值）</summary>
    public event Action DeleteDataTriggered;

    /// <summary>设备状态变化事件（PLC_TO_HMI.Equipment：1=IDLE 2=READY 3=RUNNING 4=ERROR）</summary>
    public event Action<int> EquipmentStateChanged;

    private readonly PlcConfig _cfg;
    private readonly List<PointConfig> _points;
    private readonly List<CylinderConfig> _cylinders;
    private readonly LogService _log;

    private Plc _plc;
    private System.Threading.Timer _pollTimer;
    private System.Threading.Timer _reconnectTimer;
    private bool _connected;
    private bool _lastDoneBit;
    private double _lastHeartbeatValue;
    private DateTime _lastHeartbeatTime = DateTime.Now;
    private readonly Random _rnd = new();
    private readonly object _lock = new();
    private DateTime _simStartMeasureTime = DateTime.MinValue;
    private short _pcWatchdog;            // PC→PLC 看门狗（每秒递增写 DB5.DBW0）
    private int _watchdogTick;            // 轮询计数（200ms×5=1 秒写一次看门狗）
    private int _equipmentState = -1;     // PLC_TO_HMI.Equipment 最新值
    private bool _doneMonitorArmed;       // 完成位监测是否已建立基线（连接/重连后首拍只记基线不判沿）

    /// <summary>当前点位最新值缓存（按点位 Index-1 顺序：环控点 Int 值，位移点 mm，NaN=无数据）</summary>
    public double[] CurrentValues { get; private set; }

    /// <summary>点位是否有有效数据缓存（按点位 Index-1 顺序）</summary>
    public bool[] HasData { get; private set; }

    /// <summary>PLC 报警位快照（ALARM.P_ALARM[1..N]，false=无报警）</summary>
    public bool[] AlarmBits { get; private set; }

    public bool IsConnected => _connected;
    public bool IsSimulator => _cfg.UseSimulator;

    /// <summary>设备状态（1=待机 2=就绪 3=运行 4=错误，-1=未知）</summary>
    public int EquipmentState => _equipmentState;

    /// <summary>上位机在线状态（on_line/off_line 握手，启动连接后自动上线）</summary>
    public bool PcOnlineState { get; private set; }

    /// <summary>PLC 当前模式：true=自动（DBX4.0=1），false=手动（DBX4.1=1）</summary>
    public bool IsPlcAutoMode { get; private set; } = true;

    /// <summary>设备状态中文描述</summary>
    public string EquipmentStateText => _equipmentState switch
    {
        1 => "待机", 2 => "就绪", 3 => "运行", 4 => "错误", _ => "--"
    };

    // ===== 读取计划（批量区域读取） =====

    /// <summary>一次 ReadBytes 读取块</summary>
    private sealed class ReadChunk
    {
        public DataType Area;
        public int Db;
        public int Start;
        public int Length;
        public byte[] Buffer = Array.Empty<byte>();
        public List<S7AddressTool.S7PointAddr> Points = new();
        public int FailCount;
    }

    private List<ReadChunk> _chunks = new();

    public PlcService(SystemConfig config, LogService log)
    {
        _cfg = config.Plc;
        _points = config.Points;
        _cylinders = config.Cylinders;
        _log = log;
        CurrentValues = new double[_points.Count];
        HasData = new bool[_points.Count];
        AlarmBits = new bool[Math.Max(_cfg.PlcAlarmCount, 0)];
        for (int i = 0; i < CurrentValues.Length; i++) CurrentValues[i] = double.NaN;
    }

    /// <summary>启动：构建读取计划 → 连接 PLC → 轮询</summary>
    public void Start()
    {
        BuildReadPlan();

        if (_cfg.UseSimulator)
        {
            _log.Info($"PLC 模拟模式：无硬件，模拟 {_points.Count} 个点位 / {_cylinders.Count} 个气缸数据");
            SetConnected(true);
            _pollTimer = new System.Threading.Timer(_ => SimulateTick(), null, 0, 500);
            return;
        }

        Connect();   // Connect 内部 5 秒超时；连接挂起时由重连 Timer 兜底
        _pollTimer = new System.Threading.Timer(_ => PollTick(), null, 0, _cfg.PollIntervalMs);
        _reconnectTimer = new System.Threading.Timer(_ => TryReconnect(), null, 0, _cfg.ReconnectIntervalMs);
    }

    #region 读取计划构建

    /// <summary>
    /// 把全部启用点位/气缸反馈地址解析、按 (区域, DB) 分组、合并邻近跨度、切分为 ≤160 字节的读取块
    /// </summary>
    public void BuildReadPlan()
    {
        var newChunks = new List<ReadChunk>();
        var addrs = new List<S7AddressTool.S7PointAddr>();
        int bad = 0;

        foreach (var p in _points)
        {
            if (!p.Enabled || string.IsNullOrWhiteSpace(p.PlcAddress)) continue;
            try
            {
                addrs.Add(S7AddressTool.ParseAddress(p));
            }
            catch
            {
                bad++;
                _log.Warn($"点位 {p.Name} 地址无法解析：{p.PlcAddress}（运行时按无数据处理）");
            }
            // 位移点的 PLC 判定结果地址（M_RESULT Int）
            if (p.Type == PointType.Analog && !string.IsNullOrWhiteSpace(p.ResultAddress))
            {
                try
                {
                    var ra = S7AddressTool.ParseRaw(p.ResultAddress, PointType.Result);
                    ra.Point = p;
                    ra.IsResultAddress = true;
                    addrs.Add(ra);
                }
                catch
                {
                    _log.Warn($"点位 {p.Name} 结果地址无法解析：{p.ResultAddress}（忽略 PLC 判定）");
                }
            }
        }

        // 气缸：开关状态回读 + 到位反馈（位地址）
        foreach (var c in _cylinders)
        {
            if (!c.Enabled) continue;
            if (!string.IsNullOrWhiteSpace(c.SwitchAddress))
            {
                try { addrs.Add(S7AddressTool.ParseSwitchAddress(c, c.SwitchAddress)); }
                catch { bad++; _log.Warn($"气缸 {c.Name} 开关地址无法解析：{c.SwitchAddress}"); }
            }
            if (!string.IsNullOrWhiteSpace(c.ForwardFeedbackAddress))
            {
                try { addrs.Add(S7AddressTool.ParseFeedbackAddress(c, c.ForwardFeedbackAddress, true)); }
                catch { bad++; _log.Warn($"气缸 {c.Name} 前进反馈地址无法解析：{c.ForwardFeedbackAddress}"); }
            }
            if (!string.IsNullOrWhiteSpace(c.BackwardFeedbackAddress))
            {
                try { addrs.Add(S7AddressTool.ParseFeedbackAddress(c, c.BackwardFeedbackAddress, false)); }
                catch { bad++; _log.Warn($"气缸 {c.Name} 后退反馈地址无法解析：{c.BackwardFeedbackAddress}"); }
            }
        }

        // PLC 报警位（ALARM.P_ALARM[1..N]，Bool 连续；块须为非优化访问）
        if (_cfg.PlcAlarmCount > 0 && !string.IsNullOrWhiteSpace(_cfg.PlcAlarmStartAddress))
        {
            for (int i = 0; i < _cfg.PlcAlarmCount; i++)
            {
                try
                {
                    addrs.Add(S7AddressTool.ParseAlarmAddress(i + 1, _cfg.PlcAlarmStartAddress, i));
                }
                catch
                {
                    bad++;
                    if (i == 0) _log.Warn($"报警位起始地址无法解析：{_cfg.PlcAlarmStartAddress}（报警位读取停用）");
                    break;
                }
            }
        }

        if (bad > 0)
            _log.Warn($"读取计划：{bad} 个地址解析失败");

        // 按 (区域, DB) 分组
        foreach (var group in addrs.GroupBy(a => (a.Area, a.Db)))
        {
            var spans = group
                .Select(a => (Addr: a, From: a.ByteOffset, To: a.ByteOffset + a.ByteLength - 1))
                .OrderBy(s => s.From)
                .ToList();

            int i = 0;
            while (i < spans.Count)
            {
                int from = spans[i].From;
                int to = spans[i].To;
                var members = new List<S7AddressTool.S7PointAddr> { spans[i].Addr };
                int j = i + 1;
                // 合并邻近跨度，直到区域超过 160 字节
                while (j < spans.Count && spans[j].From - to <= 16 && Math.Max(to, spans[j].To) - from + 1 <= 160)
                {
                    to = Math.Max(to, spans[j].To);
                    members.Add(spans[j].Addr);
                    j++;
                }

                var chunk = new ReadChunk
                {
                    Area = group.Key.Area,
                    Db = group.Key.Db,
                    Start = from,
                    Length = to - from + 1,
                    Buffer = new byte[to - from + 1]
                };
                foreach (var a in members)
                {
                    a.ByteOffset -= from;   // 转为块内偏移
                    chunk.Points.Add(a);
                }
                newChunks.Add(chunk);
                i = j;
            }
        }

        _chunks = newChunks;   // 原子引用替换（PollTick 正在遍历时不会崩溃）
        _log.Info($"读取计划构建完成：{_chunks.Count} 个读取块，覆盖 {addrs.Count} 个地址");
        // 诊断：dump DB5 各块的气缸成员
        foreach (var ch in _chunks.Where(c => c.Db == 5))
        {
            foreach (var pt in ch.Points.Where(p => p.Cylinder != null))
            {
                _log.Info($"  DB5块(DBB{ch.Start}x{ch.Length}) 缸{pt.Cylinder.Index} DBX{pt.ByteOffset + ch.Start}.{pt.BitOffset} switch={pt.IsSwitch} fwdFb={pt.IsForwardFeedback} fwdArrived={pt.Cylinder.ForwardArrived}");
            }
        }
    }

    #endregion

    #region 连接与重连

    private void Connect()
    {
        // S7netplus Open() 无超时参数，TCP 连接阻塞可达 20s+（连接槽被占时）——任务+超时包裹
        Plc opened = null;
        Exception openErr = null;
        var openTask = Task.Run(() =>
        {
            try
            {
                lock (_lock)
                {
                    _plc?.Close();
                    _plc = new Plc(S7AddressTool.ParseCpuType(_cfg.CpuType), _cfg.IpAddress, (short)_cfg.Rack, (short)_cfg.Slot)
                    {
                        ReadTimeout = 2000,
                        WriteTimeout = 2000
                    };
                    _plc.Open();
                    opened = _plc;
                }
            }
            catch (Exception ex)
            {
                openErr = ex;
            }
        });
        if (!openTask.Wait(5000))
        {
            lock (_lock) _plc = null;   // 丢弃挂起的连接对象（PLC 侧由 keep-alive 超时回收）
            SetConnected(false);
            _log.Warn($"PLC 连接超时（5 秒）：{_cfg.IpAddress}（PLC 连接槽可能被占用，等待释放后自动重连）");
            return;
        }
        if (openErr != null)
        {
            SetConnected(false);
            _log.Warn($"PLC 连接异常：{openErr.Message}");
            return;
        }
        SetConnected(opened != null && opened.IsConnected);
        if (_connected)
        {
            _lastHeartbeatTime = DateTime.Now;
            _log.Info($"PLC 连接成功：{_cfg.IpAddress}（{_cfg.CpuType} 机架{_cfg.Rack} 槽位{_cfg.Slot}）");
            // 连接成功自动上线（on_line=1 / off_line=0）
            SetOnline(true);
            // 清除上次运行残留的 code_ack=1：PLC 以 code_ack 的 0→1 上升沿启动检测周期，
            // 残留 1 会让下一轮扫码的 code_ack=1 变成无效电平（无沿），PLC 不启动、test_success 保持残留值 → 死锁
            try { _plc?.Write(_cfg.CodeAckAddress, (short)0); _log.Info("启动清理：code_ack = 0"); }
            catch { }
            if (!string.IsNullOrWhiteSpace(_cfg.HeartbeatAddress))
            {
                try { _lastHeartbeatValue = ReadHeartbeatValue(); } catch { }
            }
        }
        else
        {
            _log.Warn($"PLC 连接失败：{_cfg.IpAddress}");
        }
    }

    private void TryReconnect()
    {
        if (_connected || _cfg.UseSimulator) return;
        _log.Warn("PLC 断线，尝试重连...");
        Connect();
    }

    private void SetConnected(bool value)
    {
        if (_connected == value) return;
        _connected = value;
        ConnectionChanged?.Invoke(value);
    }

    #endregion

    #region 轮询采集（批量区域读取）

    /// <summary>优先从本轮块缓冲取值（零额外 PLC 往返）。
    /// test_success / PLC watch_dog / Equipment / 模式位 / delete_data 全在主读取块覆盖范围内，
    /// 之前每 200ms 额外单点读 5 次以上，把 S7-1200 通信资源打满（通讯卡的主因）。
    /// 地址不在任何块内或对应块读取失败时回退单点读。</summary>
    private object ReadCached(string address)
    {
        try
        {
            string s = (address ?? "").Trim().ToUpperInvariant().Replace(" ", "");
            if (s.StartsWith("DB"))
            {
                int dot = s.IndexOf('.');
                int db = int.Parse(s.Substring(2, dot - 2));
                string rest = s.Substring(dot + 1);
                bool isBit = rest.StartsWith("DBX"), isWord = rest.StartsWith("DBW"), isDword = rest.StartsWith("DBD");
                string body = rest.Substring(3);
                int byteOff, bitOff = 0;
                if (isBit)
                {
                    var bp = body.Split('.');
                    byteOff = int.Parse(bp[0]);
                    if (bp.Length > 1) bitOff = int.Parse(bp[1]);
                }
                else byteOff = int.Parse(body);
                int need = isDword ? 4 : isWord ? 2 : 1;

                foreach (var ch in _chunks)
                {
                    if (ch.FailCount != 0 || ch.Db != db) continue;
                    if (byteOff < ch.Start || byteOff + need > ch.Start + ch.Length) continue;
                    int o = byteOff - ch.Start;
                    if (isBit) return (ch.Buffer[o] & (1 << bitOff)) != 0;
                    if (isWord) return (short)((ch.Buffer[o] << 8) | ch.Buffer[o + 1]);
                    if (isDword) return ToReal(ch.Buffer, o);
                    return ch.Buffer[o];
                }
            }
        }
        catch { }
        return _plc?.Read(address);
    }

    private volatile bool _fastPoll = true; // 测量中全速轮询；空闲分频降载（S7-1200 通信资源有限）
    private int _idleDiv;                   // 空闲分频计数

    /// <summary>测量期间全速轮询（扫码进入 Measuring 置 true，完成/复位/超时置 false）</summary>
    public void SetFastPoll(bool on) => _fastPoll = on;

    private void PollTick()
    {
        if (!_connected || _plc == null) return;
        if (!_fastPoll && ++_idleDiv < 3) return;   // 空闲时 3 拍执行 1 拍（200ms→600ms），通信量降 2/3
        _idleDiv = 0;

        var chunks = _chunks;   // 局部引用（BuildReadPlan 原子替换期间不会崩溃）
        foreach (var chunk in chunks)
        {
            try
            {
                byte[] data = _plc.ReadBytes(chunk.Area, chunk.Db, chunk.Start, chunk.Length);
                if (data.Length >= chunk.Length)
                {
                    Array.Copy(data, chunk.Buffer, chunk.Length);
                    chunk.FailCount = 0;
                }
            }
            catch (Exception ex)
            {
                chunk.FailCount++;
                // 读取失败降频记录日志，避免每 200ms 刷屏；连续失败视为通讯异常
                if (chunk.FailCount == 1 || chunk.FailCount % 25 == 0)
                    _log.Warn($"读取块 {chunk.Area} DB{chunk.Db}.DBB{chunk.Start}×{chunk.Length} 失败（第 {chunk.FailCount} 次）：{ex.Message}");
            }
        }

        // 从块缓冲提取点位值（块读取失败的点位保持无数据）
        lock (_lock)
        {
            foreach (var chunk in chunks)
            {
                bool ok = chunk.FailCount == 0;
                foreach (var a in chunk.Points)
                {
                    if (!ok)
                    {
                        ApplyAddr(a, null);
                        continue;
                    }
                    ApplyAddr(a, chunk.Buffer);
                }
            }
        }

        CheckHeartbeat();
        CheckDoneBit();
        CheckDeleteData();
        ServiceProtocol();     // PC 看门狗递增写 + 设备状态读取（每秒一次）
        ValuesUpdated?.Invoke();
    }

    /// <summary>协议信号维护：每秒写 PC 看门狗（DB5.DBW0 递增）+ 读设备状态（DB5.DBW8）</summary>
    private void ServiceProtocol()
    {
        _watchdogTick++;
        if (_watchdogTick < Math.Max(1000 / _cfg.PollIntervalMs, 1)) return;
        _watchdogTick = 0;
        try
        {
            _pcWatchdog++;
            _plc?.Write(_cfg.PcWatchdogAddress, _pcWatchdog);
        }
        catch { /* 看门狗写失败由读取块失败/重连机制处理 */ }
        // 读取 PLC 自动/手动模式位
        try
        {
            bool auto = ReadCached(_cfg.PlcAutoModeAddress) is bool b1 && b1;
            bool manual = ReadCached(_cfg.PlcManualModeAddress) is bool b2 && b2;
            if (auto != IsPlcAutoMode)
            {
                IsPlcAutoMode = auto;
                _log.Info($"PLC 模式切换：{(auto ? "自动" : "手动")}");
                ModeChanged?.Invoke(auto);
            }
        }
        catch { }

        if (!string.IsNullOrWhiteSpace(_cfg.EquipmentStateAddress))
        {
            try
            {
                object v = ReadCached(_cfg.EquipmentStateAddress);
                int state = v switch
                {
                    short s => s,
                    ushort u => u,
                    int i => i,
                    _ => -1
                };
                if (state != _equipmentState)
                {
                    bool wasRunning = _equipmentState == 3;
                    _equipmentState = state;
                    _log.Info($"设备状态：{state}（{_equipmentState switch { 1 => "待机", 2 => "就绪", 3 => "运行", 4 => "错误", _ => "未知" }}）");
                    if (wasRunning && state != 3)
                    {
                        // 运行→就绪 = 检测周期真实结束的兜底完成信号
                        //（PLC 侧 test_success 若保持 1 不归零，上升沿检测不到，由该状态迁移兜底触发）
                        _log.Info("设备离开运行状态：触发检测完成兜底检查");
                        MeasureDoneTriggered?.Invoke();
                    }
                    EquipmentStateChanged?.Invoke(state);
                }
            }
            catch { }
        }
    }

    /// <summary>从块缓冲提取一个地址的值写入点位/气缸/报警位（buffer=null 表示无数据）</summary>
    private void ApplyAddr(S7AddressTool.S7PointAddr a, byte[] buffer)
    {
        if (a.IsAlarmBit)
        {
            // PLC 报警位（ALARM.P_ALARM[i]）
            bool bit = buffer != null && (buffer[a.ByteOffset] & (1 << a.BitOffset)) != 0;
            int ai = a.AlarmIndex - 1;
            if (ai >= 0 && ai < AlarmBits.Length) AlarmBits[ai] = bit;
            return;
        }

        if (a.Cylinder != null)
        {
            // 气缸开关位 / 到位反馈位
            bool bit = buffer != null && (buffer[a.ByteOffset] & (1 << a.BitOffset)) != 0;
            if (a.IsSwitch) a.Cylinder.SwitchState = bit;
            else if (a.IsForwardFeedback) a.Cylinder.ForwardArrived = bit;
            else a.Cylinder.BackwardArrived = bit;
            return;
        }

        var p = a.Point;
        if (p == null) return;
        int idx = p.Index - 1;

        if (a.IsResultAddress)
        {
            // 位移点的 PLC 判定结果（Int，1=OK / 2=NG 由 PLC 编码决定，1 视为 OK）
            if (buffer == null)
            {
                p.PlcResult = "";
                return;
            }
            short v = ToInt16(buffer, a.ByteOffset);
            // PLC 判定结果：1=合格(OK)，2=NG（0 兼容为 NG）
            p.PlcResult = v == 1 ? "OK" : v == 2 || v == 0 ? "NG" : $"?{v}";
            return;
        }

        if (buffer == null)
        {
            p.HasData = false;
            p.Value = double.NaN;
            if (idx >= 0 && idx < HasData.Length) HasData[idx] = false;
            return;
        }

        switch (p.Type)
        {
            case PointType.Result:
                p.Value = ToInt16(buffer, a.ByteOffset);
                break;
            case PointType.Digital:
                bool bit = (buffer[a.ByteOffset] & (1 << a.BitOffset)) != 0;
                p.RawState = bit;
                p.Value = bit ? 1 : 0;
                break;
            default:
                p.Value = ToReal(buffer, a.ByteOffset);
                break;
        }
        p.HasData = true;
        if (idx >= 0 && idx < CurrentValues.Length)
        {
            CurrentValues[idx] = p.Value;
            HasData[idx] = true;
        }
    }

    private void SimulateTick()
    {
        lock (_lock)
        {
            foreach (var p in _points)
            {
                int idx = p.Index - 1;
                switch (p.Type)
                {
                    case PointType.Result:
                        // 环控结果点：99.5% OK，偶发 NG
                        p.Value = _rnd.NextDouble() > 0.005 ? 1 : 2;
                        break;
                    case PointType.Analog:
                        p.Value = p.NominalValue + (_rnd.NextDouble() - 0.5) * 0.02;
                        break;
                    case PointType.Digital:
                        bool st = _rnd.NextDouble() > 0.003 ? p.ExpectedState : !p.ExpectedState;
                        p.RawState = st;
                        p.Value = st ? 1 : 0;
                        break;
                }
                p.HasData = true;
                p.PlcResult = p.Type == PointType.Analog ? p.Judge(p.Value) : "";
                if (idx >= 0 && idx < CurrentValues.Length)
                {
                    CurrentValues[idx] = p.Value;
                    HasData[idx] = true;
                }
            }

            // 报警位模拟：与环控 NG 点联动（NG 对应位报警）
            int ai = 0;
            foreach (var p in _points)
            {
                if (p.Type != PointType.Result) continue;
                if (ai >= AlarmBits.Length) break;
                AlarmBits[ai] = p.HasData && Math.Abs(p.Value - p.OkResultValue) > 0.5;
                ai++;
            }
            for (; ai < AlarmBits.Length; ai++) AlarmBits[ai] = false;
        }

        // 模拟检测完成：启动 3 秒后触发一次完成事件
        if (_simStartMeasureTime != DateTime.MinValue &&
            (DateTime.Now - _simStartMeasureTime).TotalSeconds >= 3)
        {
            _simStartMeasureTime = DateTime.MinValue;
            _log.Info("模拟模式：检测完成触发（启动后 3 秒）");
            MeasureDoneTriggered?.Invoke();
        }
        ValuesUpdated?.Invoke();
    }

    /// <summary>S7 REAL 大端 4 字节转本地浮点</summary>
    private static float ToReal(byte[] buf, int offset)
        => BitConverter.ToSingle(new[] { buf[offset + 3], buf[offset + 2], buf[offset + 1], buf[offset] }, 0);

    /// <summary>S7 Int 大端 2 字节转本地短整型</summary>
    private static short ToInt16(byte[] buf, int offset)
        => (short)((buf[offset] << 8) | buf[offset + 1]);

    private void CheckHeartbeat()
    {
        if (string.IsNullOrWhiteSpace(_cfg.HeartbeatAddress)) return;
        try
        {
            double v = ReadHeartbeatValue();
            if (Math.Abs(v - _lastHeartbeatValue) > 1e-9)
            {
                _lastHeartbeatValue = v;
                _lastHeartbeatTime = DateTime.Now;
            }
            else if ((DateTime.Now - _lastHeartbeatTime).TotalSeconds > 10)
            {
                _log.Warn("PLC 心跳超时（10 秒无变化）");
                HeartbeatLost?.Invoke();
                _lastHeartbeatTime = DateTime.Now;
            }
        }
        catch
        {
            // 心跳读取失败忽略，连接断开由读取块连续失败/重连处理
        }
    }

    /// <summary>读心跳值（REAL/INT/BOOL 地址均支持，值有变化即视为存活）</summary>
    private double ReadHeartbeatValue()
    {
        object v = ReadCached(_cfg.HeartbeatAddress.Trim());
        return v switch
        {
            float f => f,
            double d => d,
            int i => i,
            short s => s,
            ushort u => u,
            bool b => b ? 1 : 0,
            _ => 0
        };
    }

    private void CheckDoneBit()
    {
        if (string.IsNullOrWhiteSpace(_cfg.DoneAddress)) return;
        try
        {
            bool bit = ReadCached(_cfg.DoneAddress) is bool b && b;
            if (!_doneMonitorArmed)
            {
                // 连接/重连后首拍只建立基线，不判沿（PLC 侧 test_success 可能保持 1 未归零）
                _lastDoneBit = bit;
                _doneMonitorArmed = true;
                return;
            }
            if (bit && !_lastDoneBit)
            {
                _log.Info("test_success 上升沿：检测完成（读结果+复位code_ack）");
                MeasureDoneTriggered?.Invoke();
            }
            _lastDoneBit = bit;
        }
        catch { _doneMonitorArmed = false; }
    }

    private bool _lastDeleteData;

    /// <summary>测量开始时重置完成位监测：以 PLC 当前实际值为基线（不是强制 false）。
    /// PLC 侧 test_success 检测完成后会保持 1 不归零——若强制置 false，
    /// 下一拍轮询会误判出"假上升沿"，用上一轮旧数据瞬间完成测量并立即撤销 code_ack，
    /// 导致 PLC 真正的检测根本没有执行。以当前值做基线后只有真实 0→1 跳变才触发；
    /// test_success 保持 1 不归零的场景由 Equipment 运行→就绪迁移兜底完成。</summary>
    public void ResetDoneBitMonitor()
    {
        try
        {
            _lastDoneBit = !string.IsNullOrWhiteSpace(_cfg.DoneAddress) &&
                           (ReadCached(_cfg.DoneAddress) is bool b && b);
            _doneMonitorArmed = true;
        }
        catch { _doneMonitorArmed = false; }
    }

    private void CheckDeleteData()
    {
        if (string.IsNullOrWhiteSpace(_cfg.DeleteDataAddress)) return;
        try
        {
            bool bit = ReadCached(_cfg.DeleteDataAddress) is bool b && b;
            if (bit && !_lastDeleteData)
            {
                _log.Info("delete_data 上升沿：清除所有测量结果和值");
                DeleteDataTriggered?.Invoke();
            }
            _lastDeleteData = bit;
        }
        catch { }
    }

    #endregion

    #region 对外操作

    /// <summary>写 BOOL（启动/停止/复位/气缸开关：1 动作 0 复位）</summary>
    public bool WriteBool(string address, bool value)
    {
        if (string.IsNullOrWhiteSpace(address)) return true;
        if (_cfg.UseSimulator) return true;
        try
        {
            lock (_lock) _plc?.Write(address, value);
            _log.Info($"PLC 写入 {address} = {(value ? 1 : 0)}");
            return true;
        }
        catch (Exception ex)
        {
            _log.Error($"PLC 写入 {address} 失败", ex);
            return false;
        }
    }

    /// <summary>写 REAL（公差下发 MAX_TOL/MIN_TOL）</summary>
    public bool WriteReal(string address, float value)
    {
        if (string.IsNullOrWhiteSpace(address)) return true;
        if (_cfg.UseSimulator) return true;
        try
        {
            lock (_lock) _plc?.Write(address, value);
            return true;
        }
        catch (Exception ex)
        {
            _log.Error($"PLC 写入 {address}={value} 失败", ex);
            return false;
        }
    }

    /// <summary>气缸动作开关（写 1 动作 / 写 0 复位；swich 保持式）</summary>
    public bool CylinderSwitch(CylinderConfig cyl, bool on)
    {
        cyl.SwitchState = on;
        return WriteBool(cyl.SwitchAddress, on);
    }

    /// <summary>下发位移点上下限公差到 PLC（MAX_TOL.UP_TOL / MIN_TOL.LOW_TOL，i 从 1 起）。
    /// 写入绝对判定边界：UseNominal=true 时为 名义值+上限偏差 / 名义值+下限偏差；false 时直接用 UpperLimit/LowerLimit</summary>
    public int WriteTolerances(List<PointConfig> analogPoints)
    {
        if (!_connected && !_cfg.UseSimulator) { _log.Warn("PLC 未连接，公差下发跳过"); return 0; }
        int ok = 0;
        int i = 0;
        foreach (var p in analogPoints)
        {
            if (p.Type != PointType.Analog || !p.Enabled) continue;
            double upper = p.UseNominal ? p.NominalValue + p.UpperLimit : p.UpperLimit;
            double lower = p.UseNominal ? p.NominalValue + p.LowerLimit : p.LowerLimit;
            bool okUp = WriteReal(S7AddressTool.NextRealAddress(_cfg.MaxTolStartAddress, i), (float)upper);
            bool okLo = WriteReal(S7AddressTool.NextRealAddress(_cfg.MinTolStartAddress, i), (float)lower);
            if (okUp && okLo) ok++;
            i++;
        }
        _log.Info($"公差下发完成：{ok} 组（MAX_TOL@{_cfg.MaxTolStartAddress} / MIN_TOL@{_cfg.MinTolStartAddress}，绝对边界）");
        return ok;
    }

    /// <summary>下发指定型号的位移公差（不切型号，手动触发用）</summary>
    public int WriteTolerancesNow(SystemConfig cfg)
    {
        var analogs = cfg.Points.Where(p => p.Type == PointType.Analog).ToList();
        return WriteTolerances(analogs);
    }

    /// <summary>模拟模式：标记启动检测时间（3 秒后自动触发完成）</summary>
    public void SimulateMeasureStarted()
    {
        if (_cfg.UseSimulator) _simStartMeasureTime = DateTime.Now;
    }

    /// <summary>MES 在线/离线模式（纯软件全局变量，不写 PLC——DBX4.0/4.1 已被 PLC 用作自动/手动信号）</summary>
    public void SetOnline(bool online, bool log = true)
    {
        PcOnlineState = online;
        if (log) _log.Info(online ? "MES 在线模式" : "MES 离线模式（跳过校验）");
        OnlineChanged?.Invoke(online);
    }

    /// <summary>上位机在线/离线切换事件（MES 服务订阅联动：离线暂停重传/同步）</summary>
    public event Action<bool> OnlineChanged;

    /// <summary>写条码确认信号（code_ack Int：MES过站OK写1 / 测量完成复位0）</summary>
    public void WriteCodeAck(int value)
    {
        if (string.IsNullOrWhiteSpace(_cfg.CodeAckAddress)) return;
        try
        {
            lock (_lock) _plc?.Write(_cfg.CodeAckAddress, (short)value);
            _log.Info($"code_ack = {value}");
        }
        catch (Exception ex) { _log.Error($"code_ack 写入失败", ex); }
    }

    /// <summary>读 code_ack 当前值</summary>
    public bool ReadCodeAck()
    {
        if (string.IsNullOrWhiteSpace(_cfg.CodeAckAddress)) return false;
        try
        {
            var v = ReadCached(_cfg.CodeAckAddress);
            return v switch { bool b => b, short s => s != 0, int i => i != 0, ushort u => u != 0, _ => false };
        }
        catch { return false; }
    }

    /// <summary>写 code_ack=1 并保证产生 0→1 上升沿。
    /// PLC 以 code_ack 上升沿启动检测周期——上次异常（程序退出/完成被拒）残留 1 时，
    /// 直接写 1 是无效电平（无沿），PLC 不启动新周期、test_success 保持残留值，永远等不到完成信号（死锁）。
    /// 当前已是 1 则先写 0 短暂延时再写 1，确保 PLC 可靠看到沿。</summary>
    public void WriteCodeAckPulse(int value)
    {
        if (value == 1 && ReadCodeAck())
        {
            _log.Info("code_ack 残留 1：先归 0 再写 1（制造启动上升沿）");
            WriteCodeAck(0);
            System.Threading.Thread.Sleep(200);
        }
        WriteCodeAck(value);
    }

    /// <summary>获取最新点位值快照（环控点 Int 值，位移点 mm，NaN=无数据）</summary>
    public double[] GetValuesSnapshot()
    {
        lock (_lock) return (double[])CurrentValues.Clone();
    }

    /// <summary>获取点位有效数据标志快照</summary>
    public bool[] GetHasDataSnapshot()
    {
        lock (_lock) return (bool[])HasData.Clone();
    }

    /// <summary>获取 PLC 报警位快照（true=报警中）</summary>
    public bool[] GetAlarmSnapshot()
    {
        lock (_lock) return (bool[])AlarmBits.Clone();
    }

    #endregion

    public void Dispose()
    {
        _pollTimer?.Dispose();
        _reconnectTimer?.Dispose();
        // 退出时下线
        try { SetOnline(false, false); } catch { }
        try { _plc?.Close(); } catch { }
        _plc = null;
    }
}
