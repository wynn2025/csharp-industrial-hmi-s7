namespace GaugeDemo300.Models;

/// <summary>
/// 产品型号完整配置（配方）：所有参数跟随型号走
/// 存 Config\Models\{型号名}.json，切换型号时整体加载应用
/// </summary>
public class ProductModelConfig
{
    /// <summary>型号名称（= 参数文件名）</summary>
    public string ModelName { get; set; } = "默认型号";

    /// <summary>MES 过站同步 productName 字段值</summary>
    public string ProductName { get; set; } = "";

    /// <summary>MES 设备编码（过站校验/同步的 deviceCode/equipment）</summary>
    public string DeviceCode { get; set; } = "";

    /// <summary>MES 过站同步备注</summary>
    public string Note { get; set; } = "";

    /// <summary>穿孔检测点数量</summary>
    public int ResultCount { get; set; } = 200;

    /// <summary>位移测量点数量</summary>
    public int AnalogCount { get; set; } = 60;

    /// <summary>气缸数量</summary>
    public int CylinderCount { get; set; } = 9;

    /// <summary>穿孔检测点定义（名称+说明，按序号对应 PLC POINT_RESULT[i]）</summary>
    public List<ResultPointDef> ResultPoints { get; set; } = new();

    /// <summary>位移测量点定义（名称+说明+上下限，按序号对应 PLC Measured_value[i]）</summary>
    public List<AnalogPointDef> AnalogPoints { get; set; } = new();

    /// <summary>气缸定义（名称，按序号对应 PLC sv[i]_switch）</summary>
    public List<CylinderDef> Cylinders { get; set; } = new();
}

/// <summary>穿孔检测点定义</summary>
public class ResultPointDef
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    /// <summary>OK 值（PLC Int 结果等于该值判定 OK）</summary>
    public int OkValue { get; set; } = 1;
}

/// <summary>位移测量点定义（含上下限）</summary>
public class AnalogPointDef
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public double NominalValue { get; set; }
    public double UpperLimit { get; set; } = 0.05;
    public double LowerLimit { get; set; } = -0.05;
    public bool UseNominal { get; set; } = true;
}

/// <summary>气缸定义</summary>
public class CylinderDef
{
    public string Name { get; set; } = "";
    public string SwitchAddress { get; set; } = "";
}
