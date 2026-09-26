using GaugeDemo300.Models;
using GaugeDemo300.Services;

namespace GaugeDemo300.Forms
{
    /// <summary>MES 通讯画面：配置信息 + 过站校验记录 + 上传队列（手动重传）</summary>
    public partial class UcPageMes : UserControl
    {
        private readonly SystemConfig _cfg;
        private readonly DatabaseService _db;
        private readonly MesService _mes;
        private readonly StationFlowService _flow;

        public UcPageMes(SystemConfig cfg, DatabaseService db, MesService mes, StationFlowService flow)
        {
            _cfg = cfg;
            _db = db;
            _mes = mes;
            _flow = flow;
            InitializeComponent();
            Theme.StyleGrid(dgvCheck);
            Theme.StyleGrid(dgvQueue);
            lblMesConfig.Text = $"设备编码：{cfg.Mes.DeviceCode} | 校验：{cfg.Mes.CheckUrl} | 同步：{cfg.Mes.SyncUrl}";
            mes.StatusChanged += state => SafeInvoke(() =>
            {
                lblMesState.Text = $"状态：{state}";
                lblMesState.ForeColor = state == "正常" ? Theme.Ok : state == "通讯异常" ? Theme.Ng : Theme.Warn;
            });
        }

        public void RefreshCheckGrid()
        {
            dgvCheck.Rows.Clear();
            foreach (var c in _db.QueryChecks(DateTime.Today.AddDays(-3), DateTime.MaxValue))
            {
                int row = dgvCheck.Rows.Add(c.CheckTime.ToString("yyyy-MM-dd HH:mm:ss"), c.ProductCode, c.Code, c.Msg);
                dgvCheck.Rows[row].Cells[2].Style.ForeColor = c.Passed ? Theme.Ok : Theme.Ng;
            }
        }

        public void RefreshQueueGrid()
        {
            dgvQueue.Rows.Clear();
            foreach (var q in _db.GetPendingQueue().Take(200))
            {
                dgvQueue.Rows.Add(q.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"), q.ProductCode, q.Status,
                    q.RetryCount, q.LastError);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            string code = "TEST" + DateTime.Now.ToString("HHmmss");
            _ = _flow.CheckAsync(code);
            Task.Delay(800).ContinueWith(_ => SafeInvoke(() => { RefreshCheckGrid(); RefreshQueueGrid(); }));
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            _mes.RetryQueueOnce(manual: true);
            RefreshQueueGrid();
        }

        private void SafeInvoke(Action action)
        {
            try
            {
                if (IsDisposed) return;
                if (InvokeRequired) BeginInvoke(action);
                else action();
            }
            catch (ObjectDisposedException) { }
        }
    }
}
