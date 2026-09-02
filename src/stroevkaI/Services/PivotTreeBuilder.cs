using StorageI.ModelsStroevkaMySql;
using stroevkaI.Models;
using Microsoft.EntityFrameworkCore;

namespace stroevkaI.Services { 
public class PivotTreeBuilder
{
   static  List<PivotRow> allPivotRows;
    Dictionary<int, Psgstat> _psgDict;
    List<PsgTotalRow> psg_total_rows;
    ReportNode root = null;
    static Dictionary<int, List<PivotRow>> psgChildes;

        public static stroevkaContext _context = new stroevkaContext();

    public PivotTreeBuilder()
    {

    }

    public ReportNode BuildTree()
    {
       var allNodes = _context.Psgstats
            .Where(p => p.Used == 1) 
            .ToList();

        // Заполняем словарь для быстрого доступа по Id
        _psgDict = allNodes.ToDictionary(p => p.Id, p => p);

        // 2. Загружаем сырые данные для листьев (как было)
        var sredstvaList = _context.Sredstvas.ToList();
        var sostavList = _context.Sostavs.ToList();
        var sizodList = _context.Sizods.ToList();
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
                Name = psg.Name, // используем подходящее поле
                displayName = psg.Displayname, // используем подходящее поле
                Category = psg.Garntype ?? "",
                ParentId = psg.Parent ?? 0,
                Isitog = psg.Isitog ?? 0,
                Norder = (int)psg.Norder,
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
            if (node.Id == 11) root = node;
         
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
        #region Инициализация колонок и расчёт строк для листьев (ПЧ)
        if (rootNode == null)
            rootNode = BuildTree();

        InitializeColumnConfigs();
        var result = new List<PivotRow>();

        // 1. Листья (ПЧ)
        var leaves = GetAllLeaves(rootNode);
        foreach (ReportNode leaf in leaves)
            result.Add(CreateLeafRow(leaf)); // проверим что есть idPsg и сделаем словарь <idPsg,List<pivotRows>>
                                             // psgChildes.ToDictionary<leaf.parentId,>  
            psgChildes = result
                .GroupBy(c=>c.Parent)
                .ToDictionary(g => (int)g.Key, g => g.ToList());

            #endregion

            #region 2. Итоговые строки для районных ПСГ
        var psgNodes = rootNode.Children.Where(c => c.Children.Any()).ToList();
        var allPsgRows = new List<PivotRow>();
        foreach (var psgNode in psgNodes)
        {
            var psgRows = ComputePsgSummaryRows(psgNode);
                #region Добавляем строки с ПЧ
                    var psgВсего = psgRows.Where(c => c.Category == "всего").FirstOrDefault();
                    if (psgВсего != null) {
                        if (psgChildes.TryGetValue((int)psgВсего.PchId, out var childes))
                        {
                            psgВсего.Childes.AddRange(childes);
                        }
                    }
                #endregion
                result.AddRange(psgRows);
            allPsgRows.AddRange(psgRows);
        }
        #endregion

        #region 3. Территориальные итоги  { "ВПО", "ЧПО", "другие", "АСФ" }
        var territorialRows = new List<PivotRow>();


         //  3.1. Обычные категории (ВПО, ЧПО, другие, АСФ) ---
        foreach (var cat in new[] { "ВПО", "ЧПО", "другие", "АСФ" })
        {
            var rows = GetPsgRowsByCategory(allPsgRows, cat);
            var row = CreateTerritorialRow(rootNode, cat, rows);
            if (row != null)  territorialRows.Add(row);
        }

        #endregion

        #region ГПС, ФПС - территориальный
        // --- 3.2. ГПС ---
        List<PivotRow> gpsRows = GetPsgRowsByCategory(allPsgRows, "ФПС");
        var ppsRows = GetPsgRowsByCategory(allPsgRows, "ППС");
        gpsRows.AddRange(ppsRows);
        var gpsRow = CreateTerritorialRow(rootNode, "ГПС", gpsRows);
        if (gpsRow != null) territorialRows.Add(gpsRow);


        // --- 3.3. ФПС (особая логика) ---
        var fpsRow = ComputeTerritorialFpsRow(rootNode, allPsgRows);//,result);
        if (fpsRow != null) territorialRows.Add(fpsRow);
        #endregion

        #region 3.4. «всего» (ГПС + другие + ЧПО + ВПО) ---
        var rowsForTotal = territorialRows
            .Where(r => r.Category == "ГПС" || r.Category == "другие" || r.Category == "ЧПО" || r.Category == "ВПО")
            .ToList();
        PivotRow totalRow = CreateTerritorialRow(rootNode, "всего", rowsForTotal);
        //Итоговые по всем ПСГ -> список дочерних в "Территориальный(всего)"
        totalRow.Childes.AddRange(result.Where(c => c.Category == "всего").ToList());
        //добавляем в дочерние - ГПС,другие,ЧПО,ВПО 
        totalRow.Childes.AddRange(rowsForTotal);
            //добавляем в дочерние - ФПС,другие,ЧПО,ВПО 
        totalRow.Childes.Add(fpsRow);
            var asfRow = territorialRows.Where(c => c.Category == "АСФ").FirstOrDefault();
        if(asfRow != null)
            totalRow.Childes.Add(asfRow);
        if (totalRow != null) territorialRows.Add(totalRow);

        result.AddRange(territorialRows);
            #endregion
        allPivotRows = result;
        return result;
    }
    private PivotRow CreateTerritorialRow(ReportNode rootNode, string categoryName, List<PivotRow> rowsToSum)
    {
            Dictionary<string, string> displayNames = new Dictionary<string, string>() {
             {"всего","Территориальный" },
             {"другие","    другие категории" },
             {"другиеПСГ","    другие категории" },
             {"ФПС","    в т.ч. ФПС" },
             {"ГПС","    по ГПС" },
             {"ЧПО","    по ЧПО" },
             {"АСФ","    по АСФ" },
             {"ВПО","    по ВПО" },
             {"ППС","    по ППС" }
         };

            // Если список пуст – возвращаем null (строку не создаём)
            if (rowsToSum == null || !rowsToSum.Any())
            return null;

        var row = new PivotRow
        {
            ПСГ = "Территориальный",
            Category = categoryName,
            PchId = rootNode.Id,
            Parent = 11,  // родитель - не важно кто, для порядка поставим Территориальный (он имеет категорию "всего")
            Isitog = 1,
            Norder = rootNode.Norder
        };
            if (displayNames.ContainsKey(categoryName))
                row.ПЧ = displayNames[categoryName];
            else
                row.ПЧ = "Не определено";


            // Суммируем все числовые свойства
            foreach (var prop in typeof(PivotRow).GetProperties())
        {
            if (prop.PropertyType == typeof(decimal) && prop.CanWrite)
            {
                decimal total = 0;
                foreach (var r in rowsToSum)
                    total += (decimal)prop.GetValue(r);
                prop.SetValue(row, total);
            }
        }

        // Для итоговых строк эти поля пустые
        row.Nachkar = "";
        row.Datafilled = false;
        return row;
    }
    private List<PivotRow> GetPsgRowsByCategory(List<PivotRow> allPsgRows, string category)
    {
        return allPsgRows.Where(r => r.Category == category).ToList();
    }
    private PivotRow ComputeTerritorialFpsRow(ReportNode rootNode, List<PivotRow> allPsgRows)
    {
        // 1. Берём все строки ПСГ с категорией "ФПС"
        var fpsRows = allPsgRows.Where(r => r.Category == "ФПС").ToList();

        // 2. Исключаем строки, принадлежащие Прионежскому ПСГ
        //    Предположим, что в allPsgRows есть поле ПСГ (имя или Id) – мы можем отфильтровать
        //    Например, если мы храним имя ПСГ в свойстве ПСГ строки:
        fpsRows = fpsRows.Where(r => r.ПСГ != "Прионежский").ToList();

        // 3. Добавляем ПЧ-75 (лист) – если она не входит в уже отобранные строки
        //    Находим лист ПЧ-75
        var pch75Leaf = GetAllLeaves(rootNode).FirstOrDefault(l => l.Name.Contains("ПЧ-75"));
        if (pch75Leaf != null)
        {
            // Создаём строку для листа (как в CreateLeafRow) и добавляем
            var leafRow = CreateLeafRow(pch75Leaf);
            // Если такая строка ещё не добавлена (проверяем по Id), добавляем
            if (!fpsRows.Any(r => r.PchId == leafRow.PchId))
                fpsRows.Add(leafRow);
        }

        // 4. Создаём территориальную строку для ФПС
        return CreateTerritorialRow(rootNode, "ФПС", fpsRows);
    }

