namespace GaugeDemo300.Models;

/// <summary>PLC 通讯配置（西门子 S7 以太网）</summary>
public class PlcConfig
{
    /// <summary>PLC IP 地址（可配置，现场 192.168.0.10）</summary>
    public string IpAddress { get; set; } = "192.168.0.10";

    /// <summary>PLC CPU 类型（S7300 / S7400 / S71200 / S71500，可配置）</summary>
    public string CpuType { get; set; } = "S71200";

    /// <summary>机架号（S7-1200/1500 为 0）</summary>
    public int Rack { get; set; } = 0;

    /// <summary>槽位号（S7-1200/1500 为 1，S7-300 一般为 2）</summary>
    public int Slot { get; set; } = 1;

    /// <summary>点位轮询周期（毫秒）。批量区域读取下单周期仅 4~6 次报文交互</summary>
    public int PollIntervalMs { get; set; } = 200;

    /// <summary>断线重连间隔（毫秒）</summary>
    public int ReconnectIntervalMs { get; set; } = 5000;

    /// <summary>心跳寄存器地址（PLC_TO_HMI.watch_dog@DB5.DBW6，PLC 递增，值变化即存活；空=仅显示连接状态）</summary>
    public string HeartbeatAddress { get; set; } = "DB5.DBW6";

    /// <summary>启动信号地址（PC→PLC，对应 WORKING.START 位；DB 块号现场配置）</summary>
    public string StartAddress { get; set; } = "DB4.DBX0.1";

    /// <summary>停止信号地址（PC→PLC，停止检测时写 true；WORKING 无独立停止位，UDT 内待定）</summary>
    public string StopAddress { get; set; } = "";

    /// <summary>复位信号地址（PC→PLC，对应 WORKING.RESET 位，写脉冲）</summary>
    public string ResetAddress { get; set; } = "DB4.DBX0.2";

    /// <summary>检测完成位地址（PLC_TO_HMI.test_success@DB5.DBX10.0，上升沿触发检测完成）</summary>
    public string DoneAddress { get; set; } = "DB5.DBX10.0";

    /// <summary>数据清除信号地址（PLC→PC，DB5.DBX4.3 delete_data 上升沿=清除所有测量结果和值）</summary>
    public string DeleteDataAddress { get; set; } = "DB5.DBX4.3";

    /// <summary>声光报警输出地址（PC→PLC，过站拦截时写 true）</summary>
    public string AlarmAddress { get; set; } = "DB4.DBX1.0";

    /// <summary>自动运行状态地址（PLC→PC，对应 WORKING.AUTO_RUN 位，可空）</summary>
    public string AutoRunAddress { get; set; } = "DB4.DBX1.1";

    /// <summary>手动模式地址（PC↔PLC，对应 WORKING.MANUAL 位，可空）</summary>
    public string ManualModeAddress { get; set; } = "DB4.DBX0.0";

    // ===== 以下默认值为占位：TIA 导出的 DB 源不含块号，现场拿到实际块号后改配置 =====

    /// <summary>环控结果数组起始地址（RESULT_HMI_READ.POINT_RESULT[1]，Int 2 字节递增）</summary>
    public string ResultStartAddress { get; set; } = "DB3.DBW0";

    /// <summary>位移测量值数组起始地址（HMI_INTERFACE.Measured_value[1]，REAL 4 字节递增；UDT 偏移待确认）</summary>
    public string MeasureStartAddress { get; set; } = "DB5.DBD12";

    /// <summary>位移判定结果数组起始地址（RESULT_HMI_READ.M_RESULT[1]，Int 2 字节递增，可空=仅本地判定）</summary>
    public string MeasureResultStartAddress { get; set; } = "DB3.DBW400";

    /// <summary>上限公差数组起始地址（MAX_TOL.UP_TOL[1]，REAL；切换型号时下发）</summary>
    public string MaxTolStartAddress { get; set; } = "DB13.DBD4";

    /// <summary>下限公差数组起始地址（MIN_TOL.LOW_TOL[1]，REAL；切换型号时下发）</summary>
    public string MinTolStartAddress { get; set; } = "DB8.DBD4";

    /// <summary>气缸状态反馈起始地址（PLC_TO_HMI.sv1_state@DB5.DBX10.1，Bool 连续 9 位；=1 动作中 / =0 复位）</summary>
    public string CylinderStateStartAddress { get; set; } = "DB5.DBX10.1";

    /// <summary>气缸动作开关数组起始地址（HMI_INTERFACE.swich[1]，Bool 位连续）</summary>
    public string CylinderSwitchStartAddress { get; set; } = "DB5.DBX4.4";

    /// <summary>条码确认信号地址（HMI_TO_PLC.code_ack@DB5.DBW2 Int，MES过站OK写1，测量完成复位0）</summary>
    public string CodeAckAddress { get; set; } = "DB5.DBW2";

    /// <summary>PLC 自动模式位（DB5.DBX4.0，由 PLC 上的开关决定，上位机只读）</summary>
    public string PlcAutoModeAddress { get; set; } = "DB5.DBX4.0";

    /// <summary>PLC 手动模式位（DB5.DBX4.1，由 PLC 上的开关决定，上位机只读）</summary>
    public string PlcManualModeAddress { get; set; } = "DB5.DBX4.1";

    /// <summary>PLC 报警位数组起始地址（ALARM.P_ALARM[1..200]，Bool 位连续）。
    /// ⚠ ALARM 块必须取消"优化的块访问"（S7_Optimized_Access='FALSE'）才能绝对寻址</summary>
    public string PlcAlarmStartAddress { get; set; } = "DB9.DBX0.0";

