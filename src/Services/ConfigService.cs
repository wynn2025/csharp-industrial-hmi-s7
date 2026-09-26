using GaugeDemo300.Models;
using Newtonsoft.Json;

namespace GaugeDemo300.Services;

/// <summary>
/// 配置服务：JSON 配置文件加载/保存/备份 + 点位表批量生成/CSV 导入导出 + 多型号参数组管理
/// 配置文件位于程序目录 Config\SystemConfig.json，型号参数组位于 Config\Models\{型号名}.json
/// 默认点位按 PLC 实际接口生成：200 环控结果点（POINT_RESULT Int）+ 60 位移测量点（Measured_value Real）
/// </summary>
public class ConfigService
{
    /// <summary>默认环控结果点数量（穿孔检测 OK/NG，PLC POINT_RESULT[1..200]）</summary>
    public const int DefaultResultCount = 200;

    /// <summary>默认位移测量点数量（PLC Measured_value[1..60]，上位机界面最多支持 100）</summary>
    public const int DefaultAnalogCount = 60;

    /// <summary>默认气缸数量（PLC HMI_INTERFACE.swich[1..9]，界面最多支持 10）</summary>
    public const int DefaultCylinderCount = 9;

    public SystemConfig Config { get; private set; }

    private readonly string _configDir;
    private readonly string _configPath;
    private readonly string _modelDir;
    private readonly LogService _log;

    public ConfigService(LogService log)
    {
        _log = log;
        _configDir = Path.Combine(AppContext.BaseDirectory, "Config");
        _configPath = Path.Combine(_configDir, "SystemConfig.json");
        _modelDir = Path.Combine(_configDir, "Models");
        Config = Load();
        // 首次运行落盘默认配置（现场可直接编辑 JSON 调整 IP/地址/数量）
        if (!File.Exists(_configPath)) Save();
        RelinkAddresses();
        EnsureCylinderStateAddresses();   // 兜底：sv_state 反馈地址为空时自动补齐（手动画面颜色数据源）
        EnsureDefaultModel();
    }

    /// <summary>
    /// 起始地址配置变化时自动重排全部点位/气缸地址（保留名称与参数，仅重算 PLC 地址）。
    /// CSV 导入的自定义点位表在下次地址变更时也会按连续规则重排——现场自定义地址请勿改动起始地址配置。
    /// </summary>
    public void RelinkAddresses()
    {
        _log.Info($"地址重排检查：Gen=({Config.GenResultStart}|{Config.GenMeasureStart}|{Config.GenMeasureResultStart}|{Config.GenCylinderStart}) 配置=({Config.Plc.ResultStartAddress}|{Config.Plc.MeasureStartAddress}|{Config.Plc.MeasureResultStartAddress}|{Config.Plc.CylinderSwitchStartAddress})");
        if (Config.GenResultStart != Config.Plc.ResultStartAddress ||
            Config.GenMeasureStart != Config.Plc.MeasureStartAddress ||
            Config.GenMeasureResultStart != Config.Plc.MeasureResultStartAddress ||
            Config.GenCylinderStart != Config.Plc.CylinderSwitchStartAddress ||
            Config.GenCylinderStateStart != Config.Plc.CylinderStateStartAddress)
        {
            int ri = 0, ai = 0;
            foreach (var pt in Config.Points)
            {
                if (pt.Type == PointType.Result)
                {
                    pt.PlcAddress = S7AddressTool.NextIntAddress(Config.Plc.ResultStartAddress, ri++);
                }
                else if (pt.Type == PointType.Analog)
                {
                    pt.PlcAddress = S7AddressTool.NextRealAddress(Config.Plc.MeasureStartAddress, ai);
                    pt.ResultAddress = string.IsNullOrWhiteSpace(Config.Plc.MeasureResultStartAddress)
                        ? "" : S7AddressTool.NextIntAddress(Config.Plc.MeasureResultStartAddress, ai);
                    ai++;
                }
            }
            for (int i = 0; i < Config.Cylinders.Count; i++)
            {
                Config.Cylinders[i].SwitchAddress =
                    S7AddressTool.NextBitAddress(Config.Plc.CylinderSwitchStartAddress, i);
                // 气缸状态反馈（PLC_TO_HMI.svN_state 单状态位；独立双反馈配置会被覆盖）
                if (!string.IsNullOrWhiteSpace(Config.Plc.CylinderStateStartAddress))
                    Config.Cylinders[i].ForwardFeedbackAddress =
                        S7AddressTool.NextBitAddress(Config.Plc.CylinderStateStartAddress, i);
            }
            Config.GenResultStart = Config.Plc.ResultStartAddress;
            Config.GenMeasureStart = Config.Plc.MeasureStartAddress;
            Config.GenMeasureResultStart = Config.Plc.MeasureResultStartAddress;
            Config.GenCylinderStart = Config.Plc.CylinderSwitchStartAddress;
            Config.GenCylinderStateStart = Config.Plc.CylinderStateStartAddress;
            Save();
            _log.Info($"点位地址已按新起始地址重排（环控@{Config.Plc.ResultStartAddress} 位移@{Config.Plc.MeasureStartAddress} 判定@{Config.Plc.MeasureResultStartAddress} 气缸@{Config.Plc.CylinderSwitchStartAddress}）");
        }
    }

