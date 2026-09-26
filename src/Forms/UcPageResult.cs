using Newtonsoft.Json.Linq;

using GaugeDemo300.Models;
using GaugeDemo300.Services;

namespace GaugeDemo300.Forms
{
    /// <summary>结果显示画面：环控 200 点 OK/NG 色块（翻页）+ 位移结果表（翻页）</summary>
    public partial class UcPageResult : UserControl
    {
        private readonly PlcService _plc;
        private readonly SystemConfig _cfg;
        private readonly ConfigService _configService;
        private Func<bool> _isLoggedIn;
        private readonly List<PointConfig> _envPoints;
        private readonly List<PointConfig> _anaPoints;
        private const int EnvPageSize = 100;
        private const int APageSize = 20;
        private int _envPage;
        private int _arPage;
        private MeasurementRecord _lastResult;

        private readonly List<Panel> _envBlocks = new();
        private readonly List<Label> _envLabels = new();

        public UcPageResult(SystemConfig cfg, PlcService plc, ConfigService configService)
        {
            _plc = plc;
            _cfg = cfg;
            _configService = configService;
            _envPoints = cfg.Points.Where(p => p.Type == PointType.Result).ToList();
            _anaPoints = cfg.Points.Where(p => p.Type == PointType.Analog).ToList();
            InitializeComponent();
            Theme.StyleGrid(dgvResultA);
            // 编辑控制：整体 ReadOnly=false，列级只开下限/上限（StyleGrid 内部设 ReadOnly=true 需覆盖）
            SetupGridEditing();
            // 数值验证：上下限单元格只允许数字/小数点/负号
            dgvResultA.CellValidating += DgvResultA_CellValidating;
            BuildEnvBlocks();
            RefreshResultA();   // 首次加载位移表第一页（自动刷新）
            _plc.ValuesUpdated += () => SafeInvoke(ThrottledAnaRefresh);
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

        /// <summary>检测完成（主窗体路由），填充结果并刷新</summary>
        public void OnMeasureCompleted(MeasurementRecord rec)
        {
            _lastResult = rec;
            RefreshEnvPage();
            RefreshResultA();
        }

        /// <summary>型号切换后重建点位缓存列表（Config.Points 已重建）</summary>
        public void RefreshPoints()
        {
            _envPoints.Clear();
            _envPoints.AddRange(System.Linq.Enumerable.Where(_cfg.Points, p => p.Type == PointType.Result));
            _anaPoints.Clear();
            _anaPoints.AddRange(System.Linq.Enumerable.Where(_cfg.Points, p => p.Type == PointType.Analog));
            _envBlocks.Clear();
            _envLabels.Clear();
            BuildEnvBlocks();
            RefreshResultA();
        }

        public void ResetResult()
        {
            _lastResult = null;
            RefreshEnvPage();
        }

        private void BuildEnvBlocks()
        {
            for (int i = 0; i < _envPoints.Count; i++)
            {
                var block = new Panel
                {
                    Size = new Size(56, 44), Margin = new Padding(2),
                    BackColor = Color.FromArgb(48, 52, 64)
                };
                var lbl = new Label
                {
                    Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Theme.TextSub, Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold),
                    Text = _envPoints[i].Name
                };
                block.Controls.Add(lbl);
                _envBlocks.Add(block);
                _envLabels.Add(lbl);
            }
            RefreshEnvPage();
        }

