//Удаляем методы:
//    getCategory()(теперь категория берётся из psgstat.garntype)

//    CleanCategory()
//    FindParentPsg(), FindNodeById()(если не нужны для других целей)

using StorageI.ModelsStroevkaMySql;
using stroevkaI.Models;
using Microsoft.EntityFrameworkCore;

public class PivotTreeBuilder
{
    Dictionary<int, Psgstat> _psgDict;
    List<PsgTotalRow> psg_total_rows;


    ReportNode root = null;
    public static stroevkaContext _context = new stroevkaContext();

    public PivotTreeBuilder()
    {

    }

    public ReportNode BuildTree()
    {
        // 1. Загружаем все узлы из psgstat (только используемые, если used=1)
        //var allNodes = _context.Psgstats
        //    .Include(p => p.Sredstvas)
        //    .Include(p => p.Sostavs)
        //    .Include(p => p.Sizods)
        //    .Include(p => p.Penas)
        //    .Include(p => p.Kostyms)
        //    .Include(p => p.Waters)
        //    .Include(p => p.Contacts)
        //    .Where(p => p.Used == 1)
        //    .ToList();

        var allNodes = _context.Psgstats
            .Where(p => p.Used == 1) // если есть поле used, иначе уберите Where
            .ToList();

        // Заполняем словарь для быстрого доступа по Id
        _psgDict = allNodes.ToDictionary(p => p.Id, p => p);

        // 2. Загружаем сырые данные для листьев (как было)



        var sredstvaList = _context.Sredstvas.ToList();
        var sostavList = _context.Sostavs.ToList();
        //var sizodList = _context.Sizods.ToList();
        //var penasList = _context.Penas.ToList();
        //var kostymsList = _context.Kostyms.ToList();
        //var watersList = _context.Waters.ToList();
        //var contactsList = _context.Contacts.ToList();

        // Группируем данные по subdivision_id (Id узла)
        var sredstvaBySubdiv = sredstvaList
            .GroupBy(s => s.SubdivisionId)
            .ToDictionary(g => g.Key, g => g.ToList());

        // 3. Строим словарь узлов по Id
        var nodeDict = new Dictionary<int, ReportNode>();
        foreach (var psg in allNodes)
        {
            var node = new ReportNode
            {
                Id = psg.Id,
                Name = psg.Name ?? psg.Displayname ?? psg.Name, // используем подходящее поле
                Category = psg.Garntype ?? "",
                ParentId = psg.Parent ?? 0,
                Isitog = psg.Isitog ?? 0,
                RawData = new Dictionary<string, Dictionary<string, Dictionary<string, decimal>>>()
            };

            // Заполняем RawData для листьев (только если это ПЧ, т.е. IsItog == 0)
            if (psg.Isitog == 0 && sredstvaBySubdiv.ContainsKey(psg.Id))
            {
                var sredstvaForNode = sredstvaBySubdiv[psg.Id];
                var sredstvaDict = new Dictionary<string, Dictionary<string, decimal>>();
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
                node.RawData["sredstva"] = sredstvaDict;
            }

            // Аналогично для других таблиц (Sostav, Sizod и т.д.) – можно вынести в отдельный метод

            nodeDict[psg.Id] = node;
        }

        // 4. Строим дерево, связывая детей с родителями
        ReportNode root = null;
        foreach (var node in nodeDict.Values)
        {
            if (node.ParentId == 0 || node.ParentId == node.Id)
            {
                // Корень – обычно id=1 (территориальный)
                if (node.Id == 1) root = node;
                continue;
            }
            if (nodeDict.ContainsKey(node.ParentId))
            {
                var parent = nodeDict[node.ParentId];
                parent.Children.Add(node);
            }
        }

        // Если корень не найден, возьмём узел с ParentId == 0
        if (root == null)
            root = nodeDict.Values.FirstOrDefault(n => n.ParentId == 0);

        return root;
    }


    // -------------------------------------------
    // 3.4 ГЕНЕРАЦИЯ СТРОК PivotRow
    // -------------------------------------------
    public List<PivotRow> GeneratePivotRows(ReportNode rootNode)
    {
        if (rootNode == null)
            rootNode = BuildTree();

        InitializeColumnConfigs();

        var result = new List<PivotRow>();

        // 1. Листья (ПЧ) – это узлы с IsItog == 0
        var leaves = GetAllLeaves(rootNode); // или можно отфильтровать по IsItog == 0
        foreach (var leaf in leaves)
        {
            var row = CreateLeafRow(leaf);
            result.Add(row);
        }

        // 2. Итоги по ПСГ (для каждой категории) – узлы с IsItog == 1, но не корень
        foreach (var psgNode in rootNode.Children) // дети корня – районные ПСГ
        {
            var psgRows = CreateSummaryRows(psgNode, PivotConfigs.PsgLevelConfig, isTerritorial: false);
            result.AddRange(psgRows);
        }

        // 3. Территориальные итоги – корень
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
        };

        // Заполняем числовые поля (как было)
        foreach (var kv in columnConfigs)
        {
            var propName = kv.Key;
            var config = kv.Value;
            var value = ComputeLeafValue(leaf, config);
            SetProperty(row, propName, value);
        }

        // Nachkar и Datafilled – как было
        // ...

        return row;
    }

    // Создание итоговых строк для узла (ПСГ или территориальный)

    private List<PivotRow> CreateSummaryRows(ReportNode node, LevelConfig levelConfig, bool isTerritorial)
    {
        var rows = new List<PivotRow>();
        foreach (var categoryRule in levelConfig.Categories)
        {
            var row = new PivotRow
            {
                ПСГ = isTerritorial ? "Территориальный" : node.Name,
                ПЧ = categoryRule.CategoryId == "main" ? "всего" : "в т.ч. " + categoryRule.CategoryId.ToUpper(),
                Category = categoryRule.CategoryId,
                PchId = -node.Id, // отрицательный для итогов
                Parent = node.ParentId == 0 ? (int?)null : node.ParentId,
                Norder = 0,
                Isitog = 1,
            };

            // Вычисляем значения для колонок
            foreach (var kv in columnConfigs)
            {
                var propName = kv.Key;
                var config = kv.Value;
                var value = ComputeNodeValue(node, config, levelConfig, categoryRule.CategoryId);
                SetProperty(row, propName, value);
            }

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

    private string GetPsgNameForNode(ReportNode node)
    {
        int? currentId = node.ParentId;
        while (currentId.HasValue && currentId != 0)
        {
            if (_psgDict.TryGetValue(currentId.Value, out var psg))
            {
                if (psg.Isitog == 1)
                    return psg.Displayname ?? psg.Name ?? "ПСГ";
                currentId = psg.Parent;
            }
            else break;
        }
        return "Без ПСГ";
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


    public class PivotRow
    {
        // --- Иерархия ---
        public string ПСГ { get; set; }
        public string ПЧ { get; set; }
        public string Category { get; set; }
        public int PchId { get; set; }
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


    }


}
