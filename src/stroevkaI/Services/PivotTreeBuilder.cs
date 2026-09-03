using StorageI.ModelsStroevkaMySql;
using stroevkaI.Models;
using System.Reflection;
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
            result.Add(CreateLeafRow(leaf)); // 
                                                
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
             {"ФПС","         в т.ч. ФПС" },
             {"ГПС","    по ГПС" },
             {"ЧПО","    по ЧПО" },
             {"АСФ","    по АСФ" },
             {"ВПО","    по ВПО" },
             {"ППС","    по ППС" }
         };
            Dictionary<string, int> Norders = new Dictionary<string, int>() {
             {"всего",-50 },
             {"ГПС",-49 },
             {"ФПС",-48 },
             {"другие",-47 },
             {"другиеПСГ",-46 },
             {"ВПО",-45 },
             {"ЧПО",-44 },
             {"АСФ",-43 },
             {"ППС",-40 }
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
          
        };
            if (displayNames.ContainsKey(categoryName))
                row.ПЧ = displayNames[categoryName];
            else
                row.ПЧ = "Не определено";
            row.Norder = Norders[categoryName];

            // Суммируем все числовые свойства
            foreach (var prop in typeof(PivotRow).GetProperties())
            {
                if (IsDecimalProperty(prop))
                {
                    decimal total = 0;
                    var details = new List<DetailItem>();

                    foreach (var r in rowsToSum)
                    {
                        var val = (decimal)prop.GetValue(r);
                        if (val != 0)
                        {
                            total += val;
                            // Используем детали из строки ПСГ (они уже есть)
                            if (r.CellDetails.TryGetValue(prop.Name, out var subDetails))
                            {
                                // Можно добавить префикс с названием ПСГ для ясности
                                foreach (var d in subDetails)
                                {
                                    details.Add(new DetailItem
                                    {
                                        Name = $"{r.ПСГ} → {d.Name}",
                                        Value = d.Value,
                                        Category = d.Category
                                    });
                                }
                            }
                            else
                            {
                                // Если деталей нет (например, для особых случаев), добавляем строку целиком
                                details.Add(new DetailItem { Name = $"{r.ПСГ} ({r.ПЧ})", Value = val });
                            }
                        }
                    }

                    prop.SetValue(row, total);
                    row.CellDetails[prop.Name] = details;
                }
            }

        // Для итоговых строк эти поля пустые
        row.Начкар = "";
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
        var otherLeaves = leavesByType.Where(kv => kv.Key != "ФПС" && kv.Key != "ППС"  && kv.Key != "ВПО" && kv.Key != "АСФ" && kv.Key != "ЧПО").SelectMany(kv => kv.Value).ToList();
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
            }
            rows.AddRange(ВПО_ЧПО_АСФrows);
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
             {"всего",20 },
             {"ГПС",21 },
             {"другие",23 },
             {"другиеПСГ",23 },
             {"ФПС",22 },
             {"ЧПО",25 },
             {"АСФ",27 },
             {"ВПО",26 },
             {"ППС",26 }
         };

            var row = new PivotRow
            {
                ПСГ = psgNode.Name,
                Category = categoryName,
                PchId = psgNode.Id,
                Norder = psgNode.Norder,
                Parent = psgNode.ParentId,
                Isitog = 1,
            };
            #region Устанавливаем Norder и ПЧ в зависимости от CategoryName   
            if (categoryName == "всего")
                row.Norder = psgNode.Norder;
            else
                row.Norder = Norders[categoryName];


            if (displayNames.ContainsKey(categoryName))
                row.ПЧ = displayNames[categoryName];
            else if(categoryName == "всего")
                    row.ПЧ = psgNode.Name;
                else
                row.ПЧ = "Не определено";
            #endregion


            // Для каждой колонки суммируем значения по листьям
            foreach (var kv in columnConfigs)
            {
                var propName = kv.Key;
                var config = kv.Value;
                decimal total = 0;
                var details = new List<DetailItem>();//  детали для показа составляющих суммы при наведении мышко
                foreach (var leaf in leaves)
                {
                    var value = ComputeLeafValue(leaf, config);
                    if (value != 0) // добавляем только ненулевые, чтобы не загромождать
                    {
                        total += value;
                        details.Add(new DetailItem
                        {
                            Name = leaf.Name,
                            Value = value,
                            Category = leaf.Category
                        });
                    }
                }
                SetProperty(row, propName, total);
                row.CellDetails[propName] = details;
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
                Norder = psgNode.Norder,
            Isitog = 1,
        };

            // Суммируем все числовые свойства из переданных строк
            foreach (var prop in typeof(PivotRow).GetProperties())
            {
                if (IsDecimalProperty(prop))
                {
                    decimal total = 0;
                    var details = new List<DetailItem>();

                    foreach (var r in rowsToSum)
                    {
                        var val = (decimal)prop.GetValue(r);
                        if (val != 0)
                        {
                            total += val;
                            // Добавляем детали из исходной строки (они уже содержат список ПЧ)
                            if (r.CellDetails.TryGetValue(prop.Name, out var subDetails))
                            {
                                details.AddRange(subDetails);
                            }
                            else
                            {
                                // если деталей нет, добавляем саму строку как единый элемент
                                details.Add(new DetailItem { Name = r.ПЧ, Value = val });
                            }
                        }
                    }

                    prop.SetValue(row, total);
                    row.CellDetails[prop.Name] = details;
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
            {
                prop.SetValue(row, value); // decimal → decimal? работает неявно
            }
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

    private void InitializeColumnConfigsOld()
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
            ["АвBr"] = new ColumnConfig
            {
                PropertyName = "АвBr",
                SourceTable = "sredstva",
                FilterValues = new List<string> { "Ав" },
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
    private void InitializeColumnConfigsOld11()
        {
            columnConfigs = new Dictionary<string, ColumnConfig>
            {
                // ===============================================
                // 1. БОЕВОЙ РАСЧЁТ (br) – таблица sredstva
                // ===============================================
                ["AcBr"] = new ColumnConfig
                {
                    PropertyName = "AcBr",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АЦ" },
                    AggregateField = "br"
                },
                ["AclBr"] = new ColumnConfig
                {
                    PropertyName = "AclBr",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АЦЛ" },
                    AggregateField = "br"
                },
                ["AvBr"] = new ColumnConfig
                {
                    PropertyName = "AvBr",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АВ" },
                    AggregateField = "br"
                },
                ["AcaAppBr"] = new ColumnConfig
                {
                    PropertyName = "AcaAppBr",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АСА", "АПП" }, // объединяем два типа
                    AggregateField = "br"
                },
                ["PnsBr"] = new ColumnConfig
                {
                    PropertyName = "PnsBr",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "ПНС" },
                    AggregateField = "br"
                },
                ["AlBr"] = new ColumnConfig
                {
                    PropertyName = "AlBr",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АЛ" },
                    AggregateField = "br"
                },
                ["KpBr"] = new ColumnConfig
                {
                    PropertyName = "KpBr",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "КП" },
                    AggregateField = "br"
                },
                ["ArBr"] = new ColumnConfig
                {
                    PropertyName = "ArBr",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АР" },
                    AggregateField = "br"
                },
                ["AsmpBr"] = new ColumnConfig
                {
                    PropertyName = "AsmpBr",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АСМП" },
                    AggregateField = "br"
                },
                ["AshBr"] = new ColumnConfig
                {
                    PropertyName = "AshBr",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АШ" },
                    AggregateField = "br"
                },
                ["UksBr"] = new ColumnConfig
                {
                    PropertyName = "UksBr",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "УКС", "АБГ" },
                    AggregateField = "br"
                },
                ["FireTrainBr"] = new ColumnConfig
                {
                    PropertyName = "FireTrainBr",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "Пож. поезд" },
                    AggregateField = "br"
                },
                ["PozhKorablBr"] = new ColumnConfig
                {
                    PropertyName = "PozhKorablBr",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "Пожарный_корабль" },
                    AggregateField = "br"
                },

                // ===============================================
                // 2. РЕЗЕРВ (rezerv) – таблица sredstva
                // ===============================================
                ["AcRezerv"] = new ColumnConfig
                {
                    PropertyName = "AcRezerv",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АЦ" },
                    AggregateField = "rezerv"
                },
                ["AclRezerv"] = new ColumnConfig
                {
                    PropertyName = "AclRezerv",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АЦЛ" },
                    AggregateField = "rezerv"
                },
                ["AnrRezerv"] = new ColumnConfig
                {
                    PropertyName = "AnrRezerv",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АНР" },
                    AggregateField = "rezerv"
                },
                ["AvRezerv"] = new ColumnConfig
                {
                    PropertyName = "AvRezerv",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АВ" },
                    AggregateField = "rezerv"
                },
                ["AcaAppRezerv"] = new ColumnConfig
                {
                    PropertyName = "AcaAppRezerv",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АСА", "АПП" },
                    AggregateField = "rezerv"
                },
                ["PnsRezerv"] = new ColumnConfig
                {
                    PropertyName = "PnsRezerv",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "ПНС" },
                    AggregateField = "rezerv"
                },
                ["AlRezerv"] = new ColumnConfig
                {
                    PropertyName = "AlRezerv",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АЛ" },
                    AggregateField = "rezerv"
                },
                ["KpRezerv"] = new ColumnConfig
                {
                    PropertyName = "KpRezerv",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "КП" },
                    AggregateField = "rezerv"
                },
                ["ArRezerv"] = new ColumnConfig
                {
                    PropertyName = "ArRezerv",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АР" },
                    AggregateField = "rezerv"
                },
                ["AsmpRezerv"] = new ColumnConfig
                {
                    PropertyName = "AsmpRezerv",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АСМП" },
                    AggregateField = "rezerv"
                },
                ["AshRezerv"] = new ColumnConfig
                {
                    PropertyName = "AshRezerv",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АШ" },
                    AggregateField = "rezerv"
                },
                ["UksRezerv"] = new ColumnConfig
                {
                    PropertyName = "UksRezerv",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "УКС", "АБГ" },
                    AggregateField = "rezerv"
                },
                ["AsmrhRezerv"] = new ColumnConfig
                {
                    PropertyName = "AsmrhRezerv",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АСМПХ" },
                    AggregateField = "rezerv"
                },
                ["AvsRezerv"] = new ColumnConfig
                {
                    PropertyName = "AvsRezerv",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АВС" },
                    AggregateField = "rezerv"
                },
                ["PozhKorablRezerv"] = new ColumnConfig
                {
                    PropertyName = "PozhKorablRezerv",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "Пожарный_корабль" },
                    AggregateField = "rezerv"
                },

                // ===============================================
                // 3. РЕМОНТ (remont) – таблица sredstva
                // ===============================================
                ["RemontOsnovnoy"] = new ColumnConfig
                {
                    PropertyName = "RemontOsnovnoy",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АЦ", "АЦЛ", "АВ", "АСА", "АПП", "ПНС", "АНР" },
                    AggregateField = "remont"
                },
                // Ремонт спецтехники – пока неизвестно, какие типы считать спецтехникой.
                // Устанавливаем заглушку 0. При необходимости замените на список типов.
                ["RemontSpetsialnoy"] = new ColumnConfig
                {
                    PropertyName = "RemontSpetsialnoy",
                    SourceTable = "sredstva",
                    FilterValues = new List<string>(), // пустой список – ничего не суммируется
                    Compute = fields => 0 // можно переопределить позже
                },

                // ===============================================
                // 4. ТО (техобслуживание) – возможно, из другой таблицы.
                // Пока ставим заглушку 0. Если данные есть в sredstva, уточните названия.
                // ===============================================
                ["Tofirst"] = new ColumnConfig
                {
                    PropertyName = "Tofirst",
                    SourceTable = "sredstva", // предположительно
                    FilterValues = new List<string> { "ТО-1" },
                    AggregateField = "to" // если есть такое поле, иначе Compute = 0
                },
                ["Totow"] = new ColumnConfig
                {
                    PropertyName = "Totow",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "ТО-2" },
                    AggregateField = "to"
                },

                // ===============================================
                // 5. СПЕЦТЕХНИКА ИЗ СРЕДСТВ (суммы)
                // ===============================================
                ["PlavSredstva"] = new ColumnConfig
                {
                    PropertyName = "PlavSredstva",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "Плав_средства" },
                    Compute = fields => fields.GetValueOrDefault("br", 0) + fields.GetValueOrDefault("remont", 0) + fields.GetValueOrDefault("rezerv", 0)
                },
                ["Bolotohody"] = new ColumnConfig
                {
                    PropertyName = "Bolotohody",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "Болотоходы" },
                    Compute = fields => fields.GetValueOrDefault("br", 0) + fields.GetValueOrDefault("remont", 0) + fields.GetValueOrDefault("rezerv", 0)
                },
                ["Motopompy"] = new ColumnConfig
                {
                    PropertyName = "Motopompy",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "Мотопомпы" },
                    Compute = fields => fields.GetValueOrDefault("br", 0) + fields.GetValueOrDefault("remont", 0) + fields.GetValueOrDefault("rezerv", 0)
                },
                ["Prochee"] = new ColumnConfig
                {
                    PropertyName = "Prochee",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "Прочее" },
                    Compute = fields => fields.GetValueOrDefault("br", 0) + fields.GetValueOrDefault("remont", 0) + fields.GetValueOrDefault("rezerv", 0)
                },

                // ===============================================
                // 6. СИЗОД – таблица sizod
                // ===============================================
                ["SizodBr"] = new ColumnConfig
                {
                    PropertyName = "SizodBr",
                    SourceTable = "sizod",
                    AggregateField = "br" // предполагается поле "br" в sizod
                },
                ["SizodRezerv"] = new ColumnConfig
                {
                    PropertyName = "SizodRezerv",
                    SourceTable = "sizod",
                    AggregateField = "rezerv"
                },

                // ===============================================
                // 7. КОСТЮМЫ – таблица kostyms
                // ===============================================
                ["KostumyL1Task"] = new ColumnConfig
                {
                    PropertyName = "KostumyL1Task",
                    SourceTable = "kostyms",
                    AggregateField = "l1_task" // предположительно поле для Л1/ОЗК/ТАСК
                },
                ["KostumyTok"] = new ColumnConfig
                {
                    PropertyName = "KostumyTok",
                    SourceTable = "kostyms",
                    AggregateField = "tok"
                },

                // ===============================================
                // 8. ГАСИ (пена/порошок) – таблица penas
                // ===============================================
                ["GasiRaschet"] = new ColumnConfig
                {
                    PropertyName = "GasiRaschet",
                    SourceTable = "penas",
                    AggregateField = "pena_br" // или "poroshok_br"? скорее пена в расчёте
                },
                ["GasiRezerv"] = new ColumnConfig
                {
                    PropertyName = "GasiRezerv",
                    SourceTable = "penas",
                    AggregateField = "pena_rezerv"
                },
                // Если есть отдельно порошок, но в классе есть PenaRaschet и PoroshokRaschet ниже

                // ===============================================
                // 9. ЛИЧНЫЙ СОСТАВ – таблица sostav (одна запись на подразделение)
                // ===============================================
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
                    AggregateField = "nalico"
                },
                ["Vsego"] = new ColumnConfig
                {
                    PropertyName = "Vsego",
                    SourceTable = "sostav",
                    AggregateField = "vsego"
                },
                ["RezervLS"] = new ColumnConfig
                {
                    PropertyName = "RezervLS",
                    SourceTable = "sostav",
                    AggregateField = "rezerv"
                },
                ["Nk"] = new ColumnConfig
                {
                    PropertyName = "Nk",
                    SourceTable = "sostav",
                    AggregateField = "nk"
                },
                ["Dispetcher"] = new ColumnConfig
                {
                    PropertyName = "Dispetcher",
                    SourceTable = "sostav",
                    AggregateField = "dispetcher"
                },
                ["Pnk"] = new ColumnConfig
                {
                    PropertyName = "Pnk",
                    SourceTable = "sostav",
                    AggregateField = "pnk"
                },
                ["Ko"] = new ColumnConfig
                {
                    PropertyName = "Ko",
                    SourceTable = "sostav",
                    AggregateField = "ko"
                },
                ["Voditel"] = new ColumnConfig
                {
                    PropertyName = "Voditel",
                    SourceTable = "sostav",
                    AggregateField = "voditel"
                },
                ["Pozharny"] = new ColumnConfig
                {
                    PropertyName = "Pozharny",
                    SourceTable = "sostav",
                    AggregateField = "pozharny"
                },
                ["Gdzs"] = new ColumnConfig
                {
                    PropertyName = "Gdzs",
                    SourceTable = "sostav",
                    AggregateField = "gdzs"
                },
                ["VsegoOts"] = new ColumnConfig
                {
                    PropertyName = "VsegoOts",
                    SourceTable = "sostav",
                    AggregateField = "vsego_ots"
                },
                ["Otpusk"] = new ColumnConfig
                {
                    PropertyName = "Otpusk",
                    SourceTable = "sostav",
                    AggregateField = "otpusk"
                },
                ["PoBolnicnomu"] = new ColumnConfig
                {
                    PropertyName = "PoBolnicnomu",
                    SourceTable = "sostav",
                    AggregateField = "po_bolnicnomu"
                },
                ["Komandirovka"] = new ColumnConfig
                {
                    PropertyName = "Komandirovka",
                    SourceTable = "sostav",
                    AggregateField = "komandirovka"
                },
                ["Nekomplekt"] = new ColumnConfig
                {
                    PropertyName = "Nekomplekt",
                    SourceTable = "sostav",
                    AggregateField = "nekomplekt"
                },
                ["ProchieOts"] = new ColumnConfig
                {
                    PropertyName = "ProchieOts",
                    SourceTable = "sostav",
                    AggregateField = "prochie_ots"
                },

                // ===============================================
                // 10. ПЕНА И ПОРОШОК – таблица penas (детализировано)
                // ===============================================
                ["PenaRaschet"] = new ColumnConfig
                {
                    PropertyName = "PenaRaschet",
                    SourceTable = "penas",
                    AggregateField = "pena_br"
                },
                ["PoroshokRaschet"] = new ColumnConfig
                {
                    PropertyName = "PoroshokRaschet",
                    SourceTable = "penas",
                    AggregateField = "poroshok_br"
                },
                ["PenaRezerv"] = new ColumnConfig
                {
                    PropertyName = "PenaRezerv",
                    SourceTable = "penas",
                    AggregateField = "pena_rezerv"
                },
                ["PoroshokRezerv"] = new ColumnConfig
                {
                    PropertyName = "PoroshokRezerv",
                    SourceTable = "penas",
                    AggregateField = "poroshok_rezerv"
                },

                // ===============================================
                // 11. ТОПЛИВО – из какой таблицы? Пока заглушка 0.
                // Если есть отдельная таблица, замените.
                // ===============================================
                ["Dt"] = new ColumnConfig
                {
                    PropertyName = "Dt",
                    SourceTable = "sredstva", // предположительно, но может быть отдельная таблица "toplivo"
                    FilterValues = new List<string> { "ДТ" },
                    Compute = fields => 0 // заглушка
                },
                ["Benzin"] = new ColumnConfig
                {
                    PropertyName = "Benzin",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "Бензин" },
                    Compute = fields => 0 // заглушка
                }
            };
        }
    private void InitializeColumnConfigs()
        {
            columnConfigs = new Dictionary<string, ColumnConfig>
            {
                // ---- Боевой расчёт (br), резерв (rezerv), ремонт (remont) для каждого типа техники ----
                // АЦ
                ["AcBr"] = new ColumnConfig 
                    { PropertyName = "AcBr", 
                      SourceTable = "sredstva", 
                      FilterValues = new List<string> { "АЦ" }, 
                      AggregateField = "br" },
                ["AcRezerv"] = new ColumnConfig { PropertyName = "AcRezerv", SourceTable = "sredstva", FilterValues = new List<string> { "АЦ" }, AggregateField = "rezerv" },
                ["AcRemont"] = new ColumnConfig { PropertyName = "AcRemont", SourceTable = "sredstva", FilterValues = new List<string> { "АЦ" }, AggregateField = "remont" },

                // АЦЛ
                ["AclBr"] = new ColumnConfig { PropertyName = "AclBr", SourceTable = "sredstva", FilterValues = new List<string> { "АЦЛ" }, AggregateField = "br" },
                ["AclRezerv"] = new ColumnConfig { PropertyName = "AclRezerv", SourceTable = "sredstva", FilterValues = new List<string> { "АЦЛ" }, AggregateField = "rezerv" },
                ["AclRemont"] = new ColumnConfig { PropertyName = "AclRemont", SourceTable = "sredstva", FilterValues = new List<string> { "АЦЛ" }, AggregateField = "remont" },

                // АНР
                ["АнрBr"] = new ColumnConfig { PropertyName = "АнрBr", SourceTable = "sredstva", FilterValues = new List<string> { "АНР" }, AggregateField = "br" },
                ["АнрRezerv"] = new ColumnConfig { PropertyName = "АнрRezerv", SourceTable = "sredstva", FilterValues = new List<string> { "АНР" }, AggregateField = "rezerv" },
                ["АнрRemont"] = new ColumnConfig { PropertyName = "АнрRemont", SourceTable = "sredstva", FilterValues = new List<string> { "АНР" }, AggregateField = "remont" },

                // АСА
                ["АсаBr"] = new ColumnConfig { PropertyName = "АсаBr", SourceTable = "sredstva", FilterValues = new List<string> { "АСА" }, AggregateField = "br" },
                ["АсаRezerv"] = new ColumnConfig { PropertyName = "АсаRezerv", SourceTable = "sredstva", FilterValues = new List<string> { "АСА" }, AggregateField = "rezerv" },
                ["АсаRemont"] = new ColumnConfig { PropertyName = "АсаRemont", SourceTable = "sredstva", FilterValues = new List<string> { "АСА" }, AggregateField = "remont" },

                // АСО
                ["АсоBr"] = new ColumnConfig { PropertyName = "АсоBr", SourceTable = "sredstva", FilterValues = new List<string> { "АСО" }, AggregateField = "br" },
                ["АсоRezerv"] = new ColumnConfig { PropertyName = "АсоRezerv", SourceTable = "sredstva", FilterValues = new List<string> { "АСО" }, AggregateField = "rezerv" },
                ["АсоRemont"] = new ColumnConfig { PropertyName = "АсоRemont", SourceTable = "sredstva", FilterValues = new List<string> { "АСО" }, AggregateField = "remont" },

                // АВ (в FirePsgStat поле АвBr - обратите внимание на регистр)
                ["АвBr"] = new ColumnConfig { PropertyName = "АвBr", SourceTable = "sredstva", FilterValues = new List<string> { "АВ" }, AggregateField = "br" },
                ["АвRezerv"] = new ColumnConfig { PropertyName = "АвRezerv", SourceTable = "sredstva", FilterValues = new List<string> { "АВ" }, AggregateField = "rezerv" },
                ["АвRemont"] = new ColumnConfig { PropertyName = "АвRemont", SourceTable = "sredstva", FilterValues = new List<string> { "АВ" }, AggregateField = "remont" },

                // АСА, АПП, АСМ (объединённые)
                ["АсаАппАсмBr"] = new ColumnConfig { PropertyName = "АсаАппАсмBr", SourceTable = "sredstva", FilterValues = new List<string> { "АСА", "АПП", "АСМ" }, AggregateField = "br" },
                ["АсаАппАсмRezerv"] = new ColumnConfig { PropertyName = "АсаАппАсмRezerv", SourceTable = "sredstva", FilterValues = new List<string> { "АСА", "АПП", "АСМ" }, AggregateField = "rezerv" },
                ["АсаАппАсмRemont"] = new ColumnConfig { PropertyName = "АсаАппАсмRemont", SourceTable = "sredstva", FilterValues = new List<string> { "АСА", "АПП", "АСМ" }, AggregateField = "remont" },

                // ПНС
                ["ПнсBr"] = new ColumnConfig { PropertyName = "ПнсBr", SourceTable = "sredstva", FilterValues = new List<string> { "ПНС" }, AggregateField = "br" },
                ["ПнсRezerv"] = new ColumnConfig { PropertyName = "ПнсRezerv", SourceTable = "sredstva", FilterValues = new List<string> { "ПНС" }, AggregateField = "rezerv" },
                ["ПнсRemont"] = new ColumnConfig { PropertyName = "ПнсRemont", SourceTable = "sredstva", FilterValues = new List<string> { "ПНС" }, AggregateField = "remont" },

                // АЛ
                ["AlBr"] = new ColumnConfig { PropertyName = "AlBr", SourceTable = "sredstva", FilterValues = new List<string> { "АЛ" }, AggregateField = "br" },
                ["AlRezerv"] = new ColumnConfig { PropertyName = "AlRezerv", SourceTable = "sredstva", FilterValues = new List<string> { "АЛ" }, AggregateField = "rezerv" },
                ["AlRemont"] = new ColumnConfig { PropertyName = "AlRemont", SourceTable = "sredstva", FilterValues = new List<string> { "АЛ" }, AggregateField = "remont" },

                // КП
                ["КпBr"] = new ColumnConfig { PropertyName = "КпBr", SourceTable = "sredstva", FilterValues = new List<string> { "КП" }, AggregateField = "br" },
                ["КпRezerv"] = new ColumnConfig { PropertyName = "КпRezerv", SourceTable = "sredstva", FilterValues = new List<string> { "КП" }, AggregateField = "rezerv" },
                ["КпRemont"] = new ColumnConfig { PropertyName = "КпRemont", SourceTable = "sredstva", FilterValues = new List<string> { "КП" }, AggregateField = "remont" },

                // АР
                ["АрBr"] = new ColumnConfig { PropertyName = "АрBr", SourceTable = "sredstva", FilterValues = new List<string> { "АР" }, AggregateField = "br" },
                ["АрRezerv"] = new ColumnConfig { PropertyName = "АрRezerv", SourceTable = "sredstva", FilterValues = new List<string> { "АР" }, AggregateField = "rezerv" },
                ["АрRemont"] = new ColumnConfig { PropertyName = "АрRemont", SourceTable = "sredstva", FilterValues = new List<string> { "АР" }, AggregateField = "remont" },

                // АСМП и ПСА (объединённые)
                ["АсмпПсаBr"] = new ColumnConfig { PropertyName = "АсмпПсаBr", SourceTable = "sredstva", FilterValues = new List<string> { "АСМП", "ПСА" }, AggregateField = "br" },
                ["АсмпПсаRezerv"] = new ColumnConfig { PropertyName = "АсмпПсаRezerv", SourceTable = "sredstva", FilterValues = new List<string> { "АСМП", "ПСА" }, AggregateField = "rezerv" },
                ["АсмпПсаRemont"] = new ColumnConfig { PropertyName = "АсмпПсаRemont", SourceTable = "sredstva", FilterValues = new List<string> { "АСМП", "ПСА" }, AggregateField = "remont" },

                // АШ
                ["АшBr"] = new ColumnConfig { PropertyName = "АшBr", SourceTable = "sredstva", FilterValues = new List<string> { "АШ" }, AggregateField = "br" },
                ["АшRezerv"] = new ColumnConfig { PropertyName = "АшRezerv", SourceTable = "sredstva", FilterValues = new List<string> { "АШ" }, AggregateField = "rezerv" },
                ["АшRemont"] = new ColumnConfig { PropertyName = "АшRemont", SourceTable = "sredstva", FilterValues = new List<string> { "АШ" }, AggregateField = "remont" },

                // УКС и АБГ
                ["УксАбгBr"] = new ColumnConfig { PropertyName = "УксАбгBr", SourceTable = "sredstva", FilterValues = new List<string> { "УКС", "АБГ" }, AggregateField = "br" },
                ["УксАбгRezerv"] = new ColumnConfig { PropertyName = "УксАбгRezerv", SourceTable = "sredstva", FilterValues = new List<string> { "УКС", "АБГ" }, AggregateField = "rezerv" },
                ["УксАбгRemont"] = new ColumnConfig { PropertyName = "УксАбгRemont", SourceTable = "sredstva", FilterValues = new List<string> { "УКС", "АБГ" }, AggregateField = "remont" },

                // Пожарный поезд и корабль (объединённые)
                ["ПожПоездКорабльBr"] = new ColumnConfig { PropertyName = "ПожПоездКорабльBr", SourceTable = "sredstva", FilterValues = new List<string> { "Пож. поезд", "Пожарный_корабль" }, AggregateField = "br" },
                ["ПожПоездКорабльRezerv"] = new ColumnConfig { PropertyName = "ПожПоездКорабльRezerv", SourceTable = "sredstva", FilterValues = new List<string> { "Пож. поезд", "Пожарный_корабль" }, AggregateField = "rezerv" },
                ["ПожПоездКорабльRemont"] = new ColumnConfig { PropertyName = "ПожПоездКорабльRemont", SourceTable = "sredstva", FilterValues = new List<string> { "Пож. поезд", "Пожарный_корабль" }, AggregateField = "remont" },

                // Отдельно пожарный поезд
                ["ПожПоездBr"] = new ColumnConfig { PropertyName = "ПожПоездBr", SourceTable = "sredstva", FilterValues = new List<string> { "Пож. поезд" }, AggregateField = "br" },
                ["ПожПоездRezerv"] = new ColumnConfig { PropertyName = "ПожПоездRezerv", SourceTable = "sredstva", FilterValues = new List<string> { "Пож. поезд" }, AggregateField = "rezerv" },
                ["ПожПоездRemont"] = new ColumnConfig { PropertyName = "ПожПоездRemont", SourceTable = "sredstva", FilterValues = new List<string> { "Пож. поезд" }, AggregateField = "remont" },

                // Отдельно пожарный корабль/катер
                ["ПожКорабльКатерBr"] = new ColumnConfig { PropertyName = "ПожКорабльКатерBr", SourceTable = "sredstva", FilterValues = new List<string> { "Пожарный_корабль" }, AggregateField = "br" },
                ["ПожКорабльКатерRezerv"] = new ColumnConfig { PropertyName = "ПожКорабльКатерRezerv", SourceTable = "sredstva", FilterValues = new List<string> { "Пожарный_корабль" }, AggregateField = "rezerv" },
                ["ПожКорабльКатерRemont"] = new ColumnConfig { PropertyName = "ПожКорабльКатерRemont", SourceTable = "sredstva", FilterValues = new List<string> { "Пожарный_корабль" }, AggregateField = "remont" },

                // АСМРХ
                ["АсмрхBr"] = new ColumnConfig { PropertyName = "АсмрхBr", SourceTable = "sredstva", FilterValues = new List<string> { "АСМРХ" }, AggregateField = "br" },
                ["АсмрхRezerv"] = new ColumnConfig { PropertyName = "АсмрхRezerv", SourceTable = "sredstva", FilterValues = new List<string> { "АСМРХ" }, AggregateField = "rezerv" },

                // АВС
                ["АвсBr"] = new ColumnConfig { PropertyName = "АвсBr", SourceTable = "sredstva", FilterValues = new List<string> { "АВС" }, AggregateField = "br" },
                ["АвсRezerv"] = new ColumnConfig { PropertyName = "АвсRezerv", SourceTable = "sredstva", FilterValues = new List<string> { "АВС" }, AggregateField = "rezerv" },

                // ---- Ремонт ----
                ["РемонтОсновной"] = new ColumnConfig
                {
                    PropertyName = "РемонтОсновной",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АЦ", "АЦЛ", "АВ", "АСА", "АПП", "ПНС", "АНР" },
                    AggregateField = "remont"
                },
                ["РемонтСпециальной"] = new ColumnConfig
                {
                    PropertyName = "РемонтСпециальной",
                    SourceTable = "sredstva",
                    FilterValues = new List<string>(), // пока неизвестно
                    Compute = fields => 0
                },
                ["ПожарныйКорабльРемонт"] = new ColumnConfig
                {
                    PropertyName = "ПожарныйКорабльРемонт",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "Пожарный_корабль" },
                    AggregateField = "remont"
                },

                // ---- Спецсредства (суммы br+rezerv+remont) ----
                ["ПлавСредства"] = new ColumnConfig
                {
                    PropertyName = "ПлавСредства",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "Плав_средства" },
                    Compute = fields => fields.GetValueOrDefault("br", 0) + fields.GetValueOrDefault("rezerv", 0) + fields.GetValueOrDefault("remont", 0)
                },
                ["Болотоходы"] = new ColumnConfig
                {
                    PropertyName = "Болотоходы",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "Болотоходы" },
                    Compute = fields => fields.GetValueOrDefault("br", 0) + fields.GetValueOrDefault("rezerv", 0) + fields.GetValueOrDefault("remont", 0)
                },
                ["Мотопомпы"] = new ColumnConfig
                {
                    PropertyName = "Мотопомпы",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "Мотопомпы" },
                    Compute = fields => fields.GetValueOrDefault("br", 0) + fields.GetValueOrDefault("rezerv", 0) + fields.GetValueOrDefault("remont", 0)
                },
                ["Прочее"] = new ColumnConfig
                {
                    PropertyName = "Прочее",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "Прочее" },
                    Compute = fields => fields.GetValueOrDefault("br", 0) + fields.GetValueOrDefault("rezerv", 0) + fields.GetValueOrDefault("remont", 0)
                },

                // ---- ТО ----
                ["Tofirst"] = new ColumnConfig
                {
                    PropertyName = "Tofirst",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "ТО-1" },
                    AggregateField = "to" // если есть поле "to", иначе заглушка
                },
                ["Totow"] = new ColumnConfig
                {
                    PropertyName = "Totow",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "ТО-2" },
                    AggregateField = "to"
                },

                // ---- СИЗОД ----
                ["SizodBr"] = new ColumnConfig
                {
                    PropertyName = "SizodBr",
                    SourceTable = "sizod",
                    AggregateField = "br"
                },
                ["SizodRezerv"] = new ColumnConfig
                {
                    PropertyName = "SizodRezerv",
                    SourceTable = "sizod",
                    AggregateField = "rezerv"
                },

                // ---- Костюмы ----
                ["КостюмыЛ1Таск"] = new ColumnConfig
                {
                    PropertyName = "КостюмыЛ1Таск",
                    SourceTable = "kostyms",
                    AggregateField = "l1_task" // предположим
                },
                ["КостюмыТок"] = new ColumnConfig
                {
                    PropertyName = "КостюмыТок",
                    SourceTable = "kostyms",
                    AggregateField = "tok"
                },
                ["КостюмыДругие"] = new ColumnConfig
                {
                    PropertyName = "КостюмыДругие",
                    SourceTable = "kostyms",
                    AggregateField = "other" // предположим
                },

                // ---- Личный состав (sostav) ----
                ["Нк"] = new ColumnConfig { PropertyName = "Нк", SourceTable = "sostav", AggregateField = "nk" },
                ["Диспетчер"] = new ColumnConfig { PropertyName = "Диспетчер", SourceTable = "sostav", AggregateField = "dispetcher" },
                ["Пнк"] = new ColumnConfig { PropertyName = "Пнк", SourceTable = "sostav", AggregateField = "pnk" },
                ["Ко"] = new ColumnConfig { PropertyName = "Ко", SourceTable = "sostav", AggregateField = "ko" },
                ["Водитель"] = new ColumnConfig { PropertyName = "Водитель", SourceTable = "sostav", AggregateField = "voditel" },
                ["Пожарный"] = new ColumnConfig { PropertyName = "Пожарный", SourceTable = "sostav", AggregateField = "pozharny" },
                ["Гдзс"] = new ColumnConfig { PropertyName = "Гдзс", SourceTable = "sostav", AggregateField = "gdzs" },
                ["ПоСписку"] = new ColumnConfig { PropertyName = "ПоСписку", SourceTable = "sostav", AggregateField = "po_spisku" },
                ["Налицо"] = new ColumnConfig { PropertyName = "Налицо", SourceTable = "sostav", AggregateField = "nalico" },
                ["Всего"] = new ColumnConfig { PropertyName = "Всего", SourceTable = "sostav", AggregateField = "vsego" },
                ["Резерв"] = new ColumnConfig { PropertyName = "Резерв", SourceTable = "sostav", AggregateField = "rezerv" },

                // ---- ГАСИ (пена/порошок) ----
                ["ГасиРасчёт"] = new ColumnConfig { PropertyName = "ГасиРасчёт", SourceTable = "penas", AggregateField = "pena_br" },
                ["ГасиРезерв"] = new ColumnConfig { PropertyName = "ГасиРезерв", SourceTable = "penas", AggregateField = "pena_rezerv" },

                // ---- Отсутствующие (sostav) ----
                ["ВсегоОтс"] = new ColumnConfig { PropertyName = "ВсегоОтс", SourceTable = "sostav", AggregateField = "vsego_ots" },
                ["Отпуск"] = new ColumnConfig { PropertyName = "Отпуск", SourceTable = "sostav", AggregateField = "otpusk" },
                ["ПоБольничному"] = new ColumnConfig { PropertyName = "ПоБольничному", SourceTable = "sostav", AggregateField = "po_bolnicnomu" },
                ["Командировка"] = new ColumnConfig { PropertyName = "Командировка", SourceTable = "sostav", AggregateField = "komandirovka" },
                ["Некомплект"] = new ColumnConfig { PropertyName = "Некомплект", SourceTable = "sostav", AggregateField = "nekomplekt" },
                ["ПрочиеОтс"] = new ColumnConfig { PropertyName = "ПрочиеОтс", SourceTable = "sostav", AggregateField = "prochie_ots" },

                // ---- Пена и порошок (детализированные) ----
                ["ПенаРасчёт"] = new ColumnConfig { PropertyName = "ПенаРасчёт", SourceTable = "penas", AggregateField = "pena_br" },
                ["ПенаРезерв"] = new ColumnConfig { PropertyName = "ПенаРезерв", SourceTable = "penas", AggregateField = "pena_rezerv" },
                ["ПорошокРасчёт"] = new ColumnConfig { PropertyName = "ПорошокРасчёт", SourceTable = "penas", AggregateField = "poroshok_br" },
                ["ПорошокРезерв"] = new ColumnConfig { PropertyName = "ПорошокРезерв", SourceTable = "penas", AggregateField = "poroshok_rezerv" },

                // ---- Топливо ----
                ["Дт"] = new ColumnConfig
                {
                    PropertyName = "Дт",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "ДТ" },
                    Compute = fields => 0 // заглушка
                },
                ["Бензин"] = new ColumnConfig
                {
                    PropertyName = "Бензин",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "Бензин" },
                    Compute = fields => 0 // заглушка
                }
            };
        }

