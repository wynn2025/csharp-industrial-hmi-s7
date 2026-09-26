using System.Drawing;

namespace GaugeDemo300.Forms;

/// <summary>深色工业风共享主题色（与 GaugeDemo 系列一致）</summary>
public static class Theme
{
    public static readonly Color Bg = Color.FromArgb(23, 26, 34);
    public static readonly Color Panel = Color.FromArgb(30, 35, 46);
    public static readonly Color PanelLight = Color.FromArgb(40, 46, 60);
    public static readonly Color Header = Color.FromArgb(26, 32, 44);
    public static readonly Color TextMain = Color.FromArgb(228, 234, 244);
    public static readonly Color TextSub = Color.FromArgb(140, 155, 180);
    public static readonly Color Accent = Color.FromArgb(59, 130, 246);
    public static readonly Color Ok = Color.FromArgb(74, 200, 118);
    public static readonly Color Ng = Color.FromArgb(240, 105, 105);
    public static readonly Color Warn = Color.FromArgb(240, 200, 90);
    public static readonly Color BtnBlue = Color.FromArgb(43, 99, 158);
    public static readonly Color BtnGray = Color.FromArgb(60, 66, 88);

    /// <summary>DataGridView 深色样式（各画面表格统一调用）</summary>
    public static void StyleGrid(DataGridView dgv)
    {
        dgv.BackgroundColor = Panel;
        dgv.BorderStyle = BorderStyle.None;
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgv.EnableHeadersVisualStyles = false;
        dgv.ColumnHeadersDefaultCellStyle.BackColor = PanelLight;
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = TextMain;
        dgv.DefaultCellStyle.BackColor = Color.FromArgb(35, 40, 52);
        dgv.DefaultCellStyle.ForeColor = TextMain;
        dgv.AlternatingRowsDefaultCellStyle.BackColor = PanelLight;
        dgv.AlternatingRowsDefaultCellStyle.ForeColor = TextMain;
        dgv.DefaultCellStyle.SelectionBackColor = BtnBlue;
        dgv.DefaultCellStyle.SelectionForeColor = Color.White;
        dgv.GridColor = Color.FromArgb(52, 58, 74);
        dgv.RowHeadersVisible = false;
        dgv.AllowUserToAddRows = false;
        dgv.AllowUserToDeleteRows = false;
        dgv.AllowUserToResizeRows = false;
        dgv.MultiSelect = false;
        dgv.ReadOnly = true;
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        // 列自动填满表格宽度（不再右侧留空白）
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    }
}