    /// <summary>PLC 报警位数量（对应 ALARM.P_ALARM[1..N]）</summary>
    public int PlcAlarmCount { get; set; } = 200;

    /// <summary>切换型号时是否把上下限公差写入 PLC（MAX_TOL/MIN_TOL）</summary>
    public bool WriteToleranceOnModelChange { get; set; } = true;

    /// <summary>无 PLC 硬件时使用模拟数据（调试用）</summary>
    public bool UseSimulator { get; set; } = false;

    // ===== HMI_INTERFACE(DB5) 协议信号（UDT 定义 2026-08-30） =====

    /// <summary>PC 看门狗地址（HMI_TO_PLC.watch_dog@DB5.DBW0，上位机每秒递增写）</summary>
    public string PcWatchdogAddress { get; set; } = "DB5.DBW0";

    /// <summary>上位机在线位（HMI_TO_PLC.on_line@DB5.DBX4.0，上线写 1）</summary>
    public string PcOnlineAddress { get; set; } = "DB5.DBX4.0";

    /// <summary>上位机离线位（HMI_TO_PLC.off_line@DB5.DBX4.1，下线写 1，与在线互斥）</summary>
    public string PcOfflineAddress { get; set; } = "DB5.DBX4.1";

    /// <summary>设备状态地址（PLC_TO_HMI.Equipment@DB5.DBW8：1=IDLE 2=READY 3=RUNNING 4=ERROR）</summary>
    public string EquipmentStateAddress { get; set; } = "DB5.DBW8";
}

/// <summary>MES 通讯配置</summary>
public class MesConfig
{
    /// <summary>过站校验接口地址（上位机软件可配置）</summary>
    public string CheckUrl { get; set; } = "http://192.168.0.20:8080/mes/trace/checkProcessLeak";

    /// <summary>过站信息同步接口地址（上位机软件可配置）</summary>
    public string SyncUrl { get; set; } = "http://192.168.0.20:8080/mes/trace/sync";

    /// <summary>设备编码（上位机软件可配置）</summary>
    public string DeviceCode { get; set; } = "AWZ-DCM-5003";

    /// <summary>操作人（登录后自动更新）</summary>
    public string Operator { get; set; } = "";

    /// <summary>产品名称（默认值，型号加载后可被型号配置覆盖）</summary>
    public string ProductName { get; set; } = "";

    /// <summary>备注信息</summary>
    public string Note { get; set; } = "";

    /// <summary>请求超时（毫秒）</summary>
    public int TimeoutMs { get; set; } = 2000;   // MES HTTP 超时：过长会拖慢重传轮次

    /// <summary>同步失败自动重试间隔（毫秒）</summary>
    public int RetryIntervalMs { get; set; } = 60000;

    /// <summary>是否启用失败自动重试</summary>
    public bool EnableAutoRetry { get; set; } = true;
}

/// <summary>系统总配置</summary>
public class SystemConfig
{
    /// <summary>设备名称</summary>
    public string DeviceName { get; set; } = "电子检具";

    /// <summary>检具工号</summary>
    public string GaugeNo { get; set; } = "GaugeDemo-300";

    /// <summary>检具布局图路径（主画面大图，可配置；留空显示占位提示）</summary>
    public string LayoutImagePath { get; set; } = "Config\\layout.jpg";

    /// <summary>当前产品型号名称（对应 Config\Models\{型号}.json）</summary>
    public string CurrentModel { get; set; } = "默认型号";

    /// <summary>PLC 配置</summary>
    public PlcConfig Plc { get; set; } = new();

    /// <summary>MES 配置</summary>
    public MesConfig Mes { get; set; } = new();


    // ===== 点位表生成记录（起始地址变化时自动重排全部点位地址，见 ConfigService.RelinkAddresses） =====
    public string GenResultStart { get; set; } = "";
    public string GenMeasureStart { get; set; } = "";
    public string GenMeasureResultStart { get; set; } = "";
    public string GenCylinderStart { get; set; } = "";
    public string GenCylinderStateStart { get; set; } = "";

    /// <summary>检测点位配置（默认 200 环控数字点 + 100 位移测量点 = 300 点位）</summary>
    public List<PointConfig> Points { get; set; } = new();

    /// <summary>气缸配置（最多 10 个，手动画面点动控制）</summary>
    public List<CylinderConfig> Cylinders { get; set; } = new();

    /// <summary>SQLite 数据库文件路径（相对程序目录）</summary>
    public string DatabasePath { get; set; } = "data/gauge.db";

    /// <summary>测量数据保留天数（超期自动清理）</summary>
    public int DataRetentionDays { get; set; } = 180;

    /// <summary>扫码后是否自动调用 MES 过站校验</summary>
    public bool AutoStartCheck { get; set; } = true;

    /// <summary>过站拦截时是否声光报警</summary>
    public bool AlarmOnBlock { get; set; } = true;

    /// <summary>检测完成后是否自动复位等待下一件</summary>
    public bool AutoResetAfterDone { get; set; } = false;

    /// <summary>工程师登录密码（设置界面）</summary>
    public string EngineerPassword { get; set; } = "123456";

    /// <summary>环控数字点位数量</summary>
    public int DigitalCount => Points.Count(p => p.Type == PointType.Digital);

    /// <summary>位移点位数量</summary>
    public int AnalogCount => Points.Count(p => p.Type == PointType.Analog);
}
