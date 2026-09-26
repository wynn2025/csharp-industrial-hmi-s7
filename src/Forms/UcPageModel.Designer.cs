namespace GaugeDemo300.Forms
{
    partial class UcPageModel
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
            this.tpLeft = new System.Windows.Forms.TableLayoutPanel();
            this.lstModels = new System.Windows.Forms.ListBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.grpEditor = new System.Windows.Forms.GroupBox();
            this.tpEditor = new System.Windows.Forms.TableLayoutPanel();
            this.lblModelName = new System.Windows.Forms.Label();
            this.txtModelName = new System.Windows.Forms.TextBox();
            this.lblProductName = new System.Windows.Forms.Label();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.lblResultCount = new System.Windows.Forms.Label();
            this.numResultCount = new System.Windows.Forms.NumericUpDown();
            this.lblAnalogCount = new System.Windows.Forms.Label();
            this.numAnalogCount = new System.Windows.Forms.NumericUpDown();
            this.lblCylCount = new System.Windows.Forms.Label();
            this.numCylCount = new System.Windows.Forms.NumericUpDown();
            this.btnSaveEdit = new System.Windows.Forms.Button();
            this.lblModelCurrent = new System.Windows.Forms.Label();
            this.tpRoot.SuspendLayout();
            this.tpLeft.SuspendLayout();
            this.grpEditor.SuspendLayout();
            this.tpEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numResultCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAnalogCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCylCount)).BeginInit();
            this.SuspendLayout();
            //
            // tpRoot
            //
            this.tpRoot.ColumnCount = 2;
            this.tpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tpRoot.Controls.Add(this.tpLeft, 0, 0);
            this.tpRoot.Controls.Add(this.grpEditor, 1, 0);
            this.tpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpRoot.Location = new System.Drawing.Point(0, 0);
            this.tpRoot.Name = "tpRoot";
            this.tpRoot.RowCount = 1;
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpRoot.Size = new System.Drawing.Size(1200, 780);
            this.tpRoot.TabIndex = 0;
            //
            // tpLeft
            //
            this.tpLeft.ColumnCount = 2;
            this.tpLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tpLeft.Controls.Add(this.lstModels, 0, 0);
            this.tpLeft.Controls.Add(this.btnApply, 0, 1);
            this.tpLeft.Controls.Add(this.btnCopy, 1, 1);
            this.tpLeft.Controls.Add(this.btnNew, 0, 2);
            this.tpLeft.Controls.Add(this.btnDelete, 1, 2);
            this.tpLeft.Controls.Add(this.lblModelCurrent, 0, 3);
            this.tpLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpLeft.Location = new System.Drawing.Point(3, 3);
            this.tpLeft.Name = "tpLeft";
            this.tpLeft.RowCount = 4;
            this.tpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tpLeft.SetColumnSpan(this.lblModelCurrent, 2);
            this.tpLeft.Size = new System.Drawing.Size(414, 774);
            this.tpLeft.TabIndex = 0;
            //
            // lstModels
            //
            this.lstModels.BackColor = System.Drawing.Color.FromArgb(40, 46, 60);
            this.lstModels.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstModels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstModels.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            this.lstModels.ForeColor = System.Drawing.Color.FromArgb(228, 234, 244);
            this.lstModels.ItemHeight = 20;
            this.lstModels.Location = new System.Drawing.Point(3, 3);
            this.lstModels.Name = "lstModels";
            this.lstModels.Size = new System.Drawing.Size(298, 676);
            this.lstModels.TabIndex = 0;
            //
            // btnApply
            //
            this.btnApply.BackColor = System.Drawing.Color.FromArgb(46, 160, 90);
            this.btnApply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnApply.FlatAppearance.BorderSize = 0;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnApply.ForeColor = System.Drawing.Color.White;
            this.btnApply.Location = new System.Drawing.Point(3, 685);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(298, 40);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "应用此型号";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            //
            // btnCopy
            //
            this.btnCopy.BackColor = System.Drawing.Color.FromArgb(100, 130, 200);
            this.btnCopy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCopy.FlatAppearance.BorderSize = 0;
            this.btnCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopy.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCopy.ForeColor = System.Drawing.Color.White;
            this.btnCopy.Location = new System.Drawing.Point(307, 685);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(104, 40);
            this.btnCopy.TabIndex = 2;
            this.btnCopy.Text = "复制型号";
            this.btnCopy.UseVisualStyleBackColor = false;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            //
            // btnNew
            //
            this.btnNew.BackColor = System.Drawing.Color.FromArgb(43, 99, 158);
            this.btnNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNew.FlatAppearance.BorderSize = 0;
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnNew.ForeColor = System.Drawing.Color.White;
            this.btnNew.Location = new System.Drawing.Point(3, 731);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(298, 40);
            this.btnNew.TabIndex = 3;
            this.btnNew.Text = "新建型号（从当前配置）";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            //
            // btnDelete
            //
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(60, 66, 88);
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(307, 731);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 40);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //
            // lblModelCurrent
            //
            this.lblModelCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblModelCurrent.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblModelCurrent.ForeColor = System.Drawing.Color.FromArgb(126, 200, 255);
            this.lblModelCurrent.Location = new System.Drawing.Point(3, 777);
            this.lblModelCurrent.Name = "lblModelCurrent";
            this.lblModelCurrent.Size = new System.Drawing.Size(408, 30);
            this.lblModelCurrent.TabIndex = 5;
            this.lblModelCurrent.Text = "当前型号：--";
            this.lblModelCurrent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // grpEditor
            //
            this.grpEditor.Controls.Add(this.tpEditor);
            this.grpEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpEditor.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpEditor.ForeColor = System.Drawing.Color.FromArgb(228, 234, 244);
            this.grpEditor.Location = new System.Drawing.Point(423, 3);
            this.grpEditor.Name = "grpEditor";
            this.grpEditor.Size = new System.Drawing.Size(774, 774);
            this.grpEditor.TabIndex = 1;
            this.grpEditor.TabStop = false;
            this.grpEditor.Text = " 型号参数编辑 ";
            //
            // tpEditor
            //
            this.tpEditor.ColumnCount = 2;
            this.tpEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tpEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tpEditor.Controls.Add(this.lblModelName, 0, 0);
            this.tpEditor.Controls.Add(this.txtModelName, 1, 0);
            this.tpEditor.Controls.Add(this.lblProductName, 0, 1);
            this.tpEditor.Controls.Add(this.txtProductName, 1, 1);
            this.tpEditor.Controls.Add(this.lblResultCount, 0, 2);
            this.tpEditor.Controls.Add(this.numResultCount, 1, 2);
            this.tpEditor.Controls.Add(this.lblAnalogCount, 0, 3);
            this.tpEditor.Controls.Add(this.numAnalogCount, 1, 3);
            this.tpEditor.Controls.Add(this.lblCylCount, 0, 4);
            this.tpEditor.Controls.Add(this.numCylCount, 1, 4);
            this.tpEditor.Controls.Add(this.btnSaveEdit, 1, 6);
            this.tpEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpEditor.Location = new System.Drawing.Point(3, 22);
            this.tpEditor.Name = "tpEditor";
            this.tpEditor.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.tpEditor.RowCount = 7;
            this.tpEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tpEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tpEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tpEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tpEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tpEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tpEditor.Size = new System.Drawing.Size(768, 749);
            this.tpEditor.TabIndex = 0;
            this.lblModelName.Text = "型号名称：";
            this.lblProductName.Text = "产品名称(MES)：";
            this.lblResultCount.Text = "穿孔检测点数：";
            this.lblAnalogCount.Text = "位移检测点数：";
            this.lblCylCount.Text = "气缸数量：";
            this.lblModelName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblProductName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblResultCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblAnalogCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblCylCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.numResultCount.Minimum = 10;
            this.numResultCount.Maximum = 500;
            this.numAnalogCount.Minimum = 5;
            this.numAnalogCount.Maximum = 100;
            this.numCylCount.Minimum = 0;
            this.numCylCount.Maximum = 10;
            this.numResultCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.numAnalogCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.numCylCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            //
            // btnSaveEdit
            //
            this.btnSaveEdit.BackColor = System.Drawing.Color.FromArgb(46, 160, 90);
            this.btnSaveEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveEdit.FlatAppearance.BorderSize = 0;
            this.btnSaveEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveEdit.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnSaveEdit.ForeColor = System.Drawing.Color.White;
            this.btnSaveEdit.Location = new System.Drawing.Point(253, 693);
            this.btnSaveEdit.Name = "btnSaveEdit";
            this.btnSaveEdit.Size = new System.Drawing.Size(492, 46);
            this.btnSaveEdit.TabIndex = 6;
            this.btnSaveEdit.Text = "保存修改（数量增减自动调整点位）";
            this.btnSaveEdit.UseVisualStyleBackColor = false;
            this.btnSaveEdit.Click += new System.EventHandler(this.btnSaveEdit_Click);
            //
            // UcPageModel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(23, 26, 34);
            this.Controls.Add(this.tpRoot);
            this.Name = "UcPageModel";
            this.Size = new System.Drawing.Size(1200, 780);
            this.tpRoot.ResumeLayout(false);
            this.tpLeft.ResumeLayout(false);
            this.grpEditor.ResumeLayout(false);
            this.tpEditor.ResumeLayout(false);
            this.tpEditor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numResultCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAnalogCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCylCount)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tpRoot;
        private System.Windows.Forms.TableLayoutPanel tpLeft;
        private System.Windows.Forms.ListBox lstModels;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblModelCurrent;
        private System.Windows.Forms.GroupBox grpEditor;
        private System.Windows.Forms.TableLayoutPanel tpEditor;
        private System.Windows.Forms.Label lblModelName;
        private System.Windows.Forms.TextBox txtModelName;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.Label lblResultCount;
        private System.Windows.Forms.NumericUpDown numResultCount;
        private System.Windows.Forms.Label lblAnalogCount;
        private System.Windows.Forms.NumericUpDown numAnalogCount;
        private System.Windows.Forms.Label lblCylCount;
        private System.Windows.Forms.NumericUpDown numCylCount;
        private System.Windows.Forms.Button btnSaveEdit;
    }
}
