using StorageI.ModelsStroevkaMySql;
using stroevkaI.Models;

public class PivotTreeBuilder
{
    Dictionary<int, string> psgIdRows;
    List<PsgTotalRow> psg_total_rows;

    ReportNode root = null;
    public static stroevkaContext _context = new stroevkaContext();

    public PivotTreeBuilder()
    {
       
    }

    public ReportNode BuildTree()
    {
        // 1. Загружаем все узлы psgdata (включая ПСГ и ПЧ)  - ВСЕГО 309(пч)+18(псг)+1(террит)
        var allNodes = _context.Psgdata
            .Where(p => p.Old == true) // возможно, только актуальные
            .Where(p=>!p.Garnizon.Contains("итоги"))
            .ToList();
     
        // 2. Загружаем сырые данные для листьев
        var sredstvaList = _context.Sredstvas.ToList();
        var sostavList = _context.Sostavs.ToList();
        var sizodList = _context.Sizods.ToList();
        var penasList = _context.Penas.ToList();
        var kostymsList = _context.Kostyms.ToList();
        var watersList = _context.Waters.ToList();
        var contactsList = _context.Contacts.ToList();
        psg_total_rows = _context.PsgTotalRows.ToList();

        // Группируем данные по subdivision_id
        var sredstvaBySubdiv = sredstvaList
            .GroupBy(s => s.SubdivisionId)
            .ToDictionary(g => g.Key, g => g.ToList());
        #region другие таблицы
        //var sostavBySubdiv = sostavList
        //    .GroupBy(s => s.SubdivisionId)
        //    .ToDictionary(g => g.Key, g => g.ToList());

        //var sizodBySubdiv = sizodList
        //    .GroupBy(s => s.SubdivisionId)
        //    .ToDictionary(g => g.Key, g => g.ToList());

        //var penasBySubdiv = penasList
        //    .GroupBy(p => p.SubdivisionId)
        //    .ToDictionary(g => g.Key, g => g.ToList());

        //var kostymsBySubdiv = kostymsList
        //    .GroupBy(k => k.SubdivisionId)
        //    .ToDictionary(g => g.Key, g => g.ToList());
        #endregion
        // 3. Строим словарь узлов по Id
        var nodeDict = new Dictionary<int, ReportNode>();
        foreach (var psg in allNodes)
        {
            var node = new ReportNode
            {
                Id = psg.Id,
                Name = psg.Garnizon ?? psg.Fullname ?? psg.Garnizon,
                Category = getCategory(psg),
                ParentId = psg.Parent ?? 0,
                // взять rowId из psg - из словаря id,rowId
                RawData = new Dictionary<string, Dictionary<string, Dictionary<string, decimal>>>()
                
            };
            if (node.Category.Trim() == "")
                node.Category = "";
            // Заполняем RawData для листьев (если есть данные)
            if (sredstvaBySubdiv.ContainsKey(psg.Id))
            {
                var sredstvaForNode = sredstvaBySubdiv[psg.Id];
                var sredstvaDict = new Dictionary<string, Dictionary<string, decimal>>();// Пока только sredstva, Остальные потом
                foreach (var s in sredstvaForNode)
                {
                    var fields = new Dictionary<string, decimal>
                    {
                        ["br"] = s.Br ?? 0,
                        ["rezerv"] = s.Rezerv ?? 0,
                        ["remont"] = s.Remont ?? 0
                    };
                    sredstvaDict[s.NameSredstvo] = fields;
                }
                node.RawData["sredstva"] = sredstvaDict;  // Аналог окна средств - имя, поля
            }

            // Аналогично для Sostav, Sizod, Penas, Kostyms...
            // ... (можно вынести в отдельный метод)

            nodeDict[psg.Id] = node;
        }

        // 4. Строим дерево, связывая детей с родителями
        
        foreach (var node in nodeDict.Values)
        {
            if (node.ParentId == 0) // корень?
            {
                // Можно определить корень по условию, например, если это территориальный (id=11)
                if (node.Id == 11) root = node;
                continue;
            }
            if (nodeDict.ContainsKey(node.ParentId))
            {
                var parent = nodeDict[node.ParentId];
                parent.Children.Add(node);
            }
        }

        // Строим словарь rowId
        var rowIdMap = BuildRowIdMap();

        // Присваиваем RowId для каждого узла
        AssignRowIds(root, rowIdMap);

        return root;
    }