    private List<PivotRow> ComputePsgSummaryRows(ReportNode psgNode)
    {

            // В чём различие? -
            // если это корень - то у него есть листья - ПЧ
            // можно просто завести словарь в ReportNode <категория, displayName> или здесь.
        var rows = new List<PivotRow>();  //соберёт 
        var leaves = GetAllLeaves(psgNode);
        var leavesByType = leaves
            .Where(l => !string.IsNullOrEmpty(l.Category))
            .Where(n => n.Isitog!=1)
            .GroupBy(l => l.Category)
            .ToDictionary(g => g.Key, g => g.ToList());

        // -1.ФПС
        var fpsLeaves = leavesByType.Where(kv => kv.Key == "ФПС").SelectMany(kv => kv.Value).ToList();
        rows.Add(CreateCategoryRow(psgNode, "ФПС", fpsLeaves));
        // 0. ППС
        var ppsLeaves = leavesByType.Where(kv => kv.Key == "ППС").SelectMany(kv => kv.Value).ToList();
        rows.Add(CreateCategoryRow(psgNode, "ППС", ppsLeaves));
        // 1. ГПС
        var gpsLeaves = leavesByType.Where(kv => kv.Key == "ФПС" || kv.Key == "ППС").SelectMany(kv => kv.Value).ToList();
            var всегоПСГrow = CreateCategoryRow(psgNode, "ГПС", gpsLeaves);
        rows.Add(всегоПСГrow);

            // 2. другие
            var otherLeaves = leavesByType.Where(kv => kv.Key != "ФПС" && kv.Key != "ППС" && kv.Key != "ЧПО" && kv.Key != "ВПО" && kv.Key != "АСФ").SelectMany(kv => kv.Value).ToList();
        rows.Add(CreateCategoryRow(psgNode, "другие", otherLeaves));
        // 2. други1
        var otherLeaves1 = leavesByType.Where(kv => kv.Key != "ФПС" && kv.Key != "ППС" && kv.Key != "АСФ").SelectMany(kv => kv.Value).ToList();
        var другиеПСГRow = CreateCategoryRow(psgNode, "другиеПСГ", otherLeaves);
        rows.Add(другиеПСГRow);// это другие для ПСГ (не территориального, т.к. в том ВПО,ЧПО отдельно)
                                                                       // 3. всего
        var всегоRow = CreateTotalRow(psgNode, rows.Where(r => r.Category == "ГПС" || r.Category == "другиеПСГ").ToList());


            var ВПО_ЧПО_АСФrows = new List<PivotRow>();
        // 4. ВПО, ЧПО, АСФ
        foreach (var cat in new[] { "ВПО", "ЧПО", "АСФ" })
        {
            if (leavesByType.TryGetValue(cat, out var catLeaves)) {
                    var r = CreateCategoryRow(psgNode, cat, catLeaves);
                    ВПО_ЧПО_АСФrows.Add(r);
                }
                rows.AddRange(ВПО_ЧПО_АСФrows);
            }
            //Сформировать строку "всего" для районного ПСГ и занести все предыдущие итоговые в childes
            всегоRow.Childes.AddRange(new List<PivotRow> { всегоПСГrow,другиеПСГRow });
            всегоRow.Childes.AddRange(ВПО_ЧПО_АСФrows);
        
            rows.Add(всегоRow);
        return rows;
    }
    private PivotRow CreateCategoryRow(ReportNode psgNode, string categoryName, List<ReportNode> leaves)
    {
            Dictionary<string, string> displayNames = new Dictionary<string, string>() {
             {"всего","" },
             {"другие","    другие категории" },
             {"другиеПСГ","    другие категории" },
             {"ФПС","    в т.ч. ФПС" },
             {"ГПС","    по ГПС" },
             {"ЧПО","    по ЧПО" },
             {"АСФ","    по АСФ" },
             {"ВПО","    по ВПО" },
             {"ППС","    по ППС" }
         };
            Dictionary<string, int> Norders = new Dictionary<string, int>() {
             {"всего",-20 },
             {"ГПС",-19 },
             {"другие",-18 },
             {"другиеПСГ",-17 },
             {"ФПС",-16 },
             {"ЧПО",-15 },
             {"АСФ",-10 },
             {"ВПО",-14 },
             {"ППС",0 }
         };

            var row = new PivotRow
        {
            ПСГ = psgNode.Name,
            Category = categoryName,
            PchId = psgNode.Id,
            Parent = psgNode.ParentId,
            Isitog = 1,
        };
            row.Norder = Norders[categoryName];
            if (displayNames.ContainsKey(categoryName))
                row.ПЧ = displayNames[categoryName];
            else if(categoryName == "всего")
                   row.ПЧ = psgNode.Name;
             else
                row.ПЧ = "Не определено";



            // Для каждой колонки суммируем значения по листьям
            foreach (var kv in columnConfigs)
        {
            var propName = kv.Key;
            var config = kv.Value;
            decimal total = 0;
            foreach (var leaf in leaves)
            {
                total += ComputeLeafValue(leaf, config);
            }
            SetProperty(row, propName, total);
        }

        return row;
    }
    private PivotRow CreateLeafRow(ReportNode leaf)
    {
        var row = new PivotRow
        {
            ПСГ = GetPsgNameForNode(leaf),
            ПЧ = leaf.Name, //  это просто Name(psgstat) =  garnizon(psgdata)
            Category = leaf.Category,
            PchId = leaf.Id,            // Id ПЧ т.к. это лист
            Parent = leaf.ParentId,     // parentId(psgstat) = parent(psgdata) 
            Norder = leaf.Norder,
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
    private PivotRow CreateTotalRow(ReportNode psgNode, List<PivotRow> rowsToSum)
    {
            var row = new PivotRow
            {
                ПСГ = psgNode.Name,
                ПЧ = psgNode.Name,
                Category = "всего",
                PchId = psgNode.Id,
                Parent = psgNode.ParentId,
                Norder = -30,
            Isitog = 1,
        };

        // Суммируем все числовые свойства из переданных строк
        foreach (var prop in typeof(PivotRow).GetProperties())
        {
            if (prop.PropertyType == typeof(decimal) && prop.CanWrite)
            {
                decimal total = 0;
                foreach (var r in rowsToSum)
                    total += (decimal)prop.GetValue(r);
                prop.SetValue(row, total);
            }
        }

        // Дополнительно можно скопировать текстовые поля (если нужно)
        // Например, Nachkar, Datafilled – для итогов обычно пустые

        return row;
    }
    // Создание итоговых строк для узла (ПСГ или территориальный)

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
        if ((node.Children.Count == 0) && (node.Isitog==0))
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

    private string GetPsgNameForNode(ReportNode node)
    {
        int? currentId = node.ParentId;
        while (currentId.HasValue && currentId != 0) // никогда не равен 0, но оставляем- не хуже
        {
            if (_psgDict.TryGetValue(currentId.Value, out var psg))
            {
                if (psg.Isitog == 1)
                    return psg.Displayname;// имена итоговых держим в psgstat
                currentId = psg.Parent;// иначе - просто имя psg
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

    public static List<PivotRow> GetPsgChildes(string _psgname)
    {
        List<PivotRow> lst = new List<PivotRow>();
            if (allPivotRows == null)
                return null;
        PivotRow psgRow = allPivotRows.Where(c => ((c.ПСГ.Contains(_psgname)) && (c.Category.Contains("всего")))).FirstOrDefault();//Лучше по Id ПЧ или гарнизона
        if (psgRow == null) return lst;
        lst.Add(psgRow);
        lst.AddRange(psgRow.Childes);
        return lst.OrderBy(c => c.Norder).ToList();
     }


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
        public List<PivotRow> Childes = new List<PivotRow>();

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