    private SystemConfig Load()
    {
        try
        {
            if (File.Exists(_configPath))
            {
                string json = File.ReadAllText(_configPath);
                var cfg = JsonConvert.DeserializeObject<SystemConfig>(json);
                if (cfg != null)
                {
                    Reindex(cfg.Points);
                    _log.Info($"配置加载成功：{_configPath}（点位 {cfg.Points.Count} 个：环控 {CountOf(cfg, PointType.Result)} / 位移 {CountOf(cfg, PointType.Analog)}；气缸 {cfg.Cylinders.Count} 个）");
                    return cfg;
                }
            }
        }
        catch (Exception ex)
        {
            _log.Error("配置加载失败，使用默认配置", ex);
        }
        return CreateDefault();
    }

    private static int CountOf(SystemConfig cfg, PointType t) => cfg.Points.Count(p => p.Type == t);

    /// <summary>保存配置（自动备份旧文件，保留最近 5 份）</summary>
    public void Save()
    {
        try
        {
            Directory.CreateDirectory(_configDir);
            if (File.Exists(_configPath))
            {
                string backupDir = Path.Combine(_configDir, "Backup");
                Directory.CreateDirectory(backupDir);
                string backup = Path.Combine(backupDir, $"SystemConfig_{DateTime.Now:yyyyMMdd_HHmmss}.json");
                File.Copy(_configPath, backup, true);
                foreach (var old in Directory.GetFiles(backupDir, "SystemConfig_*.json")
                             .OrderByDescending(f => f).Skip(5))
                {
                    try { File.Delete(old); } catch { }
                }
            }
            string json = JsonConvert.SerializeObject(Config, Formatting.Indented);
            File.WriteAllText(_configPath, json);
            _log.Info($"配置已保存：{_configPath}");
        }
        catch (Exception ex)
        {
            _log.Error("配置保存失败", ex);
        }
    }

    /// <summary>恢复默认配置（200 环控 + 60 位移 + 9 气缸）</summary>
    public void ResetToDefault()
    {
        Config = CreateDefault();
        Save();
    }

    /// <summary>
    /// 创建默认配置：按 PLC 实际 DB 接口生成
    /// 环控点 = POINT_RESULT[1..200] Int（默认 DBW 2 字节递增）
    /// 位移点 = Measured_value[1..60] Real + M_RESULT[1..60] Int 判定
    /// 气缸 = swich[1..9] 位连续
    /// </summary>
    public static SystemConfig CreateDefault()
    {
        var cfg = new SystemConfig();
        var plc = cfg.Plc;
        GeneratePoints(cfg.Points,
            DefaultResultCount, plc.ResultStartAddress,
            DefaultAnalogCount, plc.MeasureStartAddress, plc.MeasureResultStartAddress);
        GenerateCylinders(cfg.Cylinders, DefaultCylinderCount, plc.CylinderSwitchStartAddress, plc.CylinderStateStartAddress);
        return cfg;
    }

