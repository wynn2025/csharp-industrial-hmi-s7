using GaugeDemo300.Models;
using GaugeDemo300.Services;

namespace GaugeDemo300.Forms
{
    /// <summary>参数设置画面：穿孔/位移数量、PLC/MES 地址、密码等（需工程师登录）</summary>
    public partial class UcPageSettings : UserControl
    {
        private readonly SystemConfig _cfg;
        private readonly ConfigService _configService;
        private readonly PlcService _plc;
        private Func<bool> _isEngineer;
        private TextBox txtMesProductName;
        private TextBox txtMesNote;

        public event Action ConfigSaved;

        public UcPageSettings(SystemConfig cfg, ConfigService configService, PlcService plc)
        {
            _cfg = cfg;
            _configService = configService;
            _plc = plc;
            InitializeComponent();
            // MES 区动态加产品名称+备注（第3行）
            var lblP = new Label { Text = "产品名称：", TextAlign = ContentAlignment.MiddleRight, Dock = DockStyle.Fill, ForeColor = Color.FromArgb(140, 155, 180) };
            txtMesProductName = new TextBox { Dock = DockStyle.Fill, Font = new Font("Microsoft YaHei UI", 9F) };
            var lblN = new Label { Text = "备注信息：", TextAlign = ContentAlignment.MiddleRight, Dock = DockStyle.Fill, ForeColor = Color.FromArgb(140, 155, 180) };
            txtMesNote = new TextBox { Dock = DockStyle.Fill, Font = new Font("Microsoft YaHei UI", 9F) };
            tpMes.RowCount = 3;
            tpMes.RowStyles.Add(new RowStyle(SizeType.Percent, 33F));
            tpMes.Controls.Add(lblP, 0, 2);
            tpMes.Controls.Add(txtMesProductName, 1, 2);
            tpMes.Controls.Add(lblN, 2, 2);
            tpMes.Controls.Add(txtMesNote, 3, 2);
            RefreshFromConfig();
        }

        public void BindEngineer(Func<bool> isEngineer) => _isEngineer = isEngineer;

        public void RefreshFromConfig()
        {
            // 权限检查：非工程师不能查看/修改参数设置
            if (_isEngineer != null && !_isEngineer())
            {
                Visible = false;
                MessageBox.Show(ParentForm, "参数设置需要工程师权限，请先登录", "权限不足",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            numResultCount.Value = _cfg.Points.Count(p => p.Type == PointType.Result);
            numAnalogCount.Value = _cfg.Points.Count(p => p.Type == PointType.Analog);
            numCylCount.Value = _cfg.Cylinders.Count;
            txtPlcIp.Text = _cfg.Plc.IpAddress;
            txtCpuType.Text = _cfg.Plc.CpuType;
            numRack.Value = _cfg.Plc.Rack;
            numSlot.Value = _cfg.Plc.Slot;
            txtResultStart.Text = _cfg.Plc.ResultStartAddress;
            txtMeasureStart.Text = _cfg.Plc.MeasureStartAddress;
            txtCylSwitchStart.Text = _cfg.Plc.CylinderSwitchStartAddress;
            txtCheckUrl.Text = _cfg.Mes.CheckUrl;
            txtSyncUrl.Text = _cfg.Mes.SyncUrl;
            txtDeviceCode.Text = _cfg.Mes.DeviceCode;  // 跟随型号
            txtPassword.Text = _cfg.EngineerPassword;
            txtMesProductName.Text = _cfg.Mes.ProductName;
            txtMesNote.Text = _cfg.Mes.Note;
            chkSimulator.Checked = _cfg.Plc.UseSimulator;
            numPollMs.Value = _cfg.Plc.PollIntervalMs;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (_isEngineer != null && !_isEngineer())
            {
                MessageBox.Show(ParentForm, "请先以工程师身份登录", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show(ParentForm, "确认应用参数？点位表将重新生成，程序需要重启生效。", "确认",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                int resultCount = (int)numResultCount.Value;
                int analogCount = (int)numAnalogCount.Value;
                int cylCount = (int)numCylCount.Value;

                // 重新生成点位表
                ConfigService.GeneratePoints(_cfg.Points, resultCount,
                    txtResultStart.Text.Trim(), analogCount, txtMeasureStart.Text.Trim(),
                    _cfg.Plc.MeasureResultStartAddress);
                ConfigService.GenerateCylinders(_cfg.Cylinders, cylCount, txtCylSwitchStart.Text.Trim(), _cfg.Plc.CylinderStateStartAddress);

                // PLC 配置
                _cfg.Plc.IpAddress = txtPlcIp.Text.Trim();
                _cfg.Plc.CpuType = txtCpuType.Text.Trim();
                _cfg.Plc.Rack = (int)numRack.Value;
                _cfg.Plc.Slot = (int)numSlot.Value;
                _cfg.Plc.ResultStartAddress = txtResultStart.Text.Trim();
                _cfg.Plc.MeasureStartAddress = txtMeasureStart.Text.Trim();
                _cfg.Plc.CylinderSwitchStartAddress = txtCylSwitchStart.Text.Trim();
                _cfg.Plc.UseSimulator = chkSimulator.Checked;
                _cfg.Plc.PollIntervalMs = (int)numPollMs.Value;

                // MES 配置
                _cfg.Mes.CheckUrl = txtCheckUrl.Text.Trim();
                _cfg.Mes.SyncUrl = txtSyncUrl.Text.Trim();
                _cfg.Mes.DeviceCode = txtDeviceCode.Text.Trim();

                // 密码
                if (txtPassword.Text.Trim().Length > 0) _cfg.EngineerPassword = txtPassword.Text.Trim();
                _cfg.Mes.ProductName = txtMesProductName.Text.Trim();
                _cfg.Mes.Note = txtMesNote.Text.Trim();

                // 清除重排记录（下次启动重排全部地址）
                _cfg.GenResultStart = "";
                _cfg.GenMeasureStart = "";
                _cfg.GenCylinderStart = "";
                _cfg.GenCylinderStateStart = "";

                _configService.Save();
                MessageBox.Show(ParentForm,
                    $"参数已保存：\n穿孔检测 {resultCount} 点\n位移检测 {analogCount} 点\n气缸 {cylCount} 个\n\n请重启程序生效。",
                    "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ConfigSaved?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ParentForm, $"保存失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
