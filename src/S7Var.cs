using GaugeDemo300.Models;
using S7.Net;

namespace GaugeDemo300
{
    /// <summary>
    /// S7 地址工具（改造自吊车控制 S7Var：原为变量封装，现为静态地址解析/生成工具）
    /// - 位地址：DB1.DBX0.3 / M10.2 / I0.5 / Q0.7
    /// - Int 地址：DB30.DBW0 / MW100（2 字节）
    /// - REAL 地址：DB2.DBD8 / MD100（4 字节）
    /// - 地址递增：位 +n 位、Int +n×2 字节、REAL +n×4 字节（点位表批量生成用）
    /// </summary>
    public static class S7AddressTool
    {
        /// <summary>解析后的点定位元数据</summary>
        public sealed class S7PointAddr
        {
            public PointConfig Point;      // 所属点位（气缸/报警地址为 null）
            public CylinderConfig Cylinder; // 所属气缸（检测点为 null）
            public bool IsSwitch;          // 气缸动作开关位
            public bool IsForwardFeedback; // 气缸前进到位反馈位（false=后退到位）
            public bool IsResultAddress;   // 位移点的 PLC 判定结果地址（区别于测量值地址）
            public bool IsAlarmBit;        // PLC 报警位（ALARM.P_ALARM）
            public int AlarmIndex;         // 报警位序号 1..N
            public DataType Area;
            public int Db;
            public int ByteOffset;   // 区域内字节偏移（位点：位所在字节；Int/REAL：首字节）
            public int BitOffset;    // 位号 0-7
            public int ByteLength;   // 占用字节数（位点 1，Int 2，REAL 4）
        }

        /// <summary>解析 PLC 报警位地址（ALARM.P_ALARM[i]，Bool 位连续）</summary>
        public static S7PointAddr ParseAlarmAddress(int alarmIndex, string startAddress, int n)
        {
            var a = ParseRaw(NextBitAddress(startAddress, n), PointType.Digital);
            a.IsAlarmBit = true;
            a.AlarmIndex = alarmIndex;
            return a;
        }

        /// <summary>
        /// 解析点位地址：Result 点（Int 2 字节）/ Analog 点（REAL 4 字节）/ Digital 点（位）
        /// </summary>
        public static S7PointAddr ParseAddress(PointConfig p)
        {
            var a = ParseRaw(p.PlcAddress, p.Type);
            a.Point = p;
            return a;
        }

        /// <summary>解析气缸反馈位地址（isForward：true=前进到位，false=后退到位）</summary>
        public static S7PointAddr ParseFeedbackAddress(CylinderConfig c, string address, bool isForward)
        {
            var a = ParseRaw(address, PointType.Digital);
            a.Cylinder = c;
            a.IsForwardFeedback = isForward;
            return a;
        }

        /// <summary>解析气缸开关位地址（读回开关状态用）</summary>
        public static S7PointAddr ParseSwitchAddress(CylinderConfig c, string address)
        {
            var a = ParseRaw(address, PointType.Digital);
            a.Cylinder = c;
            a.IsSwitch = true;
            return a;
        }