//        В CreateCategoryRow

//В CreateTotalRow

//В CreateTerritorialRow
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
        private static bool IsDecimalProperty(PropertyInfo prop)
        {
            if (!prop.CanWrite) return false;
            var type = prop.PropertyType;
            return type == typeof(decimal) || type == typeof(decimal?);
        }

    }
    public class PivotRow1
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

        /// <summary>
        /// Детали для каждой колонки (ключ – имя свойства/колонки, значение – список объектов с именем и значением)
        /// </summary>
        public Dictionary<string, List<DetailItem>> CellDetails { get; set; } = new Dictionary<string, List<DetailItem>>();

    }
    public class PivotRow
    {
        // === Иерархия (скопировано из FirePsgStat) ===
        public string ПСГ { get; set; }
        public string ПЧ { get; set; }
        public string Category { get; set; }
        public int PchId { get; set; }
        public int? Parent { get; set; }
        public int Norder { get; set; }
        public int Isitog { get; set; }
        public List<PivotRow> Childes = new List<PivotRow>();

        // === Все поля из FirePsgStat (в точности как там) ===
        public decimal? AcBr { get; set; }
        public decimal? AcRezerv { get; set; }
        public decimal? AcRemont { get; set; }
        public decimal? AclBr { get; set; }
        public decimal? AclRezerv { get; set; }
        public decimal? AclRemont { get; set; }
        public decimal? АнрBr { get; set; }
        public decimal? АнрRezerv { get; set; }
        public decimal? АнрRemont { get; set; }
        public decimal? АсаBr { get; set; }
        public decimal? АсаRezerv { get; set; }
        public decimal? АсаRemont { get; set; }
        public decimal? АсоBr { get; set; }
        public decimal? АсоRezerv { get; set; }
        public decimal? АсоRemont { get; set; }
        public decimal? АвBr { get; set; }
        public decimal? АвRezerv { get; set; }
        public decimal? АвRemont { get; set; }
        public decimal? АсаАппАсмBr { get; set; }
        public decimal? АсаАппАсмRezerv { get; set; }
        public decimal? АсаАппАсмRemont { get; set; }
        public decimal? ПнсBr { get; set; }
        public decimal? ПнсRezerv { get; set; }
        public decimal? ПнсRemont { get; set; }
        public decimal? AlBr { get; set; }
        public decimal? AlRezerv { get; set; }
        public decimal? AlRemont { get; set; }
        public decimal? КпBr { get; set; }
        public decimal? КпRezerv { get; set; }
        public decimal? КпRemont { get; set; }
        public decimal? АрBr { get; set; }
        public decimal? АрRezerv { get; set; }
        public decimal? АрRemont { get; set; }
        public decimal? АсмпПсаBr { get; set; }
        public decimal? АсмпПсаRezerv { get; set; }
        public decimal? АсмпПсаRemont { get; set; }
        public decimal? АшBr { get; set; }
        public decimal? АшRezerv { get; set; }
        public decimal? АшRemont { get; set; }
        public decimal? УксАбгBr { get; set; }
        public decimal? УксАбгRezerv { get; set; }
        public decimal? УксАбгRemont { get; set; }
        public decimal? ПожПоездКорабльBr { get; set; }
        public decimal? ПожПоездКорабльRezerv { get; set; }
        public decimal? ПожПоездКорабльRemont { get; set; }
        public decimal? ПожПоездBr { get; set; }
        public decimal? ПожПоездRezerv { get; set; }
        public decimal? ПожПоездRemont { get; set; }
        public decimal? ПожКорабльКатерBr { get; set; }
        public decimal? ПожКорабльКатерRezerv { get; set; }
        public decimal? ПожКорабльКатерRemont { get; set; }
        public decimal? АсмрхBr { get; set; }
        public decimal? АсмрхRezerv { get; set; }
        public decimal? АвсBr { get; set; }
        public decimal? АвсRezerv { get; set; }
        public decimal? РемонтОсновной { get; set; }
        public decimal? РемонтСпециальной { get; set; }
        public decimal? ПожарныйКорабльРемонт { get; set; }
        public decimal? ПлавСредства { get; set; }
        public decimal? Болотоходы { get; set; }
        public decimal? Мотопомпы { get; set; }
        public decimal? Прочее { get; set; }
        public decimal? Tofirst { get; set; }
        public decimal? Totow { get; set; }
        public decimal? SizodBr { get; set; }
        public decimal? SizodRezerv { get; set; }
        public decimal? КостюмыЛ1Таск { get; set; }
        public decimal? КостюмыТок { get; set; }
        public decimal? КостюмыДругие { get; set; }
        public decimal? Нк { get; set; }
        public decimal? Диспетчер { get; set; }
        public decimal? Пнк { get; set; }
        public decimal? Ко { get; set; }
        public decimal? Водитель { get; set; }
        public decimal? Пожарный { get; set; }
        public decimal? Гдзс { get; set; }
        public decimal? ПоСписку { get; set; }
        public decimal? Налицо { get; set; }
        public decimal? Всего { get; set; }
        public decimal? Резерв { get; set; }
        public decimal? ГасиРасчёт { get; set; }
        public decimal? ГасиРезерв { get; set; }
        public decimal? ВсегоОтс { get; set; }
        public decimal? Отпуск { get; set; }
        public decimal? ПоБольничному { get; set; }
        public decimal? Командировка { get; set; }
        public decimal? Некомплект { get; set; }
        public decimal? ПрочиеОтс { get; set; }
        public decimal? ПенаРасчёт { get; set; }
        public decimal? ПенаРезерв { get; set; }
        public decimal? ПорошокРасчёт { get; set; }
        public decimal? ПорошокРезерв { get; set; }
        public decimal? Дт { get; set; }
        public decimal? Бензин { get; set; }
        public string? Начкар { get; set; }
        // Datafilled – булево (показывает, заполнена ли строка)
        public bool Datafilled { get; set; }
        // === Словарь для деталей (подсказок) — оставляем как есть ===
        public Dictionary<string, List<DetailItem>> CellDetails { get; set; } = new Dictionary<string, List<DetailItem>>();
    }
    /// <summary>
    /// Класс для хранения информации о составляющей суммы
    /// </summary>
    public class DetailItem
    {
        public string Name { get; set; }   // например, "ПЧ-1" или "ПСГ Беломорский"
        public decimal Value { get; set; }
        public string Category { get; set; } // опционально, для группировки
    }
}
