using GaugeDemo300.Models;
using GaugeDemo300.Services;

namespace GaugeDemo300.Forms
{
    partial class UcPageAlarm
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
            this.tpOps = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnAck = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.dgvAlarm = new System.Windows.Forms.DataGridView();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMsg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAck = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpRoot.SuspendLayout();
            this.tpOps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlarm)).BeginInit();
            this.SuspendLayout();
            //
            // tpRoot
            //
            this.tpRoot.ColumnCount = 1;
            this.tpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpRoot.Controls.Add(this.tpOps, 0, 0);
            this.tpRoot.Controls.Add(this.dgvAlarm, 0, 1);
            this.tpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpRoot.Location = new System.Drawing.Point(0, 0);
            this.tpRoot.Name = "tpRoot";
            this.tpRoot.RowCount = 2;
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpRoot.Size = new System.Drawing.Size(1200, 780);
            this.tpRoot.TabIndex = 0;
            //
            // tpOps
            //
            this.tpOps.ColumnCount = 5;
            this.tpOps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tpOps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tpOps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tpOps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tpOps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tpOps.Controls.Add(this.lblTitle, 0, 0);
            this.tpOps.Controls.Add(this.btnAck, 1, 0);
            this.tpOps.Controls.Add(this.btnClear, 2, 0);
            this.tpOps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpOps.Location = new System.Drawing.Point(3, 3);
            this.tpOps.Name = "tpOps";
            this.tpOps.RowCount = 1;
            this.tpOps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpOps.Size = new System.Drawing.Size(1194, 44);
            this.tpOps.TabIndex = 0;
            //
            // lblTitle
            //
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(234)))), ((int)(((byte)(244)))));
            this.lblTitle.Location = new System.Drawing.Point(3, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(591, 44);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "报警列表（软件 + PLC P_ALARM）";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // btnAck
            //
            this.btnAck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(99)))), ((int)(((byte)(158)))));
            this.btnAck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAck.FlatAppearance.BorderSize = 0;
            this.btnAck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAck.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAck.ForeColor = System.Drawing.Color.White;
            this.btnAck.Location = new System.Drawing.Point(600, 7);
            this.btnAck.Name = "btnAck";
            this.btnAck.Size = new System.Drawing.Size(185, 30);
            this.btnAck.TabIndex = 0;
            this.btnAck.Text = "确认报警";
            this.btnAck.UseVisualStyleBackColor = false;
            this.btnAck.Click += new System.EventHandler(this.btnAck_Click);
            //
            // btnClear
            //
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(66)))), ((int)(((byte)(88)))));
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(791, 7);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(185, 30);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "清空列表";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            //
            // dgvAlarm
            //
            this.dgvAlarm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTime, this.colLevel, this.colSource, this.colMsg, this.colAck});
            this.dgvAlarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAlarm.Location = new System.Drawing.Point(3, 53);
            this.dgvAlarm.Name = "dgvAlarm";
            this.dgvAlarm.Size = new System.Drawing.Size(1194, 724);
            this.dgvAlarm.TabIndex = 1;
            //
            // colTime
            //
            this.colTime.HeaderText = "时间";
            this.colTime.Name = "colTime";
            this.colTime.ReadOnly = true;
            this.colTime.Width = 160;
            //
            // colLevel
            //
            this.colLevel.HeaderText = "级别";
            this.colLevel.Name = "colLevel";
            this.colLevel.ReadOnly = true;
            this.colLevel.Width = 70;
            //
            // colSource
            //
            this.colSource.HeaderText = "来源";
            this.colSource.Name = "colSource";
            this.colSource.ReadOnly = true;
            this.colSource.Width = 100;
            //
            // colMsg
            //
            this.colMsg.HeaderText = "报警内容";
            this.colMsg.Name = "colMsg";
            this.colMsg.ReadOnly = true;
            this.colMsg.Width = 560;
            //
            // colAck
            //
            this.colAck.HeaderText = "已确认";
            this.colAck.Name = "colAck";
            this.colAck.ReadOnly = true;
            this.colAck.Width = 70;
            //
            // UcPageAlarm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(26)))), ((int)(((byte)(34)))));
            this.Controls.Add(this.tpRoot);
            this.Name = "UcPageAlarm";
            this.Size = new System.Drawing.Size(1200, 780);
            this.tpRoot.ResumeLayout(false);
            this.tpOps.ResumeLayout(false);
            this.tpOps.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlarm)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tpRoot;
        private System.Windows.Forms.TableLayoutPanel tpOps;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnAck;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridView dgvAlarm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMsg;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAck;
    }
}
