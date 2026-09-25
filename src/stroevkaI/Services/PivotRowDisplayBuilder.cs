using StorageI.ModelsStroevkaMySql;

namespace stroevkaI.Services
{
    public class PivotRowDisplayBuilder {

        private static readonly string[] AllTerritorialCategories = { "ГПС", "ФПС", "ЧПО", "ВПО", "другие", "АСФ" };
        private static readonly string[] CategoriesForTotal       = { "ГПС", "другие", "ЧПО", "ВПО" };

        public static List<PivotRow> BuildTerritorialView(List<PivotRow> allRows)
        {
            var terrRows = allRows.Where(r => r.PsgId == 11).ToList();
            var psgItogiRows = allRows.Where(r => !r.Псг.Contains("Террит") && r.Isitog == 1).ToList();
            //var leafRows = allRows.Where(r => r.Isitog == 1  && !r.Псг.ToLower().Contains("террит")).ToList();//Здесь выбрать только итоги для районных

            //root - корень дерева - террит ПСГ PivotRow
            var root = terrRows.FirstOrDefault(r => r.Id == 11);
                    //?? terrRows.FirstOrDefault(r => r.Category == "всего");
            if (root == null) return new List<PivotRow>();

            // получим строки PivotRow для каждой категории - всего 6 строк
            var terrCategoryRows = AllTerritorialCategories
                .Select(cat => terrRows.FirstOrDefault(r => r.Category == cat))
                .Where(r => r != null)
                .ToList();

            // root.Childes = 6 категорий
            root.Childes = terrCategoryRows;// для root 6  дочерних итоговых pivotRow 

            // каждая категория ТПСГ → районные ПСГ той же категории
            foreach (var terrCat in terrCategoryRows)
            {
                terrCat.Childes = psgItogiRows
                    .Where(r => r.Category.Contains(terrCat.Category))
                    .OrderBy(r => r.Norder)
                    .ToList();

                // ПЧ — дочерние к районным строкам    - РАЗОБРАТЬСЯ ПОПАДАЮТ ЛИ В ТOOLTIPS
                //      нужны не ПЧ, а итоговые от РПСГ соответствующих категорий
                foreach (var psgcatRow in terrCat.Childes)
                {
                    psgcatRow.Childes = psgItogiRows
                        .Where(r => r.PsgId == psgcatRow.PsgId
                                    && LeafBelongsToCategory(r.Category, psgcatRow.Category))
                        .OrderBy(r => r.Norder)
                        .ToList();
                }
            }

                                                                // CellDetails для ТПСГ-строк
            // для ROOT - 4 строки  ГПС, другие, ЧПО, ВПО                       (АСФ исключено)
            var rootSources = terrCategoryRows
                .Where(r => CategoriesForTotal.Contains(r.Category))
                .ToList();

            BuildCellDetailsCustom(root, rootSources);

            //для дочерних - должен получиться список из аналогичных для районных
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



            // надо выбрать из каких частей выбраны количества
            foreach (var districtRow in districtTotals)
            {
                var pchRows = allRows.Where(r => ((r.Parent == districtRow.PchId) && (r.Isitog==0)) ).ToList();

                districtRow.Childes = new List<PivotRow>();
                if (pchRows != null) districtRow.Childes.AddRange(pchRows);

                BuildCellDetails(districtRow);
            }

            displayRows.AddRange(districtTotals);

            return displayRows;
        }

        private int ResolvePsgId(Psgstat item, Dictionary<int, Psgstat> dict)
        {
            if (item.Id == 11) return 11;
            if (item.Parent == 11) return item.Id;   // сам район

            // ПЧ или районная категорийная строка — поднимаемся к району
            var current = item;
            while (current.Parent.HasValue && current.Parent.Value != 11)
            {
                if (!dict.TryGetValue(current.Parent.Value, out current))
                    return 0;
            }
            // current — это уже узел, чей Parent = 11 → это район
            return current.Id == 11 ? 11 : current.Id;
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
                    string name = child.Пч;
                    if(itog.Псг.ToLower().Contains("террит"))
                        if(!itog.Category.Contains("всего"))
                            name = child.Псг;

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
            var psgRows = allRows.Where(r => (r.Id == psgId) || (r.Parent == psgId)).ToList();
            var leafRows = psgRows.Where(r =>  (r.Isitog == 0)).ToList();
            var itogiRows = psgRows.Where(r => (r.Isitog == 1)).ToList();

            // Найти строку "всего"
            var root = itogiRows.FirstOrDefault(r => r.Category == "всего");
            if (root == null) return new List<PivotRow>();

            // Обязательные: ГПС, другие
            var gpsRow = itogiRows.FirstOrDefault(r => r.Category.Contains( "ГПС"));
            var otherRow = itogiRows.FirstOrDefault(r => r.Category.Contains("другие"));

            // Опциональные: ЧПО, ВПО, АСФ (если есть)
            var chpoRow = itogiRows.FirstOrDefault(r => r.Category.Contains("ЧПО"));
            var vpoRow = itogiRows.FirstOrDefault(r => r.Category.Contains("ВПО"));
            var asfRow = itogiRows.FirstOrDefault(r => r.Category.Contains("АСФ"));

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
                    .Where(l => l.Category != "ФПС" && l.Category != "ППС")
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

            displayRows.AddRange(leafRows);
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