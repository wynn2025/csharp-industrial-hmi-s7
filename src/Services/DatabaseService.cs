using System.Data;
using System.Data.SQLite;
using GaugeDemo300.Models;

namespace GaugeDemo300.Services;

/// <summary>
/// SQLite 数据库服务：测量记录 / 过站校验记录 / MES 同步队列
/// （System.Data.SQLite.Core：net48 原生 Interop，x86/x64 SQLite.Interop.dll 随包输出）
/// </summary>
public class DatabaseService : IDisposable
{
    private readonly string _connStr;
    private readonly LogService _log;
    private readonly SemaphoreSlim _writeLock = new(1, 1);

    public DatabaseService(string dbPath, LogService log)
    {
        _log = log;
        string dir = Path.GetDirectoryName(dbPath);
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
        _connStr = $"Data Source={dbPath};Version=3;";
        InitSchema();
    }

    private SQLiteConnection Open()
    {
        var conn = new SQLiteConnection(_connStr);
        conn.Open();
        return conn;
    }

    private void InitSchema()
    {
        using var conn = Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            CREATE TABLE IF NOT EXISTS measurement_record (
                id              INTEGER PRIMARY KEY AUTOINCREMENT,
                product_code    TEXT NOT NULL,
                device_code     TEXT,
                operator        TEXT,
                product_name    TEXT,
                product_model   TEXT,
                station_time    TEXT NOT NULL,
                note            TEXT,
                overall_result  TEXT NOT NULL,
                ok_points       TEXT,
                ng_points       TEXT,
                measure_points  TEXT,
                extra_results   TEXT,
                channel_values  TEXT,
                sync_success    INTEGER DEFAULT 0,
                sync_time       TEXT,
                create_time     TEXT NOT NULL
            );
            CREATE TABLE IF NOT EXISTS station_check (
                id           INTEGER PRIMARY KEY AUTOINCREMENT,
                product_code TEXT NOT NULL,
                check_time   TEXT NOT NULL,
                code         TEXT,
                msg          TEXT,
                passed       INTEGER
            );
            CREATE TABLE IF NOT EXISTS sync_queue (
                id           INTEGER PRIMARY KEY AUTOINCREMENT,
                record_id    INTEGER,
                product_code TEXT,
                create_time  TEXT NOT NULL,
                status       TEXT DEFAULT 'PENDING',
                retry_count  INTEGER DEFAULT 0,
                last_error   TEXT,
                last_try_time TEXT
            );
            CREATE INDEX IF NOT EXISTS idx_measure_time ON measurement_record(station_time);
            CREATE INDEX IF NOT EXISTS idx_measure_code ON measurement_record(product_code);
            CREATE INDEX IF NOT EXISTS idx_queue_status ON sync_queue(status);
            """;
        cmd.ExecuteNonQuery();
        // 兼容升级：早期无 product_model 列时补列
        try
        {
            using var addCol = conn.CreateCommand();
            addCol.CommandText = "ALTER TABLE measurement_record ADD COLUMN product_model TEXT";
            addCol.ExecuteNonQuery();
        }
        catch { /* 列已存在 */ }
        foreach (var col in new[] { "perforation_results TEXT", "displacement_values TEXT", "displacement_limits TEXT" })
        {
            try { using var ac = conn.CreateCommand(); ac.CommandText = $"ALTER TABLE measurement_record ADD COLUMN {col}"; ac.ExecuteNonQuery(); }
            catch { }
        }
        _log.Info("SQLite 数据库初始化完成");
    }

    #region 测量记录

    /// <summary>插入测量记录，返回自增 Id</summary>
    public long InsertMeasurement(MeasurementRecord rec)
    {
        _writeLock.Wait();
        try
        {
            using var conn = Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
                INSERT INTO measurement_record
                    (product_code, device_code, operator, product_name, product_model, station_time, note, perforation_results, displacement_values, displacement_limits,
                     overall_result, ok_points, ng_points, measure_points, extra_results,
                     channel_values, sync_success, sync_time, create_time)
                VALUES
                    ($code, $device, $op, $pname, $pmodel, $stime, $note, $perf, $disp, $lim,
                     $result, $okp, $ngp, $mpoints, $eresults,
                     $chvals, 0, NULL, $ctime);
                SELECT last_insert_rowid();
                """;
            cmd.Parameters.AddWithValue("$code", rec.ProductCode);
            cmd.Parameters.AddWithValue("$device", rec.DeviceCode);
            cmd.Parameters.AddWithValue("$op", rec.Operator);
            cmd.Parameters.AddWithValue("$pname", rec.ProductName);
            cmd.Parameters.AddWithValue("$pmodel", rec.ProductModel);
            cmd.Parameters.AddWithValue("$stime", rec.StationTime.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("$note", rec.Note);
            cmd.Parameters.AddWithValue("$result", rec.OverallResult);
            cmd.Parameters.AddWithValue("$okp", rec.OkPoints);
            cmd.Parameters.AddWithValue("$ngp", rec.NgPoints);
            cmd.Parameters.AddWithValue("$mpoints", string.Join("|", rec.MeasurePoints));
            cmd.Parameters.AddWithValue("$eresults", string.Join("|", rec.ExtraResults));
            cmd.Parameters.AddWithValue("$perf", rec.PerforationResults);
            cmd.Parameters.AddWithValue("$disp", rec.DisplacementValues);
            cmd.Parameters.AddWithValue("$lim", rec.DisplacementLimits);
            cmd.Parameters.AddWithValue("$chvals", rec.ChannelValuesJson);
            cmd.Parameters.AddWithValue("$ctime", rec.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            long id = (long)cmd.ExecuteScalar();
            rec.Id = id;
            return id;
        }
        catch (Exception ex)
        {
            _log.Error("插入测量记录失败", ex);
            return 0;
        }
        finally { _writeLock.Release(); }
    }

    /// <summary>标记测量记录已同步 MES</summary>
    public void MarkMeasurementSynced(long id)
    {
        _writeLock.Wait();
        try
        {
            using var conn = Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE measurement_record SET sync_success = 1, sync_time = $t WHERE id = $id";
            cmd.Parameters.AddWithValue("$t", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex) { _log.Error("更新同步状态失败", ex); }
        finally { _writeLock.Release(); }
    }

    /// <summary>查询测量记录（按时间范围/产品码/结果）</summary>
    public List<MeasurementRecord> QueryMeasurements(DateTime from, DateTime to, string productCode = "", string result = "")
    {
        var list = new List<MeasurementRecord>();
        try
        {
            using var conn = Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
                SELECT id, product_code, device_code, operator, product_name, IFNULL(product_model,''), station_time, note,
                       overall_result, ok_points, ng_points, measure_points, extra_results,
                       channel_values, sync_success, sync_time, create_time,
                       IFNULL(perforation_results,''), IFNULL(displacement_values,''), IFNULL(displacement_limits,'')
                FROM measurement_record
                WHERE station_time >= $from AND station_time <= $to
                """;
            cmd.Parameters.AddWithValue("$from", from == DateTime.MinValue ? "2000-01-01 00:00:00" : from.ToString("yyyy-MM-dd 00:00:00"));
            cmd.Parameters.AddWithValue("$to", to == DateTime.MaxValue ? "2099-12-31 23:59:59" : to.ToString("yyyy-MM-dd 23:59:59"));
            if (!string.IsNullOrWhiteSpace(productCode))
            {
                cmd.CommandText += " AND product_code LIKE $code";
                cmd.Parameters.AddWithValue("$code", $"%{productCode.Trim()}%");
            }
            if (!string.IsNullOrWhiteSpace(result))
            {
                cmd.CommandText += " AND overall_result = $result";
                cmd.Parameters.AddWithValue("$result", result);
            }
            cmd.CommandText += " ORDER BY station_time DESC LIMIT 5000";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var rec = new MeasurementRecord
                {
                    Id = reader.GetInt64(0),
                    ProductCode = reader.GetString(1),
                    DeviceCode = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Operator = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    ProductName = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    ProductModel = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    StationTime = DateTime.Parse(reader.GetString(6)),
                    Note = reader.IsDBNull(7) ? "" : reader.GetString(7),
                    OverallResult = reader.GetString(8),
                    OkPoints = reader.IsDBNull(9) ? "" : reader.GetString(9),
                    NgPoints = reader.IsDBNull(10) ? "" : reader.GetString(10),
                    ChannelValuesJson = reader.IsDBNull(13) ? "[]" : reader.GetString(13),
                    SyncSuccess = reader.GetInt64(14) == 1,
                    SyncTime = reader.IsDBNull(15) ? null : DateTime.Parse(reader.GetString(15)),
                    CreateTime = DateTime.Parse(reader.GetString(16)),
                    PerforationResults = reader.IsDBNull(17) ? "" : reader.GetString(17),
                    DisplacementValues = reader.IsDBNull(18) ? "" : reader.GetString(18),
                    DisplacementLimits = reader.IsDBNull(19) ? "" : reader.GetString(19)
                };
                string mp = reader.IsDBNull(11) ? "" : reader.GetString(11);
                rec.MeasurePoints = mp.Length > 0 ? mp.Split('|') : new string[8];
                string er = reader.IsDBNull(12) ? "" : reader.GetString(12);
                rec.ExtraResults = er.Length > 0 ? er.Split('|') : new string[4];
                list.Add(rec);
            }
        }
        catch (Exception ex) { _log.Error("查询测量记录失败", ex); }
        return list;
    }

    /// <summary>查询生产统计（指定日期）</summary>
    public (int total, int ok, int ng) GetDailyStats(DateTime day)
    {
        try
        {
            using var conn = Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
                SELECT COUNT(*),
                       SUM(CASE WHEN overall_result = 'OK' THEN 1 ELSE 0 END),
                       SUM(CASE WHEN overall_result = 'NG' THEN 1 ELSE 0 END)
                FROM measurement_record
                WHERE station_time >= $from AND station_time <= $to
                """;
            cmd.Parameters.AddWithValue("$from", day.ToString("yyyy-MM-dd 00:00:00"));
            cmd.Parameters.AddWithValue("$to", day.ToString("yyyy-MM-dd 23:59:59"));
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int total = Convert.ToInt32(reader.GetInt64(0));
                int ok = reader.IsDBNull(1) ? 0 : Convert.ToInt32(reader.GetInt64(1));
                int ng = reader.IsDBNull(2) ? 0 : Convert.ToInt32(reader.GetInt64(2));
                return (total, ok, ng);
            }
        }
        catch (Exception ex) { _log.Error("查询生产统计失败", ex); }
        return (0, 0, 0);
    }

    #endregion

    #region 过站校验记录

    public void InsertStationCheck(StationCheckRecord rec)
    {
        _writeLock.Wait();
        try
        {
            using var conn = Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
                INSERT INTO station_check (product_code, check_time, code, msg, passed)
                VALUES ($code, $t, $c, $m, $p)
                """;
            cmd.Parameters.AddWithValue("$code", rec.ProductCode);
            cmd.Parameters.AddWithValue("$t", rec.CheckTime.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("$c", rec.Code);
            cmd.Parameters.AddWithValue("$m", rec.Msg);
            cmd.Parameters.AddWithValue("$p", rec.Passed ? 1 : 0);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex) { _log.Error("插入过站校验记录失败", ex); }
        finally { _writeLock.Release(); }
    }

    public List<StationCheckRecord> QueryChecks(DateTime from, DateTime to)
    {
        var list = new List<StationCheckRecord>();
        try
        {
            using var conn = Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
                SELECT id, product_code, check_time, code, msg, passed
                FROM station_check
                WHERE check_time >= $from AND check_time <= $to
                ORDER BY check_time DESC LIMIT 500
                """;
            cmd.Parameters.AddWithValue("$from", from == DateTime.MinValue ? "2000-01-01 00:00:00" : from.ToString("yyyy-MM-dd 00:00:00"));
            cmd.Parameters.AddWithValue("$to", to == DateTime.MaxValue ? "2099-12-31 23:59:59" : to.ToString("yyyy-MM-dd 23:59:59"));
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new StationCheckRecord
                {
                    Id = reader.GetInt64(0),
                    ProductCode = reader.GetString(1),
                    CheckTime = DateTime.Parse(reader.GetString(2)),
                    Code = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Msg = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    Passed = reader.GetInt64(5) == 1
                });
            }
        }
        catch (Exception ex) { _log.Error("查询过站校验记录失败", ex); }
        return list;
    }

    #endregion

    #region MES 同步队列

    /// <summary>同步失败入队（离线补传）</summary>
    public void EnqueueSync(long recordId, string productCode)
    {
        _writeLock.Wait();
        try
        {
            using var conn = Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
                INSERT INTO sync_queue (record_id, product_code, create_time, status, retry_count)
                VALUES ($rid, $code, $t, 'PENDING', 0)
                """;
            cmd.Parameters.AddWithValue("$rid", recordId);
            cmd.Parameters.AddWithValue("$code", productCode);
            cmd.Parameters.AddWithValue("$t", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex) { _log.Error("同步失败入队失败", ex); }
        finally { _writeLock.Release(); }
    }

    /// <summary>获取待重传队列（PENDING/FAILED）</summary>
    public List<SyncQueueItem> GetPendingQueue()
    {
        var list = new List<SyncQueueItem>();
        try
        {
            using var conn = Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
                SELECT id, record_id, product_code, create_time, status, retry_count, last_error, last_try_time
                FROM sync_queue WHERE status IN ('PENDING','FAILED') ORDER BY id
                """;
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new SyncQueueItem
                {
                    Id = reader.GetInt64(0),
                    RecordId = reader.GetInt64(1),
                    ProductCode = reader.GetString(2),
                    CreateTime = DateTime.Parse(reader.GetString(3)),
                    Status = reader.GetString(4),
                    RetryCount = Convert.ToInt32(reader.GetInt64(5)),
                    LastError = reader.IsDBNull(6) ? "" : reader.GetString(6),
                    LastTryTime = reader.IsDBNull(7) ? null : DateTime.Parse(reader.GetString(7))
                });
            }
        }
        catch (Exception ex) { _log.Error("查询同步队列失败", ex); }
        return list;
    }

    /// <summary>更新队列项状态（重试结果）</summary>
    public void UpdateQueueStatus(long queueId, string status, string error = "", int retryCount = 0)
    {
        _writeLock.Wait();
        try
        {
            using var conn = Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
                UPDATE sync_queue SET status = $s, retry_count = $r, last_error = $e, last_try_time = $t
                WHERE id = $id
                """;
            cmd.Parameters.AddWithValue("$s", status);
            cmd.Parameters.AddWithValue("$r", retryCount);
            cmd.Parameters.AddWithValue("$e", error);
            cmd.Parameters.AddWithValue("$t", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("$id", queueId);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex) { _log.Error("更新队列状态失败", ex); }
        finally { _writeLock.Release(); }
    }

    #endregion

    #region 数据维护

    /// <summary>按保留天数清理过期数据（测量记录 + 校验记录）</summary>
    public void CleanupOldData(int retentionDays)
    {
        if (retentionDays <= 0) return;
        _writeLock.Wait();
        try
        {
            string cutoff = DateTime.Now.AddDays(-retentionDays).ToString("yyyy-MM-dd 00:00:00");
            using var conn = Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM measurement_record WHERE station_time < $c; DELETE FROM station_check WHERE check_time < $c;";
            cmd.Parameters.AddWithValue("$c", cutoff);
            int n = cmd.ExecuteNonQuery();
            if (n > 0) _log.Info($"数据清理完成，删除过期记录 {n} 条");
        }
        catch (Exception ex) { _log.Error("数据清理失败", ex); }
        finally { _writeLock.Release(); }
    }

    #endregion

    public void Dispose() { }
}