    /// <summary>
    /// 批量生成点位表（覆盖现有 Points）
    /// resultStart：环控结果起始 Int 地址（如 DB30.DBW0），每点 +2 字节
    /// analogStart：位移值起始 REAL 地址（如 DB2.DBD100），每点 +4 字节
    /// analogResultStart：位移判定结果起始 Int 地址（如 DB30.DBW400），可空
    /// </summary>
    public static void GeneratePoints(List<PointConfig> points, int resultCount,
        string resultStart, int analogCount, string analogStart, string analogResultStart)
    {
        points.Clear();
        for (int i = 0; i < resultCount; i++)
        {
            points.Add(new PointConfig
            {
                Name = $"D{i + 1:D3}",
                Type = PointType.Result,
                Zone = "环控检测",
                Description = $"穿孔检测点 {i + 1}",
                PlcAddress = S7AddressTool.NextIntAddress(resultStart, i),
                OkResultValue = 1,
                Unit = "-",
                Enabled = true
            });
        }
        for (int i = 0; i < analogCount; i++)
        {
            points.Add(new PointConfig
            {
                Name = $"A{i + 1:D3}",
                Type = PointType.Analog,
                Zone = "位移测量",
                Description = $"位移检测点 {i + 1}",
                PlcAddress = S7AddressTool.NextRealAddress(analogStart, i),
                ResultAddress = string.IsNullOrWhiteSpace(analogResultStart)
                    ? "" : S7AddressTool.NextIntAddress(analogResultStart, i),
                NominalValue = 0,
                UpperLimit = 0.05,
                LowerLimit = -0.05,
                UseNominal = true,
                Unit = "mm",
                Enabled = true
            });
        }
        Reindex(points);
    }

    /// <summary>批量生成气缸表（覆盖现有 Cylinders，开关位从起始地址连续递增）</summary>
    public static void GenerateCylinders(List<CylinderConfig> cylinders, int count, string switchStart,
        string stateStart = "")
    {
        cylinders.Clear();
        for (int i = 0; i < count; i++)
        {
            cylinders.Add(new CylinderConfig
            {
                Index = i + 1,
                Name = $"气缸{i + 1}",
                SwitchAddress = S7AddressTool.NextBitAddress(switchStart, i),
                // sv_state 反馈位（PLC_TO_HMI.svN_state）：手动画面按钮颜色的数据源，
                // 重建气缸时必须同步生成，否则颜色失效
                ForwardFeedbackAddress = string.IsNullOrWhiteSpace(stateStart)
                    ? "" : S7AddressTool.NextBitAddress(stateStart, i),
                Enabled = true
            });
        }
    }

    /// <summary>兜底自愈：任何气缸的 sv_state 反馈地址为空时按 CylinderStateStartAddress 补齐并保存。
    /// （历史配置/重建路径只写了开关地址，导致手动画面按钮颜色失效）</summary>
    public void EnsureCylinderStateAddresses()
    {
        if (string.IsNullOrWhiteSpace(Config.Plc.CylinderStateStartAddress)) return;
        bool changed = false;
        for (int i = 0; i < Config.Cylinders.Count; i++)
        {
            if (string.IsNullOrWhiteSpace(Config.Cylinders[i].ForwardFeedbackAddress))
            {
                Config.Cylinders[i].ForwardFeedbackAddress =
                    S7AddressTool.NextBitAddress(Config.Plc.CylinderStateStartAddress, i);
                changed = true;
            }
        }
        if (changed)
        {
            Save();
            _log.Info("气缸 sv_state 反馈地址已自动补齐（依据 CylinderStateStartAddress）");
        }
    }

    /// <summary>重排点位序号（按列表顺序连续编号 1..N）</summary>
    public static void Reindex(List<PointConfig> points)
    {
        for (int i = 0; i < points.Count; i++) points[i].Index = i + 1;
    }

