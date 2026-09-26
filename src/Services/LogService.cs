using System.Text;

namespace GaugeDemo300.Services;

/// <summary>
/// 运行日志服务：界面消息 + 按天文件落盘
/// </summary>
public class LogService
{
    /// <summary>日志消息事件（界面滚动显示用）</summary>
    public event Action<string> MessageLogged;

    private readonly object _lock = new();
    private readonly string _logDir;

    public LogService(string baseDir)
    {
        _logDir = Path.Combine(baseDir, "logs");
        try { Directory.CreateDirectory(_logDir); } catch { }
    }

    public void Info(string msg) => Write("INFO", msg);
    public void Warn(string msg) => Write("WARN", msg);
    public void Error(string msg, Exception ex = null)
        => Write("ERROR", ex == null ? msg : $"{msg} | {ex.Message}");

    private void Write(string level, string msg)
    {
        string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {msg}";
        try
        {
            MessageLogged?.Invoke(line);
            lock (_lock)
            {
                File.AppendAllText(
                    Path.Combine(_logDir, $"log_{DateTime.Now:yyyyMMdd}.txt"),
                    line + Environment.NewLine, Encoding.UTF8);
            }
        }
        catch { /* 日志失败不影响主流程 */ }
    }
}