    private string getCategory(Psgdatum psgdata) {

        if (psgdata.Garntype.Trim() != "")
            return psgdata.Garntype;
        var ptr = psg_total_rows.Where(p => p.PsgId == psgdata.Id).FirstOrDefault();
        if (ptr == null)
            return "";
        return ptr.CategoryDisplay;


    }

    // -------------------------------------------
    // 3.4 ГЕНЕРАЦИЯ СТРОК PivotRow
    // -------------------------------------------
    public List<PivotRow> GeneratePivotRows(ReportNode rootNode)
    {
        if (rootNode == null)
            rootNode = BuildTree();

        //Инициализируем конфиги всех колонок
        InitializeColumnConfigs();

        var result = new List<PivotRow>();

        // 1. Листья (ПЧ)
        var leaves = GetAllLeaves(rootNode);
        foreach (var leaf in leaves)
        {
            var row = CreateLeafRow(leaf);
            result.Add(row);
        }

        // 2. Итоги по ПСГ (для каждой категории)
        foreach (var psgNode in rootNode.Children)
        {
            var psgRows = CreateSummaryRows(psgNode, PivotConfigs.PsgLevelConfig, isTerritorial: false);
            result.AddRange(psgRows);
        }

        // 3. Территориальные итоги
        var territorialRows = CreateSummaryRows(rootNode, PivotConfigs.TerritorialLevelConfig, isTerritorial: true);
        result.AddRange(territorialRows);

        return result;
    }

    // Создание строки для листа
    private PivotRow CreateLeafRow(ReportNode leaf)
    {
        var row = new PivotRow
        {
            ПСГ = GetPsgNameForNode(leaf),
            ПЧ = leaf.Name,
            Category = leaf.Category,
            PchId = leaf.Id,
            Parent = leaf.ParentId,
            Norder = 0,
            Isitog = 0,
            //RowId1 = leaf.
        };

        // Заполняем все числовые поля с помощью конфигураций
        foreach (var kv in columnConfigs)
        {
            var propName = kv.Key;
            var config = kv.Value;
            var value = ComputeLeafValue(leaf, config);
            SetProperty(row, propName, value);
        }

        // Отдельно обрабатываем текстовое поле Nachkar
        if (leaf.RawData.TryGetValue("sostav", out var sostavDict) && sostavDict.TryGetValue("sostav", out var fields))
        {
            // Предположим, что Nachkar хранится в отдельном поле, или берём из sostav
            // В реальности может быть свойство Nachkar в таблице Sostav.
            row.Nachkar = fields.TryGetValue("nachkar_text", out var val) ? val.ToString() : "";
        }

        // Datafilled – вычисляем как наличие данных в ключевых полях (например, сумма всех br > 0)
       // row.Datafilled = IsDatafilled(leaf);  ДРУГОЙ СМЫСЛ ПОЛЯ

        return row;
    }

