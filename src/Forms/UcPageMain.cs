using GaugeDemo300.Models;
using GaugeDemo300.Services;

namespace GaugeDemo300.Forms
{
    /// <summary>
    /// 主画面：条码行 + 检具布局图（叠加点位标注：穿孔=代号+颜色，位移=代号+值+颜色）
    /// + 过站状态 + 统计卡。标注支持拖拽定位、XML 保存、跟随型号。
    /// </summary>
    public partial class UcPageMain : UserControl
    {
        private readonly SystemConfig _cfg;
        private readonly DatabaseService _db;
        private readonly StationFlowService _flow;
        private readonly PlcService _plc;

        private Label[] _statLabels;
        private Func<bool> _isEngineer;

        // ===== 标注布局 =====
        private LayoutConfig _layout = new();
        private bool _editMode;
        private Panel? _dragOverlay;
        private Point _dragOffset;
        private Panel? _selectedOverlay;
        private ComboBox? _cmbPoint;
        private Panel? _toolbar;

        /// <summary>扫码提交</summary>
        public event Action<string> Scanned;

        public UcPageMain(SystemConfig cfg, DatabaseService db, StationFlowService flow, PlcService plc)
        {
            _cfg = cfg;
            _db = db;
            _flow = flow;
            _plc = plc;
            InitializeComponent();
            LoadLayoutImage();
            LoadLayout(_cfg.CurrentModel);   // 启动即从 Config\Layouts\{型号}.xml 恢复上次保存的点位标注
            LoadLogo();
            BuildStatCards();
            BuildEditButton();
            _flow.StateChanged += (state, msg) => SafeInvoke(() =>
            {
                lblFlowStatus.Text = msg;
                lblFlowStatus.ForeColor = state switch
                {
                    StationFlowService.FlowState.Passed => Theme.Ok,
                    StationFlowService.FlowState.Blocked => Theme.Ng,
                    StationFlowService.FlowState.Measuring => Theme.Accent,
                    StationFlowService.FlowState.Completed => msg.StartsWith("检测完成：NG") ? Theme.Ng : Theme.Ok,
                    _ => Theme.TextMain
                };
            });
            _flow.StateChanged += (state, _) => SafeInvoke(() =>
            {
                if (state == StationFlowService.FlowState.Measuring) SetOverlaysTesting();
            });
            _flow.MeasurementCompleted += rec => SafeInvoke(() =>
            {
                RefreshDailyStats();
                ApplyMeasureResult(rec);
            });
        }

        /// <summary>主窗体注入工程师权限</summary>
        public void BindEngineer(Func<bool> isEngineer) => _isEngineer = isEngineer;

        /// <summary>型号切换后加载对应布局并重建标注</summary>
        public void LoadLayout(string modelName)
        {
            _layout = LayoutService.Load(modelName);
            SafeInvoke(RebuildOverlays);
        }

        // ===== 标注管理 =====

        /// <summary>按布局重建全部标注面板</summary>
        private void RebuildOverlays()
        {
            foreach (Control c in panelImage.Controls.OfType<Panel>().Where(p => p.Tag is PointConfig).ToList())
                panelImage.Controls.Remove(c);
            foreach (var ov in _layout.Overlays)
            {
                var p = _cfg.Points.FirstOrDefault(x => x.Name == ov.PointName);
                if (p == null) continue;
                CreateOverlay(p, ov.X, ov.Y);
            }
            RefreshOverlays();
        }

        /// <summary>创建单个标注面板</summary>
        private Panel CreateOverlay(PointConfig p, int x, int y)
        {
            bool isAnalog = p.Type == PointType.Analog;
            var panel = new Panel
            {
                Size = new Size(isAnalog ? 78 : 42, 20),
                Location = new Point(x, y),
                BackColor = Color.FromArgb(60, 66, 88),
                Tag = p,
                Cursor = _editMode ? Cursors.Hand : Cursors.Default
            };
            var lbl = new Label
            {
                Name = "lbl",   // Controls.Find("lbl") 按 Name 查找——缺 Name 会导致刷新永远跳过
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                Font = new Font("Consolas", 8F, FontStyle.Bold),
                Text = p.Name,
                Tag = "lbl"
            };
            panel.Controls.Add(lbl);

            // 拖拽事件同时挂 Panel 和 Label——Label Dock=Fill 盖满 Panel，
            // 鼠标事件全落在 Label 上，只挂 Panel 时 MouseDown 永远不触发（拖不动）
            panel.MouseDown += Overlay_MouseDown;
            panel.MouseMove += Overlay_MouseMove;
            panel.MouseUp += Overlay_MouseUp;
            lbl.MouseDown += Overlay_MouseDown;
            lbl.MouseMove += Overlay_MouseMove;
            lbl.MouseUp += Overlay_MouseUp;
            panelImage.Controls.Add(panel);
            panel.BringToFront();
            return panel;
        }

