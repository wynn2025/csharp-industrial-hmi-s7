using GaugeDemo300.Models;
using GaugeDemo300.Services;

namespace GaugeDemo300.Forms
{
    partial class UcPageHistory
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        private void InitializeComponent()
        {
            this.tpRoot = new System.Windows.Forms.TableLayoutPanel();
            this.tpFilter = new System.Windows.Forms.TableLayoutPanel();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.txtHistCode = new System.Windows.Forms.TextBox();
            this.cmbHistResult = new System.Windows.Forms.ComboBox();
            this.btnHistQuery = new System.Windows.Forms.Button();
            this.dgvHistory = new System.Windows.Forms.DataGridView();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOk = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSync = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDisp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpRoot.SuspendLayout();
            this.tpFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            this.SuspendLayout();
            //
            // tpRoot
            //
            this.tpRoot.ColumnCount = 1;
            this.tpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpRoot.Controls.Add(this.tpFilter, 0, 0);
            this.tpRoot.Controls.Add(this.dgvHistory, 0, 1);
            this.tpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpRoot.Location = new System.Drawing.Point(0, 0);
            this.tpRoot.Name = "tpRoot";
            this.tpRoot.RowCount = 2;
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpRoot.Size = new System.Drawing.Size(1200, 780);
            this.tpRoot.TabIndex = 0;
            //
            // tpFilter
            //
            this.tpFilter.ColumnCount = 6;
            this.tpFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tpFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tpFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24F));
            this.tpFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tpFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tpFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tpFilter.Controls.Add(this.dtpFrom, 0, 0);
            this.tpFilter.Controls.Add(this.dtpTo, 1, 0);
            this.tpFilter.Controls.Add(this.txtHistCode, 2, 0);
            this.tpFilter.Controls.Add(this.cmbHistResult, 3, 0);
            this.tpFilter.Controls.Add(this.btnHistQuery, 5, 0);
            this.tpFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpFilter.Location = new System.Drawing.Point(3, 3);
            this.tpFilter.Name = "tpFilter";
            this.tpFilter.RowCount = 1;
            this.tpFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpFilter.Size = new System.Drawing.Size(1194, 42);
            this.tpFilter.TabIndex = 0;
            //
            // dtpFrom
            //
            this.dtpFrom.CustomFormat = "yyyy-MM-dd";
            this.dtpFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpFrom.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(3, 9);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(232, 25);
            this.dtpFrom.TabIndex = 0;
            //
            // dtpTo
            //
            this.dtpTo.CustomFormat = "yyyy-MM-dd";
            this.dtpTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpTo.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(241, 9);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(232, 25);
            this.dtpTo.TabIndex = 1;
            //
            // txtHistCode
            //
            this.txtHistCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHistCode.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.txtHistCode.Location = new System.Drawing.Point(479, 11);
            this.txtHistCode.Name = "txtHistCode";
            this.txtHistCode.Size = new System.Drawing.Size(280, 23);
            this.txtHistCode.TabIndex = 2;
            //
            // cmbHistResult
            //
            this.cmbHistResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbHistResult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHistResult.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.cmbHistResult.Items.AddRange(new object[] { "全部", "OK", "NG" });
            this.cmbHistResult.Location = new System.Drawing.Point(765, 11);
            this.cmbHistResult.Name = "cmbHistResult";
            this.cmbHistResult.Size = new System.Drawing.Size(137, 25);
            this.cmbHistResult.TabIndex = 3;
            //
            // btnHistQuery
            //
            this.btnHistQuery.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(99)))), ((int)(((byte)(158)))));
            this.btnHistQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHistQuery.FlatAppearance.BorderSize = 0;
            this.btnHistQuery.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHistQuery.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnHistQuery.ForeColor = System.Drawing.Color.White;
            this.btnHistQuery.Location = new System.Drawing.Point(1052, 6);
            this.btnHistQuery.Name = "btnHistQuery";
            this.btnHistQuery.Size = new System.Drawing.Size(139, 30);
            this.btnHistQuery.TabIndex = 4;
            this.btnHistQuery.Text = "查 询";
            this.btnHistQuery.UseVisualStyleBackColor = false;
            this.btnHistQuery.Click += new System.EventHandler(this.btnHistQuery_Click);
            //
            // dgvHistory
            //
            this.dgvHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTime, this.colCode, this.colModel, this.colResult, this.colOk, this.colNg, this.colDisp, this.colSync});
            this.dgvHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHistory.Location = new System.Drawing.Point(3, 51);
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.Size = new System.Drawing.Size(1194, 726);
            this.dgvHistory.TabIndex = 1;
            this.dgvHistory.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHistory_CellDoubleClick);
            //
            // colTime
            //
            this.colTime.HeaderText = "过站时间";
            this.colTime.Name = "colTime";
            this.colTime.ReadOnly = true;
            this.colTime.Width = 150;
            //
            // colCode
            //
            this.colCode.HeaderText = "产品码";
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            this.colCode.Width = 220;
            //
            // colModel
            //
            this.colModel.HeaderText = "型号";
            this.colModel.Name = "colModel";
            this.colModel.ReadOnly = true;
            this.colModel.Width = 110;
            //
            // colResult
            //
            this.colResult.HeaderText = "结果";
            this.colResult.Name = "colResult";
            this.colResult.ReadOnly = true;
            this.colResult.Width = 70;
            //
            // colOk
            //
            this.colOk.HeaderText = "OK点数";
            this.colOk.Name = "colOk";
            this.colOk.ReadOnly = true;
            this.colOk.Width = 90;
            //
            // colNg
            //
            this.colNg.HeaderText = "NG点数";
            this.colNg.Name = "colNg";
            this.colNg.ReadOnly = true;
            this.colNg.Width = 90;
            //
            // colDisp
            //
            this.colDisp.HeaderText = "位移值(前3)·双击行看全部明细";
            this.colDisp.Name = "colDisp";
            this.colDisp.Width = 200;
            //
            // colSync
            //
            this.colSync.HeaderText = "MES同步";
            this.colSync.Name = "colSync";
            this.colSync.ReadOnly = true;
            this.colSync.Width = 80;
            //
            // UcPageHistory
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(26)))), ((int)(((byte)(34)))));
            this.Controls.Add(this.tpRoot);
            this.Name = "UcPageHistory";
            this.Size = new System.Drawing.Size(1200, 780);
            this.tpRoot.ResumeLayout(false);
            this.tpFilter.ResumeLayout(false);
            this.tpFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tpRoot;
        private System.Windows.Forms.TableLayoutPanel tpFilter;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.TextBox txtHistCode;
        private System.Windows.Forms.ComboBox cmbHistResult;
        private System.Windows.Forms.Button btnHistQuery;
        private System.Windows.Forms.DataGridView dgvHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOk;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNg;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSync;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDisp;
    }
}