    // Создание итоговых строк для узла (ПСГ или территориальный)
    private List<PivotRow> CreateSummaryRows(ReportNode node, LevelConfig levelConfig, bool isTerritorial)
    {
        var rows = new List<PivotRow>();
        foreach (var categoryRule in levelConfig.Categories)
        {

            // Если нет в списке итогов - пропускаем

            var row = new PivotRow
            {
                ПСГ = isTerritorial ? "Территориальный" : node.Name,
                ПЧ = categoryRule.CategoryId == "main" ? "всего" : "в т.ч. " + categoryRule.CategoryId.ToUpper(),
                Category = categoryRule.CategoryId,
                PchId = -node.Id, // отрицательный для итогов
                Parent = node.ParentId == 0 ? (int?)null : node.ParentId,
                Norder = 0,
                Isitog = 1,
                RowId1 = $"{node.Id}_{categoryRule.CategoryId}" // можно сгенерировать
            };

            // Вычисляем значения для каждой колонки с учётом категории
            foreach (var kv in columnConfigs)
            {
                var propName = kv.Key;
                var config = kv.Value;
                var value = ComputeNodeValue(node, config, levelConfig, categoryRule.CategoryId);
                SetProperty(row, propName, value);
            }

            // Текстовое поле Nachkar для итоговых строк – пустое
            row.Nachkar = "";
            row.Datafilled = false;

            rows.Add(row);
        }
        return rows;
    }
    private decimal ComputeNodeValue(ReportNode node, ColumnConfig config, LevelConfig levelConfig, string categoryId)
    {
        var rule = levelConfig.Categories.FirstOrDefault(r => r.CategoryId == categoryId);
        if (rule == null) return 0;

        decimal total = 0;
        // Если узел лист – проверяем условие
        if (node.Children.Count == 0)
        {
            if (rule.Condition(node))
                return ComputeLeafValue(node, config);
            return 0;
        }

        // Иначе суммируем детей   ВОЗМОЖНО ЛЕГЧЕ НЕ РАСКРУЧИВАТЬ ДЕРЕВО, А СДЕЛАТЬ 3 ПРОЦЕДУРЫ - ДЛЯ ЛИСТА, ПСГ , ТЕРРИТОРИАЛЬНОГО
        // Для категорий ВСЕГО или ГПС - везде одинаково - проще
        // Для остальных посмотреть

        foreach (var child in node.Children)
        {
            // Для территориального уровня исключаем некоторые узлы (флаг IncludeInTerritorial) - ИЛИКАК ТО ЕЩЁ ВРОДЕ ВИДЕЛ - В ВИДЕ УСЛОВИЯ
            if (levelConfig.LevelId == "territorial" && !child.IncludeInTerritorial)
                continue;

            if (rule.Condition(child))
                total += ComputeNodeValue(child, config, levelConfig, categoryId);
        }
        return total;
    }


    // Получить все листья дерева
    List<ReportNode> GetAllLeaves(ReportNode node)
    {
        var leaves = new List<ReportNode>();
        if (node.Children.Count == 0)
            leaves.Add(node);
        else
            foreach (var child in node.Children)
                leaves.AddRange(GetAllLeaves(child));
        return leaves;
    }
    // Функция вычисления значения для листа по колонке
    private decimal ComputeLeafValue(ReportNode node, ColumnConfig config)
    {
        if (!node.RawData.TryGetValue(config.SourceTable, out var sourceDict))
            return 0;

        decimal total = 0;
        foreach (var kv in sourceDict)
        {
            var key = kv.Key;
            var fields = kv.Value;

            // Фильтр по списку имён (если задан)
            if (config.FilterValues.Count > 0 && !config.FilterValues.Contains(key))
                continue;

            // Используем делегат вычисления
            total += config.GetValue(fields);
        }
        return total;
    }

    // Вспомогательный метод для получения имени ПСГ для листа
    private string GetPsgNameForNode(ReportNode node)
    {
        // Ищем родителя с типом "ПСГ" (у него есть дети, и он не корень)
        var parent = FindParentPsg(node);
        return parent?.Name ?? "Без ПСГ";
    }

    private ReportNode FindParentPsg(ReportNode node)
    {
        if (node.ParentId == 0) return null;
        var parent = FindNodeById(root, node.ParentId); // нужен доступ к root
        if (parent == null) return null;
        if (parent.Children.Count > 0 && parent.ParentId != 0) // если не корень и есть дети
            return parent;
        return FindParentPsg(parent);
    }

