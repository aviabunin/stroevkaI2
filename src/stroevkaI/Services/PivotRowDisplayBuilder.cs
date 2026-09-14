using StorageI.ModelsStroevkaMySql;

namespace stroevkaI.Services
{
    public class PivotRowDisplayBuilder {

        private static readonly string[] AllTerritorialCategories =
                   { "ГПС", "ФПС", "ЧПО", "ВПО", "другие", "АСФ" };
        private static readonly string[] CategoriesForTotal =
    { "ГПС", "другие", "ЧПО", "ВПО" };

        public static List<PivotRow> BuildTerritorialView(List<PivotRow> allRows)
        {
            var terrRows = allRows.Where(r => r.PsgId == 11).ToList();
            var psgItogiRows = allRows.Where(r => r.PsgId != 11 && r.Isitog == 1).ToList();
            var leafRows = allRows.Where(r => r.Isitog == 0).ToList();

            var root = terrRows.FirstOrDefault(r => r.Parent == 0)
                    ?? terrRows.FirstOrDefault(r => r.Category == "всего");
            if (root == null) return new List<PivotRow>();

            var terrCategoryRows = AllTerritorialCategories
                .Select(cat => terrRows.FirstOrDefault(r => r.Category == cat))
                .Where(r => r != null)
                .ToList();

            // root.Childes = 6 категорий
            root.Childes = terrCategoryRows;

            // каждая категория ТПСГ → районные ПСГ той же категории
            foreach (var terrCat in terrCategoryRows)
            {
                terrCat.Childes = psgItogiRows
                    .Where(r => r.Category == terrCat.Category)
                    .OrderBy(r => r.Norder)
                    .ToList();

                // ПЧ — дочерние к районным строкам
                foreach (var psgRow in terrCat.Childes)
                {
                    psgRow.Childes = leafRows
                        .Where(r => r.PsgId == psgRow.PsgId
                                    && LeafBelongsToCategory(r.Category, psgRow.Category))
                        .OrderBy(r => r.Norder)
                        .ToList();
                }
            }

            // CellDetails для ТПСГ-строк
            var rootSources = terrCategoryRows
                .Where(r => CategoriesForTotal.Contains(r.Category))
                .ToList();
            BuildCellDetailsCustom(root, rootSources);
            foreach (var terrCat in terrCategoryRows)
                BuildCellDetails(terrCat);

            // === Собираем 25 строк для показа ===
            var displayRows = new List<PivotRow> { root };
            displayRows.AddRange(terrCategoryRows);

            // 18 строк "ПСГ (всего)" районных
            var districtTotals = psgItogiRows
                .Where(r => r.Category == "всего")
                .OrderBy(r => r.Norder)
                .ToList();

            foreach (var districtRow in districtTotals)
            {
                // Childes районного "всего" = его ГПС + другие (для tooltip)
                var gps = psgItogiRows.FirstOrDefault(r => r.PsgId == districtRow.PsgId && r.Category == "ГПС");
                var other = psgItogiRows.FirstOrDefault(r => r.PsgId == districtRow.PsgId && r.Category == "другие");

                districtRow.Childes = new List<PivotRow>();
                if (gps != null) districtRow.Childes.Add(gps);
                if (other != null) districtRow.Childes.Add(other);

                BuildCellDetails(districtRow);
            }

            displayRows.AddRange(districtTotals);

            return displayRows;
        }
        /// <summary>
        /// Стандартный вариант: источники = itog.Childes.
        /// </summary>
        private static void BuildCellDetails(PivotRow itog)
            => BuildCellDetailsCustom(itog, itog.Childes);

        /// <summary>
        /// Собирает CellDetails для строки из переданного набора источников.
        /// Для каждой decimal?-колонки — список "имя + значение".
        /// </summary>
        private static void BuildCellDetailsCustom(PivotRow itog, List<PivotRow> sources)
        {
            foreach (var prop in typeof(PivotRow).GetProperties())
            {
                if (prop.PropertyType != typeof(decimal?)) continue;
                if (!prop.CanRead) continue;

                var details = new List<DetailItem>();

                foreach (var child in sources)
                {
                    var val = (decimal?)prop.GetValue(child) ?? 0;
                    if (val == 0) continue;

                    // Для ПСГ-строки — имя ПСГ. Для ПЧ-строки — имя ПЧ.
                    var name = !string.IsNullOrEmpty(child.Псг)
                               && child.Псг != "Территориальный"
                        ? child.Псг
                        : child.Пч;

                    details.Add(new DetailItem
                    {
                        Name = name,
                        Value = val,
                        Category = child.Category
                    });
                }

                itog.CellDetails[prop.Name] = details;
            }
        }