        /// <summary>最近一次检测的逐点结果快照（pointName → 结果+显示文本）。
        /// 布局标注显示快照而非 PLC 实时值——与结果页一致：测完保持，直到下一轮测量开始。</summary>
        private readonly Dictionary<string, (string result, string text)> _lastResults = new();

        /// <summary>检测完成：从记录明细更新快照并刷新标注</summary>
        private void ApplyMeasureResult(GaugeDemo300.Models.MeasurementRecord rec)
        {
            _lastResults.Clear();
            try
            {
                var arr = Newtonsoft.Json.Linq.JArray.Parse(
                    string.IsNullOrWhiteSpace(rec.ChannelValuesJson) ? "[]" : rec.ChannelValuesJson);
                foreach (var item in arr)
                {
                    string name = item["name"]?.ToString();
                    if (name == null) continue;
                    string result = item["result"]?.ToString() ?? "";
                    bool isDisp = item["type"]?.ToString() == "位移";
                    string val = item["value"]?.ToString() ?? "-";
                    _lastResults[name] = (result, isDisp && val != "-" ? $"{name} {val}" : name);
                }
            }
            catch { }
            RefreshOverlays();
        }

        /// <summary>测量中灰化标注</summary>
        private void SetOverlaysTesting()
        {
            foreach (var panel in panelImage.Controls.OfType<Panel>().Where(p => p.Tag is PointConfig))
            {
                panel.BackColor = Color.FromArgb(60, 66, 88);
                panel.Controls.OfType<Label>().FirstOrDefault()?.ForeColor = Color.FromArgb(140, 155, 180);
            }
        }

        public void RefreshOverlays()
        {
            foreach (var panel in panelImage.Controls.OfType<Panel>().Where(p => p.Tag is PointConfig))
            {
                var p = (PointConfig)panel.Tag;
                var lbl = panel.Controls.OfType<Label>().FirstOrDefault();
                if (lbl == null) continue;
                if (_lastResults.TryGetValue(p.Name, out var r))
                {
                    lbl.Text = r.text;
                    lbl.ForeColor = Color.White;
                    panel.BackColor = r.result == "OK" ? Color.FromArgb(30, 90, 50)
                        : r.result == "NG" ? Color.FromArgb(120, 40, 42)
                        : Color.FromArgb(60, 66, 88);
                }
                else
                {
                    // 尚无检测结果（启动后/新放置点位）：中性显示，不被 PLC 实时值干扰
                    lbl.Text = p.Name;
                    lbl.ForeColor = Color.FromArgb(180, 186, 199);
                    panel.BackColor = Color.FromArgb(60, 66, 88);
                }
            }
        }

        // ===== 编辑模式 =====

        private void BuildEditButton()
        {
            var btn = new Button
            {
                Text = "编辑布局", Size = new Size(80, 26),
                Location = new Point(panelImage.Width - 85, 5),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.FromArgb(43, 99, 158), ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Font = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold)
            };
            btn.Click += (_, _) => { ToggleEditMode(); panelImage.Focus(); };
            btn.TabStop = false;
            panelImage.Controls.Add(btn);
            btn.BringToFront();
        }

