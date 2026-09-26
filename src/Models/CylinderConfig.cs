using Newtonsoft.Json;

namespace GaugeDemo300.Models;

/// <summary>
/// 气缸配置（PLC 接口：HMI_INTERFACE 内 swich 开关，9 组，写 1=动作 / 写 0=复位）
/// 手动画面：每缸一对按钮（动作 / 复位），数量/名称/开关地址全部可配置，最多 10 个
/// 反馈到位地址可选：纳入 PLC 批量读取计划，实时显示到位状态灯
/// </summary>
public class CylinderConfig
{
    /// <summary>气缸序号 1..N（默认 9 个）</summary>
    public int Index { get; set; }

    /// <summary>气缸名称（可配置，如 "压紧气缸1"）</summary>
    public string Name { get; set; } = "气缸";

    /// <summary>动作开关地址（PC→PLC swich：写 1=动作，写 0=复位）</summary>
    public string SwitchAddress { get; set; } = "";

    /// <summary>动作到位反馈地址（PLC→PC，可选）</summary>
    public string ForwardFeedbackAddress { get; set; } = "";

    /// <summary>复位到位反馈地址（PLC→PC，可选）</summary>
    public string BackwardFeedbackAddress { get; set; } = "";

    /// <summary>是否启用</summary>
    public bool Enabled { get; set; } = true;

    /// <summary>当前开关状态（运行时缓存，不序列化；true=动作中）</summary>
    [JsonIgnore]
    public bool SwitchState { get; set; }

    /// <summary>动作到位状态（运行时缓存，不序列化）</summary>
    [JsonIgnore]
    public bool ForwardArrived { get; set; }

    /// <summary>复位到位状态（运行时缓存，不序列化）</summary>
    [JsonIgnore]
    public bool BackwardArrived { get; set; }
}
