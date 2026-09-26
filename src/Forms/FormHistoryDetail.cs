using Newtonsoft.Json.Linq;

using GaugeDemo300.Models;

namespace GaugeDemo300.Forms
{
    /// <summary>历史记录明细窗体：顶部记录概要 + 全部点位明细表（名称/类型/测量值/上下限/结果）</summary>
    public partial class FormHistoryDetail : Form
    {
        public FormHistoryDetail(MeasurementRecord rec)
        {
            InitializeComponent();
            Theme.StyleGrid(dgvPoints);
            Text = $"检测明细 - {rec.ProductCode}";

            lblSummary.Text = $"条码：{rec.ProductCode}    型号：{rec.ProductModel}    结果：{rec.OverallResult}    " +
                              $"时间：{rec.StationTime:yyyy-MM-dd HH:mm:ss}    操作人：{rec.Operator}    " +
                              $"MES：{(rec.SyncSuccess ? "已同步" : "待同步")}";
            lblSummary.ForeColor = rec.OverallResult == "OK" ? Theme.Ok : Theme.Ng;

            LoadPoints(rec);
        }

        private void LoadPoints(MeasurementRecord rec)
        {
            // 位移上下限成对存储："上1,下1,上2,下2,..."，按位移点出现顺序配对
            string[] lims = rec.DisplacementLimits.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int dispIdx = 0;

            try
            {
                var arr = JArray.Parse(string.IsNullOrWhiteSpace(rec.ChannelValuesJson) ? "[]" : rec.ChannelValuesJson);
                foreach (var item in arr)
                {
                    string type = item["type"]?.ToString() ?? "";
                    bool isDisp = type == "位移";
                    string upper = "-", lower = "-";
                    if (isDisp && dispIdx * 2 + 1 < lims.Length)
                    {
                        upper = lims[dispIdx * 2];
                        lower = lims[dispIdx * 2 + 1];
                        dispIdx++;
                    }
                    string result = item["result"]?.ToString() ?? "";
                    int row = dgvPoints.Rows.Add(
                        dgvPoints.Rows.Count + 1,
                        item["name"]?.ToString() ?? "",
                        type,
                        item["value"]?.ToString() ?? "-",
                        isDisp ? upper : "-",
                        isDisp ? lower : "-",
                        result,
                        item["description"]?.ToString() ?? "");
                    if (result == "OK")
                        dgvPoints.Rows[row].Cells[6].Style.ForeColor = Theme.Ok;
                    else if (result == "NG")
                        dgvPoints.Rows[row].Cells[6].Style.ForeColor = Theme.Ng;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"点位明细解析失败：{ex.Message}", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClose_Click(object sender, EventArgs e) => Close();
    }
}