        private void RefreshEnvPage()
        {
            int pages = Math.Max((_envPoints.Count + EnvPageSize - 1) / EnvPageSize, 1);
            if (_envPage >= pages) _envPage = pages - 1;
            lblEnvPage.Text = $"第 {_envPage + 1}/{pages} 页";

            // 明细结果（检测完成后）
            var resultByName = new Dictionary<string, string>();
            if (_lastResult != null)
            {
                try
                {
                    var arr = JArray.Parse(_lastResult.ChannelValuesJson);
                    foreach (var it in arr)
                        resultByName[it["name"]?.ToString() ?? ""] = it["result"]?.ToString() ?? "";
                }
                catch { }
            }

            flowEnv.SuspendLayout();
            flowEnv.Controls.Clear();
            int from = _envPage * EnvPageSize;
            int to = Math.Min(from + EnvPageSize, _envPoints.Count);
            for (int i = from; i < to; i++)
            {
                var p = _envPoints[i];
                string judge = resultByName.TryGetValue(p.Name, out var r) ? r
                    : (p.HasData ? p.FinalJudge() : "");
                var block = _envBlocks[i];
                var lbl = _envLabels[i];
                switch (judge)
                {
                    case "OK":
                        block.BackColor = Color.FromArgb(30, 74, 46);
                        lbl.ForeColor = Theme.Ok;
                        lbl.Text = $"{p.Name}\nOK";
                        break;
                    case "NG":
                        block.BackColor = Color.FromArgb(96, 40, 42);
                        lbl.ForeColor = Theme.Ng;
                        lbl.Text = $"{p.Name}\nNG";
                        break;
                    default:
                        block.BackColor = Color.FromArgb(48, 52, 64);
                        lbl.ForeColor = Theme.TextSub;
                        lbl.Text = p.Name;
                        break;
                }
                flowEnv.Controls.Add(block);
            }
            flowEnv.ResumeLayout();
        }

        private void RefreshResultA()
        {
            int pages = Math.Max((_anaPoints.Count + APageSize - 1) / APageSize, 1);
            if (_arPage >= pages) _arPage = pages - 1;
            lblARPage.Text = $"第 {_arPage + 1}/{pages} 页";
            dgvResultA.Rows.Clear();
            int from = _arPage * APageSize;
            for (int i = from; i < Math.Min(from + APageSize, _anaPoints.Count); i++)
            {
                var p = _anaPoints[i];
                string val = p.HasData && !double.IsNaN(p.Value) ? p.Value.ToString("F3") : "--";
                double lo = p.UseNominal ? p.NominalValue + p.LowerLimit : p.LowerLimit;
                double hi = p.UseNominal ? p.NominalValue + p.UpperLimit : p.UpperLimit;
                string judge = p.FinalJudge();
                int row = dgvResultA.Rows.Add(p.Name, p.Description, val, lo.ToString("F3"), hi.ToString("F3"), judge);
                dgvResultA.Rows[row].Tag = p;
                dgvResultA.Rows[row].Cells[5].Style.ForeColor = judge == "OK" ? Theme.Ok : Theme.Ng;
                dgvResultA.Rows[row].Cells[5].Style.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            }
        }

        /// <summary>实时刷新当前页位移值与判定（PLC 200ms 轮询驱动）</summary>
        private void RefreshAnaValues()
        {
            foreach (DataGridViewRow row in dgvResultA.Rows)
            {
                if (row.Tag is not PointConfig p) continue;
                string val = p.HasData && !double.IsNaN(p.Value) ? p.Value.ToString("F3") : "--";
                if ((string)row.Cells[2].Value != val) row.Cells[2].Value = val;
                string judge = p.FinalJudge();
                if ((string)row.Cells[5].Value != judge)
                {
                    row.Cells[5].Value = judge;
                    row.Cells[5].Style.ForeColor = judge == "OK" ? Theme.Ok : Theme.Ng;
                    row.Cells[5].Style.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
                }
            }
        }

        /// <summary>主窗体注入登录校验</summary>
        public void BindLogin(Func<bool> isLoggedIn) => _isLoggedIn = isLoggedIn;

        private bool _editMode = false;

        /// <summary>设置表格编辑：整体可编辑，但只有下限/上限列在编辑模式下才可编辑</summary>
        private void SetupGridEditing()
        {
            dgvResultA.ReadOnly = false;                    // 控件级必须 false
            dgvResultA.EditMode = DataGridViewEditMode.EditOnEnter;  // 单击即编辑
            foreach (DataGridViewColumn col in dgvResultA.Columns)
                col.ReadOnly = !_editMode && (col.Name != "colLower" && col.Name != "colUpper") 
                               || _editMode == false && (col.Name != "colLower" && col.Name != "colUpper");
            // 简化：非编辑模式全部锁，编辑模式只开下限/上限
            foreach (DataGridViewColumn col in dgvResultA.Columns)
                col.ReadOnly = !_editMode;
            colLower.ReadOnly = !_editMode;
            colUpper.ReadOnly = !_editMode;
        }