        public static S7PointAddr ParseRaw(string address, PointType type)
        {
            string s = (address ?? "").Trim().ToUpperInvariant().Replace(" ", "");
            var a = new S7PointAddr();

            if (s.StartsWith("DB"))
            {
                // 首个点号后的整段（位地址含第二个点号，不能 Split('.') 全切）
                int dot = s.IndexOf('.');
                int db = int.Parse(s.Substring(2, dot - 2));
                string rest = s.Substring(dot + 1);
                if (type == PointType.Digital)
                {
                    // DB1.DBX0.3（位）
                    if (!rest.StartsWith("DBX") && !rest.StartsWith("X")) throw new FormatException(s);
                    string bitStr = rest.StartsWith("DBX") ? rest.Substring(3) : rest.Substring(1);
                    var bp = bitStr.Split('.');
                    a.Area = DataType.DataBlock;
                    a.Db = db;
                    a.ByteOffset = int.Parse(bp[0]);
                    a.BitOffset = bp.Length > 1 ? int.Parse(bp[1]) : 0;
                    a.ByteLength = 1;
                    if (a.BitOffset is < 0 or > 7) throw new FormatException(s);
                }
                else
                {
                    // DB30.DBW0（Int）/ DB2.DBD8（REAL）
                    bool isInt = type == PointType.Result;
                    string prefix = isInt ? "DBW" : "DBD";
                    if (!rest.StartsWith(prefix) && !rest.StartsWith("W") && !rest.StartsWith("D")) throw new FormatException(s);
                    string off;
                    if (rest.StartsWith(prefix)) off = rest.Substring(3);
                    else off = rest.Substring(1);
                    a.Area = DataType.DataBlock;
                    a.Db = db;
                    a.ByteOffset = int.Parse(off);
                    a.ByteLength = isInt ? 2 : 4;
                }
                return a;
            }

            if (s.StartsWith("M"))
            {
                if (type == PointType.Digital)
                {
                    // M10.2（位）
                    var bp = s.Substring(1).Split('.');  // M区只有一个点号，Split 安全
                    a.Area = DataType.Memory;
                    a.Db = 0;
                    a.ByteOffset = int.Parse(bp[0]);
                    a.BitOffset = bp.Length > 1 ? int.Parse(bp[1]) : 0;
                    a.ByteLength = 1;
                    if (a.BitOffset is < 0 or > 7) throw new FormatException(s);
                }
                else
                {
                    // MW100（Int）/ MD100（REAL）
                    bool isInt = type == PointType.Result;
                    string off = s.Substring(1);
                    if (isInt)
                    {
                        if (off.StartsWith("W")) off = off.Substring(1);
                    }
                    else
                    {
                        if (off.StartsWith("D")) off = off.Substring(1);
                    }
                    a.Area = DataType.Memory;
                    a.Db = 0;
                    a.ByteOffset = int.Parse(off);
                    a.ByteLength = isInt ? 2 : 4;
                }
                return a;
            }

            if (s.StartsWith("I") || s.StartsWith("E"))
            {
                var bp = s.Substring(1).Split('.');
                a.Area = DataType.Input;
                a.Db = 0;
                a.ByteOffset = int.Parse(bp[0]);
                a.BitOffset = bp.Length > 1 ? int.Parse(bp[1]) : 0;
                a.ByteLength = 1;
                if (type != PointType.Digital || a.BitOffset is < 0 or > 7) throw new FormatException(s);
                return a;
            }

            if (s.StartsWith("Q") || s.StartsWith("A"))
            {
                var bp = s.Substring(1).Split('.');
                a.Area = DataType.Output;
                a.Db = 0;
                a.ByteOffset = int.Parse(bp[0]);
                a.BitOffset = bp.Length > 1 ? int.Parse(bp[1]) : 0;
                a.ByteLength = 1;
                if (type != PointType.Digital || a.BitOffset is < 0 or > 7) throw new FormatException(s);
                return a;
            }

            throw new FormatException($"无法解析点位地址：{s}");
        }

        /// <summary>位地址 + n：DB1.DBX0.0 起第 n 个位（也支持 M10.2 形式）</summary>
        public static string NextBitAddress(string start, int n)
        {
            string s = start.Trim().ToUpperInvariant();
            if (s.StartsWith("DB"))
            {
                int dot = s.IndexOf('.');
                int db = int.Parse(s.Substring(2, dot - 2));
                string rest = s.Substring(dot + 1);            // "DBX4.4"（不能再 Split，位号含点）
                string bit = rest.StartsWith("DBX") ? rest.Substring(3) : rest;
                var bp = bit.Split('.');
                int linear = int.Parse(bp[0]) * 8 + (bp.Length > 1 ? int.Parse(bp[1]) : 0) + n;
                return $"DB{db}.DBX{linear / 8}.{linear % 8}";
            }
            var seg = s.Substring(1).Split('.');
            int lin = int.Parse(seg[0]) * 8 + (seg.Length > 1 ? int.Parse(seg[1]) : 0) + n;
            return $"{s[0]}{lin / 8}.{lin % 8}";
        }

        /// <summary>REAL 地址 + n×4 字节：DB2.DBD0 起第 n 个 REAL（也支持 MD100 形式）</summary>
        public static string NextRealAddress(string start, int n)
        {
            string s = start.Trim().ToUpperInvariant();
            if (s.StartsWith("DB"))
            {
                var parts = s.Split('.');
                int db = int.Parse(parts[0].Substring(2));
                string off = parts[1].StartsWith("DBD") ? parts[1].Substring(3) : parts[1];
                return $"DB{db}.DBD{int.Parse(off) + n * 4}";
            }
            string o = s.Substring(1);
            if (o.StartsWith("D")) o = o.Substring(1);
            return $"{s[0]}D{int.Parse(o) + n * 4}";
        }

        /// <summary>Int 地址 + n×2 字节：DB30.DBW0 起第 n 个 Int（也支持 MW100 形式）</summary>
        public static string NextIntAddress(string start, int n)
        {
            string s = start.Trim().ToUpperInvariant();
            if (s.StartsWith("DB"))
            {
                var parts = s.Split('.');
                int db = int.Parse(parts[0].Substring(2));
                string off = parts[1].StartsWith("DBW") ? parts[1].Substring(3) : parts[1];
                return $"DB{db}.DBW{int.Parse(off) + n * 2}";
            }
            string o = s.Substring(1);
            if (o.StartsWith("W")) o = o.Substring(1);
            return $"{s[0]}W{int.Parse(o) + n * 2}";
        }

        /// <summary>解析 CPU 类型字符串（S7300/S7400/S71200/S71500，大小写不敏感）</summary>
        public static CpuType ParseCpuType(string s)
        {
            switch ((s ?? "").Trim().ToUpperInvariant().Replace("-", "").Replace(" ", ""))
            {
                case "S7300": return CpuType.S7300;
                case "S7400": return CpuType.S7400;
                case "S71500": return CpuType.S71500;
                case "S71200":
                default: return CpuType.S71200;
            }
        }
    }
}
