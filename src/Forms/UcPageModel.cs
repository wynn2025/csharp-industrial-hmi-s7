using GaugeDemo300.Models;
using GaugeDemo300.Services;

namespace GaugeDemo300.Forms
{
    /// <summary>
    /// 产品型号画面：型号列表 + 参数编辑（名称/数量）+ 复制型号 + 应用
    /// 左侧：型号列表 + 操作按钮
    /// 右侧：型号参数编辑器（名称/穿孔数/位移数/气缸数/保存）
    /// </summary>
    public partial class UcPageModel : UserControl
    {
        private readonly SystemConfig _cfg;
        private readonly ConfigService _configService;
        private readonly StationFlowService _flow;
        private readonly PlcService _plc;

        /// <summary>型号切换后（主窗体刷新顶栏产品名）</summary>
        public event Action ModelApplied;

        private Func<bool> _isEngineer;

        /// <summary>主窗体注入工程师权限校验</summary>
        public void BindEngineer(Func<bool> isEngineer) => _isEngineer = isEngineer;

        /// <summary>操作权限检查（应用/保存/复制/新建/删除需工程师）</summary>
        private bool CheckPermission(string action)
        {
            if (_isEngineer != null && !_isEngineer())
            {
                MessageBox.Show(ParentForm, $"{action}需要工程师权限，请先登录", "权限不足",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        public UcPageModel(SystemConfig cfg, ConfigService configService, StationFlowService flow, PlcService plc)
        {
            _cfg = cfg;
            _configService = configService;
            _flow = flow;
            _plc = plc;
            InitializeComponent();
            lstModels.SelectedIndexChanged += (_, _) => LoadSelectedToEditor();
            RefreshList();
        }

        public void RefreshList()
        {
            lstModels.Items.Clear();
            foreach (var name in _configService.ListModels()) lstModels.Items.Add(name);
            int idx = lstModels.Items.IndexOf(_cfg.CurrentModel);
            if (idx >= 0) lstModels.SelectedIndex = idx;
            lblModelCurrent.Text = $"当前型号：{_cfg.CurrentModel}";
        }

        /// <summary>选中型号加载到右侧编辑器</summary>
        private void LoadSelectedToEditor()
        {
            if (lstModels.SelectedItem is not string name) return;
            var model = _configService.LoadModel(name);
            if (model == null) return;
            txtModelName.Text = model.ModelName;
            txtProductName.Text = model.ProductName;
            numResultCount.Value = Math.Min(Math.Max(model.ResultCount, 10), 500);
            numAnalogCount.Value = Math.Min(Math.Max(model.AnalogCount, 5), 100);
            numCylCount.Value = Math.Min(Math.Max(model.CylinderCount, 0), 10);
        }

        /// <summary>从编辑器构建型号对象</summary>
        private ProductModelConfig BuildFromEditor()
        {
            return new ProductModelConfig
            {
                ModelName = txtModelName.Text.Trim(),
                ProductName = txtProductName.Text.Trim(),
                ResultCount = (int)numResultCount.Value,
                AnalogCount = (int)numAnalogCount.Value,
                CylinderCount = (int)numCylCount.Value,
                ResultPoints = new List<ResultPointDef>(),
                AnalogPoints = new List<AnalogPointDef>(),
                Cylinders = new List<CylinderDef>()
            };
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (!CheckPermission("应用型号")) return;
            if (lstModels.SelectedItem is not string name)
            {
                MessageBox.Show(ParentForm, "请先选择型号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_flow.SwitchModel(name))
            {
                RefreshList();
                ModelApplied?.Invoke();
                MessageBox.Show(ParentForm, $"型号已切换：{name}", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>保存编辑器中的修改到型号文件（数量变化时自动增减点位）</summary>
        private void btnSaveEdit_Click(object sender, EventArgs e)
        {
            if (!CheckPermission("保存型号参数")) return;
            string name = lstModels.SelectedItem as string ?? _cfg.CurrentModel;
            var old = _configService.LoadModel(name);
            if (old == null)
            {
                MessageBox.Show(ParentForm, "请先选择型号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var edited = BuildFromEditor();
            // 保留已有的点位定义（数量增减时截断/补默认）
            edited.ResultPoints = old.ResultPoints.Take(edited.ResultCount).ToList();
            while (edited.ResultPoints.Count < edited.ResultCount)
                edited.ResultPoints.Add(new ResultPointDef { Name = $"D{edited.ResultPoints.Count + 1:D3}", Description = $"穿孔检测点 {edited.ResultPoints.Count + 1}" });
            edited.AnalogPoints = old.AnalogPoints.Take(edited.AnalogCount).ToList();
            while (edited.AnalogPoints.Count < edited.AnalogCount)
                edited.AnalogPoints.Add(new AnalogPointDef { Name = $"A{edited.AnalogPoints.Count + 1:D3}", Description = $"位移检测点 {edited.AnalogPoints.Count + 1}" });
            edited.Cylinders = old.Cylinders.Take(edited.CylinderCount).ToList();
            while (edited.Cylinders.Count < edited.CylinderCount)
                edited.Cylinders.Add(new CylinderDef { Name = $"气缸{edited.Cylinders.Count + 1}" });

            // 如果改了名称，删旧文件
            if (edited.ModelName != name && edited.ModelName.Length > 0)
            {
                _configService.DeleteModel(name);
            }
            if (edited.ModelName.Length == 0) edited.ModelName = name;

            _configService.SaveModel(edited);
            RefreshList();
            MessageBox.Show(ParentForm,
                $"型号已保存：{edited.ModelName}\n穿孔 {edited.ResultCount} / 位移 {edited.AnalogCount} / 气缸 {edited.CylinderCount}",
                "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>复制型号（全部参数一起复制到新名称）</summary>
        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (!CheckPermission("复制型号")) return;
            if (lstModels.SelectedItem is not string name)
            {
                MessageBox.Show(ParentForm, "请先选择要复制的型号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string newName = Microsoft.VisualBasic.Interaction.InputBox($"复制型号「{name}」到新名称：", "复制型号", name + "-副本", -1, -1);
            newName = newName.Trim();
            if (newName.Length == 0 || newName == name) return;

            var source = _configService.LoadModel(name);
            if (source == null) return;
            source.ModelName = newName;
            _configService.SaveModel(source);
            RefreshList();
            MessageBox.Show(ParentForm, $"已复制「{name}」→「{newName}」（全部参数已复制）", "复制成功",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (!CheckPermission("新建型号")) return;
            string name = Microsoft.VisualBasic.Interaction.InputBox("输入新型号名称：", "新建型号", "新型号", -1, -1);
            if (string.IsNullOrWhiteSpace(name)) return;
            _flow.SaveCurrentAsModel(name.Trim());
            _configService.ApplyModel(_configService.LoadModel(name.Trim()));
            RefreshList();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!CheckPermission("删除型号")) return;
            if (lstModels.SelectedItem is not string name) return;
            if (MessageBox.Show(ParentForm, $"确认删除型号 {name}？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                != DialogResult.Yes) return;
            if (_configService.DeleteModel(name)) RefreshList();
            else MessageBox.Show(ParentForm, "至少保留一个型号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
