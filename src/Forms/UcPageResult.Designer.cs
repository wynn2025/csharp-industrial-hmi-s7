using GaugeDemo300.Models;
using GaugeDemo300.Services;

namespace GaugeDemo300.Forms
{
    partial class UcPageResult
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
            this.tpEnvHead = new System.Windows.Forms.TableLayoutPanel();
            this.lblEnvTitle = new System.Windows.Forms.Label();
            this.btnEnvPrev = new System.Windows.Forms.Button();
            this.lblEnvPage = new System.Windows.Forms.Label();
            this.btnEnvNext = new System.Windows.Forms.Button();
            this.flowEnv = new System.Windows.Forms.FlowLayoutPanel();
            this.tpARHead = new System.Windows.Forms.TableLayoutPanel();
            this.lblARTitle = new System.Windows.Forms.Label();
            this.btnARPrev = new System.Windows.Forms.Button();
            this.lblARPage = new System.Windows.Forms.Label();
            this.btnARNext = new System.Windows.Forms.Button();
            this.dgvResultA = new System.Windows.Forms.DataGridView();
            this.btnSaveTol = new System.Windows.Forms.Button();
            this.colNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLower = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUpper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJudge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpRoot.SuspendLayout();
            this.tpEnvHead.SuspendLayout();
            this.tpARHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResultA)).BeginInit();
            this.SuspendLayout();
            // 
            // tpRoot
            // 
            this.tpRoot.ColumnCount = 2;
            this.tpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52F));
            this.tpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48F));
            this.tpRoot.Controls.Add(this.tpEnvHead, 0, 0);
            this.tpRoot.Controls.Add(this.flowEnv, 0, 1);
            this.tpRoot.Controls.Add(this.tpARHead, 1, 0);
            this.tpRoot.Controls.Add(this.dgvResultA, 1, 1);
            this.tpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpRoot.Location = new System.Drawing.Point(0, 0);
            this.tpRoot.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpRoot.Name = "tpRoot";
            this.tpRoot.RowCount = 2;
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpRoot.Size = new System.Drawing.Size(1543, 826);
            this.tpRoot.TabIndex = 0;
            // 
            // tpEnvHead
            // 
            this.tpEnvHead.ColumnCount = 6;
            this.tpEnvHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tpEnvHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tpEnvHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tpEnvHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tpEnvHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tpEnvHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tpEnvHead.Controls.Add(this.lblEnvTitle, 0, 0);
            this.tpEnvHead.Controls.Add(this.btnEnvPrev, 1, 0);
            this.tpEnvHead.Controls.Add(this.lblEnvPage, 2, 0);
            this.tpEnvHead.Controls.Add(this.btnEnvNext, 3, 0);
            this.tpEnvHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpEnvHead.Location = new System.Drawing.Point(4, 3);
            this.tpEnvHead.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpEnvHead.Name = "tpEnvHead";
            this.tpEnvHead.RowCount = 1;
            this.tpEnvHead.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpEnvHead.Size = new System.Drawing.Size(794, 43);
            this.tpEnvHead.TabIndex = 0;
            // 
            // lblEnvTitle
            // 
            this.lblEnvTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEnvTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblEnvTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(234)))), ((int)(((byte)(244)))));
            this.lblEnvTitle.Location = new System.Drawing.Point(4, 0);
            this.lblEnvTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEnvTitle.Name = "lblEnvTitle";
            this.lblEnvTitle.Size = new System.Drawing.Size(190, 43);
            this.lblEnvTitle.TabIndex = 4;
            this.lblEnvTitle.Text = "环控检测结果（200 点 OK/NG）";
            this.lblEnvTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnEnvPrev
            // 
            this.btnEnvPrev.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(99)))), ((int)(((byte)(158)))));
            this.btnEnvPrev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEnvPrev.FlatAppearance.BorderSize = 0;
            this.btnEnvPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnvPrev.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEnvPrev.ForeColor = System.Drawing.Color.White;
            this.btnEnvPrev.Location = new System.Drawing.Point(202, 3);
            this.btnEnvPrev.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnEnvPrev.Name = "btnEnvPrev";
            this.btnEnvPrev.Size = new System.Drawing.Size(55, 37);
            this.btnEnvPrev.TabIndex = 0;
            this.btnEnvPrev.Text = "◀ 上页";
            this.btnEnvPrev.UseVisualStyleBackColor = false;
            this.btnEnvPrev.Click += new System.EventHandler(this.btnEnvPrev_Click);
            // 
            // lblEnvPage
            // 
            this.lblEnvPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEnvPage.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblEnvPage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(234)))), ((int)(((byte)(244)))));
            this.lblEnvPage.Location = new System.Drawing.Point(265, 0);
            this.lblEnvPage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEnvPage.Name = "lblEnvPage";
            this.lblEnvPage.Size = new System.Drawing.Size(126, 43);
            this.lblEnvPage.TabIndex = 1;
            this.lblEnvPage.Text = "第 1/2 页";
            this.lblEnvPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnEnvNext
            // 
            this.btnEnvNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(99)))), ((int)(((byte)(158)))));
            this.btnEnvNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEnvNext.FlatAppearance.BorderSize = 0;
            this.btnEnvNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnvNext.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEnvNext.ForeColor = System.Drawing.Color.White;
            this.btnEnvNext.Location = new System.Drawing.Point(399, 3);
            this.btnEnvNext.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnEnvNext.Name = "btnEnvNext";
            this.btnEnvNext.Size = new System.Drawing.Size(55, 37);
            this.btnEnvNext.TabIndex = 2;
            this.btnEnvNext.Text = "下页 ▶";
            this.btnEnvNext.UseVisualStyleBackColor = false;
            this.btnEnvNext.Click += new System.EventHandler(this.btnEnvNext_Click);
            // 
            // flowEnv
            // 
            this.flowEnv.AutoScroll = true;
            this.flowEnv.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(35)))), ((int)(((byte)(46)))));
            this.flowEnv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowEnv.Location = new System.Drawing.Point(4, 52);
            this.flowEnv.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.flowEnv.Name = "flowEnv";
            this.flowEnv.Size = new System.Drawing.Size(794, 771);
            this.flowEnv.TabIndex = 1;
            // 
            // tpARHead
            // 
            this.tpARHead.ColumnCount = 6;
            this.tpARHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.tpARHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9F));
            this.tpARHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tpARHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9F));
            this.tpARHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21F));
            this.tpARHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21F));
            this.tpARHead.Controls.Add(this.lblARTitle, 0, 0);
            this.tpARHead.Controls.Add(this.btnARPrev, 1, 0);
            this.tpARHead.Controls.Add(this.lblARPage, 2, 0);
            this.tpARHead.Controls.Add(this.btnARNext, 3, 0);
            this.tpARHead.Controls.Add(this.btnSaveTol, 4, 0);
            this.tpARHead.SetColumnSpan(this.btnSaveTol, 2);
            this.tpARHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpARHead.Location = new System.Drawing.Point(806, 3);
            this.tpARHead.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpARHead.Name = "tpARHead";
            this.tpARHead.RowCount = 1;
            this.tpARHead.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpARHead.Size = new System.Drawing.Size(733, 43);
            this.tpARHead.TabIndex = 2;
            // 
            // btnSaveTol
            // 
            this.btnSaveTol.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(99)))), ((int)(((byte)(158)))));
            this.btnSaveTol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveTol.FlatAppearance.BorderSize = 0;
            this.btnSaveTol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveTol.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSaveTol.ForeColor = System.Drawing.Color.White;
            this.btnSaveTol.Location = new System.Drawing.Point(301, 7);
            this.btnSaveTol.Name = "btnSaveTol";
            this.btnSaveTol.Size = new System.Drawing.Size(412, 30);
            this.btnSaveTol.TabIndex = 5;
            this.btnSaveTol.Text = "修改";
            this.btnSaveTol.UseVisualStyleBackColor = false;
            this.btnSaveTol.Click += new System.EventHandler(this.btnSaveTol_Click);
            // 
            // lblARTitle
            // 
            this.lblARTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblARTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblARTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(234)))), ((int)(((byte)(244)))));
            this.lblARTitle.Location = new System.Drawing.Point(4, 0);
            this.lblARTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblARTitle.Name = "lblARTitle";
            this.lblARTitle.Size = new System.Drawing.Size(153, 43);
            this.lblARTitle.TabIndex = 4;
            this.lblARTitle.Text = "位移测量值与判定（实时）";
            this.lblARTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnARPrev
            // 
            this.btnARPrev.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(99)))), ((int)(((byte)(158)))));
            this.btnARPrev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnARPrev.FlatAppearance.BorderSize = 0;
            this.btnARPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnARPrev.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnARPrev.ForeColor = System.Drawing.Color.White;
            this.btnARPrev.Location = new System.Drawing.Point(165, 3);
            this.btnARPrev.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnARPrev.Name = "btnARPrev";
            this.btnARPrev.Size = new System.Drawing.Size(57, 37);
            this.btnARPrev.TabIndex = 0;
            this.btnARPrev.Text = "◀ 上页";
            this.btnARPrev.UseVisualStyleBackColor = false;
            this.btnARPrev.Click += new System.EventHandler(this.btnARPrev_Click);
            // 
            // lblARPage
            // 
            this.lblARPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblARPage.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblARPage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(234)))), ((int)(((byte)(244)))));
            this.lblARPage.Location = new System.Drawing.Point(230, 0);
            this.lblARPage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblARPage.Name = "lblARPage";
            this.lblARPage.Size = new System.Drawing.Size(123, 43);
            this.lblARPage.TabIndex = 1;
            this.lblARPage.Text = "第 1/3 页";
            this.lblARPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnARNext
            // 
            this.btnARNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(99)))), ((int)(((byte)(158)))));
            this.btnARNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnARNext.FlatAppearance.BorderSize = 0;
            this.btnARNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnARNext.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnARNext.ForeColor = System.Drawing.Color.White;
            this.btnARNext.Location = new System.Drawing.Point(361, 3);
            this.btnARNext.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnARNext.Name = "btnARNext";
            this.btnARNext.Size = new System.Drawing.Size(57, 37);
            this.btnARNext.TabIndex = 2;
            this.btnARNext.Text = "下页 ▶";
            this.btnARNext.UseVisualStyleBackColor = false;
            this.btnARNext.Click += new System.EventHandler(this.btnARNext_Click);
            // 
            // dgvResultA
            // 
            this.dgvResultA.ColumnHeadersHeight = 34;
            this.dgvResultA.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNo,
            this.colName,
            this.colValue,
            this.colLower,
            this.colUpper,
            this.colJudge});
            this.dgvResultA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResultA.Location = new System.Drawing.Point(806, 52);
            this.dgvResultA.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgvResultA.Name = "dgvResultA";
            this.dgvResultA.RowHeadersWidth = 62;
            this.dgvResultA.Size = new System.Drawing.Size(733, 771);
            this.dgvResultA.TabIndex = 3;
            // 
            // colNo
            // 
            this.colNo.HeaderText = "位移编号";
            this.colNo.MinimumWidth = 8;
            this.colNo.Name = "colNo";
            this.colNo.ReadOnly = true;
            this.colNo.Width = 120;
            // 
            // colName
            // 
            this.colName.MinimumWidth = 8;
            this.colName.Name = "colName";
            this.colName.Width = 150;
            // 
            // colValue
            // 
            this.colValue.HeaderText = "测量值";
            this.colValue.MinimumWidth = 8;
            this.colValue.Name = "colValue";
            this.colValue.ReadOnly = true;
            this.colValue.Width = 160;
            // 
            // colLower
            // 
            this.colLower.HeaderText = "下限";
            this.colLower.MinimumWidth = 8;
            this.colLower.Name = "colLower";
            this.colLower.Width = 140;
            // 
            // colUpper
            // 
            this.colUpper.HeaderText = "上限";
            this.colUpper.MinimumWidth = 8;
            this.colUpper.Name = "colUpper";
            this.colUpper.Width = 140;
            // 
            // colJudge
            // 
            this.colJudge.HeaderText = "结果";
            this.colJudge.MinimumWidth = 8;
            this.colJudge.Name = "colJudge";
            this.colJudge.ReadOnly = true;
            this.colJudge.Width = 90;
            // 
            // UcPageResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(26)))), ((int)(((byte)(34)))));
            this.Controls.Add(this.tpRoot);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "UcPageResult";
            this.Size = new System.Drawing.Size(1543, 826);
            this.tpRoot.ResumeLayout(false);
            this.tpEnvHead.ResumeLayout(false);
            this.tpARHead.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResultA)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tpRoot;
        private System.Windows.Forms.TableLayoutPanel tpEnvHead;
        private System.Windows.Forms.Label lblEnvTitle;
        private System.Windows.Forms.Button btnEnvPrev;
        private System.Windows.Forms.Label lblEnvPage;
        private System.Windows.Forms.Button btnEnvNext;
        private System.Windows.Forms.FlowLayoutPanel flowEnv;
        private System.Windows.Forms.TableLayoutPanel tpARHead;
        private System.Windows.Forms.Label lblARTitle;
        private System.Windows.Forms.Button btnARPrev;
        private System.Windows.Forms.Label lblARPage;
        private System.Windows.Forms.Button btnARNext;
        private System.Windows.Forms.DataGridView dgvResultA;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLower;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUpper;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJudge;
        private System.Windows.Forms.Button btnSaveTol;
    }
}
