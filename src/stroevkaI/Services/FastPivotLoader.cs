using System.Diagnostics;
using System.Reflection;
using System.Text;
using MySql.Data.MySqlClient;
using StorageI.ModelsStroevkaMySql;

namespace stroevkaI.Services
{
    public static class FastPivotLoader
    {
        private const string ConnString =
            "server=127.0.0.1;port=3306;user=root;password=Djkjlz1;" +
            "database=stroevka;Character Set=utf8;Convert Zero Datetime=True;Allow Zero Datetime=True;" +
            "Pooling=true;MinimumPoolSize=1;MaximumPoolSize=10;SslMode=None;";

        // Карта: нормализованное имя колонки → PropertyInfo
        private static readonly Dictionary<string, PropertyInfo> _propMap = BuildPropertyMap();

        public static List<PivotRow> LoadTerritorialFast()
        {
            var sw = Stopwatch.StartNew();
            var result = new List<PivotRow>();

            using var conn = new MySqlConnection(ConnString);
            conn.Open();
            var tConnect = sw.ElapsedMilliseconds;

            string sql = @"
                SELECT * FROM pivot_rows"; //                WHERE psg_id = 11  OR(isitog = 1 AND psg_id <> 11)

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var row = new PivotRow();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.IsDBNull(i)) continue;

                    var colName = reader.GetName(i);
                    var normalized = NormalizeColumnName(colName);

                    if (!_propMap.TryGetValue(normalized, out var prop))
                        continue;

                    try
                    {
                        var value = reader.GetValue(i);
                        var target = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                        prop.SetValue(row, Convert.ChangeType(value, target));
                    }
                    catch { /* пропускаем несовпадения типов */ }
                }
                result.Add(row);
            }

            var tTotal = sw.ElapsedMilliseconds;
            Debug.WriteLine($"[ADO.NET] connect={tConnect}ms, total={tTotal}ms, rows={result.Count}");
            return result;
        }

        /// <summary>
        /// ac_br → AcBr, ГАСИ_расчёт → ГасиРасчёт, костюмы_Л-1_ТАСК → КостюмыЛ1Таск
        /// </summary>
        private static string NormalizeColumnName(string dbName)
        {
            if (string.IsNullOrEmpty(dbName)) return dbName;

            var sb = new StringBuilder(dbName.Length);
            bool nextUpper = true;

            foreach (var ch in dbName)
            {
                if (ch == '_' || ch == '-' || ch == ' ')
                {
                    nextUpper = true;
                    continue;
                }
                sb.Append(nextUpper ? char.ToUpper(ch) : char.ToLower(ch));
                nextUpper = false;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Собираем словарь нормализованных имён свойств PivotRow.
        /// Исключаем: NotMapped, коллекции, свойства без setter.
        /// </summary>
        private static Dictionary<string, PropertyInfo> BuildPropertyMap()
        {
            var map = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);

            foreach (var prop in typeof(PivotRow).GetProperties())
            {
                if (!prop.CanWrite) continue;

                // исключаем [NotMapped] (CellDetails, Childes)
                if (prop.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute>() != null)
                    continue;

                // исключаем коллекции и словари
                if (typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType)
                    && prop.PropertyType != typeof(string))
                    continue;

                map[prop.Name] = prop;
            }

            return map;
        }
    }
}