    #region 多型号参数组管理

    /// <summary>型号参数文件路径（Config\Models\{型号名}.json）</summary>
    private string ModelPath(string modelName) => Path.Combine(_modelDir, modelName.Trim() + ".json");

    /// <summary>列出全部已保存型号名（按文件名）</summary>
    public List<string> ListModels()
    {
        var names = new List<string>();
        try
        {
            if (Directory.Exists(_modelDir))
                names.AddRange(Directory.GetFiles(_modelDir, "*.json").Select(Path.GetFileNameWithoutExtension));
        }
        catch { }
        return names.OrderBy(n => n).ToList();
    }

    /// <summary>确保默认型号参数文件存在（不存在时按当前点位参数创建）</summary>
    private void EnsureDefaultModel()
    {
        try
        {
            var models = ListModels();
            if (models.Count == 0)
            {
                var model = BuildModelFromPoints(string.IsNullOrEmpty(Config.CurrentModel) ? "默认型号" : Config.CurrentModel);
                SaveModel(model);
                Config.CurrentModel = model.ModelName;
                _log.Info($"已创建默认型号参数组：{model.ModelName}");
            }
        }
        catch (Exception ex)
        {
            _log.Error("初始化型号参数失败", ex);
        }
    }

    /// <summary>从当前点位表提取参数生成型号参数组</summary>
    public ProductModelConfig BuildModelFromPoints(string modelName)
    {
        var model = new ProductModelConfig
        {
            ModelName = modelName.Trim(),
            ProductName = Config.Mes.ProductName,
                DeviceCode = Config.Mes.DeviceCode,
                Note = Config.Mes.Note,
            ResultCount = Config.Points.Count(p => p.Type == PointType.Result),
            AnalogCount = Config.Points.Count(p => p.Type == PointType.Analog),
            CylinderCount = Config.Cylinders.Count
        };
        foreach (var p in Config.Points.Where(p => p.Type == PointType.Result))
            model.ResultPoints.Add(new ResultPointDef { Name = p.Name, Description = p.Description, OkValue = p.OkResultValue });
        foreach (var p in Config.Points.Where(p => p.Type == PointType.Analog))
            model.AnalogPoints.Add(new AnalogPointDef
            {
                Name = p.Name, Description = p.Description,
                NominalValue = p.NominalValue, UpperLimit = p.UpperLimit,
                LowerLimit = p.LowerLimit, UseNominal = p.UseNominal
            });
        foreach (var c in Config.Cylinders)
            model.Cylinders.Add(new CylinderDef { Name = c.Name, SwitchAddress = c.SwitchAddress });
        return model;
    }

    /// <summary>保存型号参数组</summary>
    public void SaveModel(ProductModelConfig model)
    {
        try
        {
            Directory.CreateDirectory(_modelDir);
            string json = JsonConvert.SerializeObject(model, Formatting.Indented);
            File.WriteAllText(ModelPath(model.ModelName), json);
            _log.Info($"型号参数已保存：{model.ModelName}（穿孔 {model.ResultCount} / 位移 {model.AnalogCount} / 气缸 {model.CylinderCount}）");
        }
        catch (Exception ex)
        {
            _log.Error($"保存型号参数失败：{model.ModelName}", ex);
        }
    }

    /// <summary>加载型号参数组（文件不存在返回 null）</summary>
    public ProductModelConfig LoadModel(string modelName)
    {
        try
        {
            string path = ModelPath(modelName);
            if (!File.Exists(path)) return null;
            var model = JsonConvert.DeserializeObject<ProductModelConfig>(File.ReadAllText(path));
            if (model == null) return null;
            // 旧格式检测：数量为 0 或点位列表为空 → 用当前配置重建
            if (model.ResultCount <= 0 || model.AnalogCount <= 0 || model.ResultPoints.Count == 0)
            {
                _log.Warn($"型号 {modelName} 是旧格式，自动用当前配置重建");
                model = BuildModelFromPoints(modelName);
                SaveModel(model);
            }
            return model;
        }
        catch (Exception ex)
        {
            _log.Error($"加载型号参数失败：{modelName}", ex);
            return null;
        }
    }

