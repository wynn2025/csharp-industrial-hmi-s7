using GaugeDemo300.Models;
using GaugeDemo300.Services;

namespace GaugeDemo300.Forms
{
    partial class UcPageManual
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
            this.lblManualTip = new System.Windows.Forms.Label();
            this.panelCylRows = new System.Windows.Forms.FlowLayoutPanel();
            this.tpRoot.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpRoot
            // 
            this.tpRoot.ColumnCount = 1;
            this.tpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpRoot.Controls.Add(this.lblManualTip, 0, 0);
            this.tpRoot.Controls.Add(this.panelCylRows, 0, 1);
            this.tpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpRoot.Location = new System.Drawing.Point(0, 0);
            this.tpRoot.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpRoot.Name = "tpRoot";
            this.tpRoot.RowCount = 2;
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpRoot.Size = new System.Drawing.Size(1543, 254);
            this.tpRoot.TabIndex = 0;
            // 
            // lblManualTip
            // 
            this.lblManualTip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblManualTip.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblManualTip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(90)))));
            this.lblManualTip.Location = new System.Drawing.Point(4, 0);
            this.lblManualTip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblManualTip.Name = "lblManualTip";
            this.lblManualTip.Size = new System.Drawing.Size(1535, 55);
            this.lblManualTip.TabIndex = 0;
            this.lblManualTip.Text = "提示：请切换到手动模式并登录后操作气缸";
            this.lblManualTip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelCylRows
            // 
            this.panelCylRows.AutoScroll = true;
            this.panelCylRows.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(35)))), ((int)(((byte)(46)))));
            this.panelCylRows.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panelCylRows.WrapContents = false;
            this.panelCylRows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCylRows.Location = new System.Drawing.Point(4, 58);
            this.panelCylRows.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panelCylRows.Name = "panelCylRows";
            this.panelCylRows.Size = new System.Drawing.Size(1535, 193);
            this.panelCylRows.TabIndex = 1;
            // 
            // UcPageManual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(26)))), ((int)(((byte)(34)))));
            this.Controls.Add(this.tpRoot);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "UcPageManual";
            this.Size = new System.Drawing.Size(1543, 254);
            this.tpRoot.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tpRoot;
        private System.Windows.Forms.Label lblManualTip;
        private System.Windows.Forms.FlowLayoutPanel panelCylRows;
    }
}
