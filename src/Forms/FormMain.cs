using GaugeDemo300.Forms;
using GaugeDemo300.Models;
using GaugeDemo300.Services;

namespace GaugeDemo300
{
    /// <summary>
    /// 电子检具主界面骨架：顶栏（log/PLC心跳/产品名称/编号/操作员/条码/时间）
    /// + 右侧按钮栏（显示/登录/手动/自动/启动/停止/复位）
    /// + 中间画面区（8 个 UserControl 画面，见 Forms\UcPage*.cs 各自独立设计器可编辑）
    /// + 底部页签栏。画面逻辑在各 UcPageXxx 内，本类只负责切换与全局路由。
    /// </summary>
    public partial class FormMain : Form
    {
        private readonly SystemConfig _cfg;
        private readonly ConfigService _configService;
        private readonly DatabaseService _db;
        private readonly PlcService _plc;
        private readonly MesService _mes;
        private readonly StationFlowService _flow;
        private readonly LogService _log;

        private UserInfo _user = new();
        private readonly Dictionary<string, Control> _pages = new();
        private readonly List<Button> _tabButtons = new();

        public FormMain(SystemConfig cfg, ConfigService configService, DatabaseService db,
            PlcService plc, MesService mes, StationFlowService flow, LogService log)
        {
            _cfg = cfg;
            _configService = configService;
            _db = db;
            _plc = plc;
            _mes = mes;
            _flow = flow;
            _log = log;
            InitializeComponent();

            // 8 个画面（各自独立设计器文件，可单独在 VS 中编辑）
            ucMain = new UcPageMain(cfg, db, flow, plc);
            ucHistory = new UcPageHistory(db);
            ucManual = new UcPageManual(cfg, plc, flow, log, configService);
            ucResult = new UcPageResult(cfg, plc, configService);
            ucAlarm = new UcPageAlarm(cfg, log, plc);
            ucModel = new UcPageModel(cfg, configService, flow, plc);
            ucMes = new UcPageMes(cfg, db, mes, flow);
            ucSettings = new Forms.UcPageSettings(cfg, configService, plc);
                        ucAlarm.BindAlarmBits(() => _plc.GetAlarmSnapshot());
            ucResult.BindLogin(() => _user.Id.Length > 0);
            ucSettings.BindEngineer(() => _user.Role == "engineer");
            ucMain.BindEngineer(() => _user.Role == "engineer");
            ucModel.BindEngineer(() => _user.Role == "engineer");

            _pages["main"] = ucMain;
            _pages["history"] = ucHistory;
            _pages["manual"] = ucManual;
            _pages["result"] = ucResult;
            _pages["alarm"] = ucAlarm;
            _pages["model"] = ucModel;
            _pages["mes"] = ucMes;
            _pages["settings"] = ucSettings;
            foreach (var p in _pages.Values)
            {
                p.Dock = DockStyle.Fill;
                panelStage.Controls.Add(p);
            }
            _tabButtons.AddRange(new[] { tabMain, tabHistory, tabManual, tabResult, tabAlarm, tabModel, tabMes, tabSettings });

            BindServices();
            // 启动默认待机（设计器默认文本是 OK，不能一开机就显示 OK）
            labelResult.Text = "待机";
            labelResult.BackColor = Theme.BtnGray;
            labelResult.ForeColor = Color.White;
            ShowPage("main");
        }

