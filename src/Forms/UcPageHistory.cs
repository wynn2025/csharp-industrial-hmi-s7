using Newtonsoft.Json.Linq;

using GaugeDemo300.Models;
using GaugeDemo300.Services;

namespace GaugeDemo300.Forms
{
    /// <summary>历史记录画面：时间/产品码/结果筛选 + 记录表格（双击看明细）</summary>
    public partial class UcPageHistory : UserControl
    {
        private readonly DatabaseService _db;

        public UcPageHistory(DatabaseService db)
        {
            _db = db;
            InitializeComponent();
            Theme.StyleGrid(dgvHistory);
            dtpFrom.Value = DateTime.Today;
            dtpTo.Value = DateTime.Today;
            cmbHistResult.SelectedIndex = 0;
            RefreshHistory();
        }

        public void RefreshHistory()
        {
            string result = cmbHistResult.SelectedIndex <= 0 ? "" : cmbHistResult.Text;
            var list = _db.QueryMeasurements(dtpFrom.Value, dtpTo.Value, txtHistCode.Text.Trim(), result);
            dgvHistory.Rows.Clear();
            foreach (var rec in list.Take(2000))
            {
                int okCount = string.IsNullOrEmpty(rec.OkPoints) ? 0 :
                    (rec.OkPoints.Contains('/') ? 0 : rec.OkPoints.Split(',').Length);
                int ngCount = string.IsNullOrEmpty(rec.NgPoints) ? 0 : CountNgApprox(rec.NgPoints);
                // 位移值预览（前 3 个），完整 224 点明细双击行查看
                string dispPrev = string.Join(" / ", rec.MeasurePoints.Take(3).Where(v => v != null));
                int row = dgvHistory.Rows.Add(
                    rec.StationTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    rec.ProductCode, rec.ProductModel, rec.OverallResult,
                    okCount, ngCount, dispPrev, rec.SyncSuccess ? "已同步" : "待同步");
                dgvHistory.Rows[row].Cells[3].Style.ForeColor = rec.OverallResult == "OK" ? Theme.Ok : Theme.Ng;
                dgvHistory.Rows[row].Cells[3].Style.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
                dgvHistory.Rows[row].Tag = rec;
            }
        }

        private static int CountNgApprox(string ngPoints)
        {
            int idx = ngPoints.IndexOf("等", StringComparison.Ordinal);
            if (idx < 0) return ngPoints.Split(',').Length;
            string num = new string(ngPoints.Substring(idx + 1).TakeWhile(char.IsDigit).ToArray());
            return int.TryParse(num, out int n) ? n : ngPoints.Split(',').Length;
        }

        private void btnHistQuery_Click(object sender, EventArgs e) => RefreshHistory();

        private void dgvHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvHistory.Rows[e.RowIndex].Tag is not MeasurementRecord rec) return;
            using var frm = new FormHistoryDetail(rec);
            frm.ShowDialog(ParentForm);
        }
    }
}
