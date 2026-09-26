using Newtonsoft.Json;

namespace GaugeDemo300.Models;

/// <summary>点位类型</summary>
public enum PointType
{
    /// <summary>环控结果点：穿孔检测 OK/NG（POINT_RESULT[1..200] Int），无测量值，值==OK值(默认1)判定</summary>
    Result = 0,

    /// <summary>位移测量点：位移传感器 REAL 值（Measured_value[1..60]），名义值±公差判定 + PLC 结果 M_RESULT</summary>
    Analog = 1,

    /// <summary>数字位点：BOOL 位读取按期望状态判定（兼容保留，本检具默认不用）</summary>
    Digital = 2
}

/// <summary>
/// 检测点位配置（默认 200 环控结果点 + 60 位移测量点，数量/地址全部可配置，位移最多支持 100）
/// 环控点：Int 结果数组（默认 DBW 2 字节递增），PLC 侧判定
/// 位移点：REAL 测量值（每型号参数组判定）+ 可选 Int 判定结果地址（PLC 判定优先）
/// </summary>
public class PointConfig
{
    /// <summary>点位序号 1..N（环控点与位移点统一连续编号）</summary>
    public int Index { get; set; }

    /// <summary>点位名称（环控点 D001..D200，位移点 A001..A060）</summary>
    public string Name { get; set; } = "";

    /// <summary>点位类型</summary>
    public PointType Type { get; set; } = PointType.Result;

    /// <summary>区域/分组（如 A区、B区、位移测量，用于界面筛选与分组统计）</summary>
    public string Zone { get; set; } = "检测区";

    /// <summary>检测点位说明</summary>
    public string Description { get; set; } = "";

    /// <summary>PLC 读取地址：结果点为 Int 地址（DBW），位移点为 REAL 地址（DBD），位点为位地址（DBX）</summary>
    public string PlcAddress { get; set; } = "";

    // ===== 环控结果点参数 =====

    /// <summary>结果点 OK 值（PLC Int 结果等于该值判定 OK，默认 1；现场按 PLC 编码配置）</summary>
    public int OkResultValue { get; set; } = 1;

    // ===== 位移点参数 =====

    /// <summary>位移点 PLC 判定结果地址（M_RESULT 数组，可选；配置后以 PLC 判定优先显示）</summary>
    public string ResultAddress { get; set; } = "";

    /// <summary>名义值（基准值，mm）</summary>
    public double NominalValue { get; set; } = 0;

    /// <summary>判定上限（mm，相对名义值或绝对值，由 UseNominal 决定）</summary>
    public double UpperLimit { get; set; } = 0.05;

    /// <summary>判定下限（mm）</summary>
    public double LowerLimit { get; set; } = -0.05;

    /// <summary>是否以名义值为中心判定（true: 名义值+上下限；false: 直接按上下限绝对值）</summary>
    public bool UseNominal { get; set; } = true;

    /// <summary>单位（位移点 mm，结果点 -）</summary>
    public string Unit { get; set; } = "mm";

    /// <summary>点位是否启用</summary>
    public bool Enabled { get; set; } = true;

    /// <summary>最近一次读取的原始值（运行时缓存，不序列化）：结果点 Int 值，位移点 mm，位点 0/1；NaN=无数据</summary>
    [JsonIgnore]
    public double Value { get; set; } = double.NaN;

    /// <summary>位点原始信号状态（运行时缓存，不序列化）</summary>
    [JsonIgnore]
    public bool RawState { get; set; }

    /// <summary>位移点 PLC 判定结果（运行时缓存，不序列化）：OK/NG/""=未读到</summary>
    [JsonIgnore]
    public string PlcResult { get; set; } = "";

    /// <summary>是否已取到有效数据（运行时缓存，不序列化）</summary>
    [JsonIgnore]
    public bool HasData { get; set; }

    /// <summary>判定单点位结果：OK / NG（无数据按 NG 处理，保证防错）</summary>
    public string Judge(double value)
    {
        if (!Enabled || double.IsNaN(value)) return "NG";
        switch (Type)
        {
            case PointType.Result:
                // 结果点：Int 值等于 OK 值判定 OK（穿孔检测：气缸穿孔通过=OK）
                return Math.Abs(value - OkResultValue) < 0.5 ? "OK" : "NG";
            case PointType.Digital:
                bool state = value >= 0.5;
                return state == ExpectedState ? "OK" : "NG";
            default:
            {
                double lo = UseNominal ? NominalValue + LowerLimit : LowerLimit;
                double hi = UseNominal ? NominalValue + UpperLimit : UpperLimit;
                return value >= lo && value <= hi ? "OK" : "NG";
            }
        }
    }

    /// <summary>最终判定：位移点优先取 PLC 判定结果（配了 ResultAddress 且读到时）</summary>
    public string FinalJudge()
    {
        if (Type == PointType.Analog && !string.IsNullOrEmpty(ResultAddress) && PlcResult.Length > 0)
            return PlcResult;
        return Judge(Value);
    }

    // ===== 位点参数（兼容保留） =====

    /// <summary>位点期望状态（true=信号为 1 时 OK）</summary>
    public bool ExpectedState { get; set; } = true;
}
