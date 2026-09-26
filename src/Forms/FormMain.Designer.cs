namespace GaugeDemo300
{
    partial class FormMain
    {
        /// <summary>必需的设计器变量。</summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>清理所有正在使用的资源。</summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tpRoot = new System.Windows.Forms.TableLayoutPanel();
            this.tpTop = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlHeartLed = new System.Windows.Forms.Panel();
            this.lblHeartbeat = new System.Windows.Forms.Label();
            this.lblTopProductName = new System.Windows.Forms.Label();
            this.lblTopProductCode = new System.Windows.Forms.Label();
            this.lblTopOperator = new System.Windows.Forms.Label();
            this.lblTopTime = new System.Windows.Forms.Label();
            this.tpBody = new System.Windows.Forms.TableLayoutPanel();
            this.panelStage = new System.Windows.Forms.Panel();
            this.tpRight = new System.Windows.Forms.TableLayoutPanel();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblModeDisplay = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnOnline = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.tpTabs = new System.Windows.Forms.TableLayoutPanel();
            this.tabMain = new System.Windows.Forms.Button();
            this.tabHistory = new System.Windows.Forms.Button();
            this.tabManual = new System.Windows.Forms.Button();
            this.tabResult = new System.Windows.Forms.Button();
            this.tabAlarm = new System.Windows.Forms.Button();
            this.tabModel = new System.Windows.Forms.Button();
            this.tabMes = new System.Windows.Forms.Button();
            this.tabSettings = new System.Windows.Forms.Button();
            this.lblTopLog = new System.Windows.Forms.Label();
            this.timerUi = new System.Windows.Forms.Timer(this.components);
            this.labelResult = new System.Windows.Forms.Label();
            this.tpRoot.SuspendLayout();
            this.tpTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlHeartLed.SuspendLayout();
            this.tpBody.SuspendLayout();
            this.tpRight.SuspendLayout();
            this.tpTabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpRoot
            // 
            this.tpRoot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(26)))), ((int)(((byte)(34)))));
            this.tpRoot.ColumnCount = 1;
            this.tpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpRoot.Controls.Add(this.tpTop, 0, 0);
            this.tpRoot.Controls.Add(this.tpBody, 0, 1);
            this.tpRoot.Controls.Add(this.tpTabs, 0, 2);
            this.tpRoot.Controls.Add(this.lblTopLog, 0, 3);
            this.tpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpRoot.Location = new System.Drawing.Point(0, 0);
            this.tpRoot.Name = "tpRoot";
            this.tpRoot.RowCount = 4;
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 104F));
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tpRoot.Size = new System.Drawing.Size(1912, 1191);
            this.tpRoot.TabIndex = 0;
            // 
            // tpTop
            // 
            this.tpTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(32)))), ((int)(((byte)(44)))));
            this.tpTop.ColumnCount = 6;
            this.tpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24F));
            this.tpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tpTop.Controls.Add(this.pictureBox1, 0, 0);
            this.tpTop.Controls.Add(this.pnlHeartLed, 1, 0);
            this.tpTop.Controls.Add(this.lblTopProductName, 2, 0);
            this.tpTop.Controls.Add(this.lblTopProductCode, 3, 0);
            this.tpTop.Controls.Add(this.lblTopOperator, 4, 0);
            this.tpTop.Controls.Add(this.lblTopTime, 5, 0);
            this.tpTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpTop.Location = new System.Drawing.Point(3, 3);
            this.tpTop.Name = "tpTop";
            this.tpTop.RowCount = 1;
            this.tpTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpTop.Size = new System.Drawing.Size(1906, 101);
            this.tpTop.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(322, 95);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // pnlHeartLed
            // 
            this.pnlHeartLed.Controls.Add(this.lblHeartbeat);
            this.pnlHeartLed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeartLed.Location = new System.Drawing.Point(331, 3);
            this.pnlHeartLed.Name = "pnlHeartLed";
            this.pnlHeartLed.Size = new System.Drawing.Size(74, 95);
            this.pnlHeartLed.TabIndex = 1;
            this.pnlHeartLed.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHeartLed_Paint);
            // 
            // lblHeartbeat
            // 
            this.lblHeartbeat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeartbeat.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblHeartbeat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(234)))), ((int)(((byte)(244)))));
            this.lblHeartbeat.Location = new System.Drawing.Point(0, 0);
            this.lblHeartbeat.Name = "lblHeartbeat";
            this.lblHeartbeat.Size = new System.Drawing.Size(74, 95);
            this.lblHeartbeat.TabIndex = 0;
            this.lblHeartbeat.Text = "PLC";
            this.lblHeartbeat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTopProductName
            // 
            this.lblTopProductName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTopProductName.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblTopProductName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(234)))), ((int)(((byte)(244)))));
            this.lblTopProductName.Location = new System.Drawing.Point(411, 0);
            this.lblTopProductName.Name = "lblTopProductName";
            this.lblTopProductName.Size = new System.Drawing.Size(359, 101);
            this.lblTopProductName.TabIndex = 2;
            this.lblTopProductName.Text = "产品名称：--";
            this.lblTopProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTopProductCode
            // 
            this.lblTopProductCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTopProductCode.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblTopProductCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(234)))), ((int)(((byte)(244)))));
            this.lblTopProductCode.Location = new System.Drawing.Point(776, 0);
            this.lblTopProductCode.Name = "lblTopProductCode";
            this.lblTopProductCode.Size = new System.Drawing.Size(432, 101);
            this.lblTopProductCode.TabIndex = 3;
            this.lblTopProductCode.Text = "产品编号：--";
            this.lblTopProductCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTopOperator
            // 
            this.lblTopOperator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTopOperator.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblTopOperator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(234)))), ((int)(((byte)(244)))));
            this.lblTopOperator.Location = new System.Drawing.Point(1214, 0);
            this.lblTopOperator.Name = "lblTopOperator";
            this.lblTopOperator.Size = new System.Drawing.Size(359, 101);
            this.lblTopOperator.TabIndex = 4;
            this.lblTopOperator.Text = "操作人员：--";
            this.lblTopOperator.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTopTime
            // 
            this.lblTopTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTopTime.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblTopTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(155)))), ((int)(((byte)(180)))));
            this.lblTopTime.Location = new System.Drawing.Point(1579, 0);
            this.lblTopTime.Name = "lblTopTime";
            this.lblTopTime.Size = new System.Drawing.Size(324, 101);
            this.lblTopTime.TabIndex = 6;
            this.lblTopTime.Text = "--:--:--";
            this.lblTopTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tpBody
            // 
            this.tpBody.ColumnCount = 2;
            this.tpBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 190F));
            this.tpBody.Controls.Add(this.panelStage, 0, 0);
            this.tpBody.Controls.Add(this.tpRight, 1, 0);
            this.tpBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpBody.Location = new System.Drawing.Point(3, 110);
            this.tpBody.Name = "tpBody";
            this.tpBody.RowCount = 1;
            this.tpBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpBody.Size = new System.Drawing.Size(1906, 944);
            this.tpBody.TabIndex = 1;
            // 
            // panelStage
            // 
            this.panelStage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(26)))), ((int)(((byte)(34)))));
            this.panelStage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStage.Location = new System.Drawing.Point(0, 0);
            this.panelStage.Margin = new System.Windows.Forms.Padding(0);
            this.panelStage.Name = "panelStage";
            this.panelStage.Size = new System.Drawing.Size(1716, 944);
            this.panelStage.TabIndex = 0;
            // 
            // tpRight
            // 
            this.tpRight.ColumnCount = 1;
            this.tpRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpRight.Controls.Add(this.labelResult, 0, 0);
            this.tpRight.Controls.Add(this.btnLogin, 0, 1);
            this.tpRight.Controls.Add(this.lblModeDisplay, 0, 2);
            this.tpRight.Controls.Add(this.btnReset, 0, 3);
            this.tpRight.Controls.Add(this.btnOnline, 0, 4);
            this.tpRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpRight.Location = new System.Drawing.Point(1719, 3);
            this.tpRight.Name = "tpRight";
            this.tpRight.RowCount = 6;
            this.tpRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.67F));
            this.tpRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.67F));
            this.tpRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.67F));
            this.tpRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.67F));
            this.tpRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.67F));
            this.tpRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.67F));
            this.tpRight.Size = new System.Drawing.Size(184, 938);
            this.tpRight.TabIndex = 1;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(66)))), ((int)(((byte)(88)))));
            this.btnLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(3, 159);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(178, 150);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lblModeDisplay
            // 
            this.lblModeDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(66)))), ((int)(((byte)(88)))));
            this.lblModeDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblModeDisplay.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblModeDisplay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(90)))));
            this.lblModeDisplay.Location = new System.Drawing.Point(3, 312);
            this.lblModeDisplay.Name = "lblModeDisplay";
            this.lblModeDisplay.Size = new System.Drawing.Size(178, 156);
            this.lblModeDisplay.TabIndex = 9;
            this.lblModeDisplay.Text = "自动模式";
            this.lblModeDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(66)))), ((int)(((byte)(88)))));
            this.btnReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReset.FlatAppearance.BorderSize = 0;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(3, 471);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(178, 150);
            this.btnReset.TabIndex = 6;
            this.btnReset.Text = "复位";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnOnline
            // 
            this.btnOnline.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(160)))), ((int)(((byte)(90)))));
            this.btnOnline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOnline.FlatAppearance.BorderSize = 0;
            this.btnOnline.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOnline.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnOnline.ForeColor = System.Drawing.Color.White;
            this.btnOnline.Location = new System.Drawing.Point(3, 627);
            this.btnOnline.Name = "btnOnline";
            this.btnOnline.Size = new System.Drawing.Size(178, 150);
            this.btnOnline.TabIndex = 7;
            this.btnOnline.Text = "在线";
            this.btnOnline.UseVisualStyleBackColor = false;
            this.btnOnline.Click += new System.EventHandler(this.btnOnline_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(66)))), ((int)(((byte)(88)))));
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.btnExit.Location = new System.Drawing.Point(1691, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(212, 92);
            this.btnExit.TabIndex = 8;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // tpTabs
            // 
            this.tpTabs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(32)))), ((int)(((byte)(44)))));
            this.tpTabs.ColumnCount = 9;
            this.tpTabs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tpTabs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tpTabs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tpTabs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tpTabs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tpTabs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tpTabs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tpTabs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tpTabs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tpTabs.Controls.Add(this.tabMain, 0, 0);
            this.tpTabs.Controls.Add(this.tabHistory, 1, 0);
            this.tpTabs.Controls.Add(this.tabManual, 2, 0);
            this.tpTabs.Controls.Add(this.tabResult, 3, 0);
            this.tpTabs.Controls.Add(this.tabAlarm, 4, 0);
            this.tpTabs.Controls.Add(this.btnExit, 8, 0);
            this.tpTabs.Controls.Add(this.tabModel, 5, 0);
            this.tpTabs.Controls.Add(this.tabMes, 6, 0);
            this.tpTabs.Controls.Add(this.tabSettings, 7, 0);
            this.tpTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpTabs.Location = new System.Drawing.Point(3, 1060);
            this.tpTabs.Name = "tpTabs";
            this.tpTabs.RowCount = 1;
            this.tpTabs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpTabs.Size = new System.Drawing.Size(1906, 98);
            this.tpTabs.TabIndex = 2;
            // 
            // tabMain
            // 
            this.tabMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(99)))), ((int)(((byte)(158)))));
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.FlatAppearance.BorderSize = 0;
            this.tabMain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tabMain.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.tabMain.ForeColor = System.Drawing.Color.White;
            this.tabMain.Location = new System.Drawing.Point(3, 3);
            this.tabMain.Name = "tabMain";
            this.tabMain.Size = new System.Drawing.Size(205, 92);
            this.tabMain.TabIndex = 0;
            this.tabMain.Text = "主画面";
            this.tabMain.UseVisualStyleBackColor = false;
            this.tabMain.Click += new System.EventHandler(this.tabMain_Click);
            // 
            // tabHistory
            // 
            this.tabHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(66)))), ((int)(((byte)(88)))));
            this.tabHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabHistory.FlatAppearance.BorderSize = 0;
            this.tabHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tabHistory.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.tabHistory.ForeColor = System.Drawing.Color.White;
            this.tabHistory.Location = new System.Drawing.Point(214, 3);
            this.tabHistory.Name = "tabHistory";
            this.tabHistory.Size = new System.Drawing.Size(205, 92);
            this.tabHistory.TabIndex = 1;
            this.tabHistory.Text = "历史记录";
            this.tabHistory.UseVisualStyleBackColor = false;
            this.tabHistory.Click += new System.EventHandler(this.tabHistory_Click);
            // 
            // tabManual
            // 
            this.tabManual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(66)))), ((int)(((byte)(88)))));
            this.tabManual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabManual.FlatAppearance.BorderSize = 0;
            this.tabManual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tabManual.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.tabManual.ForeColor = System.Drawing.Color.White;
            this.tabManual.Location = new System.Drawing.Point(425, 3);
            this.tabManual.Name = "tabManual";
            this.tabManual.Size = new System.Drawing.Size(205, 92);
            this.tabManual.TabIndex = 2;
            this.tabManual.Text = "手动画面";
            this.tabManual.UseVisualStyleBackColor = false;
            this.tabManual.Click += new System.EventHandler(this.tabManual_Click);
            // 
            // tabResult
            // 
            this.tabResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(66)))), ((int)(((byte)(88)))));
            this.tabResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabResult.FlatAppearance.BorderSize = 0;
            this.tabResult.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tabResult.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.tabResult.ForeColor = System.Drawing.Color.White;
            this.tabResult.Location = new System.Drawing.Point(636, 3);
            this.tabResult.Name = "tabResult";
            this.tabResult.Size = new System.Drawing.Size(205, 92);
            this.tabResult.TabIndex = 4;
            this.tabResult.Text = "测量结果";
            this.tabResult.UseVisualStyleBackColor = false;
            this.tabResult.Click += new System.EventHandler(this.tabResult_Click);
            // 
            // tabAlarm
            // 
            this.tabAlarm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(66)))), ((int)(((byte)(88)))));
            this.tabAlarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabAlarm.FlatAppearance.BorderSize = 0;
            this.tabAlarm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tabAlarm.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.tabAlarm.ForeColor = System.Drawing.Color.White;
            this.tabAlarm.Location = new System.Drawing.Point(847, 3);
            this.tabAlarm.Name = "tabAlarm";
            this.tabAlarm.Size = new System.Drawing.Size(205, 92);
            this.tabAlarm.TabIndex = 5;
            this.tabAlarm.Text = "报警画面";
            this.tabAlarm.UseVisualStyleBackColor = false;
            this.tabAlarm.Click += new System.EventHandler(this.tabAlarm_Click);
            // 
            // tabModel
            // 
            this.tabModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(66)))), ((int)(((byte)(88)))));
            this.tabModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabModel.FlatAppearance.BorderSize = 0;
            this.tabModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tabModel.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.tabModel.ForeColor = System.Drawing.Color.White;
            this.tabModel.Location = new System.Drawing.Point(1058, 3);
            this.tabModel.Name = "tabModel";
            this.tabModel.Size = new System.Drawing.Size(205, 92);
            this.tabModel.TabIndex = 6;
            this.tabModel.Text = "产品型号";
            this.tabModel.UseVisualStyleBackColor = false;
            this.tabModel.Click += new System.EventHandler(this.tabModel_Click);
            // 
            // tabMes
            // 
            this.tabMes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(66)))), ((int)(((byte)(88)))));
            this.tabMes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMes.FlatAppearance.BorderSize = 0;
            this.tabMes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tabMes.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.tabMes.ForeColor = System.Drawing.Color.White;
            this.tabMes.Location = new System.Drawing.Point(1269, 3);
            this.tabMes.Name = "tabMes";
            this.tabMes.Size = new System.Drawing.Size(205, 92);
            this.tabMes.TabIndex = 7;
            this.tabMes.Text = "MES通讯";
            this.tabMes.UseVisualStyleBackColor = false;
            this.tabMes.Click += new System.EventHandler(this.tabMes_Click);
            // 
            // tabSettings
            // 
            this.tabSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(66)))), ((int)(((byte)(88)))));
            this.tabSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSettings.FlatAppearance.BorderSize = 0;
            this.tabSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tabSettings.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.tabSettings.ForeColor = System.Drawing.Color.White;
            this.tabSettings.Location = new System.Drawing.Point(1480, 3);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Size = new System.Drawing.Size(205, 92);
            this.tabSettings.TabIndex = 8;
            this.tabSettings.Text = "参数设置";
            this.tabSettings.UseVisualStyleBackColor = false;
            this.tabSettings.Click += new System.EventHandler(this.tabSettings_Click);
            // 
            // lblTopLog
            // 
            this.lblTopLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(32)))), ((int)(((byte)(44)))));
            this.lblTopLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTopLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.lblTopLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(155)))), ((int)(((byte)(180)))));
            this.lblTopLog.Location = new System.Drawing.Point(3, 1161);
            this.lblTopLog.Name = "lblTopLog";
            this.lblTopLog.Size = new System.Drawing.Size(1906, 30);
            this.lblTopLog.TabIndex = 3;
            this.lblTopLog.Text = "日志";
            this.lblTopLog.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timerUi
            // 
            this.timerUi.Interval = 1000;
            this.timerUi.Tick += new System.EventHandler(this.timerUi_Tick);
            // 
            // labelResult
            // 
            this.labelResult.BackColor = System.Drawing.Color.Lime;
            this.labelResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelResult.Font = new System.Drawing.Font("Microsoft YaHei UI", 42F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelResult.ForeColor = System.Drawing.Color.White;
            this.labelResult.Location = new System.Drawing.Point(3, 0);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(178, 156);
            this.labelResult.TabIndex = 10;
            this.labelResult.Text = "OK";
            this.labelResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(26)))), ((int)(((byte)(34)))));
            this.ClientSize = new System.Drawing.Size(1912, 1191);
            this.Controls.Add(this.tpRoot);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.KeyPreview = true;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "电子检具上位机（300点）";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown);
            this.tpRoot.ResumeLayout(false);
            this.tpTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlHeartLed.ResumeLayout(false);
            this.tpBody.ResumeLayout(false);
            this.tpRight.ResumeLayout(false);
            this.tpTabs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tpRoot;
        private System.Windows.Forms.TableLayoutPanel tpTop;
        private System.Windows.Forms.Label lblTopLog;
        private System.Windows.Forms.Panel pnlHeartLed;
        private System.Windows.Forms.Label lblHeartbeat;
        private System.Windows.Forms.Label lblTopProductName;
        private System.Windows.Forms.Label lblTopProductCode;
        private System.Windows.Forms.Label lblTopOperator;
        private System.Windows.Forms.Label lblTopTime;
        private System.Windows.Forms.TableLayoutPanel tpBody;
        private System.Windows.Forms.Panel panelStage;
        private System.Windows.Forms.TableLayoutPanel tpRight;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblModeDisplay;
                                        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnOnline;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TableLayoutPanel tpTabs;
        private System.Windows.Forms.Button tabMain;
        private System.Windows.Forms.Button tabHistory;
        private System.Windows.Forms.Button tabManual;
        private System.Windows.Forms.Button tabResult;
        private System.Windows.Forms.Button tabAlarm;
        private System.Windows.Forms.Button tabModel;
        private System.Windows.Forms.Button tabMes;
        private System.Windows.Forms.Button tabSettings;
        private System.Windows.Forms.Timer timerUi;

        // 画面用户控件（运行时在 FormMain.cs 构造中创建并挂入 panelStage，各画面设计器单独可编辑）
        private Forms.UcPageMain ucMain;
        private Forms.UcPageHistory ucHistory;
        private Forms.UcPageManual ucManual;
        private Forms.UcPageResult ucResult;
        private Forms.UcPageAlarm ucAlarm;
        private Forms.UcPageModel ucModel;
        private Forms.UcPageMes ucMes;
        private Forms.UcPageSettings ucSettings;
        private PictureBox pictureBox1;
        private Label labelResult;
    }
}