    /// <summary>删除型号参数组（不允许删除最后一个）</summary>
    public bool DeleteModel(string modelName)
    {
        if (ListModels().Count <= 1) return false;
        try
        {
            File.Delete(ModelPath(modelName));
            _log.Info($"型号已删除：{modelName}");
            return true;
        }
        catch (Exception ex)
        {
            _log.Error($"删除型号失败：{modelName}", ex);
            return false;
        }
    }

    /// <summary>
    /// 应用型号到当前点位表：覆盖位移点参数 + 环控点 OK 值 + MES 产品名，并保存配置
    /// </summary>
    public void ApplyModel(ProductModelConfig model)
    {
        if (model == null) return;
        Config.Points.Clear();
        for (int i = 0; i < model.ResultCount; i++)
        {
            var def = i < model.ResultPoints.Count ? model.ResultPoints[i] : null;
            Config.Points.Add(new PointConfig
            {
                Index = Config.Points.Count + 1,
                Name = def?.Name ?? $"D{i + 1:D3}",
                Type = PointType.Result, Zone = "穿孔检测",
                Description = def?.Description ?? $"穿孔检测点 {i + 1}",
                PlcAddress = S7AddressTool.NextIntAddress(Config.Plc.ResultStartAddress, i),
                OkResultValue = def?.OkValue ?? 1, Unit = "-", Enabled = true
            });
        }
        for (int i = 0; i < model.AnalogCount; i++)
        {
            var def = i < model.AnalogPoints.Count ? model.AnalogPoints[i] : null;
            Config.Points.Add(new PointConfig
            {
                Index = Config.Points.Count + 1,
                Name = def?.Name ?? $"A{i + 1:D3}",
                Type = PointType.Analog, Zone = "位移测量",
                Description = def?.Description ?? $"位移检测点 {i + 1}",
                PlcAddress = S7AddressTool.NextRealAddress(Config.Plc.MeasureStartAddress, i),
                ResultAddress = string.IsNullOrWhiteSpace(Config.Plc.MeasureResultStartAddress) ? "" : S7AddressTool.NextIntAddress(Config.Plc.MeasureResultStartAddress, i),
                NominalValue = def?.NominalValue ?? 0,
                UpperLimit = def?.UpperLimit ?? 0.05,
                LowerLimit = def?.LowerLimit ?? -0.05,
                UseNominal = def?.UseNominal ?? true, Unit = "mm", Enabled = true
            });
        }
        Reindex(Config.Points);
        Config.Cylinders.Clear();
        for (int i = 0; i < model.CylinderCount; i++)
        {
            var def = i < model.Cylinders.Count ? model.Cylinders[i] : null;
            Config.Cylinders.Add(new CylinderConfig
            {
                Index = i + 1,
                Name = def?.Name ?? $"气缸{i + 1}",
                SwitchAddress = !string.IsNullOrWhiteSpace(def?.SwitchAddress) ? def.SwitchAddress : S7AddressTool.NextBitAddress(Config.Plc.CylinderSwitchStartAddress, i),
                Enabled = true
            });
        }
        Config.CurrentModel = model.ModelName;
        Config.Mes.ProductName = string.IsNullOrEmpty(model.ProductName) ? model.ModelName : model.ProductName;
            if (!string.IsNullOrEmpty(model.DeviceCode)) Config.Mes.DeviceCode = model.DeviceCode;
            if (model.Note != null) Config.Mes.Note = model.Note;
        Save();
        _log.Info($"型号已应用：{model.ModelName}（穿孔 {model.ResultCount} / 位移 {model.AnalogCount} / 气缸 {model.CylinderCount}）");
    }

    #endregion

    #region 点位表 CSV 导入导出（现场可用 Excel 批量编辑点位）

    /// <summary>CSV 列头</summary>
    public static readonly string[] CsvHeader =
        { "序号", "点位", "类型", "区域", "说明", "PLC地址", "OK值", "结果地址", "名义值", "下限", "上限", "单位", "启用" };