    private ReportNode FindNodeById(ReportNode root_, int id)
    {
        if (root_.Id == id) return root_;
        foreach (var child in root_.Children)
        {
            var found = FindNodeById(child, id);
            if (found != null) return found;
        }
        return null;
    }
    private void AssignRowIds(ReportNode node, Dictionary<string, string> rowIdMap)
    {
        if (node == null) return;

        string key = null;
        if (node.Children.Count == 0)
        {
            // Лист: ключ = Id
            key = node.Id.ToString();
        }
        else
        {
            // Итоговый узел: ключ = Name + "_" + категория (очищенная)
            var psg = node.Name?.Trim();
            var category = node.Category?.Trim();
            if (!string.IsNullOrEmpty(psg) && !string.IsNullOrEmpty(category))
            {
                category = CleanCategory(category);
                key = $"{psg}_{category}";
            }
        }

        if (key != null && rowIdMap.TryGetValue(key, out var rowId))
        {
            node.RowId = rowId;
        }

        // Рекурсивно для детей
        foreach (var child in node.Children)
        {
            AssignRowIds(child, rowIdMap);
        }
    }
    public Dictionary<string, string> BuildRowIdMap()
    {
        var map = new Dictionary<string, string>();

        var allRows = _context.FirePsgStats.ToList();

        foreach (var row in allRows)
        {
            string key = null;
            if (row.Isitog == 0 && row.PchId.HasValue)
            {
                // Лист: ключ = код ПЧ (как строка)
                key = row.PchId.Value.ToString();
            }
            else if (row.Isitog == 1)
            {
                // Итог: ключ = ПСГ + "_" + категория (очищенная)
                var psg = row.Псг?.Trim();
                var category = row.Category?.Trim();
                if (!string.IsNullOrEmpty(psg) && !string.IsNullOrEmpty(category))
                {
                    // Удаляем "в т.ч.", "по" и лишние пробелы
                    category = CleanCategory(category);
                    key = $"{psg}_{category}";
                }
            }

            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(row.RowId1))
            {
                // Если ключ уже есть, можно перезаписать (последняя запись), или проверить конфликт
                if (!map.ContainsKey(key))
                    map[key] = row.RowId1;
            }
        }

