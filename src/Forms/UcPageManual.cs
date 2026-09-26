using GaugeDemo300.Models;
using GaugeDemo300.Services;

namespace GaugeDemo300.Forms
{
    /// <summary>手动画面：气缸行（名称+动作/复位按钮对+到位灯），数量/名称可配置，动态生成</summary>
    public partial class UcPageManual : UserControl
    {
        private readonly SystemConfig _cfg;
        private readonly PlcService _plc;
        private readonly StationFlowService _flow;
        private readonly LogService _log;
        private readonly ConfigService _configService;
        private Func<bool> _isManualMode;
        private Func<bool> _isLoggedIn;

        private readonly List<(CylinderConfig cyl, Button fwd, Button bwd)> _rows = new();

        public UcPageManual(SystemConfig cfg, PlcService plc, StationFlowService flow, LogService log, ConfigService configService)
        {
            _cfg = cfg;
            _plc = plc;
            _flow = flow;
            _log = log;
            _configService = configService;
            InitializeComponent();
            BuildCylinderRows();
            // sv_state 跟踪不依赖画面可见（首次状态也记录日志），可见时刷新按钮色
            _plc.ValuesUpdated += () => SafeInvoke(ThrottledCylRefresh);
            _flow.StateChanged += (_, msg) => SafeInvoke(UpdateTip);
        }

        /// <summary>由主窗体注入模式/登录判定（避免循环依赖）</summary>

        private void UpdateTip()
        {
            lblManualTip.Text = _plc.IsPlcAutoMode
                ? "⚠ 当前为自动模式，请到 PLC 切换手动模式开关"
                : "手动模式：气缸动作写 1 / 复位写 0（PLC 开关控制模式）";
        }

        /// <summary>每缸行高（像素，等高统一）</summary>
        private const int RowHeight = 56;

        /// <summary>型号切换后重建气缸行（Config.Cylinders 已更新）</summary>
        public void RefreshCylinders()
        {
            panelCylRows.Controls.Clear();
            _rows.Clear();
            _lastSvState.Clear();
            BuildCylinderRows();
        }