    /// <summary>导出点位表 CSV（UTF-8 BOM，Excel 直接打开）</summary>
    public void ExportCsv(string path)
    {
        var lines = new List<string> { string.Join(",", CsvHeader) };
        foreach (var p in Config.Points)
        {
            string type = p.Type switch
            {
                PointType.Result => "环控",
                PointType.Analog => "位移",
                _ => "位点"
            };
            lines.Add(string.Join(",",
                p.Index, p.Name, type,
                EscapeCsv(p.Zone), EscapeCsv(p.Description), p.PlcAddress,
                p.Type == PointType.Result ? p.OkResultValue.ToString() : p.ExpectedState ? "1" : "0",
                p.ResultAddress,
                p.NominalValue.ToString("G6"), p.LowerLimit.ToString("G6"), p.UpperLimit.ToString("G6"),
                p.Unit, p.Enabled ? "1" : "0"));
        }
        File.WriteAllLines(path, lines, new System.Text.UTF8Encoding(true));
        _log.Info($"点位表已导出：{path}（{Config.Points.Count} 点）");
    }

    /// <summary>导入点位表 CSV（覆盖当前点位表；格式错误行跳过并告警）</summary>
    public (int ok, int skip) ImportCsv(string path)
    {
        var points = new List<PointConfig>();
        int skip = 0;
        string[] lines = File.ReadAllLines(path);
        for (int i = 1; i < lines.Length; i++)   // 跳过列头
        {
            string line = lines[i].Trim();
            if (line.Length == 0) { skip++; continue; }
            string[] f = SplitCsv(line);
            if (f.Length < 6) { skip++; _log.Warn($"CSV 第 {i + 1} 行列数不足，跳过"); continue; }
            try
            {
                var p = new PointConfig
                {
                    Name = f[1].Trim(),
                    Type = f[2].Trim() switch
                    {
                        "位移" => PointType.Analog,
                        "位点" => PointType.Digital,
                        _ => PointType.Result
                    },
                    Zone = f[3].Trim(),
                    Description = f[4].Trim(),
                    PlcAddress = f[5].Trim()
                };
                if (f.Length > 6 && int.TryParse(f[6].Trim(), out int okv)) p.OkResultValue = okv;
                if (f.Length > 7) p.ResultAddress = f[7].Trim();
                if (f.Length > 8 && double.TryParse(f[8].Trim(), out double nom)) p.NominalValue = nom;
                if (f.Length > 9 && double.TryParse(f[9].Trim(), out double lo)) p.LowerLimit = lo;
                if (f.Length > 10 && double.TryParse(f[10].Trim(), out double hi)) p.UpperLimit = hi;
                if (f.Length > 11) p.Unit = f[11].Trim();
                if (f.Length > 12) p.Enabled = f[12].Trim() != "0";
                if (string.IsNullOrWhiteSpace(p.Name) || string.IsNullOrWhiteSpace(p.PlcAddress)) { skip++; continue; }
                points.Add(p);
            }
            catch
            {
                skip++;
                _log.Warn($"CSV 第 {i + 1} 行解析失败，跳过");
            }
        }
        if (points.Count > 0)
        {
            Reindex(points);
            Config.Points = points;
            Save();
            _log.Info($"点位表已导入：{path}（成功 {points.Count}，跳过 {skip}）");
        }
        return (points.Count, skip);
    }

    private static string EscapeCsv(string s)
        => s.Contains(',') || s.Contains('"') ? $"\"{s.Replace("\"", "\"\"")}\"" : s;

    private static string[] SplitCsv(string line)
    {
        var result = new List<string>();
        bool inQuote = false;
        var cur = new System.Text.StringBuilder();
        foreach (char c in line)
        {
            if (c == '"') inQuote = !inQuote;
            else if (c == ',' && !inQuote) { result.Add(cur.ToString()); cur.Clear(); }
            else cur.Append(c);
        }
        result.Add(cur.ToString());
        return result.ToArray();
    }

    #endregion
}
