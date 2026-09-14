using System.Reflection;
using StorageI.ModelsStroevkaMySql;

namespace stroevkaI.Services
{
    public static class PivotRowDisplayBuilder
    {
        /// <summary>
        /// Из полного списка pivot_rows (ТПСГ + все районные итоги)
        /// формирует 25 строк для отображения с готовыми Childes и CellDetails.
        /// </summary>
        public static List<PivotRow> BuildTerritorialView(List<PivotRow> allRows)
        {
            // 1. Отбираем 25 строк для показа
            var displayRows = allRows
                .Where(r => r.PsgId == 11 || r.Category == "всего")
                .OrderBy(r => r.Norder)
                .ToList();

            // 2. Группируем все 75 строк по категории — для быстрого поиска Childes
            var byCategory = allRows
                .GroupBy(r => r.Category)
                .ToDictionary(g => g.Key, g => g.ToList());

            // 3. Для каждой строки собираем Childes и CellDetails
            foreach (var row in displayRows)
            {
                if (row.PsgId == 11)
                {
                    // ТПСГ-строка: Childes = все районные ПСГ той же категории
                    row.Childes = byCategory.TryGetValue(row.Category, out var list)
                        ? list.Where(r => r.PsgId != 11)
                              .OrderBy(r => r.Norder)
                              .ToList()
                        : new List<PivotRow>();
                }
                else
                {
                    // Районный ПСГ (строка "всего"): Childes пока пустые.
                    // Для расшифровки нужны ПЧ этого района — их загрузим отдельно
                    // при hover'е, либо упростим: tooltip показывает категории
                    // этого же ПСГ из allRows.
                    row.Childes = allRows
                        .Where(r => r.PsgId == row.PsgId && r.Category != "всего")
                        .OrderBy(r => r.Norder)
                        .ToList();
                }

                // 4. Строим CellDetails по Childes
                BuildCellDetails(row);
            }

            return displayRows;
        }

        /// <summary>
        /// Для каждой итоговой строки и каждой decimal?-колонки
        /// собирает список составляющих из Childes.
        /// </summary>
        private static void BuildCellDetails(PivotRow itog)
        {
            foreach (var prop in typeof(PivotRow).GetProperties())
            {
                // только decimal?
                if (prop.PropertyType != typeof(decimal?)) continue;
                if (!prop.CanRead) continue;

                var details = new List<DetailItem>();

                foreach (var child in itog.Childes)
                {
                    var val = (decimal?)prop.GetValue(child) ?? 0;
                    if (val == 0) continue;

                    details.Add(new DetailItem
                    {
                        Name = string.IsNullOrEmpty(child.Псг) ? child.Пч : child.Псг,
                        Value = val,
                        Category = child.Category
                    });
                }

                itog.CellDetails[prop.Name] = details;
            }
        }
    }
}