        private void BuildCylinderRows()
        {
            foreach (var cyl in _cfg.Cylinders.Where(c => c.Enabled))
            {
                // 每缸一行独立面板（固定等高），流式自上而下
                var row = new TableLayoutPanel
                {
                    Size = new Size(1160, RowHeight), Margin = new Padding(0, 3, 0, 3),
                    BackColor = Theme.Panel, ColumnCount = 4
                };
                row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
                row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27F));
                row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27F));
                row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8F));
                var name = new Label
                {
                    Text = $"{cyl.Index}. {cyl.Name}", Dock = DockStyle.Fill, ForeColor = Theme.TextMain,
                    Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(10, 0, 0, 0)
                };
                var fwd = MakeCylBtn("动作", Color.FromArgb(43, 99, 158));
                var bwd = MakeCylBtn("复位", Color.FromArgb(60, 66, 88));
                fwd.Click += (_, _) => Operate(cyl, true);
                bwd.Click += (_, _) => Operate(cyl, false);
                var btnRename = new Button
                {
                    Text = "改名", Dock = DockStyle.Fill, BackColor = Theme.BtnGray, ForeColor = Theme.TextSub,
                    FlatStyle = FlatStyle.Flat, FlatAppearance = { BorderSize = 0 },
                    Font = new Font("Microsoft YaHei UI", 8.5F), Margin = new Padding(2, 12, 2, 12)
                };
                btnRename.Click += (_, _) => RenameCylinder(cyl, name);
                row.Controls.Add(name, 0, 0);
                row.Controls.Add(fwd, 1, 0);
                row.Controls.Add(bwd, 2, 0);
                row.Controls.Add(btnRename, 3, 0);
                panelCylRows.Controls.Add(row);
                _rows.Add((cyl, fwd, bwd));
            }
        }

        private static Button MakeCylBtn(string text, Color color) => new()
        {
            Text = text, Dock = DockStyle.Fill, BackColor = color, ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat, FlatAppearance = { BorderSize = 0 },
            Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold), Margin = new Padding(4, 8, 4, 8)
        };

        /// <summary>修改气缸名称（需登录权限，保存到配置文件持久化）</summary>
        private void RenameCylinder(CylinderConfig cyl, Label nameLabel)
        {
            if (_isLoggedIn != null && !_isLoggedIn())
            {
                MessageBox.Show(ParentForm, "请先登录后再修改名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string newName = Microsoft.VisualBasic.Interaction.InputBox($"修改气缸 {cyl.Index} 的名称：", "气缸改名", cyl.Name, -1, -1);
            newName = newName.Trim();
            if (newName.Length == 0 || newName == cyl.Name) return;
            cyl.Name = newName;
            nameLabel.Text = $"{cyl.Index}. {newName}";
            _configService.Save();   // 持久化到 Config/SystemConfig.json
            _log.Info($"气缸名称已修改：{cyl.Index} → {newName}（已保存配置）");
        }

        private readonly Dictionary<int, bool> _lastSvState = new();

        private void Operate(CylinderConfig cyl, bool on)
        {
            // 手动操作不再需要登录（用户要求），但仍需手动模式
            if (_plc.IsPlcAutoMode)
            {
                MessageBox.Show(ParentForm, "当前为自动模式，请到 PLC 切换手动模式开关后再操作", "模式限制",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _log.Info($"气缸手动：{cyl.Name} {(on ? "动作" : "复位")} → {cyl.SwitchAddress}");
            _plc.CylinderSwitch(cyl, on);
        }

        /// <summary>跟踪+按需重绘（含 sv_state 变化诊断日志）</summary>
        private void RefreshCylinderLedsInternal()
        {
            bool visible = Visible;
            foreach (var (cyl, fwd, bwd) in _rows)
            {
                bool acting = cyl.ForwardArrived;
                if (_lastSvState.TryGetValue(cyl.Index, out bool last) && last != acting)
                {
                    _log.Info($"[sv_state] 气缸{cyl.Index}({cyl.Name}) → {(acting ? 1 : 0)}（{(acting ? "动作" : "复位")}）");
                }
                _lastSvState[cyl.Index] = acting;
                if (visible)
                {
                    fwd.BackColor = acting ? Color.FromArgb(46, 160, 90) : Color.FromArgb(43, 99, 158);
                    bwd.BackColor = acting ? Color.FromArgb(60, 66, 88) : Color.FromArgb(100, 130, 200);
                }
            }
        }

        public void RefreshCylinderLeds()
        {
            // sv_state 直接驱动按钮颜色：1=动作按钮亮绿，0=复位按钮亮蓝（无动作位/复位位灯）
            foreach (var (cyl, fwd, bwd) in _rows)
            {
                bool acting = cyl.ForwardArrived;   // = sv_state
                fwd.BackColor = acting ? Color.FromArgb(46, 160, 90) : Color.FromArgb(43, 99, 158);
                bwd.BackColor = acting ? Color.FromArgb(60, 66, 88) : Color.FromArgb(100, 130, 200);
                // sv_state 变化诊断：PLC 反馈一到就打日志（排查"颜色不变"是 PLC 没反馈还是界面没刷新）
                if (_lastSvState.TryGetValue(cyl.Index, out bool last) && last != acting)
                {
                    _log.Info($"[sv_state] 气缸{cyl.Index}({cyl.Name}) → {(acting ? 1 : 0)}（{(acting ? "动作" : "复位")}），按钮已变色");
                    _lastSvState[cyl.Index] = acting;
                }
                else _lastSvState[cyl.Index] = acting;
            }
        }

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

        private DateTime _lastCylRefresh = DateTime.MinValue;
        private void ThrottledCylRefresh()
        {
            if ((DateTime.Now - _lastCylRefresh).TotalMilliseconds < 500) return;
            _lastCylRefresh = DateTime.Now;
            RefreshCylinderLedsInternal();
        }

    }
}