        private void ToggleEditMode()
        {
            if (!_editMode && (_isEngineer != null && !_isEngineer()))
            {
                MessageBox.Show(ParentForm, "编辑布局需要工程师权限", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _editMode = !_editMode;
            if (_editMode) BuildToolbar(); else RemoveToolbar();
            // 切换标注的拖拽事件
            foreach (var panel in panelImage.Controls.OfType<Panel>().Where(p => p.Tag is PointConfig))
            {
                if (_editMode)
                {
                    panel.Cursor = Cursors.Hand;
                    // 拖拽事件已在 CreateOverlay 常驻挂载（Panel+Label），由 _editMode 门控
                }
                else
                {
                    panel.Cursor = Cursors.Default;
                    // 退出编辑：事件保留，_editMode 门控已禁止拖拽
                }
            }
            picLayout.MouseClick -= PicLayout_MouseClick;
            if (_editMode) picLayout.MouseClick += PicLayout_MouseClick;
        }

        private void BuildToolbar()
        {
            _toolbar = new Panel
            {
                Dock = DockStyle.Top, Height = 34,
                BackColor = Color.FromArgb(30, 35, 46)
            };
            _cmbPoint = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Size = new Size(120, 24), Location = new Point(8, 5),
                Font = new Font("Microsoft YaHei UI", 8.5F)
            };
            // 填充未放置的点位
            var placed = _layout.Overlays.Select(o => o.PointName).ToHashSet();
            foreach (var p in _cfg.Points.Where(p => !placed.Contains(p.Name)))
                _cmbPoint.Items.Add(p.Name);
            if (_cmbPoint.Items.Count > 0) _cmbPoint.SelectedIndex = 0;

            var btnDelete = MakeTbBtn("删除选中", 80, Color.FromArgb(150, 60, 60));
            btnDelete.Location = new Point(135, 5);
            btnDelete.Click += (_, _) => DeleteSelected();

            var btnSave = MakeTbBtn("保存布局", 80, Color.FromArgb(46, 160, 90));
            btnSave.Location = new Point(220, 5);
            btnSave.Click += (_, _) => SaveLayout();

            var btnClear = MakeTbBtn("清空全部", 80, Color.FromArgb(150, 60, 60));
            btnClear.Location = new Point(305, 5);
            btnClear.Click += (_, _) => ClearAllOverlays();

            var btnExit = MakeTbBtn("退出编辑", 80, Color.FromArgb(60, 66, 88));
            btnExit.Location = new Point(390, 5);
            btnExit.Click += (_, _) => ToggleEditMode();

            var lblTip = new Label
            {
                Text = "← 选点后在图上点击放置，拖拽移动", ForeColor = Theme.TextSub,
                AutoSize = true, Location = new Point(480, 9),
                Font = new Font("Microsoft YaHei UI", 8F)
            };

            _toolbar.Controls.AddRange(new Control[] { _cmbPoint, btnDelete, btnSave, btnClear, btnExit, lblTip });
            panelImage.Controls.Add(_toolbar);
            _toolbar.BringToFront();
        }

        private void RemoveToolbar()
        {
            if (_toolbar != null) { panelImage.Controls.Remove(_toolbar); _toolbar = null; }
            _selectedOverlay?.Invalidate();
        }

        private static Button MakeTbBtn(string text, int w, Color color) => new()
        {
            Text = text, Size = new Size(w, 24), BackColor = color, ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat, Font = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold)
        };

        private void PicLayout_MouseClick(object? sender, MouseEventArgs e)
        {
            if (!_editMode || _cmbPoint?.SelectedItem == null) return;
            string pointName = _cmbPoint.SelectedItem.ToString()!;
            var p = _cfg.Points.FirstOrDefault(x => x.Name == pointName);
            if (p == null) return;

            // 放置
            var panel = CreateOverlay(p, e.X - 20, e.Y - 10);
            _layout.Overlays.Add(new PointOverlayConfig { PointName = pointName, X = panel.Left, Y = panel.Top });

            // 从下拉移除
            _cmbPoint.Items.Remove(pointName);
            if (_cmbPoint.Items.Count > 0) _cmbPoint.SelectedIndex = 0;
            RefreshOverlays();
        }

        private void Overlay_MouseDown(object? sender, MouseEventArgs e)
        {
            if (!_editMode || e.Button != MouseButtons.Left) return;
            var panel = sender as Panel ?? (sender as Control)?.Parent as Panel;
            if (panel == null) return;
            _dragOverlay = panel;
            _selectedOverlay = panel;
            _dragOffset = e.Location;
        }

        private void Overlay_MouseMove(object? sender, MouseEventArgs e)
        {
            if (_dragOverlay == null) return;
            int nx = _dragOverlay.Left + e.X - _dragOffset.X;
            int ny = _dragOverlay.Top + e.Y - _dragOffset.Y;
            nx = Math.Max(0, Math.Min(nx, panelImage.Width - _dragOverlay.Width));
            ny = Math.Max(0, Math.Min(ny, panelImage.Height - _dragOverlay.Height));
            _dragOverlay.Location = new Point(nx, ny);
        }

        private void Overlay_MouseUp(object? sender, MouseEventArgs e)
        {
            if (_dragOverlay == null) return;
            var name = ((PointConfig)_dragOverlay.Tag!).Name;
            var ov = _layout.Overlays.FirstOrDefault(o => o.PointName == name);
            if (ov != null) { ov.X = _dragOverlay.Left; ov.Y = _dragOverlay.Top; }
            _dragOverlay = null;
        }

