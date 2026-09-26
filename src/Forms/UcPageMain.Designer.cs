namespace GaugeDemo300.Forms
{
    partial class UcPageMain
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
            this.tpBarcode = new System.Windows.Forms.TableLayoutPanel();
            this.lblBarcode = new System.Windows.Forms.Label();
            this.lblBarcodeValue = new System.Windows.Forms.Label();
            this.panelImage = new System.Windows.Forms.Panel();
            this.picLayout = new System.Windows.Forms.PictureBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblFlowStatus = new System.Windows.Forms.Label();
            this.tpMainStats = new System.Windows.Forms.TableLayoutPanel();
            this.tpRoot.SuspendLayout();
            this.tpBarcode.SuspendLayout();
            this.panelImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLayout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            //
            // tpRoot
            //
            this.tpRoot.ColumnCount = 1;
            this.tpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpRoot.Controls.Add(this.tpBarcode, 0, 0);
            this.tpRoot.Controls.Add(this.panelImage, 0, 1);
            this.tpRoot.Controls.Add(this.lblFlowStatus, 0, 2);
            this.tpRoot.Controls.Add(this.tpMainStats, 0, 3);
            this.tpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpRoot.Location = new System.Drawing.Point(0, 0);
            this.tpRoot.Name = "tpRoot";
            this.tpRoot.RowCount = 4;
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tpRoot.Size = new System.Drawing.Size(1200, 780);
            this.tpRoot.TabIndex = 0;
            //
            // tpBarcode
            //
            this.tpBarcode.ColumnCount = 2;
            this.tpBarcode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tpBarcode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpBarcode.Controls.Add(this.lblBarcode, 0, 0);
            this.tpBarcode.Controls.Add(this.lblBarcodeValue, 1, 0);
            this.tpBarcode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpBarcode.Location = new System.Drawing.Point(3, 3);
            this.tpBarcode.Name = "tpBarcode";
            this.tpBarcode.RowCount = 1;
            this.tpBarcode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpBarcode.Size = new System.Drawing.Size(1194, 38);
            this.tpBarcode.TabIndex = 0;
            //
            // lblBarcode
            //
            this.lblBarcode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBarcode.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblBarcode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(155)))), ((int)(((byte)(180)))));
            this.lblBarcode.Location = new System.Drawing.Point(3, 0);
            this.lblBarcode.Name = "lblBarcode";
            this.lblBarcode.Size = new System.Drawing.Size(64, 38);
            this.lblBarcode.TabIndex = 0;
            this.lblBarcode.Text = "条码：";
            this.lblBarcode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblBarcodeValue
            //
            this.lblBarcodeValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(46)))), ((int)(((byte)(60)))));
            this.lblBarcodeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBarcodeValue.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.lblBarcodeValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.lblBarcodeValue.Location = new System.Drawing.Point(73, 0);
            this.lblBarcodeValue.Name = "lblBarcodeValue";
            this.lblBarcodeValue.Size = new System.Drawing.Size(1118, 38);
            this.lblBarcodeValue.TabIndex = 1;
            this.lblBarcodeValue.Text = "（扫码自动录入）";
            this.lblBarcodeValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // panelImage
            //
            this.panelImage.Controls.Add(this.picLayout);
            this.panelImage.Controls.Add(this.picLogo);
            this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImage.Location = new System.Drawing.Point(3, 47);
            this.panelImage.Name = "panelImage";
            this.panelImage.Size = new System.Drawing.Size(1194, 580);
            this.panelImage.TabIndex = 1;
            //
            // picLayout
            //
            this.picLayout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(35)))), ((int)(((byte)(46)))));
            this.picLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picLayout.Location = new System.Drawing.Point(0, 0);
            this.picLayout.Name = "picLayout";
            this.picLayout.Size = new System.Drawing.Size(1194, 580);
            this.picLayout.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLayout.TabIndex = 0;
            this.picLayout.TabStop = false;
            //
            // picLogo
            //
            this.picLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(38)))), ((int)(((byte)(50)))));
            this.picLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picLogo.Location = new System.Drawing.Point(8, 8);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(220, 90);
            this.picLogo.TabIndex = 1;
            this.picLogo.TabStop = false;
            //
            // lblFlowStatus
            //
            this.lblFlowStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFlowStatus.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblFlowStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(234)))), ((int)(((byte)(244)))));
            this.lblFlowStatus.Location = new System.Drawing.Point(3, 633);
            this.lblFlowStatus.Name = "lblFlowStatus";
            this.lblFlowStatus.Size = new System.Drawing.Size(1194, 64);
            this.lblFlowStatus.TabIndex = 2;
            this.lblFlowStatus.Text = "就绪，请扫码";
            this.lblFlowStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // tpMainStats
            //
            this.tpMainStats.ColumnCount = 4;
            this.tpMainStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tpMainStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tpMainStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tpMainStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tpMainStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpMainStats.Location = new System.Drawing.Point(3, 703);
            this.tpMainStats.Name = "tpMainStats";
            this.tpMainStats.RowCount = 1;
            this.tpMainStats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpMainStats.Size = new System.Drawing.Size(1194, 74);
            this.tpMainStats.TabIndex = 3;
            //
            // UcPageMain
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(26)))), ((int)(((byte)(34)))));
            this.Controls.Add(this.tpRoot);
            this.Name = "UcPageMain";
            this.Size = new System.Drawing.Size(1200, 780);
            this.tpRoot.ResumeLayout(false);
            this.tpBarcode.ResumeLayout(false);
            this.tpBarcode.PerformLayout();
            this.panelImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLayout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tpRoot;
        private System.Windows.Forms.TableLayoutPanel tpBarcode;
        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.Label lblBarcodeValue;
        private System.Windows.Forms.Panel panelImage;
        private System.Windows.Forms.PictureBox picLayout;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblFlowStatus;
        private System.Windows.Forms.TableLayoutPanel tpMainStats;
    }
}
