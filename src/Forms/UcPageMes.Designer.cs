using GaugeDemo300.Models;
using GaugeDemo300.Services;

namespace GaugeDemo300.Forms
{
    partial class UcPageMes
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
            this.tpTop = new System.Windows.Forms.TableLayoutPanel();
            this.lblMesConfig = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnRetry = new System.Windows.Forms.Button();
            this.lblMesState = new System.Windows.Forms.Label();
            this.lblCheckTitle = new System.Windows.Forms.Label();
            this.dgvCheck = new System.Windows.Forms.DataGridView();
            this.colCheckTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCheckCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCheckRet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCheckMsg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblQueueTitle = new System.Windows.Forms.Label();
            this.dgvQueue = new System.Windows.Forms.DataGridView();
            this.colQueueTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQueueCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQueueStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQueueRetry = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQueueErr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpRoot.SuspendLayout();
            this.tpTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).BeginInit();
            this.SuspendLayout();
            //
            // tpRoot
            //
            this.tpRoot.ColumnCount = 1;
            this.tpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpRoot.Controls.Add(this.tpTop, 0, 0);
            this.tpRoot.Controls.Add(this.lblCheckTitle, 0, 1);
            this.tpRoot.Controls.Add(this.dgvCheck, 0, 2);
            this.tpRoot.Controls.Add(this.lblQueueTitle, 0, 3);
            this.tpRoot.Controls.Add(this.dgvQueue, 0, 4);
            this.tpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpRoot.Location = new System.Drawing.Point(0, 0);
            this.tpRoot.Name = "tpRoot";
            this.tpRoot.RowCount = 5;
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tpRoot.Size = new System.Drawing.Size(1200, 780);
            this.tpRoot.TabIndex = 0;
            //
            // tpTop
            //
            this.tpTop.ColumnCount = 4;
            this.tpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tpTop.Controls.Add(this.lblMesConfig, 0, 0);
            this.tpTop.Controls.Add(this.btnTest, 1, 0);
            this.tpTop.Controls.Add(this.btnRetry, 2, 0);
            this.tpTop.Controls.Add(this.lblMesState, 3, 0);
            this.tpTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpTop.Location = new System.Drawing.Point(3, 3);
            this.tpTop.Name = "tpTop";
            this.tpTop.RowCount = 1;
            this.tpTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpTop.Size = new System.Drawing.Size(1194, 78);
            this.tpTop.TabIndex = 0;
            //
            // lblMesConfig
            //
            this.lblMesConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMesConfig.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblMesConfig.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(155)))), ((int)(((byte)(180)))));
            this.lblMesConfig.Location = new System.Drawing.Point(3, 0);
            this.lblMesConfig.Name = "lblMesConfig";
            this.lblMesConfig.Size = new System.Drawing.Size(651, 78);
            this.lblMesConfig.TabIndex = 0;
            this.lblMesConfig.Text = "MES：--";
            this.lblMesConfig.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // btnTest
            //
            this.btnTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(99)))), ((int)(((byte)(158)))));
            this.btnTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTest.FlatAppearance.BorderSize = 0;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTest.ForeColor = System.Drawing.Color.White;
            this.btnTest.Location = new System.Drawing.Point(660, 24);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(173, 31);
            this.btnTest.TabIndex = 1;
            this.btnTest.Text = "过站校验测试";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            //
            // btnRetry
            //
            this.btnRetry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(99)))), ((int)(((byte)(158)))));
            this.btnRetry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRetry.FlatAppearance.BorderSize = 0;
            this.btnRetry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRetry.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRetry.ForeColor = System.Drawing.Color.White;
            this.btnRetry.Location = new System.Drawing.Point(839, 24);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Size = new System.Drawing.Size(173, 31);
            this.btnRetry.TabIndex = 2;
            this.btnRetry.Text = "手动重传队列";
            this.btnRetry.UseVisualStyleBackColor = false;
            this.btnRetry.Click += new System.EventHandler(this.btnRetry_Click);
            //
            // lblMesState
            //
            this.lblMesState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMesState.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblMesState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(200)))), ((int)(((byte)(118)))));
            this.lblMesState.Location = new System.Drawing.Point(1018, 0);
            this.lblMesState.Name = "lblMesState";
            this.lblMesState.Size = new System.Drawing.Size(173, 78);
            this.lblMesState.TabIndex = 3;
            this.lblMesState.Text = "状态：--";
            this.lblMesState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblCheckTitle
            //
            this.lblCheckTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCheckTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCheckTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(234)))), ((int)(((byte)(244)))));
            this.lblCheckTitle.Location = new System.Drawing.Point(3, 87);
            this.lblCheckTitle.Name = "lblCheckTitle";
            this.lblCheckTitle.Size = new System.Drawing.Size(1194, 30);
            this.lblCheckTitle.TabIndex = 1;
            this.lblCheckTitle.Text = "过站校验记录（近 3 天）";
            this.lblCheckTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            //
            // dgvCheck
            //
            this.dgvCheck.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCheckTime, this.colCheckCode, this.colCheckRet, this.colCheckMsg});
            this.dgvCheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCheck.Location = new System.Drawing.Point(3, 123);
            this.dgvCheck.Name = "dgvCheck";
            this.dgvCheck.Size = new System.Drawing.Size(1194, 296);
            this.dgvCheck.TabIndex = 2;
            //
            // colCheckTime
            //
            this.colCheckTime.HeaderText = "校验时间";
            this.colCheckTime.Name = "colCheckTime";
            this.colCheckTime.ReadOnly = true;
            this.colCheckTime.Width = 150;
            //
            // colCheckCode
            //
            this.colCheckCode.HeaderText = "产品码";
            this.colCheckCode.Name = "colCheckCode";
            this.colCheckCode.ReadOnly = true;
            this.colCheckCode.Width = 250;
            //
            // colCheckRet
            //
            this.colCheckRet.HeaderText = "code";
            this.colCheckRet.Name = "colCheckRet";
            this.colCheckRet.ReadOnly = true;
            this.colCheckRet.Width = 70;
            //
            // colCheckMsg
            //
            this.colCheckMsg.HeaderText = "返回信息";
            this.colCheckMsg.Name = "colCheckMsg";
            this.colCheckMsg.ReadOnly = true;
            this.colCheckMsg.Width = 500;
            //
            // lblQueueTitle
            //
            this.lblQueueTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblQueueTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblQueueTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(234)))), ((int)(((byte)(244)))));
            this.lblQueueTitle.Location = new System.Drawing.Point(3, 425);
            this.lblQueueTitle.Name = "lblQueueTitle";
            this.lblQueueTitle.Size = new System.Drawing.Size(1194, 30);
            this.lblQueueTitle.TabIndex = 3;
            this.lblQueueTitle.Text = "同步失败队列（待重传）";
            this.lblQueueTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            //
            // dgvQueue
            //
            this.dgvQueue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colQueueTime, this.colQueueCode, this.colQueueStatus, this.colQueueRetry, this.colQueueErr});
            this.dgvQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvQueue.Location = new System.Drawing.Point(3, 461);
            this.dgvQueue.Name = "dgvQueue";
            this.dgvQueue.Size = new System.Drawing.Size(1194, 316);
            this.dgvQueue.TabIndex = 4;
            //
            // colQueueTime
            //
            this.colQueueTime.HeaderText = "入队时间";
            this.colQueueTime.Name = "colQueueTime";
            this.colQueueTime.ReadOnly = true;
            this.colQueueTime.Width = 150;
            //
            // colQueueCode
            //
            this.colQueueCode.HeaderText = "产品码";
            this.colQueueCode.Name = "colQueueCode";
            this.colQueueCode.ReadOnly = true;
            this.colQueueCode.Width = 250;
            //
            // colQueueStatus
            //
            this.colQueueStatus.HeaderText = "状态";
            this.colQueueStatus.Name = "colQueueStatus";
            this.colQueueStatus.ReadOnly = true;
            this.colQueueStatus.Width = 80;
            //
            // colQueueRetry
            //
            this.colQueueRetry.HeaderText = "重试次数";
            this.colQueueRetry.Name = "colQueueRetry";
            this.colQueueRetry.ReadOnly = true;
            this.colQueueRetry.Width = 80;
            //
            // colQueueErr
            //
            this.colQueueErr.HeaderText = "最近错误";
            this.colQueueErr.Name = "colQueueErr";
            this.colQueueErr.ReadOnly = true;
            this.colQueueErr.Width = 500;
            //
            // UcPageMes
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(26)))), ((int)(((byte)(34)))));
            this.Controls.Add(this.tpRoot);
            this.Name = "UcPageMes";
            this.Size = new System.Drawing.Size(1200, 780);
            this.tpRoot.ResumeLayout(false);
            this.tpTop.ResumeLayout(false);
            this.tpTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tpRoot;
        private System.Windows.Forms.TableLayoutPanel tpTop;
        private System.Windows.Forms.Label lblMesConfig;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnRetry;
        private System.Windows.Forms.Label lblMesState;
        private System.Windows.Forms.Label lblCheckTitle;
        private System.Windows.Forms.DataGridView dgvCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCheckTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCheckCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCheckRet;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCheckMsg;
        private System.Windows.Forms.Label lblQueueTitle;
        private System.Windows.Forms.DataGridView dgvQueue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQueueTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQueueCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQueueStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQueueRetry;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQueueErr;
    }
}