        private void BindServices()
        {
            _log.MessageLogged += line => SafeInvoke(() =>
                lblTopLog.Text = line.Length > 160 ? line.Substring(line.Length - 160) : line);

            _plc.ConnectionChanged += ok => SafeInvoke(() =>
            {
                _heartAlive = ok;
                _lastHeartOk = DateTime.Now;
            });
            _plc.HeartbeatLost += () => SafeInvoke(() => _heartAlive = false);
            // 在线/离线与 MES 服务联动：离线时暂停队列重传、即时同步直接入队（不再对不通的 MES 地址刷超时报错）
            _mes.Online = _plc.PcOnlineState;
            _plc.OnlineChanged += on => _mes.Online = on;
            _plc.MeasureDoneTriggered += () => Task.Run(() => _flow.CompleteMeasure());
            _plc.ModeChanged += autoMode => SafeInvoke(() =>
            {
                lblModeDisplay.Text = autoMode ? "自动模式" : "手动模式";
                lblModeDisplay.BackColor = autoMode ? Color.FromArgb(46, 160, 90) : Color.FromArgb(240, 200, 90);
                lblModeDisplay.ForeColor = Color.Black;
            });
            _plc.DeleteDataTriggered += () => SafeInvoke(OnDeleteData);

            _flow.CheckBlocked += msg => SafeInvoke(() =>
                MessageBox.Show(this, $"过站校验未通过：\n{msg}", "禁止启动", MessageBoxButtons.OK, MessageBoxIcon.Warning));
            _flow.StateChanged += (state, msg) => SafeInvoke(() =>
            {
                switch (state)
                {
                    case StationFlowService.FlowState.MesChecking:
                        labelResult.Text = "校验中";
                        labelResult.BackColor = Color.FromArgb(240, 200, 90);
                        labelResult.ForeColor = Color.Black;
                        break;
                    case StationFlowService.FlowState.Measuring:
                        labelResult.Text = "测试中";
                        labelResult.BackColor = Color.FromArgb(240, 200, 90);
                        labelResult.ForeColor = Color.Black;
                        break;
                    case StationFlowService.FlowState.Completed:
                        bool ng = msg.StartsWith("检测完成：NG");
                        labelResult.Text = ng ? "NG" : "OK";
                        labelResult.BackColor = ng ? Theme.Ng : Theme.Ok;
                        labelResult.ForeColor = Color.White;
                        break;
                    case StationFlowService.FlowState.Blocked:
                        labelResult.Text = "拦截";
                        labelResult.BackColor = Theme.Ng;
                        labelResult.ForeColor = Color.White;
                        break;
                    default:
                        labelResult.Text = "待机";
                        labelResult.BackColor = Theme.BtnGray;
                        labelResult.ForeColor = Color.White;
                        break;
                }
            });
            ucMain.Scanned += code => SafeInvoke(() => ucMain.ShowBarcode(code));

            _flow.MeasurementCompleted += rec => SafeInvoke(() =>
            {
                labelResult.Text = rec.OverallResult;
                labelResult.BackColor = rec.OverallResult == "OK" ? Theme.Ok : Theme.Ng;
                labelResult.ForeColor = Color.White;
                ucMain.ShowBarcode(rec.ProductCode);
                ucResult.OnMeasureCompleted(rec);
                ucMain.RefreshDailyStats();
                ucMes.RefreshCheckGrid();
                ucMes.RefreshQueueGrid();
            });

            ucModel.ModelApplied += () => SafeInvoke(() =>
            {
                lblTopProductName.Text = $"产品名称：{_cfg.Mes.ProductName}";
            lblModeDisplay.Text = _plc.IsPlcAutoMode ? "自动模式" : "手动模式";
            lblModeDisplay.BackColor = _plc.IsPlcAutoMode ? Color.FromArgb(46, 160, 90) : Color.FromArgb(240, 200, 90);
            lblModeDisplay.ForeColor = Color.Black;
            lblTopProductCode.Text = $"产品编号：{_cfg.CurrentModel}";
            lblTopOperator.Text = _user.Id.Length > 0
                ? $"操作人员：{_user.Name}({_user.Id})" : "操作人员：未登录";
                // 型号切换后：重建 PLC 读取计划 + 刷新各画面点位缓存
                _plc.BuildReadPlan();
                ucResult.RefreshPoints();
                ucManual.RefreshCylinders();
                ucMain.RefreshDailyStats();
                ucMain.LoadLayout(_cfg.CurrentModel);
                _log.Info($"型号切换后界面已刷新（当前型号：{_cfg.CurrentModel}，" +
                          $"穿孔 {_cfg.Points.Count(p => p.Type == PointType.Result)} / " +
                          $"位移 {_cfg.Points.Count(p => p.Type == PointType.Analog)}）");
            });
            ucAlarm.UnacknowledgedChanged += () => SafeInvoke(() =>
                tabAlarm.BackColor = ucAlarm.HasUnacknowledged ? Theme.Ng : Color.FromArgb(33, 38, 50));
        }

        #region 画面切换