        /// <summary>清空当前型号全部点位标注（下拉恢复全部点位，需保存布局后才落盘）</summary>
        private void ClearAllOverlays()
        {
            if (_layout.Overlays.Count == 0) return;
            if (MessageBox.Show(ParentForm, $"确定清空当前型号的全部 {_layout.Overlays.Count} 个点位标注？\n（需点\"保存布局\"才会写入文件）",
                    "清空布局", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            _layout.Overlays.Clear();
            _selectedOverlay = null;
            _dragOverlay = null;
            RebuildOverlays();
            // 下拉恢复全部点位
            _cmbPoint.Items.Clear();
            foreach (var p in _cfg.Points) _cmbPoint.Items.Add(p.Name);
            if (_cmbPoint.Items.Count > 0) _cmbPoint.SelectedIndex = 0;
        }

        private void DeleteSelected()
        {
            if (_selectedOverlay == null) return;
            var name = ((PointConfig)_selectedOverlay.Tag!).Name;
            _layout.Overlays.RemoveAll(o => o.PointName == name);
            panelImage.Controls.Remove(_selectedOverlay);
            _cmbPoint?.Items.Add(name);
            _selectedOverlay = null;
        }

        private void SaveLayout()
        {
            _layout.ModelName = _cfg.CurrentModel;
            LayoutService.Save(_layout);
            MessageBox.Show(ParentForm, $"布局已保存：{_layout.Overlays.Count} 个标注 → Config\\Layouts\\{_cfg.CurrentModel}.xml",
                "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ===== 原有功能 =====

        private bool _lastOk;

        private void LoadLayoutImage()
        {
            try
            {
                string img = _cfg.LayoutImagePath;
                if (string.IsNullOrWhiteSpace(img)) return;
                string path = Path.IsPathRooted(img) ? img : Path.Combine(AppContext.BaseDirectory, img);
                if (File.Exists(path)) picLayout.Image = Image.FromFile(path);
            }
            catch { }
        }

        private void LoadLogo()
        {
            try
            {
                string path = Path.Combine(AppContext.BaseDirectory, "Config", "logo.png");
                if (File.Exists(path)) picLogo.Image = Image.FromFile(path);
            }
            catch { }
        }

        private void BuildStatCards()
        {
            var stats = new[] { ("今日检测", Theme.TextMain), ("OK", Theme.Ok), ("NG", Theme.Ng),
                ("合格率", Theme.Accent) };
            _statLabels = new Label[stats.Length];
            for (int i = 0; i < stats.Length; i++)
            {
                var card = MakeStatCard(stats[i].Item1, "--", stats[i].Item2);
                card.Dock = DockStyle.Fill;
                card.Margin = new Padding(4, 6, 4, 6);
                tpMainStats.Controls.Add(card, i, 0);
                _statLabels[i] = (Label)card.Tag;
            }
            RefreshDailyStats();
        }

        private static Panel MakeStatCard(string title, string value, Color valueColor)
        {
            var card = new Panel { BackColor = Color.FromArgb(33, 38, 50) };
            var bar = new Panel { Dock = DockStyle.Left, Width = 4, BackColor = valueColor };
            var tp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Margin = new Padding(8, 4, 4, 4) };
            tp.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            tp.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            var lbTitle = new Label { Text = title, Dock = DockStyle.Fill, ForeColor = Theme.TextSub, Font = new Font("Microsoft YaHei UI", 9F), TextAlign = ContentAlignment.BottomCenter };
            var lbValue = new Label { Text = value, Dock = DockStyle.Fill, ForeColor = valueColor, Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter };
            tp.Controls.Add(lbTitle, 0, 0);
            tp.Controls.Add(lbValue, 0, 1);
            card.Controls.Add(tp);
            card.Controls.Add(bar);
            card.Tag = lbValue;
            return card;
        }

        public void RefreshDailyStats()
        {
            if (_statLabels == null) return;
            var (total, ok, ng) = _db.GetDailyStats(DateTime.Today);
            _statLabels[0].Text = total.ToString();
            _statLabels[1].Text = ok.ToString();
            _statLabels[2].Text = ng.ToString();
            _statLabels[3].Text = total > 0 ? $"{(double)ok / total * 100:F1}%" : "--";
        }

        public void ClearLastResult() => _lastOk = false;

        public void ShowBarcode(string code) => lblBarcodeValue.Text = code;

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
    }
}