        return map;
    }

    // Вспомогательный метод для очистки категории
    private string CleanCategory(string category)
    {
        if (string.IsNullOrEmpty(category)) return category;

        // Удаляем "в т.ч." и "по" (регистронезависимо)
        var cleaned = category;
        cleaned = cleaned.Replace("в т.ч.", "", StringComparison.OrdinalIgnoreCase);
        cleaned = cleaned.Replace(" по ", "", StringComparison.OrdinalIgnoreCase);

        // Заменяем множественные пробелы на один
        cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, @"\s+", " ");

        // Обрезаем пробелы в начале и конце
        cleaned = cleaned.Trim();

        // Если очищенная строка пуста, возвращаем исходную (или что-то по умолчанию)
        if (string.IsNullOrEmpty(cleaned))
            return category.Trim();

        return cleaned;
    }
    // -------------------------------------------
    // 3.6 ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ
    // -------------------------------------------

    private void SetProperty(PivotRow row, string propName, decimal value)
    {
        var prop = typeof(PivotRow).GetProperty(propName);
        if (prop != null && prop.CanWrite)
            prop.SetValue(row, Convert.ChangeType(value, prop.PropertyType));
    }

    private bool IsDatafilled(ReportNode node)
    {
        // Проверяем наличие данных в ключевых полях (например, хотя бы одна единица техники > 0)
        if (node.RawData.TryGetValue("sredstva", out var srcDict))
        {
            foreach (var kv in srcDict)
            {
                foreach (var f in kv.Value.Values)
                    if (f > 0) return true;
            }
        }
        return false;
    }

    Dictionary<string, ColumnConfig> columnConfigs;

    private void InitializeColumnConfigs()
    {
        columnConfigs = new Dictionary<string, ColumnConfig>
        {
            // ---- Боевой расчёт (br) ----
            ["AcBr"] = new ColumnConfig
            {
                PropertyName = "AcBr",
                SourceTable = "sredstva",
                FilterValues = new List<string> { "АЦ" },
                AggregateField = "br"   // простое суммирование br по АЦ
            },
            ["AclBr"] = new ColumnConfig
            {
                PropertyName = "AclBr",
                SourceTable = "sredstva",
                FilterValues = new List<string> { "АЦЛ" },
                AggregateField = "br"
            },
            // ... аналогично для всех br-колонок

            // ---- Ремонт основной (remont по списку) ----
            ["RemontOsnovnoy"] = new ColumnConfig
            {
                PropertyName = "RemontOsnovnoy",
                SourceTable = "sredstva",
                FilterValues = new List<string> { "АЦ", "АЦЛ", "АВ", "АСА", "АПП", "ПНС", "АНР" },
                AggregateField = "remont"   // суммируем remont по этим наименованиям
            },

            // ---- ПОЖАРНЫЙ_КОРАБЛЬ_РЕМОНТ (remont + rezerv) ----
            ["PozhKorablRemont"] = new ColumnConfig
            {
                PropertyName = "PozhKorablRemont",
                SourceTable = "sredstva",
                FilterValues = new List<string> { "Пожарный_корабль" },
                Compute = fields => fields.GetValueOrDefault("remont", 0) + fields.GetValueOrDefault("rezerv", 0)
            },

            // ---- ПЛАВ_СРЕДСТВА (br+remont+rezerv) ----
            ["PlavSredstva"] = new ColumnConfig
            {
                PropertyName = "PlavSredstva",
                SourceTable = "sredstva",
                FilterValues = new List<string> { "Плав_средства" }, // название в БД
                Compute = fields => fields.GetValueOrDefault("br", 0) + fields.GetValueOrDefault("remont", 0) + fields.GetValueOrDefault("rezerv", 0)
            },

            // ---- БОЛОТОХОДЫ (br+remont+rezerv) ----
            ["Bolotohody"] = new ColumnConfig
            {
                PropertyName = "Bolotohody",
                SourceTable = "sredstva",
                FilterValues = new List<string> { "Болотоходы" },
                Compute = fields => fields.GetValueOrDefault("br", 0) + fields.GetValueOrDefault("remont", 0) + fields.GetValueOrDefault("rezerv", 0)
            },

            // ---- ДТ (br+rezerv) ----
            ["Dt"] = new ColumnConfig
            {
                PropertyName = "Dt",
                SourceTable = "sredstva",
                FilterValues = new List<string> { "ДТ" },
                Compute = fields => fields.GetValueOrDefault("br", 0) + fields.GetValueOrDefault("rezerv", 0)
            },

            // ---- СИЗОД (br) ----
            ["SizodBr"] = new ColumnConfig
            {
                PropertyName = "SizodBr",
                SourceTable = "sizod",
                AggregateField = "br"   // в sizod есть поля br и rezerv
            },
            ["SizodRezerv"] = new ColumnConfig
            {
                PropertyName = "SizodRezerv",
                SourceTable = "sizod",
                AggregateField = "rezerv"
            },

            // ---- Личный состав (sostav) ----
            ["PoSpisku"] = new ColumnConfig
            {
                PropertyName = "PoSpisku",
                SourceTable = "sostav",
                AggregateField = "po_spisku"
            },
            ["Nalico"] = new ColumnConfig
            {
                PropertyName = "Nalico",
                SourceTable = "sostav",
                // в sostav поле "nalico" вычисляется через сумму, но мы можем оставить как есть
                AggregateField = "nalico"
            },
            // ... все остальные колонки по аналогии

            // ---- Пена и порошок (penas) ----
            ["PenaRaschet"] = new ColumnConfig
            {
                PropertyName = "PenaRaschet",
                SourceTable = "penas",
                AggregateField = "pena_br"
            },
            ["PenaRezerv"] = new ColumnConfig
            {
                PropertyName = "PenaRezerv",
                SourceTable = "penas",
                AggregateField = "pena_rezerv"
            },
            // ... и т.д.
        };
    }


    //public class ReportNode
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string Category { get; set; }
    //    public int ParentId { get; set; }
    //    public List<ReportNode> Children { get; set; } = new List<ReportNode>();
    //    public string RowId { get; set; }  // Id строки

    //    // RawData: sourceType -> (itemName -> (fieldName -> value))
    //    public Dictionary<string, Dictionary<string, Dictionary<string, decimal>>> RawData { get; set; }
    //        = new Dictionary<string, Dictionary<string, Dictionary<string, decimal>>>();
    //    //RawData = new Dictionary<string, Dictionary<string, decimal>>()
    //    // Признак участия в территориальном итоге (по умолчанию true)
    //    public bool IncludeInTerritorial { get; set; } = true;
    //}

    // ============================================================
    // 1. РАСШИРЕННЫЙ КЛАСС PivotRow (все поля грида)
    // ============================================================
    public class PivotRow
    {
        // --- Иерархия ---
        public string ПСГ { get; set; }
        public string ПЧ { get; set; }
        public string Category { get; set; }
        public int PchId { get; set; }
        public string RowId1 { get; set; }   // 14-символьный идентификатор (как в FirePsgStat)
        public int? Parent { get; set; }
        public int Norder { get; set; }
        public int Isitog { get; set; }

        // --- Колонки (свойства, соответствующие DataPropertyName в гриде) ---

        // Боевой расчёт (br)
        public decimal AcBr { get; set; }               // АЦ
        public decimal AclBr { get; set; }              // АЦЛ
        public decimal AvBr { get; set; }               // АВ
        public decimal AcaAppBr { get; set; }           // АСА/АПП
        public decimal PnsBr { get; set; }              // ПНС
        public decimal AlBr { get; set; }               // АЛ
        public decimal KpBr { get; set; }               // КП
        public decimal ArBr { get; set; }               // АР
        public decimal AsmpBr { get; set; }             // АСМП
        public decimal AshBr { get; set; }              // АШ
        public decimal UksBr { get; set; }              // УКС / АБГ
        public decimal FireTrainBr { get; set; }        // Пож. поезд
        public decimal PozhKorablBr { get; set; }       // Пож. корабль/катер

        // Резерв (rezerv)
        public decimal AcRezerv { get; set; }
        public decimal AclRezerv { get; set; }
        public decimal AnrRezerv { get; set; }
        public decimal AvRezerv { get; set; }
        public decimal AcaAppRezerv { get; set; }
        public decimal PnsRezerv { get; set; }
        public decimal AlRezerv { get; set; }
        public decimal KpRezerv { get; set; }
        public decimal ArRezerv { get; set; }
        public decimal AsmpRezerv { get; set; }
        public decimal AshRezerv { get; set; }
        public decimal UksRezerv { get; set; }
        public decimal AsmrhRezerv { get; set; }        // АСМПХ
        public decimal AvsRezerv { get; set; }          // АВС
        public decimal PozhKorablRezerv { get; set; }   // Пож. корабль/катер резерв

        // Ремонт (remont)
        public decimal RemontOsnovnoy { get; set; }     // основная техника
        public decimal RemontSpetsialnoy { get; set; }  // спецтехника

        // ТО
        public decimal Tofirst { get; set; }            // ТО-1
        public decimal Totow { get; set; }              // ТО-2

        // Прочие средства (из других таблиц)
        public decimal PlavSredstva { get; set; }       // плавсредства
        public decimal Bolotohody { get; set; }         // снегоходы/болотоходы
        public decimal Motopompy { get; set; }          // мотопомпы
        public decimal Prochee { get; set; }            // прочее

        // СИЗОД
        public decimal SizodBr { get; set; }            // в расчёте
        public decimal SizodRezerv { get; set; }        // в резерве

        // Костюмы
        public decimal KostumyL1Task { get; set; }      // Л1/ОЗК/ТАСК
        public decimal KostumyTok { get; set; }         // ТОК

        // ГАСИ
        public decimal GasiRaschet { get; set; }        // в расчёте
        public decimal GasiRezerv { get; set; }         // в резерве

        // Личный состав
        public decimal PoSpisku { get; set; }
        public decimal Nalico { get; set; }
        public decimal Vsego { get; set; }
        public decimal RezervLS { get; set; }
        public decimal Nk { get; set; }                 // начальник караула
        public decimal Dispetcher { get; set; }
        public decimal Pnk { get; set; }
        public decimal Ko { get; set; }                 // командир отделения
        public decimal Voditel { get; set; }
        public decimal Pozharny { get; set; }
        public decimal Gdzs { get; set; }
        public decimal VsegoOts { get; set; }
        public decimal Otpusk { get; set; }
        public decimal PoBolnicnomu { get; set; }
        public decimal Komandirovka { get; set; }
        public decimal Nekomplekt { get; set; }
        public decimal ProchieOts { get; set; }

        // Пена и порошок
        public decimal PenaRaschet { get; set; }
        public decimal PoroshokRaschet { get; set; }
        public decimal PenaRezerv { get; set; }
        public decimal PoroshokRezerv { get; set; }

        // Топливо
        public decimal Dt { get; set; }                 // дизтопливо
        public decimal Benzin { get; set; }             // бензин

        // Начкар (текстовое поле) – в FirePsgStat это string? В гриде Column53_67 – "Начкар". Оставим как string.
        public string Nachkar { get; set; }

        // Datafilled – булево (показывает, заполнена ли строка)
        public bool Datafilled { get; set; }

        // RowId (из таблицы)
        public string RowId { get; set; }  // может дублироваться с RowId1
    }


}
// АСФ: 1889 - асф итоги территориальный, то только 1 строка из Прионежского района. 


