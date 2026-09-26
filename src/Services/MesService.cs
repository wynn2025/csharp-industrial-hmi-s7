using System.Net.Http;
using GaugeDemo300.Models;

namespace GaugeDemo300.Services;

/// <summary>
/// MES 通讯服务（接口文档：设备过站校验 + 过站信息同步，POST formdata）
/// - checkProcessLeak：上料时校验，code≠200 不允许开机并显示 msg
/// - sync：检测完成后过站信息同步（point1-10 测量值 + result1-5 OK/NG）
/// - 同步失败自动入本地队列，定时重试 + 手动重传
/// </summary>
public class MesService : IDisposable
{
    /// <summary>MES 通讯状态事件（界面状态灯用）</summary>
    public event Action<string> StatusChanged;

    private readonly MesConfig _cfg;
    private readonly DatabaseService _db;
    private readonly LogService _log;
    private readonly HttpClient _http;
    private System.Threading.Timer _retryTimer;
    private int _retryingFlag;             // 防重入（Interlocked 原子操作）
    private int _consecFailRounds;         // 连续失败轮数（服务器不可达判定）
    private int _skipRounds;               // 不可达时跳过的轮数（退避，避免每 60s 全量重发刷错误+占网络）             // 防重入标志（Interlocked 原子操作）

    /// <summary>最近一次接口调用是否成功（状态显示用）</summary>
    public bool LastCallSuccess { get; private set; } = true;

    /// <summary>MES 在线/离线（与上位机在线/离线按钮联动）。
    /// 离线时：重传定时器暂停、即时同步跳过 HTTP 直接入队——不再对着不通的服务器刷超时报错。</summary>
    public volatile bool Online = true;

    public MesService(SystemConfig config, DatabaseService db, LogService log)
    {
        _cfg = config.Mes;
        _db = db;
        _log = log;
        _http = new HttpClient { Timeout = TimeSpan.FromMilliseconds(_cfg.TimeoutMs) };
    }

    /// <summary>启动失败自动重试定时器</summary>
    public void StartAutoRetry()
    {
        if (!_cfg.EnableAutoRetry) return;
        _retryTimer = new System.Threading.Timer(
            _ => RetryQueueOnce(), null, _cfg.RetryIntervalMs, _cfg.RetryIntervalMs);
        _log.Info($"MES 失败重试已启动（间隔 {_cfg.RetryIntervalMs / 1000}s）");
    }

    #region 接口调用

    /// <summary>
    /// 过站校验接口 checkProcessLeak
    /// </summary>
    /// <returns>(code, msg)：code=200 通过；否则不通过，msg 为原因</returns>
    public async Task<(string code, string msg)> CheckProcessLeakAsync(string productCode)
    {
        var form = new Dictionary<string, string>
        {
            ["deviceCode"] = _cfg.DeviceCode,
            ["productCode"] = productCode.Trim()
        };
        return await PostFormAsync("过站校验", _cfg.CheckUrl, form);
    }

    /// <summary>
    /// 过站信息同步接口 sync
    /// 入参：code/equipment/operationTime/operator/productName/note/point1-10/result1-5
    /// </summary>
    public async Task<(bool ok, string msg)> SyncStationAsync(MeasurementRecord rec)
    {
        // 离线模式：不发起 HTTP（避免对不通的 MES 地址超时报错刷屏），记录直接入队待联网后重传
        if (!Online)
        {
            _db.EnqueueSync(rec.Id, rec.ProductCode);
            _log.Info($"离线模式：跳过 MES 同步，已入队待联网重传：{rec.ProductCode}");
            return (false, "离线模式：已入队");
        }

        var form = BuildSyncForm(rec);
        var (code, msg) = await PostFormAsync("过站同步", _cfg.SyncUrl, form);
        if (code == "200")
        {
            rec.SyncSuccess = true;
            rec.SyncTime = DateTime.Now;
            _db.MarkMeasurementSynced(rec.Id);
            _log.Info($"过站同步成功：{rec.ProductCode} 结果 {rec.OverallResult}");
            return (true, msg);
        }

        // 失败入队，等待重试
        _db.EnqueueSync(rec.Id, rec.ProductCode);
        _log.Warn($"过站同步失败（已入队待重传）：{rec.ProductCode} | {msg}");
        return (false, msg);
    }

