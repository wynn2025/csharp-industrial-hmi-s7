namespace GaugeDemo300.Forms
{
    partial class UcPageSettings
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tpRoot = new System.Windows.Forms.TableLayoutPanel();
            this.grpPoints = new System.Windows.Forms.GroupBox();
            this.tpPoints = new System.Windows.Forms.TableLayoutPanel();
            this.lblResultCount = new System.Windows.Forms.Label();
            this.numResultCount = new System.Windows.Forms.NumericUpDown();
            this.lblAnalogCount = new System.Windows.Forms.Label();
            this.numAnalogCount = new System.Windows.Forms.NumericUpDown();
            this.lblCylCount = new System.Windows.Forms.Label();
            this.numCylCount = new System.Windows.Forms.NumericUpDown();
            this.lblResultStart = new System.Windows.Forms.Label();
            this.txtResultStart = new System.Windows.Forms.TextBox();
            this.lblMeasureStart = new System.Windows.Forms.Label();
            this.txtMeasureStart = new System.Windows.Forms.TextBox();
            this.lblCylSwitchStart = new System.Windows.Forms.Label();
            this.txtCylSwitchStart = new System.Windows.Forms.TextBox();
            this.grpPlc = new System.Windows.Forms.GroupBox();
            this.tpPlc = new System.Windows.Forms.TableLayoutPanel();
            this.lblPlcIp = new System.Windows.Forms.Label();
            this.txtPlcIp = new System.Windows.Forms.TextBox();
            this.lblCpuType = new System.Windows.Forms.Label();
            this.txtCpuType = new System.Windows.Forms.TextBox();
            this.lblRack = new System.Windows.Forms.Label();
            this.numRack = new System.Windows.Forms.NumericUpDown();
            this.lblSlot = new System.Windows.Forms.Label();
            this.numSlot = new System.Windows.Forms.NumericUpDown();
            this.lblPollMs = new System.Windows.Forms.Label();
            this.numPollMs = new System.Windows.Forms.NumericUpDown();
            this.chkSimulator = new System.Windows.Forms.CheckBox();
            this.grpMes = new System.Windows.Forms.GroupBox();
            this.tpMes = new System.Windows.Forms.TableLayoutPanel();
            this.lblCheckUrl = new System.Windows.Forms.Label();
            this.txtCheckUrl = new System.Windows.Forms.TextBox();
            this.lblSyncUrl = new System.Windows.Forms.Label();
            this.txtSyncUrl = new System.Windows.Forms.TextBox();
            this.lblDeviceCode = new System.Windows.Forms.Label();
            this.txtDeviceCode = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.tpRoot.SuspendLayout();
            this.grpPoints.SuspendLayout();
            this.tpPoints.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numResultCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAnalogCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCylCount)).BeginInit();
            this.grpPlc.SuspendLayout();
            this.tpPlc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSlot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPollMs)).BeginInit();
            this.grpMes.SuspendLayout();
            this.tpMes.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpRoot
            // 
            this.tpRoot.ColumnCount = 1;
            this.tpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpRoot.Controls.Add(this.grpPoints, 0, 0);
            this.tpRoot.Controls.Add(this.grpPlc, 0, 1);
            this.tpRoot.Controls.Add(this.grpMes, 0, 2);
            this.tpRoot.Controls.Add(this.btnApply, 0, 3);
            this.tpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpRoot.Location = new System.Drawing.Point(0, 0);
            this.tpRoot.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpRoot.Name = "tpRoot";
            this.tpRoot.Padding = new System.Windows.Forms.Padding(13, 11, 13, 11);
            this.tpRoot.RowCount = 4;
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpRoot.Size = new System.Drawing.Size(1543, 826);
            this.tpRoot.TabIndex = 0;
            // 
            // grpPoints
            // 
            this.grpPoints.Controls.Add(this.tpPoints);
            this.grpPoints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPoints.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpPoints.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(234)))), ((int)(((byte)(244)))));
            this.grpPoints.Location = new System.Drawing.Point(17, 14);
            this.grpPoints.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpPoints.Name = "grpPoints";
            this.grpPoints.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpPoints.Size = new System.Drawing.Size(1509, 191);
            this.grpPoints.TabIndex = 0;
            this.grpPoints.TabStop = false;
            this.grpPoints.Text = " 检测点位配置 ";
            // 
            // tpPoints
            // 
            this.tpPoints.ColumnCount = 4;
            this.tpPoints.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tpPoints.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tpPoints.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tpPoints.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tpPoints.Controls.Add(this.lblResultCount, 0, 0);
            this.tpPoints.Controls.Add(this.numResultCount, 1, 0);
            this.tpPoints.Controls.Add(this.lblAnalogCount, 0, 1);
            this.tpPoints.Controls.Add(this.numAnalogCount, 1, 1);
            this.tpPoints.Controls.Add(this.lblCylCount, 0, 2);
            this.tpPoints.Controls.Add(this.numCylCount, 1, 2);
            this.tpPoints.Controls.Add(this.lblResultStart, 2, 0);
            this.tpPoints.Controls.Add(this.txtResultStart, 3, 0);
            this.tpPoints.Controls.Add(this.lblMeasureStart, 2, 1);
            this.tpPoints.Controls.Add(this.txtMeasureStart, 3, 1);
            this.tpPoints.Controls.Add(this.lblCylSwitchStart, 2, 2);
            this.tpPoints.Controls.Add(this.txtCylSwitchStart, 3, 2);
            this.tpPoints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpPoints.Location = new System.Drawing.Point(4, 29);
            this.tpPoints.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpPoints.Name = "tpPoints";
            this.tpPoints.RowCount = 4;
            this.tpPoints.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tpPoints.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tpPoints.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tpPoints.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tpPoints.Size = new System.Drawing.Size(1501, 159);
            this.tpPoints.TabIndex = 0;
            // 
            // lblResultCount
            // 
            this.lblResultCount.Location = new System.Drawing.Point(4, 0);
            this.lblResultCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResultCount.Name = "lblResultCount";
            this.lblResultCount.Size = new System.Drawing.Size(129, 24);
            this.lblResultCount.TabIndex = 0;
            this.lblResultCount.Text = "穿孔检测点数：";
            // 
            // numResultCount
            // 
            this.numResultCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numResultCount.Location = new System.Drawing.Point(379, 3);
            this.numResultCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numResultCount.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numResultCount.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numResultCount.Name = "numResultCount";
            this.numResultCount.Size = new System.Drawing.Size(367, 33);
            this.numResultCount.TabIndex = 1;
            this.numResultCount.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // lblAnalogCount
            // 
            this.lblAnalogCount.Location = new System.Drawing.Point(4, 39);
            this.lblAnalogCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnalogCount.Name = "lblAnalogCount";
            this.lblAnalogCount.Size = new System.Drawing.Size(129, 24);
            this.lblAnalogCount.TabIndex = 2;
            this.lblAnalogCount.Text = "位移检测点数：";
            // 
            // numAnalogCount
            // 
            this.numAnalogCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numAnalogCount.Location = new System.Drawing.Point(379, 42);
            this.numAnalogCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numAnalogCount.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numAnalogCount.Name = "numAnalogCount";
            this.numAnalogCount.Size = new System.Drawing.Size(367, 33);
            this.numAnalogCount.TabIndex = 3;
            this.numAnalogCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lblCylCount
            // 
            this.lblCylCount.Location = new System.Drawing.Point(4, 78);
            this.lblCylCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCylCount.Name = "lblCylCount";
            this.lblCylCount.Size = new System.Drawing.Size(129, 24);
            this.lblCylCount.TabIndex = 4;
            this.lblCylCount.Text = "气缸数量：";
            // 
            // numCylCount
            // 
            this.numCylCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numCylCount.Location = new System.Drawing.Point(379, 81);
            this.numCylCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numCylCount.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numCylCount.Name = "numCylCount";
            this.numCylCount.Size = new System.Drawing.Size(367, 33);
            this.numCylCount.TabIndex = 5;
            // 
            // lblResultStart
            // 
            this.lblResultStart.Location = new System.Drawing.Point(754, 0);
            this.lblResultStart.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResultStart.Name = "lblResultStart";
            this.lblResultStart.Size = new System.Drawing.Size(129, 24);
            this.lblResultStart.TabIndex = 6;
            this.lblResultStart.Text = "穿孔起始地址：";
            // 
            // txtResultStart
            // 
            this.txtResultStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResultStart.Location = new System.Drawing.Point(1129, 3);
            this.txtResultStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtResultStart.Name = "txtResultStart";
            this.txtResultStart.Size = new System.Drawing.Size(368, 33);
            this.txtResultStart.TabIndex = 7;
            // 
            // lblMeasureStart
            // 
            this.lblMeasureStart.Location = new System.Drawing.Point(754, 39);
            this.lblMeasureStart.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMeasureStart.Name = "lblMeasureStart";
            this.lblMeasureStart.Size = new System.Drawing.Size(129, 24);
            this.lblMeasureStart.TabIndex = 8;
            this.lblMeasureStart.Text = "位移起始地址：";
            // 
            // txtMeasureStart
            // 
            this.txtMeasureStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMeasureStart.Location = new System.Drawing.Point(1129, 42);
            this.txtMeasureStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtMeasureStart.Name = "txtMeasureStart";
            this.txtMeasureStart.Size = new System.Drawing.Size(368, 33);
            this.txtMeasureStart.TabIndex = 9;
            // 
            // lblCylSwitchStart
            // 
            this.lblCylSwitchStart.Location = new System.Drawing.Point(754, 78);
            this.lblCylSwitchStart.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCylSwitchStart.Name = "lblCylSwitchStart";
            this.lblCylSwitchStart.Size = new System.Drawing.Size(129, 24);
            this.lblCylSwitchStart.TabIndex = 10;
            this.lblCylSwitchStart.Text = "气缸开关起始：";
            // 
            // txtCylSwitchStart
            // 
            this.txtCylSwitchStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCylSwitchStart.Location = new System.Drawing.Point(1129, 81);
            this.txtCylSwitchStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtCylSwitchStart.Name = "txtCylSwitchStart";
            this.txtCylSwitchStart.Size = new System.Drawing.Size(368, 33);
            this.txtCylSwitchStart.TabIndex = 11;
            // 
            // grpPlc
            // 
            this.grpPlc.Controls.Add(this.tpPlc);
            this.grpPlc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPlc.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpPlc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(234)))), ((int)(((byte)(244)))));
            this.grpPlc.Location = new System.Drawing.Point(17, 211);
            this.grpPlc.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpPlc.Name = "grpPlc";
            this.grpPlc.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpPlc.Size = new System.Drawing.Size(1509, 138);
            this.grpPlc.TabIndex = 1;
            this.grpPlc.TabStop = false;
            this.grpPlc.Text = " PLC 配置 ";
            // 
            // tpPlc
            // 
            this.tpPlc.ColumnCount = 4;
            this.tpPlc.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tpPlc.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tpPlc.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tpPlc.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tpPlc.Controls.Add(this.lblPlcIp, 0, 0);
            this.tpPlc.Controls.Add(this.txtPlcIp, 1, 0);
            this.tpPlc.Controls.Add(this.lblCpuType, 2, 0);
            this.tpPlc.Controls.Add(this.txtCpuType, 3, 0);
            this.tpPlc.Controls.Add(this.lblRack, 0, 1);
            this.tpPlc.Controls.Add(this.numRack, 1, 1);
            this.tpPlc.Controls.Add(this.lblSlot, 2, 1);
            this.tpPlc.Controls.Add(this.numSlot, 3, 1);
            this.tpPlc.Controls.Add(this.lblPollMs, 0, 2);
            this.tpPlc.Controls.Add(this.numPollMs, 1, 2);
            this.tpPlc.Controls.Add(this.chkSimulator, 2, 2);
            this.tpPlc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpPlc.Location = new System.Drawing.Point(4, 29);
            this.tpPlc.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpPlc.Name = "tpPlc";
            this.tpPlc.RowCount = 3;
            this.tpPlc.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tpPlc.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tpPlc.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tpPlc.Size = new System.Drawing.Size(1501, 106);
            this.tpPlc.TabIndex = 0;
            // 
            // lblPlcIp
            // 
            this.lblPlcIp.Location = new System.Drawing.Point(4, 0);
            this.lblPlcIp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPlcIp.Name = "lblPlcIp";
            this.lblPlcIp.Size = new System.Drawing.Size(129, 24);
            this.lblPlcIp.TabIndex = 0;
            this.lblPlcIp.Text = "PLC IP：";
            // 
            // txtPlcIp
            // 
            this.txtPlcIp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPlcIp.Location = new System.Drawing.Point(379, 3);
            this.txtPlcIp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtPlcIp.Name = "txtPlcIp";
            this.txtPlcIp.Size = new System.Drawing.Size(367, 33);
            this.txtPlcIp.TabIndex = 1;
            // 
            // lblCpuType
            // 
            this.lblCpuType.Location = new System.Drawing.Point(754, 0);
            this.lblCpuType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCpuType.Name = "lblCpuType";
            this.lblCpuType.Size = new System.Drawing.Size(129, 24);
            this.lblCpuType.TabIndex = 2;
            this.lblCpuType.Text = "CPU 型号：";
            // 
            // txtCpuType
            // 
            this.txtCpuType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCpuType.Location = new System.Drawing.Point(1129, 3);
            this.txtCpuType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtCpuType.Name = "txtCpuType";
            this.txtCpuType.Size = new System.Drawing.Size(368, 33);
            this.txtCpuType.TabIndex = 3;
            // 
            // lblRack
            // 
            this.lblRack.Location = new System.Drawing.Point(4, 35);
            this.lblRack.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRack.Name = "lblRack";
            this.lblRack.Size = new System.Drawing.Size(129, 24);
            this.lblRack.TabIndex = 4;
            this.lblRack.Text = "机架号：";
            // 
            // numRack
            // 
            this.numRack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numRack.Location = new System.Drawing.Point(379, 38);
            this.numRack.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numRack.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numRack.Name = "numRack";
            this.numRack.Size = new System.Drawing.Size(367, 33);
            this.numRack.TabIndex = 5;
            // 
            // lblSlot
            // 
            this.lblSlot.Location = new System.Drawing.Point(754, 35);
            this.lblSlot.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSlot.Name = "lblSlot";
            this.lblSlot.Size = new System.Drawing.Size(129, 24);
            this.lblSlot.TabIndex = 6;
            this.lblSlot.Text = "槽位号：";
            // 
            // numSlot
            // 
            this.numSlot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numSlot.Location = new System.Drawing.Point(1129, 38);
            this.numSlot.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numSlot.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.numSlot.Name = "numSlot";
            this.numSlot.Size = new System.Drawing.Size(368, 33);
            this.numSlot.TabIndex = 7;
            // 
            // lblPollMs
            // 
            this.lblPollMs.Location = new System.Drawing.Point(4, 70);
            this.lblPollMs.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPollMs.Name = "lblPollMs";
            this.lblPollMs.Size = new System.Drawing.Size(129, 24);
            this.lblPollMs.TabIndex = 8;
            this.lblPollMs.Text = "轮询周期(ms)：";
            // 
            // numPollMs
            // 
            this.numPollMs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numPollMs.Location = new System.Drawing.Point(379, 73);
            this.numPollMs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numPollMs.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numPollMs.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numPollMs.Name = "numPollMs";
            this.numPollMs.Size = new System.Drawing.Size(367, 33);
            this.numPollMs.TabIndex = 9;
            this.numPollMs.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // chkSimulator
            // 
            this.chkSimulator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(90)))));
            this.chkSimulator.Location = new System.Drawing.Point(754, 73);
            this.chkSimulator.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkSimulator.Name = "chkSimulator";
            this.chkSimulator.Size = new System.Drawing.Size(134, 25);
            this.chkSimulator.TabIndex = 10;
            this.chkSimulator.Text = "模拟模式";
            // 
            // grpMes
            // 
            this.grpMes.Controls.Add(this.tpMes);
            this.grpMes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpMes.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpMes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(234)))), ((int)(((byte)(244)))));
            this.grpMes.Location = new System.Drawing.Point(17, 355);
            this.grpMes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpMes.Name = "grpMes";
            this.grpMes.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpMes.Size = new System.Drawing.Size(1509, 138);
            this.grpMes.TabIndex = 2;
            this.grpMes.TabStop = false;
            this.grpMes.Text = " MES 配置 ";
            // 
            // tpMes
            // 
            this.tpMes.ColumnCount = 4;
            this.tpMes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tpMes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tpMes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tpMes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tpMes.Controls.Add(this.lblCheckUrl, 0, 0);
            this.tpMes.Controls.Add(this.txtCheckUrl, 1, 0);
            this.tpMes.Controls.Add(this.lblSyncUrl, 2, 0);
            this.tpMes.Controls.Add(this.txtSyncUrl, 3, 0);
            this.tpMes.Controls.Add(this.lblDeviceCode, 0, 1);
            this.tpMes.Controls.Add(this.txtDeviceCode, 1, 1);
            this.tpMes.Controls.Add(this.lblPassword, 2, 1);
            this.tpMes.Controls.Add(this.txtPassword, 3, 1);
            this.tpMes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpMes.Location = new System.Drawing.Point(4, 29);
            this.tpMes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpMes.Name = "tpMes";
            this.tpMes.RowCount = 2;
            this.tpMes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tpMes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tpMes.Size = new System.Drawing.Size(1501, 106);
            this.tpMes.TabIndex = 0;
            // 
            // lblCheckUrl
            // 
            this.lblCheckUrl.Location = new System.Drawing.Point(4, 0);
            this.lblCheckUrl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCheckUrl.Name = "lblCheckUrl";
            this.lblCheckUrl.Size = new System.Drawing.Size(129, 24);
            this.lblCheckUrl.TabIndex = 0;
            this.lblCheckUrl.Text = "过站校验URL：";
            // 
            // txtCheckUrl
            // 
            this.txtCheckUrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCheckUrl.Location = new System.Drawing.Point(229, 3);
            this.txtCheckUrl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtCheckUrl.Name = "txtCheckUrl";
            this.txtCheckUrl.Size = new System.Drawing.Size(517, 33);
            this.txtCheckUrl.TabIndex = 1;
            // 
            // lblSyncUrl
            // 
            this.lblSyncUrl.Location = new System.Drawing.Point(754, 0);
            this.lblSyncUrl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSyncUrl.Name = "lblSyncUrl";
            this.lblSyncUrl.Size = new System.Drawing.Size(129, 24);
            this.lblSyncUrl.TabIndex = 2;
            this.lblSyncUrl.Text = "过站同步URL：";
            // 
            // txtSyncUrl
            // 
            this.txtSyncUrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSyncUrl.Location = new System.Drawing.Point(979, 3);
            this.txtSyncUrl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtSyncUrl.Name = "txtSyncUrl";
            this.txtSyncUrl.Size = new System.Drawing.Size(518, 33);
            this.txtSyncUrl.TabIndex = 3;
            // 
            // lblDeviceCode
            // 
            this.lblDeviceCode.Location = new System.Drawing.Point(4, 53);
            this.lblDeviceCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDeviceCode.Name = "lblDeviceCode";
            this.lblDeviceCode.Size = new System.Drawing.Size(129, 24);
            this.lblDeviceCode.TabIndex = 4;
            this.lblDeviceCode.Text = "设备编码：";
            // 
            // txtDeviceCode
            // 
            this.txtDeviceCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDeviceCode.Location = new System.Drawing.Point(229, 56);
            this.txtDeviceCode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtDeviceCode.Name = "txtDeviceCode";
            this.txtDeviceCode.Size = new System.Drawing.Size(517, 33);
            this.txtDeviceCode.TabIndex = 5;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(754, 53);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(129, 24);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "工程师密码：";
            // 
            // txtPassword
            // 
            this.txtPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPassword.Location = new System.Drawing.Point(979, 56);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(518, 33);
            this.txtPassword.TabIndex = 7;
            // 
            // btnApply
            // 
            this.btnApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(160)))), ((int)(((byte)(90)))));
            this.btnApply.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnApply.FlatAppearance.BorderSize = 0;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Font = new System.Drawing.Font("Microsoft YaHei UI", 13F, System.Drawing.FontStyle.Bold);
            this.btnApply.ForeColor = System.Drawing.Color.White;
            this.btnApply.Location = new System.Drawing.Point(17, 738);
            this.btnApply.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(1509, 74);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "应用参数（需工程师登录，重启后生效）";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // UcPageSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(26)))), ((int)(((byte)(34)))));
            this.Controls.Add(this.tpRoot);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "UcPageSettings";
            this.Size = new System.Drawing.Size(1543, 826);
            this.tpRoot.ResumeLayout(false);
            this.grpPoints.ResumeLayout(false);
            this.tpPoints.ResumeLayout(false);
            this.tpPoints.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numResultCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAnalogCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCylCount)).EndInit();
            this.grpPlc.ResumeLayout(false);
            this.tpPlc.ResumeLayout(false);
            this.tpPlc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSlot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPollMs)).EndInit();
            this.grpMes.ResumeLayout(false);
            this.tpMes.ResumeLayout(false);
            this.tpMes.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TableLayoutPanel tpRoot;
        private System.Windows.Forms.GroupBox grpPoints;
        private System.Windows.Forms.TableLayoutPanel tpPoints;
        private System.Windows.Forms.Label lblResultCount;
        private System.Windows.Forms.NumericUpDown numResultCount;
        private System.Windows.Forms.Label lblAnalogCount;
        private System.Windows.Forms.NumericUpDown numAnalogCount;
        private System.Windows.Forms.Label lblCylCount;
        private System.Windows.Forms.NumericUpDown numCylCount;
        private System.Windows.Forms.Label lblResultStart;
        private System.Windows.Forms.TextBox txtResultStart;
        private System.Windows.Forms.Label lblMeasureStart;
        private System.Windows.Forms.TextBox txtMeasureStart;
        private System.Windows.Forms.Label lblCylSwitchStart;
        private System.Windows.Forms.TextBox txtCylSwitchStart;
        private System.Windows.Forms.GroupBox grpPlc;
        private System.Windows.Forms.TableLayoutPanel tpPlc;
        private System.Windows.Forms.Label lblPlcIp;
        private System.Windows.Forms.TextBox txtPlcIp;
        private System.Windows.Forms.Label lblCpuType;
        private System.Windows.Forms.TextBox txtCpuType;
        private System.Windows.Forms.Label lblRack;
        private System.Windows.Forms.NumericUpDown numRack;
        private System.Windows.Forms.Label lblSlot;
        private System.Windows.Forms.NumericUpDown numSlot;
        private System.Windows.Forms.Label lblPollMs;
        private System.Windows.Forms.NumericUpDown numPollMs;
        private System.Windows.Forms.CheckBox chkSimulator;
        private System.Windows.Forms.GroupBox grpMes;
        private System.Windows.Forms.TableLayoutPanel tpMes;
        private System.Windows.Forms.Label lblCheckUrl;
        private System.Windows.Forms.TextBox txtCheckUrl;
        private System.Windows.Forms.Label lblSyncUrl;
        private System.Windows.Forms.TextBox txtSyncUrl;
        private System.Windows.Forms.Label lblDeviceCode;
        private System.Windows.Forms.TextBox txtDeviceCode;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnApply;
    }
}
