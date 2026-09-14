using StorageI.ModelsStroevkaMySql;

namespace stroevkaI.Services
{
    public static class PivotRowDisplayBuilder
    {
        // 6 категорий ТПСГ, которые показываются как дочерние к root в дереве и в гриде.
        private static readonly string[] AllTerritorialCategories =
            { "ГПС", "ФПС", "ЧПО", "ВПО", "другие", "АСФ" };

        // 4 категории, из которых складывается "Территориальный (всего)".
        // ФПС исключён (уже входит в ГПС). АСФ исключён (не входит по требованию).
        private static readonly string[] CategoriesForTotal =
            { "ГПС", "другие", "ЧПО", "ВПО" };

        /// <summary>
        /// Строит дерево для территориального уровня:
        ///   root "Территориальный (всего)"
        ///     ├─ 6 категорий ТПСГ (ГПС, ФПС, ЧПО, ВПО, другие, АСФ)
        ///     │    └─ районные ПСГ той же категории
        ///     │         └─ ПЧ с соответствующими категориями
        /// Возвращает плоский список 7 строк для показа.
        /// </summary>
        public static List<PivotRow> BuildTerritorialView(List<PivotRow> allRows)
        {
            var terrRows = allRows.Where(r => r.PsgId == 11).ToList();
            var psgItogiRows = allRows.Where(r => r.PsgId != 11 && r.Isitog == 1).ToList();
            var leafRows = allRows.Where(r => r.Isitog == 0).ToList();

            // Root — Территориальный (всего). Ищем по parent = 0 либо ПСГ == "Территориальный" + category = 'всего'.
            var root = terrRows.FirstOrDefault(r => r.Parent == 0)
                        ?? terrRows.FirstOrDefault(r => ((r.Псг.Trim() == "Территориальный") && (r.Category == "всего")));
            if (root == null) return new List<PivotRow>();

            // 6 категорий ТПСГ в фиксированном порядке
            var terrCategoryRows = AllTerritorialCategories
                .Select(cat => terrRows.FirstOrDefault(r => r.Category == cat))
                .Where(r => r != null)
                .ToList();

            // root.Childes = 6 категорий (для дерева/раскрытия)
            root.Childes = terrCategoryRows;

            // Каждая категория ТПСГ → районные ПСГ той же категории
            foreach (var terrCat in terrCategoryRows)
            {
                terrCat.Childes = psgItogiRows
                    .Where(r => r.Category == terrCat.Category)
                    .OrderBy(r => r.Norder)
                    .ToList();

                // каждый районный ПСГ → ПЧ с соответствующими категориями
                foreach (var psgRow in terrCat.Childes)
                {
                    psgRow.Childes = leafRows
                        .Where(r => r.PsgId == psgRow.PsgId
                                    && LeafBelongsToCategory(r.Category, psgRow.Category))
                        .OrderBy(r => r.Norder)
                        .ToList();
                }
            }

            // ⚠️ Для tooltip на root используем ТОЛЬКО 4 категории (без ФПС и АСФ).
            // Root.Childes при этом остаётся 6 — он используется в дереве, если раскрывать.
            var rootSources = terrCategoryRows
                .Where(r => CategoriesForTotal.Contains(r.Category))
                .ToList();

            // CellDetails для каждой показываемой строки
            BuildCellDetailsCustom(root, rootSources);   // root — 4 категории
            foreach (var terrCat in terrCategoryRows)
                BuildCellDetails(terrCat);                // остальные — по своим Childes

            var displayRows = new List<PivotRow> { root };
            displayRows.AddRange(terrCategoryRows);
            return displayRows;
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
    }
}