    /// <summary>组装 sync 接口 formdata 参数</summary>
    private Dictionary<string, string> BuildSyncForm(MeasurementRecord rec)
    {
        var form = new Dictionary<string, string>
        {
            ["code"] = rec.ProductCode,
            ["equipment"] = string.IsNullOrWhiteSpace(rec.DeviceCode) ? _cfg.DeviceCode : rec.DeviceCode,
            ["operationTime"] = rec.StationTime.ToString("yyyy-MM-dd HH:mm:ss"),
            ["operator"] = rec.Operator,
            ["productName"] = rec.ProductName,
            ["note"] = rec.Note,
            // point1 = OK 点位列表，point2 = NG 点位列表（接口文档约定）
            ["point1"] = rec.OkPoints,
            ["point2"] = rec.NgPoints,
            ["result1"] = rec.OverallResult,
        };
        // point3..point10 = 前 8 个测量值
        for (int i = 0; i < rec.MeasurePoints.Length && i < 8; i++)
        {
            form[$"point{i + 3}"] = rec.MeasurePoints[i] ?? "";
        }
        // result2..result5 备用
        for (int i = 0; i < rec.ExtraResults.Length && i < 4; i++)
        {
            form[$"result{i + 2}"] = rec.ExtraResults[i] ?? "";
        }
        return form;
    }

    /// <summary>POST formdata 通用方法，返回 (code, msg)</summary>
    private async Task<(string code, string msg)> PostFormAsync(string action, string url, Dictionary<string, string> form)
    {
        try
        {
            using var content = new FormUrlEncodedContent(form);
            using var response = await _http.PostAsync(url, content);
            string body = await response.Content.ReadAsStringAsync();
            // 解析 {code, msg}（兼容大小写）
            var obj = Newtonsoft.Json.Linq.JObject.Parse(body);
            string code = obj["code"]?.ToString() ?? obj["Code"]?.ToString() ?? response.StatusCode.ToString();
            string msg = obj["msg"]?.ToString() ?? obj["Msg"]?.ToString() ?? "";
            LastCallSuccess = code == "200";
            StatusChanged?.Invoke(code == "200" ? "正常" : $"返回 {code}");
            _log.Info($"{action}：{url} → code={code} msg={msg}");
            return (code, msg);
        }
        catch (Exception ex)
        {
            LastCallSuccess = false;
            StatusChanged?.Invoke("通讯异常");
            _log.Error($"{action}通讯异常：{url}", ex);
            return ("ERROR", $"网络异常：{ex.Message}");
        }
    }

    #endregion

    #region 失败队列重传

    /// <summary>触发一次队列重传（manual=true 为界面按钮点击，离线时给出提示；定时器调用离线时静默跳过）</summary>
    public void RetryQueueOnce(bool manual = false)
    {
        if (!Online)
        {
            if (manual) _log.Info("离线模式：MES 重传已暂停（切回在线后自动恢复）");
            return;
        }
        if (_skipRounds > 0)
        {
            _skipRounds--;
            if (manual) _log.Info($"MES 服务器连续失败，重传退避中（还需等待 {_skipRounds + 1} 轮）");
            return;
        }
        // Interlocked 原子防重入：定时器与手动按钮可能同时触发
        if (System.Threading.Interlocked.CompareExchange(ref _retryingFlag, 1, 0) != 0) return;
        try
        {
            var items = _db.GetPendingQueue();
            if (items.Count == 0) { _consecFailRounds = 0; return; }
            _log.Info($"MES 重传队列：待重传 {items.Count} 条");
            int sentOk = 0;
            foreach (var item in items)
            {
                var rec = _db.QueryMeasurements(DateTime.MinValue, DateTime.MaxValue)
                    .FirstOrDefault(r => r.Id == item.RecordId);
                if (rec == null)
                {
                    _db.UpdateQueueStatus(item.Id, "FAILED", "关联记录不存在", item.RetryCount + 1);
                    continue;
                }
                var form = BuildSyncForm(rec);
                var (code, msg) = PostFormAsync("队列重传", _cfg.SyncUrl, form).GetAwaiter().GetResult();
                if (code == "200")
                {
                    _db.UpdateQueueStatus(item.Id, "SENT", "", item.RetryCount + 1);
                    _db.MarkMeasurementSynced(rec.Id);
                    _log.Info($"队列重传成功：{rec.ProductCode}");
                    sentOk++;
                }
                else
                {
                    _db.UpdateQueueStatus(item.Id, "FAILED", msg, item.RetryCount + 1);
                }
            }
            // 连续整轮零成功 = 服务器不可达：退避 5 轮（约 5 分钟），
            // 避免 60 秒一轮全量重发（10 条 × 2s 超时）持续占用网络、刷错误日志
            if (sentOk == 0)
            {
                _consecFailRounds++;
                if (_consecFailRounds >= 2)
                {
                    _skipRounds = 5;
                    _consecFailRounds = 0;
                    _log.Warn("MES 服务器连续不可达：重传退避 5 分钟（恢复后自动继续）");
                }
            }
            else _consecFailRounds = 0;
        }
        catch (Exception ex)
        {
            _log.Error("队列重传异常", ex);
        }
        finally { System.Threading.Interlocked.Exchange(ref _retryingFlag, 0); }
    }

    #endregion

    public void Dispose() => _http.Dispose();
}
