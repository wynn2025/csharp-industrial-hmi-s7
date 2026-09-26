namespace GaugeDemo300.Models;

/// <summary>
/// 单条检测记录（对应 MES 过站同步接口一次上报）
/// 字段与 MES sync 接口入参对应：code/equipment/operationTime/operator/productName/note/point1-10/result1-5
/// </summary>
public class MeasurementRecord
{
    public long Id { get; set; }

    /// <summary>产品码（MES 字段 code）</summary>
    public string ProductCode { get; set; } = "";

    /// <summary>设备编码（MES 字段 equipment，配置）</summary>
    public string DeviceCode { get; set; } = "";

    /// <summary>操作人（MES 字段 operator，登录用户）</summary>
    public string Operator { get; set; } = "";

    /// <summary>产品名称（MES 字段 productName，型号配置）</summary>
    public string ProductName { get; set; } = "";

    /// <summary>产品型号（本机记录用）</summary>
    public string ProductModel { get; set; } = "";

    /// <summary>过站时间（MES 字段 operationTime）</summary>
    public DateTime StationTime { get; set; } = DateTime.Now;

    /// <summary>备注（MES 字段 note）</summary>
    public string Note { get; set; } = "";

    /// <summary>总结果（MES 字段 result1：OK/NG）</summary>
    public string OverallResult { get; set; } = "NG";

    /// <summary>OK 点位列表（MES 字段 point1）</summary>
    public string OkPoints { get; set; } = "";

    /// <summary>NG 点位列表（MES 字段 point2）</summary>
    public string NgPoints { get; set; } = "";

    /// <summary>测量值 1..8（MES 字段 point3..point10）</summary>
    public string[] MeasurePoints { get; set; } = new string[8];

    /// <summary>结果 2..5（MES 字段 result2..result5，备用）</summary>
    public string[] ExtraResults { get; set; } = new string[4];

    /// <summary>穿孔检测结果原始值（逗号分隔 0/1，200 点）</summary>
    public string PerforationResults { get; set; } = "";

    /// <summary>位移测量上下限（逗号分隔 "上1,下1,上2,下2,..."，60 点）</summary>
    public string DisplacementLimits { get; set; } = "";

    /// <summary>位移测量值原始值（逗号分隔浮点数，60 点）</summary>
    public string DisplacementValues { get; set; } = "";

    /// <summary>全部点位测量明细（JSON 数组：[{name,description,zone,type,value,result}]）</summary>
    public string ChannelValuesJson { get; set; } = "[]";

    /// <summary>是否已成功同步 MES</summary>
    public bool SyncSuccess { get; set; }

    /// <summary>最近同步时间</summary>
    public DateTime? SyncTime { get; set; }

    /// <summary>记录创建时间</summary>
    public DateTime CreateTime { get; set; } = DateTime.Now;
}

/// <summary>过站校验记录（checkProcessLeak 调用结果）</summary>
public class StationCheckRecord
{
    public long Id { get; set; }
    public string ProductCode { get; set; } = "";
    public DateTime CheckTime { get; set; } = DateTime.Now;
    public string Code { get; set; } = "";       // 200 成功 / 其他失败
    public string Msg { get; set; } = "";
    public bool Passed { get; set; }
}

/// <summary>MES 同步失败队列项（离线自动补传）</summary>
public class SyncQueueItem
{
    public long Id { get; set; }
    public long RecordId { get; set; }
    public string ProductCode { get; set; } = "";
    public DateTime CreateTime { get; set; } = DateTime.Now;
    public string Status { get; set; } = "PENDING";   // PENDING/FAILED/SENT
    public int RetryCount { get; set; }
    public string LastError { get; set; } = "";
    public DateTime? LastTryTime { get; set; }
}

/// <summary>软件报警项（报警画面显示）</summary>
public class AlarmItem
{
    public DateTime Time { get; set; } = DateTime.Now;
    /// <summary>报警来源（PLC/MES/系统）</summary>
    public string Source { get; set; } = "";
    /// <summary>报警内容</summary>
    public string Message { get; set; } = "";
    /// <summary>级别（WARN/ERROR）</summary>
    public string Level { get; set; } = "WARN";
    /// <summary>是否已确认</summary>
    public bool Acknowledged { get; set; }
}

/// <summary>操作员信息</summary>
public class UserInfo
{
    public string Id { get; set; } = "";      // 工号
    public string Name { get; set; } = "";    // 姓名
    public string Role { get; set; } = "operator"; // operator / engineer
}
