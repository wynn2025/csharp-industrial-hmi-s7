# C# 工控上位机通讯框架（西门子 PLC + MES + SQLite）

从真实产线电子检具项目完整脱敏而来的 C# WinForms 上位机源码：300 点位 S7 批量采集优化、过站防错状态机、MES 断线补传队列、无硬件模拟降级，全部是产线上跑过的工程代码，中文注释逐条到位。

技术栈：.NET Framework 4.8（WinForms）+ S7netplus 0.20.0 + HttpClient + SQLite + Newtonsoft.Json 13.0.3。全部依赖为 NuGet 包，不含第三方 DLL 二进制，还原即可编译。

## 目录结构

```
.
├── README.md            # 本文件
├── 设计说明.md           # 分层架构与关键机制讲解
└── src/
    ├── GaugeDemo.sln / GaugeDemo.csproj   # VS2019+ 直接打开
    ├── App.config / Program.cs / S7Var.cs
    ├── Forms/            # FormMain、FrmLogin 及 UcPage 系列分页控件（报警/历史/手动/MES 等）
    ├── Models/           # ProductModel、SystemConfig、MeasurementModels、PointConfig、CylinderConfig、PointOverlayConfig
    ├── Services/         # PlcService、MesService、DatabaseService、ConfigService、LogService、StationFlowService
    └── Properties/
```

分层：Models（数据模型）→ Services（业务与通讯服务，单向依赖）→ Forms（界面只消费服务事件）。

## FAQ

1. **没有 PLC 硬件能跑起来吗？** 能。源码内置无硬件模拟降级，不接 PLC/MES 也能完整走通界面与业务流程。
2. **支持哪些 PLC、点位怎么改？** 西门子 S7-1200（以太网 PROFINET，S7netplus）；点位在 JSON 配置文件与型号参数组中维护，界面点位叠加可编辑拖拽，换机型只改配置不改代码。
3. **MES 连不上会怎样？** 内置断线补传队列，网络恢复后自动补齐过站数据，不丢单。
4. **需要什么编译环境？** VS2019 及以上、.NET Framework 4.8；首次打开后 NuGet 还原，无需任何手动 DLL 配置。
5. **能二次开发或商用吗？** 完整源码 + 全中文注释 + 设计说明，二次开发指南随包提供；商用授权范围见购买指引。

## 样章导引

本 repo 为精简展示版。完整 62 页逐模块讲解与全套源码（含 CHANGELOG、10 分钟上手指南、常见问题文档）见购买指引所指渠道；样章（设计说明摘录）随 repo 一并可见。

## 购买指引

- 面包多（全套源码 + 62 页讲解）：`【待回填：面包多 P1 商品页链接】`
- CSDN（相关博文/资源）：`【待回填：CSDN 首发博文链接】`
