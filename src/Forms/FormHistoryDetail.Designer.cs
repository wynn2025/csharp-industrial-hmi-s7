namespace GaugeDemo300.Forms
{
    partial class FormHistoryDetail
    {
        /// <summary>必需的设计器变量。</summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>清理所有正在使用的资源。</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>设计器支持所需的方法</summary>
        private void InitializeComponent()
        {
            this.lblSummary = new System.Windows.Forms.Label();
            this.dgvPoints = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.colNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUpper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLower = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPoints)).BeginInit();
            this.SuspendLayout();
            //
            // lblSummary
            //
            this.lblSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSummary.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(234)))), ((int)(((byte)(244)))));
            this.lblSummary.Location = new System.Drawing.Point(12, 9);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(1060, 30);
            this.lblSummary.TabIndex = 0;
            this.lblSummary.Text = "记录概要";
            this.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // dgvPoints
            //
            this.dgvPoints.AllowUserToAddRows = false;
            this.dgvPoints.AllowUserToDeleteRows = false;
            this.dgvPoints.AllowUserToResizeRows = false;
            this.dgvPoints.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(26)))), ((int)(((byte)(32)))));
            this.dgvPoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPoints.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNo,
            this.colName,
            this.colType,
            this.colValue,
            this.colUpper,
            this.colLower,
            this.colResult,
            this.colDesc});
            this.dgvPoints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPoints.Location = new System.Drawing.Point(12, 42);
            this.dgvPoints.MultiSelect = false;
            this.dgvPoints.Name = "dgvPoints";
            this.dgvPoints.ReadOnly = true;
            this.dgvPoints.RowHeadersVisible = false;
            this.dgvPoints.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPoints.Size = new System.Drawing.Size(1060, 480);
            this.dgvPoints.TabIndex = 1;
            //
            // colNo
            //
            this.colNo.HeaderText = "序号";
            this.colNo.Name = "colNo";
            this.colNo.Width = 50;
            //
            // colName
            //
            this.colName.HeaderText = "点位";
            this.colName.Name = "colName";
            this.colName.Width = 80;
            //
            // colType
            //
            this.colType.HeaderText = "类型";
            this.colType.Name = "colType";
            this.colType.Width = 60;
            //
            // colValue
            //
            this.colValue.HeaderText = "测量值";
            this.colValue.Name = "colValue";
            this.colValue.Width = 90;
            //
            // colUpper
            //
            this.colUpper.HeaderText = "上限";
            this.colUpper.Name = "colUpper";
            this.colUpper.Width = 80;
            //
            // colLower
            //
            this.colLower.HeaderText = "下限";
            this.colLower.Name = "colLower";
            this.colLower.Width = 80;
            //
            // colResult
            //
            this.colResult.HeaderText = "结果";
            this.colResult.Name = "colResult";
            this.colResult.Width = 60;
            //
            // colDesc
            //
            this.colDesc.HeaderText = "描述";
            this.colDesc.Name = "colDesc";
            this.colDesc.Width = 240;
            //
            // btnClose
            //
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(990, 530);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(82, 32);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // FormHistoryDetail
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(26)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(1084, 571);
            this.Controls.Add(this.dgvPoints);
            this.Controls.Add(this.lblSummary);
            this.Controls.Add(this.btnClose);
            this.Name = "FormHistoryDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "检测明细";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPoints)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.DataGridView dgvPoints;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUpper;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLower;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDesc;
    }
}