        private void ShowPage(string key)
        {
            foreach (var kv in _pages) kv.Value.Visible = kv.Key == key;
            Button active = key switch
            {
                "history" => tabHistory, "manual" => tabManual,
                "result" => tabResult, "alarm" => tabAlarm, "model" => tabModel,
                "mes" => tabMes, "settings" => tabSettings, _ => tabMain
            };
            foreach (var b in _tabButtons)
            {
                bool on = b == active;
                b.BackColor = on ? Theme.BtnBlue : Theme.BtnGray;
                b.ForeColor = Color.White;
            }
            switch (key)
            {
                case "history": ucHistory.RefreshHistory(); break;
                case "alarm": ucAlarm.RefreshGrid(); break;
                case "model": ucModel.RefreshList(); break;
                case "settings": ucSettings.RefreshFromConfig(); break;
                case "mes": ucMes.RefreshCheckGrid(); ucMes.RefreshQueueGrid(); break;
                case "manual": ucManual.RefreshCylinderLeds(); break;
            }
        }

        #endregion

        #region 顶栏/计时器

        private int _statsTick = 0;

        private void timerUi_Tick(object sender, EventArgs e)
        {
            lblTopTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // 扫码枪焦点保障：焦点在按钮上时自动移除（KeyPreview 全局捕获不受影响，防止 Enter 误触按钮）
            if (ActiveControl is Button)
            {
                ActiveControl = null;
            }
            lblTopProductName.Text = $"产品名称：{_cfg.Mes.ProductName}";
            if (_plc.IsConnected)
            {
                lblHeartbeat.Text = $"PLC·{_plc.EquipmentStateText}";
                lblHeartbeat.ForeColor = _plc.EquipmentState == 4 ? Theme.Ng : Theme.Ok;
            }
            else
            {
                lblHeartbeat.Text = _plc.IsSimulator ? "PLC·模拟" : "PLC未连接";
                lblHeartbeat.ForeColor = _plc.IsSimulator ? Theme.Warn : Theme.Ng;
            }
            pnlHeartLed.Invalidate();
            UpdateOnlineButton();
            ucMain.RefreshOverlays();
            // SQLite 统计查询降频到 5 秒一次（每秒查询导致界面卡顿）
            if (++_statsTick >= 5)
            {
                _statsTick = 0;
                ucMain.RefreshDailyStats();
            }
        }

        private bool _heartAlive;
        private DateTime _lastHeartOk = DateTime.Now;

        private void pnlHeartLed_Paint(object sender, PaintEventArgs e)
        {
            Color c = _plc.IsSimulator ? Theme.Warn : _plc.IsConnected ? (_heartAlive ? Theme.Ok : Theme.Warn) : Theme.Ng;
            using var brush = new SolidBrush(c);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int w = pnlHeartLed.ClientSize.Width;
            e.Graphics.FillEllipse(brush, w / 2 - 7, 4, 14, 14);
        }

