using System.Xml.Linq;

namespace GaugeDemo300.Models;

/// <summary>设备图点位标注配置（单个标注的位置）</summary>
public class PointOverlayConfig
{
    /// <summary>关联的点位名称（D001/A001...）</summary>
    public string PointName { get; set; } = "";
    /// <summary>图片上的 X 坐标（像素，相对于 panelImage）</summary>
    public int X { get; set; }
    /// <summary>图片上的 Y 坐标</summary>
    public int Y { get; set; }
}

/// <summary>设备图标注布局（一个型号一份 XML）</summary>
public class LayoutConfig
{
    public string ModelName { get; set; } = "";
    public List<PointOverlayConfig> Overlays { get; set; } = new();
}

/// <summary>
/// 标注布局 XML 读写服务
/// 文件：Config\Layouts\{型号名}.xml
/// </summary>
public static class LayoutService
{
    private static string LayoutDir => Path.Combine(AppContext.BaseDirectory, "Config", "Layouts");
    private static string LayoutPath(string modelName) => Path.Combine(LayoutDir, modelName.Trim() + ".xml");

    public static LayoutConfig Load(string modelName)
    {
        try
        {
            string path = LayoutPath(modelName);
            if (!File.Exists(path)) return new LayoutConfig { ModelName = modelName };
            var doc = XDocument.Load(path);
            var cfg = new LayoutConfig { ModelName = modelName };
            foreach (var el in doc.Descendants("Overlay"))
            {
                cfg.Overlays.Add(new PointOverlayConfig
                {
                    PointName = (string?)el.Attribute("PointName") ?? "",
                    X = (int?)el.Attribute("X") ?? 0,
                    Y = (int?)el.Attribute("Y") ?? 0
                });
            }
            return cfg;
        }
        catch { return new LayoutConfig { ModelName = modelName }; }
    }

    public static void Save(LayoutConfig cfg)
    {
        try
        {
            Directory.CreateDirectory(LayoutDir);
            var doc = new XDocument(
                new XElement("Layout",
                    new XElement("ModelName", cfg.ModelName),
                    new XElement("Overlays",
                        cfg.Overlays.Select(o => new XElement("Overlay",
                            new XAttribute("PointName", o.PointName),
                            new XAttribute("X", o.X),
                            new XAttribute("Y", o.Y))))));
            doc.Save(LayoutPath(cfg.ModelName));
        }
        catch { }
    }
}