        /// <summary>上下限单元格只允许数字/小数点/负号</summary>
        private void DgvResultA_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex != 3 && e.ColumnIndex != 4) return;  // 只验证下限/上限
            string val = e.FormattedValue?.ToString()?.Trim() ?? "";
            if (val.Length == 0) return;
            if (!double.TryParse(val, out _))
            {
                e.Cancel = true;
                MessageBox.Show(ParentForm, $"请输入有效数字（当前输入：{val}）", "输入错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSaveTol_Click(object sender, EventArgs e)
        {
            if (!_editMode)
            {
                // 进入修改模式
                if (_isLoggedIn != null && !_isLoggedIn())
                {
                    MessageBox.Show(ParentForm, "请先登录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                _editMode = true;
                btnSaveTol.Text = "保存并下发PLC";
                btnSaveTol.BackColor = Color.FromArgb(190, 120, 30);
                SetupGridEditing();
                MessageBox.Show(ParentForm, "已进入编辑模式：直接点击表格中的下限/上限单元格修改数值", "编辑模式",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 保存模式：写回 + 保存配置 + 下发 PLC
            int changed = 0;
            foreach (DataGridViewRow row in dgvResultA.Rows)
            {
                if (row.Tag is not PointConfig p) continue;
                if (double.TryParse(row.Cells[3].Value?.ToString(), out double lo) && Math.Abs(lo - p.LowerLimit) > 1e-9)
                { p.LowerLimit = lo; changed++; }
                if (double.TryParse(row.Cells[4].Value?.ToString(), out double hi) && Math.Abs(hi - p.UpperLimit) > 1e-9)
                { p.UpperLimit = hi; changed++; }
            }

            _editMode = false;
            btnSaveTol.Text = "下发中...";
            btnSaveTol.Enabled = false;
            SetupGridEditing();

            if (changed > 0) _configService.Save();
            // PLC 写入在后台线程执行（60 组同步写最多卡 UI 2 分钟）
            Task.Run(() =>
            {
                int ok = _plc.WriteTolerancesNow(_cfg);
                SafeInvoke(() =>
                {
                    btnSaveTol.Text = "修改";
                    btnSaveTol.Enabled = true;
                    MessageBox.Show(ParentForm,
                        changed > 0 ? $"修改 {changed} 处已保存，公差下发 {ok} 组到 PLC" : $"公差下发 {ok} 组到 PLC",
                        "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
            });
        }

        private void btnEnvPrev_Click(object sender, EventArgs e) { if (_envPage > 0) { _envPage--; RefreshEnvPage(); } }
        private void btnEnvNext_Click(object sender, EventArgs e)
        {
            int pages = Math.Max((_envPoints.Count + EnvPageSize - 1) / EnvPageSize, 1);
            if (_envPage < pages - 1) { _envPage++; RefreshEnvPage(); }
        }
        private void btnARPrev_Click(object sender, EventArgs e) { if (_arPage > 0) { _arPage--; RefreshResultA(); } }
        private void btnARNext_Click(object sender, EventArgs e)
        {
            int pages = Math.Max((_anaPoints.Count + APageSize - 1) / APageSize, 1);
            if (_arPage < pages - 1) { _arPage++; RefreshResultA(); }
        }

        private DateTime _lastAnaRefresh = DateTime.MinValue;
        private void ThrottledAnaRefresh()
        {
            if (!Visible || _editMode) return;
            if ((DateTime.Now - _lastAnaRefresh).TotalMilliseconds < 1000) return;
            _lastAnaRefresh = DateTime.Now;
            RefreshAnaValues();
        }

    }
}