        /// <summary>delete_data：清除所有测量结果和值（界面重置）</summary>
        private void OnDeleteData()
        {
            foreach (var p in _cfg.Points)
            {
                p.Value = double.NaN;
                p.HasData = false;
                p.PlcResult = "";
            }
            ucResult.ResetResult();
            ucMain.ClearLastResult();
            _log.Info("delete_data：所有测量结果和值已清除");
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

        #endregion

        #region 窗体事件/扫码

        private void FormMain_Load(object sender, EventArgs e)
        {
            // 顶栏 logo：从 Config/logo.png 加载（无则留空）
            try
            {
                string logoPath = Path.Combine(AppContext.BaseDirectory, "Config", "logo.png");
                if (File.Exists(logoPath))
                {
                    pictureBox1.BackgroundImage = Image.FromFile(logoPath);
                    pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            catch { }
          
            lblTopProductName.Text = $"产品名称：{_cfg.Mes.ProductName}";
            lblTopOperator.Text = "操作人员：--";
            timerUi.Start();
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            Activate();
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1: ShowPage("main"); e.Handled = true; break;
                case Keys.F2: ShowPage("history"); e.Handled = true; break;
                case Keys.F3: ShowPage("manual"); e.Handled = true; break;
                case Keys.F4: ShowPage("disp"); e.Handled = true; break;
                case Keys.F5: ShowPage("result"); e.Handled = true; break;
                case Keys.F6: ShowPage("alarm"); e.Handled = true; break;
                case Keys.F7: ShowPage("model"); e.Handled = true; break;
                case Keys.F8: ShowPage("mes"); e.Handled = true; break;
                // F9 启动已移除（PLC 自动启动）
                // F10 停止已移除
                case Keys.F11: btnReset_Click(sender, e); e.Handled = true; break;
            }
            if (e.Handled) { _scanBuffer.Clear(); return; }
            // 任何文本编辑控件获得焦点时跳过扫码捕获（含 DataGridView 内部编辑框、UserControl 内嵌控件）
            var focused = GetFocusedControl();
            if (focused is TextBox || focused is ComboBox) return;

            // 扫码枪全局捕获：可见字符进缓冲，回车提交（无需焦点在条码框）
            char c = ScanChar(e);
            if (c != '\0')
            {
                _scanBuffer.Append(c);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Enter && _scanBuffer.Length > 0)
            {
                string code = _scanBuffer.ToString();
                _scanBuffer.Clear();
                e.Handled = true;
                HandleScan(code);
            }
        }

        private readonly System.Text.StringBuilder _scanBuffer = new();

        /// <summary>递归查找当前真正获得焦点的控件（穿透 UserControl/DataGridView 层级）</summary>
        private Control GetFocusedControl()
        {
            Control c = this;
            while (c is ContainerControl cc && cc.ActiveControl != null)
            {
                c = cc.ActiveControl;
            }
            return c;
        }

        private static char ScanChar(KeyEventArgs e)
        {
            var k = e.KeyCode;
            if (k >= Keys.D0 && k <= Keys.D9) return (char)('0' + (k - Keys.D0));
            if (k >= Keys.NumPad0 && k <= Keys.NumPad9) return (char)('0' + (k - Keys.NumPad0));
            if (k >= Keys.A && k <= Keys.Z) return e.Shift ? (char)('A' + (k - Keys.A)) : (char)('a' + (k - Keys.A));
            if (k == Keys.OemMinus) return '-';
            if (k == Keys.OemPeriod) return '.';
            return '\0';
        }

        private void HandleScan(string code)
        {
            code = code.Trim().ToUpperInvariant();
            if (code.Length == 0) return;
            _flow.OnScan(code);
            ucMain.ShowBarcode(code);   // 扫码内容显示在主画面条码标签；顶栏产品编号固定显示当前型号
        }

        #endregion

        #region 右侧按钮栏

        private void btnShowMain_Click(object sender, EventArgs e) => ShowPage("model");

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (_user.Id.Length > 0)
            {
                _user = new UserInfo();
                _cfg.Mes.Operator = "";
                lblTopOperator.Text = "操作人员：--";
                btnLogin.Text = "登录";
                _log.Info("操作员已注销");
                return;
            }
            using var frm = new FrmLogin(_cfg);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                _user = frm.LoggedUser;
                _cfg.Mes.Operator = $"{_user.Id} {_user.Name}";
                lblTopOperator.Text = $"操作人员：{_user.Name}({_user.Id})";
                btnLogin.Text = "注销";
                _log.Info($"操作员登录：{_user.Id} {_user.Name}（{_user.Role}）");
            }
        }






        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "确认退出程序？", "退出", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Close();
        }

        private void btnOnline_Click(object sender, EventArgs e)
        {
            // 上位机在线/离线切换（写 PLC：on_line@DB5.DBX4.0 / off_line@DB5.DBX4.1 互斥）
            if (!_plc.IsConnected && !_plc.IsSimulator)
            {
                MessageBox.Show(this, "PLC 未连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _plc.SetOnline(!_plc.PcOnlineState);
            UpdateOnlineButton();
        }

        private void UpdateOnlineButton()
        {
            bool on = _plc.PcOnlineState;
            btnOnline.Text = on ? "在线·点击下线" : "离线·点击上线";
            btnOnline.BackColor = on ? Color.FromArgb(46, 160, 90) : Theme.BtnGray;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ucResult.ResetResult();
            ucMain.ClearLastResult();
            _flow.Reset();
        }

        #endregion

        #region 底部页签

        private void tabMain_Click(object sender, EventArgs e) => ShowPage("main");
        private void tabHistory_Click(object sender, EventArgs e) => ShowPage("history");
        private void tabManual_Click(object sender, EventArgs e) => ShowPage("manual");
        private void tabResult_Click(object sender, EventArgs e) => ShowPage("result");
        private void tabAlarm_Click(object sender, EventArgs e) => ShowPage("alarm");
        private void tabModel_Click(object sender, EventArgs e) => ShowPage("model");
        private void tabMes_Click(object sender, EventArgs e) => ShowPage("mes");
        private void tabSettings_Click(object sender, EventArgs e) => ShowPage("settings");

        #endregion
    }
}
