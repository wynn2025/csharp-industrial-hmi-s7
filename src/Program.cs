using GaugeDemo300.Forms;
using GaugeDemo300.Services;

namespace GaugeDemo300
{
    internal static class Program
    {
        /// <summary>
        /// 电子检具上位机（300 点位：200 环控数字 + 100 位移）
        /// 改造自吊车控制工程：入口初始化 配置/数据库/PLC/MES/过站防错 服务，启动主界面
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 单实例保护：双开会导致两个进程同时轮询 PLC（通信量翻倍把 S7-1200 打满）
            // 并抢写 code_ack 等互斥信号，流程错乱。检测到已运行直接提示退出。
            using var single = new System.Threading.Mutex(true, @"Global\GaugeDemo_SingleInstance", out bool first);
            if (!first)
            {
                MessageBox.Show("程序已在运行，请勿重复启动！\n\n（双开会导致 PLC 通讯过载、流程信号错乱）",
                    "电子检具上位机", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // DPI 感知由 App.config（SystemAware）声明，net48 无 SetHighDpiMode API

            var log = new LogService(AppContext.BaseDirectory);

            // 全局异常捕获：记录退出原因
            Application.ThreadException += (_, e) =>
            {
                log.Error("UI 线程未处理异常: " + e.Exception.ToString());
                MessageBox.Show($"程序发生未处理异常：{e.Exception.Message}\n\n{e.Exception.StackTrace}",
                    "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
            AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            {
                log.Error("AppDomain 未处理异常: " + (e.ExceptionObject as Exception)?.ToString());
            };

            var configService = new ConfigService(log);
            var cfg = configService.Config;

            string dbPath = Path.IsPathRooted(cfg.DatabasePath)
                ? cfg.DatabasePath
                : Path.Combine(AppContext.BaseDirectory, cfg.DatabasePath);
            using var db = new DatabaseService(dbPath, log);
            using var plc = new PlcService(cfg, log);
            using var mes = new MesService(cfg, db, log);

            // 启动时清理过期数据
            db.CleanupOldData(cfg.DataRetentionDays);

            var flow = new StationFlowService(cfg, configService, plc, mes, db, log);

            plc.Start();
            mes.StartAutoRetry();

            using var main = new FormMain(cfg, configService, db, plc, mes, flow, log);
            main.FormClosing += (_, e) => log.Info($"主窗体正在关闭：{e.CloseReason}");
            Application.Run(main);

            log.Info("程序退出");
        }
    }
}