        /// <summary>
        /// Какие категории ПЧ попадают в данную категорию ПСГ.
        /// </summary>
        private static bool LeafBelongsToCategory(string leafCategory, string psgCategory)
        {
            return psgCategory switch
            {
                "ГПС" => leafCategory == "ФПС" || leafCategory == "ППС",
                "ФПС" => leafCategory == "ФПС",
                "ППС" => leafCategory == "ППС",
                "ЧПО" => leafCategory == "ЧПО",
                "ВПО" => leafCategory == "ВПО",
                "АСФ" => leafCategory == "АСФ",
                "другие" => leafCategory != "ФПС" && leafCategory != "ППС"
                            && leafCategory != "ЧПО" && leafCategory != "ВПО"
                            && leafCategory != "АСФ",
                _ => false
            };
        }

        public static List<PivotRow> BuildPsgView(List<PivotRow> allRows, int psgId)
        {
            var psgRows = allRows.Where(r => r.PsgId == psgId).ToList();
            var leafRows = psgRows.Where(r => r.Isitog == 0).ToList();
            var itogiRows = psgRows.Where(r => r.Isitog == 1).ToList();

            // Найти строку "всего"
            var root = itogiRows.FirstOrDefault(r => r.Category == "всего");
            if (root == null) return new List<PivotRow>();

            // Обязательные: ГПС, другие
            var gpsRow = itogiRows.FirstOrDefault(r => r.Category == "ГПС");
            var otherRow = itogiRows.FirstOrDefault(r => r.Category == "другие");

            // Опциональные: ЧПО, ВПО, АСФ (если есть)
            var chpoRow = itogiRows.FirstOrDefault(r => r.Category == "ЧПО");
            var vpoRow = itogiRows.FirstOrDefault(r => r.Category == "ВПО");
            var asfRow = itogiRows.FirstOrDefault(r => r.Category == "АСФ");

            var displayRows = new List<PivotRow> { root };
            if (gpsRow != null) displayRows.Add(gpsRow);
            if (otherRow != null) displayRows.Add(otherRow);
            if (chpoRow != null) displayRows.Add(chpoRow);
            if (vpoRow != null) displayRows.Add(vpoRow);
            if (asfRow != null) displayRows.Add(asfRow);

            // Children для root = [ГПС, другие] (только эти два входят в "всего")
            root.Childes = new List<PivotRow>();
            if (gpsRow != null) root.Childes.Add(gpsRow);
            if (otherRow != null) root.Childes.Add(otherRow);

            // Children для ГПС = ПЧ с ФПС/ППС
            if (gpsRow != null)
            {
                gpsRow.Childes = leafRows
                    .Where(l => l.Category == "ФПС" || l.Category == "ППС")
                    .OrderBy(l => l.Norder)
                    .ToList();
            }

            // Children для другие = ПЧ с остальными категориями (ДПО, ДПК, etc.)
            // Исключаем ФПС/ППС (уже в ГПС) и ЧПО/ВПО/АСФ (у них свои строки)
            if (otherRow != null)
            {
                otherRow.Childes = leafRows
                    .Where(l => l.Category != "ФПС" && l.Category != "ППС"
                                && l.Category != "ЧПО" && l.Category != "ВПО"
                                && l.Category != "АСФ")
                    .OrderBy(l => l.Norder)
                    .ToList();
            }

            // Children для ЧПО/ВПО/АСФ
            if (chpoRow != null)
                chpoRow.Childes = leafRows.Where(l => l.Category == "ЧПО").OrderBy(l => l.Norder).ToList();
            if (vpoRow != null)
                vpoRow.Childes = leafRows.Where(l => l.Category == "ВПО").OrderBy(l => l.Norder).ToList();
            if (asfRow != null)
                asfRow.Childes = leafRows.Where(l => l.Category == "АСФ").OrderBy(l => l.Norder).ToList();

            // CellDetails
            foreach (var row in displayRows)
                BuildCellDetails(row);

            return displayRows;
        }
        ///// <summary>
        ///// Собирает CellDetails для строки из её Childes:
        ///// для каждой decimal?-колонки — список "имя + значение".
        ///// </summary>
        //private static void BuildCellDetails(PivotRow itog)
        //{
        //    foreach (var prop in typeof(PivotRow).GetProperties())
        //    {
        //        if (prop.PropertyType != typeof(decimal?)) continue;
        //        if (!prop.CanRead) continue;

        //        var details = new List<DetailItem>();

        //        foreach (var child in itog.Childes)
        //        {
        //            var val = (decimal?)prop.GetValue(child) ?? 0;
        //            if (val == 0) continue;

        //            // Для ПСГ-строки имя = Псг (Костомукшский),
        //            // для ПЧ-строки — Псг пустой или "Территориальный", тогда берём Пч
        //            var name = !string.IsNullOrEmpty(child.Псг)
        //                       && child.Псг != "Территориальный"
        //                ? child.Псг
        //                : child.Пч;

        //            details.Add(new DetailItem
        //            {
        //                Name = name,
        //                Value = val,
        //                Category = child.Category
        //            });
        //        }

        //        itog.CellDetails[prop.Name] = details;
        //    }
        //}
    }
}