using GaugeDemo300.Models;
using GaugeDemo300.Services;

namespace GaugeDemo300.Forms
{
    /// <summary>报警画面：软件日志报警 + PLC 报警位（P_ALARM 上升沿），确认/清空</summary>
    public partial class UcPageAlarm : UserControl
    {
        private readonly List<AlarmItem> _alarms = new();
        private readonly List<PointConfig> _envPoints;
        private bool[] _lastPlcBits;

        /// <summary>未确认报警数变化（主窗体页签红点用）</summary>
        public event Action UnacknowledgedChanged;

        public UcPageAlarm(SystemConfig cfg, LogService log, PlcService plc)
        {
            _envPoints = cfg.Points.Where(p => p.Type == PointType.Result).ToList();
            InitializeComponent();
            Theme.StyleGrid(dgvAlarm);

            log.MessageLogged += line => SafeInvoke(() =>
            {
                if (line.Contains("[WARN]") || line.Contains("[ERROR]"))
                {
                    string level = line.Contains("[ERROR]") ? "ERROR" : "WARN";
                    int sep = line.IndexOf("] ", StringComparison.Ordinal);
                    string body = sep >= 0 ? line.Substring(sep + 2) : line;
                    _alarms.Insert(0, new AlarmItem
                    {
                        Time = DateTime.Now,
                        Source = body.Contains("MES") ? "MES" : body.Contains("PLC") ? "PLC" : "系统",
                        Message = body,
                        Level = level
                    });
                    TrimAndNotify();
                    if (Visible) RefreshGrid();
                }
            });

            // PLC 报警位轮询（主窗体 timer 每 3 秒调用 PollPlcAlarms）
            plc.ValuesUpdated += () => SafeInvoke(ThrottledPollAlarms);
        }

        /// <summary>PLC 报警位检测（上升沿入列）</summary>
        private void PollPlcAlarms()
        {
            var bits = _plcAlarmBits?.Invoke();
            if (bits == null || bits.Length == 0) return;
            if (_lastPlcBits == null || _lastPlcBits.Length != bits.Length)
            {
                _lastPlcBits = new bool[bits.Length];
                bits.CopyTo(_lastPlcBits, 0);
                return;
            }
            bool changed = false;
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i] && !_lastPlcBits[i])
                {
                    string name = i < _envPoints.Count ? _envPoints[i].Name : $"P{i + 1:D3}";
                    string desc = i < _envPoints.Count ? _envPoints[i].Description : "";
                    _alarms.Insert(0, new AlarmItem
                    {
                        Time = DateTime.Now, Source = "PLC",
                        Message = $"P_ALARM[{i + 1}] {name} {desc} 报警", Level = "WARN"
                    });
                    changed = true;
                }
            }
            _lastPlcBits = (bool[])bits.Clone();
            if (changed)
            {
                TrimAndNotify();
                if (Visible) RefreshGrid();
            }
        }

        private Func<bool[]> _plcAlarmBits;

        /// <summary>主窗体注入 PLC 报警位快照获取</summary>
        public void BindAlarmBits(Func<bool[]> getter) => _plcAlarmBits = getter;

        private void TrimAndNotify()
        {
            if (_alarms.Count > 500) _alarms.RemoveRange(500, _alarms.Count - 500);
            UnacknowledgedChanged?.Invoke();
        }

        public void RefreshGrid()
        {
            dgvAlarm.Rows.Clear();
            foreach (var a in _alarms.Take(500))
            {
                int row = dgvAlarm.Rows.Add(a.Time.ToString("yyyy-MM-dd HH:mm:ss"), a.Level, a.Source, a.Message,
                    a.Acknowledged ? "√" : "");
                dgvAlarm.Rows[row].Cells[1].Style.ForeColor = a.Level == "ERROR" ? Theme.Ng : Theme.Warn;
            }
        }

        private void btnAck_Click(object sender, EventArgs e)
        {
            foreach (var a in _alarms) a.Acknowledged = true;
            UnacknowledgedChanged?.Invoke();
            RefreshGrid();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _alarms.Clear();
            UnacknowledgedChanged?.Invoke();
            RefreshGrid();
        }

        public bool HasUnacknowledged => _alarms.Any(a => !a.Acknowledged);

        private void SafeInvoke(Action action)
        {
            try
            {
                if (IsDisposed) return;
                if (InvokeRequired) BeginInvoke(action);
                else action();
            }
            catch (ObjectDisposedException) { }
        }

        private DateTime _lastAlarmPoll = DateTime.MinValue;
        private void ThrottledPollAlarms()
        {
            if ((DateTime.Now - _lastAlarmPoll).TotalMilliseconds < 1000) return;
            _lastAlarmPoll = DateTime.Now;
            PollPlcAlarms();
        }

    }
}