//private List<psgdata> подчиненныеИтога(psgdata корень)
//{
//    if (корень.id == 1889)
//    {
//        List<psgdata> asfList = new List<psgdata>();
//        psgdata asfPetro = mainForm.context.psgdatas.Where(c => c.id == 1793).FirstOrDefault();
//        asfList.Add(asfPetro);
//        return asfList;
//    }
//    //List<psgdata> lll = new List<psgdata>();
//    //    psgdata родитель = корень;

//    List<psgdata> lst1 = new List<psgdata>();
//    List<psgdata> lst2;

//    #region Если это итоги то родитель - это гарнизон территориальный или местный
//    psgdata родитель = корень;
//    if (корень.garnizon.Contains("итоги"))
//        родитель = корень.psgdata2;
//    else
//        родитель = корень;
//    List<psgdata> lst = родитель.psgdata1.Where(c => c.old).ToList();

//    #endregion
//    #region Формируем список подчиненных -> lst1      ИСКЛЮЧАЕМ ВСЕ СТРОКИ КОТОРЫЕ СОДЕРЖАТ В НАЗВАНИИ ПОДСТРОКУ итоги
//    if (родитель.id != 11)//tpsg)
//        lst1 = lst.Where(c => !c.garnizon.Contains("итоги")).ToList(); //Если не корневой, то просто подчиненные -> в список  ЭТО ДЛЯ    ТЕРРИТОРИАЛЬНОГО ГАРНИЗОНА
//    else
//    {
//        foreach (psgdata p in lst)
//            lst1.AddRange(p.psgdata1.Where(c => !c.garnizon.Contains("итоги"))); // если корневой, то у всех дочерних складываем в один списки подчиненных   ВКЛЮЧАЕМ ВСЕ СТРОКИ 
//    }
//    #endregion
//    #region Теперь выделяем одну из частей списка - для "ГПС" (т.е. ФПС или ППС) или "другие"  -> lst2
//    if (корень.garnizon.Contains("ГПС"))
//        lst2 = lst1.Where(c => (c.garntype.Contains("ФПС")) || (c.garntype.Contains("ППС"))).ToList();
//    else if (корень.garnizon.Contains("ЧПО"))
//        lst2 = lst1.Where(c => c.garntype.Contains("ЧПО")).ToList();
//    else if (корень.garnizon.Contains("ФПС"))
//        lst2 = lst1.Where(c => (c.garntype.Contains("ФПС") || (c.garnizon.Trim() == "ПЧ-75")) && (c.parent != 1744)).ToList();
//    else if (корень.garnizon.Contains("другие") || (корень.garnizon.Contains("ВПО")))
//    {
//        if (корень.parent != 11)
//            lst2 = lst1.Where(c => (!c.garntype.Contains("ФПС")) && (!c.garntype.Contains("ППС"))).ToList();
//        else
//        {
//            if (корень.garnizon.Contains("ВПО"))
//                lst2 = lst1.Where(c => (c.garntype.Contains("ВПО"))).ToList();
//            else
//            {
//                lst2 = lst1.Where(c => (!c.garntype.Contains("ФПС")) && (!c.garntype.Contains("ППС")) && (!c.garntype.Contains("ВПО"))).ToList();
//                lst2 = lst2.Where(c => !c.garntype.Contains("ЧПО")).ToList();
//            }

//        }

//    }
//    else
//        lst2 = lst1;


//    //else if (корень.garnizon.Contains("другие"))
//    //    lst2 = lst1.Where(c => (!c.garntype.Contains("ФПС")) && (!c.garntype.Contains("ППС"))).ToList();
//    //else
//    //    lst2 = lst1;


//    #endregion
//    List<psgdata> lst3 = lst2.Where(c => c.old && !c.garnizon.Contains("АСФ")).ToList();
//    return lst3;// lst2.Where(c => c.old).ToList();

//}