using StorageI.ModelsStroevkaMySql;
using stroevkaI.Models;
using System.Reflection;
using Microsoft.EntityFrameworkCore;


using StorageI.ModelsStroevkaMySql;
using stroevkaI.Models;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace stroevkaI.Services
{
    public class PivotTreeBuilder
    {
        // ---------- КЭШ ----------
        private static readonly ConcurrentDictionary<string, List<PivotRow>> _pivotCache = new();
        private static readonly ConcurrentDictionary<string, DateTime> _cacheTime = new();

        // ---------- СОСТОЯНИЕ ----------
        private Dictionary<int, Psgstat> _psgDict;
        private ReportNode _root;
        private readonly stroevkaContext _context;

        public static Dictionary<int, CacheNachkar> nachkarBySubdiv;
        private readonly AppStatusService _appStatus;
        private readonly JsonDataService _jsonService;

        public PivotTreeBuilder(stroevkaContext context, AppStatusService appStatus, JsonDataService jsonService)
        {
            _context = context;
            _appStatus = appStatus;
            _jsonService = jsonService;
        }

        // ==========================================================
        // ЗАГРУЗКА ДАННЫХ ДЛЯ ОДНОГО ПСГ  123
        // ==========================================================
        private async Task<PchData> LoadPchDataAsync(string psgName)
        {
            var data = new PchData();

            bool isTerr = psgName.Contains("Террит");
            // Найти psgstat-строку по имени (это может быть районный ПСГ или Территориальный)
            var psgRow = _context.Psgstats
                .AsNoTracking()
                .FirstOrDefault(p => p.Used == 1 && p.Name == psgName);


            if (psgRow == null)
                return data;

            int psgId = psgRow.Id;

            if (_appStatus.Status.IsDatabaseOnline)
            {
                // ID подразделений этого ПСГ (сам ПСГ + его прямые дети-ПЧ)
                var subdivIds = await _context.Psgstats
                    .AsNoTracking()
                    .Where(p => p.Used == 1 && (p.Id == psgId || p.Parent == psgId))
                    .Select(p => p.Id)
                    .ToListAsync();

                // Параллельная загрузка
                var sredstvaTask = _context.Sredstvas.AsNoTracking()
                    .Where(s => isTerr || subdivIds.Contains(s.SubdivisionId.Value))
                    .ToListAsync();
                var sostavTask = _context.Sostavs.AsNoTracking()
                    .Where(s => isTerr || subdivIds.Contains(s.SubdivisionId.Value))
                    .ToListAsync();
                var sizodsTask = _context.Sizods.AsNoTracking()
                    .Where(s => isTerr || subdivIds.Contains(s.SubdivisionId.Value))
                    .ToListAsync();
                var penasTask = _context.Penas.AsNoTracking()
                    .Where(s => isTerr || subdivIds.Contains(s.SubdivisionId.Value))
                    .ToListAsync();
                var kostymsTask = _context.Kostyms.AsNoTracking()
                    .Where(s => isTerr || subdivIds.Contains(s.SubdivisionId.Value))
                    .ToListAsync();
                var watersTask = _context.Waters.AsNoTracking()
                    .Where(s => isTerr || subdivIds.Contains(s.SubdivisionId.Value))
                    .ToListAsync();
                var contactsTask = _context.Contacts.AsNoTracking()
                    .Where(s => isTerr || subdivIds.Contains(s.SubdivisionId.Value))
                    .ToListAsync();
                var nachkarTask = _context.CacheNachkars.AsNoTracking()
                    .Where(n => isTerr || subdivIds.Contains(n.SubdivisionId))
                    .ToListAsync();

                await Task.WhenAll(sredstvaTask, sostavTask, sizodsTask, penasTask,
                                   kostymsTask, watersTask, contactsTask, nachkarTask);

                data.SredstvaList = await sredstvaTask;
                data.SostavList = await sostavTask;
                data.SizodsList = await sizodsTask;
                data.PenasList = await penasTask;
                data.KostymsList = await kostymsTask;
                data.WatersList = await watersTask;
                data.ContactsList = await contactsTask;

                nachkarBySubdiv = (await nachkarTask)
                    .GroupBy(n => n.SubdivisionId)
                    .ToDictionary(g => g.Key, g => g.FirstOrDefault());

                // Обновляем локальный кэш JSON — на случай офлайна
                data.PchId = psgId;
                data.LastModified = DateTime.Now;
                try { await _jsonService.SaveDataAsync(data); } catch { /* не критично */ }
            }
            else
            {
                // Офлайн — читаем JSON
                data = await _jsonService.LoadDataAsync(psgId) ?? new PchData { PchId = psgId };

                var nachkarsList = data.ContactsList ?? new List<Contact>();
                nachkarBySubdiv = new Dictionary<int, CacheNachkar>();
            }

            return data;
        }

        // ==========================================================
        // ПОСТРОЕНИЕ ДЕРЕВА
        // ==========================================================
        public async Task<ReportNode> BuildTreeAsync(string psgName)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();

            // 1. Все узлы (psgstat — маленькая таблица, её грузим целиком, она нужна для GetPsgNameForNode)
            var allNodes = await _context.Psgstats
                .AsNoTracking()
                .Where(p => p.Used == 1)
                .ToListAsync();

            _psgDict = allNodes.ToDictionary(p => p.Id, p => p);

            // 2. Находим сам ПСГ по имени
            var psgRow = allNodes.FirstOrDefault(p => p.Name.Trim() == psgName);
            if (psgRow == null)
                return null;

            // 3. Оставляем только ПСГ + его прямых детей + (при желании) вложенные уровни
            var allowedIds = new HashSet<int> { psgRow.Id };

            // Прямые дети
            foreach (var c in allNodes.Where(p => p.Parent == psgRow.Id))
                allowedIds.Add(c.Id);

            // Если у детей есть свои дети (например, "итоги" ПСГ) — добавляем
            bool added = true;
            while (added)
            {
                added = false;
                foreach (var n in allNodes)
                {
                    if (!allowedIds.Contains(n.Id) && n.Parent.HasValue && allowedIds.Contains(n.Parent.Value))
                    {
                        allowedIds.Add(n.Id);
                        added = true;
                    }
                }
            }

            // 4. Отфильтрованный список узлов
            var nodesForTree = allNodes.Where(n => allowedIds.Contains(n.Id)).ToList();

            // 5. Данные — только по этому ПСГ (уже так и было)
            var data = await LoadPchDataAsync(psgName);

            // 6. Группировки
            var sredstvaBySubdiv = (data.SredstvaList ?? new())
                .GroupBy(s => s.SubdivisionId)
                .ToDictionary(g => g.Key, g => g.ToList());
            var sostavBySubdiv = (data.SostavList ?? new())
                .GroupBy(s => s.SubdivisionId)
                .ToDictionary(g => g.Key, g => g.ToList());
            var sizodsBySubdiv = (data.SizodsList ?? new())
                .GroupBy(s => s.SubdivisionId)
                .ToDictionary(g => g.Key, g => g.ToList());
            var penasBySubdiv = (data.PenasList ?? new())
                .GroupBy(s => s.SubdivisionId)
                .ToDictionary(g => g.Key, g => g.ToList());
            var kostymsBySubdiv = (data.KostymsList ?? new())
                .GroupBy(s => s.SubdivisionId)
                .ToDictionary(g => g.Key, g => g.ToList());

            // psgdata (По списку)
            List<Psgdatum> psgdataList = new();
            if (_appStatus.Status.IsDatabaseOnline)
            {
                var subdivIds = allowedIds.ToList();
                psgdataList = await _context.Psgdata.AsNoTracking()
                    .Where(p => subdivIds.Contains(p.Id))
                    .ToListAsync();
            }
            var psgdataBySubdiv = psgdataList
                .GroupBy(s => s.Id)
                .ToDictionary(g => g.Key, g => g.ToList());

            // 4. Сборка ReportNode
            var nodeDict = new Dictionary<int, ReportNode>();
            foreach (var psg in allNodes)
            {
                var node = new ReportNode
                {
                    Id = psg.Id,
                    Name = psg.Name,
                    displayName = psg.Displayname,
                    Category = psg.Garntype ?? "",
                    ParentId = psg.Parent ?? 0,
                    Isitog = psg.Isitog ?? 0,
                    Norder = (int)psg.Norder,
                    RawData = new Dictionary<string, Dictionary<string, Dictionary<string, decimal>>>()
                };

                if (psg.Isitog == 0)
                {
                    // sredstva
                    if (sredstvaBySubdiv.TryGetValue(psg.Id, out var sredstvaForNode))
                    {
                        var sredstvaDict = new Dictionary<string, Dictionary<string, decimal>>();
                        foreach (var s in sredstvaForNode)
                        {
                            sredstvaDict[s.NameSredstvo] = new Dictionary<string, decimal>
                            {
                                ["br"] = s.Br ?? 0,
                                ["rezerv"] = s.Rezerv ?? 0,
                                ["remont"] = s.Remont ?? 0,
                                ["to1"] = (int?)s.Tofirst ?? 0,
                                ["to2"] = (int?)s.Totow ?? 0
                            };
                        }
                        node.RawData["sredstva"] = sredstvaDict;
                    }

                    // sostav
                    if (sostavBySubdiv.TryGetValue(psg.Id, out var sostavForNode))
                    {
                        var sostavDict = new Dictionary<string, Dictionary<string, decimal>>();
                        foreach (var s in sostavForNode)
                        {
                            string key = $"{s.Name}_{s.SostavVid}";
                            sostavDict[key] = new Dictionary<string, decimal>
                            {
                                ["count"] = s.Count ?? 0
                            };
                        }
                        node.RawData["sostav"] = sostavDict;
                    }

                    // psgdata
                    if (psgdataBySubdiv.TryGetValue(psg.Id, out var psgdataForNode))
                    {
                        var psgdataDict = new Dictionary<string, Dictionary<string, decimal>>();
                        foreach (var s in psgdataForNode)
                        {
                            psgdataDict["ПоСписку"] = new Dictionary<string, decimal>
                            {
                                ["count"] = s.ПоСписку ?? 0
                            };
                        }
                        node.RawData["psgdata"] = psgdataDict;
                    }

                    // penas
                    if (penasBySubdiv.TryGetValue(psg.Id, out var penasForNode))
                    {
                        var penasDict = new Dictionary<string, Dictionary<string, decimal>>();
                        foreach (var s in penasForNode)
                        {
                            penasDict[s.Mname] = new Dictionary<string, decimal>
                            {
                                ["inwork"] = s.Inwork ?? 0,
                                ["inrezerv"] = s.Inrezerv ?? 0
                            };
                        }
                        node.RawData["penas"] = penasDict;
                    }

                    // sizod
                    if (sizodsBySubdiv.TryGetValue(psg.Id, out var sizodForNode))
                    {
                        var sizodDict = new Dictionary<string, Dictionary<string, decimal>>();
                        foreach (var s in sizodForNode)
                        {
                            if ((s.Raschet ?? 0) + (s.Rezerv ?? 0) == 0) continue;
                            sizodDict[s.Mname] = new Dictionary<string, decimal>
                            {
                                ["raschet"] = s.Raschet ?? 0,
                                ["rezerv"] = s.Rezerv ?? 0
                            };
                        }
                        node.RawData["sizod"] = sizodDict;
                    }

                    // kostyms
                    if (kostymsBySubdiv.TryGetValue(psg.Id, out var kostymsForNode))
                    {
                        var kostymsDict = new Dictionary<string, Dictionary<string, decimal>>();
                        foreach (var s in kostymsForNode)
                        {
                            kostymsDict[s.Mname] = new Dictionary<string, decimal>
                            {
                                ["n"] = s.N ?? 0
                            };
                        }
                        node.RawData["kostyms"] = kostymsDict;
                    }
                }

                nodeDict[psg.Id] = node;
            }

            // 5. Связывание
            ReportNode root = null;
            foreach (var node in nodeDict.Values)
            {
                if (node.Id == psgRow.Id) root = node;
                if (nodeDict.TryGetValue(node.ParentId, out var parent))
                    parent.Children.Add(node);
            }
            if (root == null)
                root = nodeDict.Values.FirstOrDefault(n => n.ParentId == 0);

            _root = root;

            sw.Stop();
            System.Diagnostics.Debug.WriteLine($"BuildTreeAsync({psgName}): {sw.ElapsedMilliseconds} ms, nodes={nodeDict.Count}");

            return root;
        }

        // ==========================================================
        // ГЕНЕРАЦИЯ PivotRow (остаётся почти без изменений, но без
        // загрузки всех данных — она уже сделана в BuildTreeAsync)
        // ==========================================================

        public async Task<List<PivotRow>> GeneratePivotRowsAsync(string psgName, bool forceReload = false)
        {
            if (!forceReload && _pivotCache.TryGetValue(psgName, out var cached))
                return cached;

            var rootNode = await BuildTreeAsync(psgName);
            if (rootNode == null)
            {
                var empty = new List<PivotRow>();
                _pivotCache[psgName] = empty;
                return empty;
            }

            InitializeColumnConfigs();
            var result = new List<PivotRow>();

            // 1. Листья
            var leaves = GetAllLeaves(rootNode);
            foreach (var leaf in leaves)
                result.Add(CreateLeafRow(leaf));

            var psgChildes = result
                .GroupBy(c => c.Parent ?? 0)
                .ToDictionary(g => g.Key, g => g.ToList());

            // 2. Определяем, что суммировать: 
            //    - если корень территориальный (id=11) — берём его детей-ПСГ (те, у которых есть свои дети)
            //    - иначе корень сам является районным ПСГ — суммируем по нему одному
            bool isTerritorial = (rootNode.Id == 11);

            var psgNodes = isTerritorial
                ? rootNode.Children.Where(c => c.Children.Any()).ToList()
                : new List<ReportNode> { rootNode };

            var allPsgRows = new List<PivotRow>();
            foreach (var psgNode in psgNodes)
            {
                var psgRows = ComputePsgSummaryRows(psgNode);
                var psgВсего = psgRows.FirstOrDefault(c => c.Category == "всего");

                result.Add(psgВсего);
                result.AddRange(psgВсего.Childes);
                allPsgRows.AddRange(psgRows);
            }

            // 3. Территориальные итоги — только когда корень территориальный
            if (isTerritorial)
            {
                var territorialRows = new List<PivotRow>();

                foreach (var cat in new[] { "ВПО", "ЧПО", "другие", "АСФ" })
                {
                    var rows = GetPsgRowsByCategory(allPsgRows, cat);
                    var row = CreateTerritorialRow(rootNode, cat, rows);
                    if (row != null) territorialRows.Add(row);
                }

                var gpsRows = GetPsgRowsByCategory(allPsgRows, "ФПС");
                gpsRows.AddRange(GetPsgRowsByCategory(allPsgRows, "ППС"));
                var gpsRow = CreateTerritorialRow(rootNode, "ГПС", gpsRows);
                if (gpsRow != null) territorialRows.Add(gpsRow);

                var fpsRow = ComputeTerritorialFpsRow(rootNode, allPsgRows);
                if (fpsRow != null) territorialRows.Add(fpsRow);

                var rowsForTotal = territorialRows
                    .Where(r => r.Category == "ГПС" || r.Category == "другие" ||
                                r.Category == "ЧПО" || r.Category == "ВПО")
                    .ToList();

                var totalRow = CreateTerritorialRow(rootNode, "всего", rowsForTotal);
                if (totalRow != null)
                {
                    totalRow.Childes.AddRange(result.Where(c => c.Category == "всего").ToList());
                    totalRow.Childes.AddRange(rowsForTotal);
                    if (fpsRow != null) totalRow.Childes.Add(fpsRow);
                    var asfRow = territorialRows.FirstOrDefault(c => c.Category == "АСФ");
                    if (asfRow != null) totalRow.Childes.Add(asfRow);
                    territorialRows.Add(totalRow);
                }

                result.AddRange(territorialRows);
            }

            _pivotCache[psgName] = result;
            _cacheTime[psgName] = DateTime.Now;

            return result.OrderBy(c=>c.Norder).ToList();
        }

        public static void InvalidateCache(string psgName)
        {
            _pivotCache.TryRemove(psgName, out _);
            _cacheTime.TryRemove(psgName, out _);
        }

        public static void InvalidateAllCache() => _pivotCache.Clear();

        // ==========================================================
        // Вспомогательные
        // ==========================================================
        public static List<PivotRow> GetPsgChildes(string psgName)
        {
            if (_pivotCache.TryGetValue(psgName, out var rows))
            {
                var psgRow = rows.FirstOrDefault(c => c.Псг != null
                    && c.Псг.Contains(psgName)
                    && c.Category != null
                    && c.Category.Contains("всего"));
                if (psgRow == null) return new List<PivotRow>();

                var lst = new List<PivotRow> { psgRow };
                lst.AddRange(psgRow.Childes);
                return lst.OrderBy(c => c.Norder).ToList();
            }
            return new List<PivotRow>();
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
                Псг = "Территориальный",
                Category = categoryName,
                PchId = rootNode.Id,
                Parent = 11,  // родитель - не важно кто, для порядка поставим Территориальный (он имеет категорию "всего")
                Isitog = 1,

            };
            if (displayNames.ContainsKey(categoryName))
                row.Пч = displayNames[categoryName];
            else
                row.Пч = "Не определено";
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
                                        Name = $"{r.Псг} → {d.Name}",
                                        Value = d.Value,
                                        Category = d.Category
                                    });
                                }
                            }
                            else
                            {
                                // Если деталей нет (например, для особых случаев), добавляем строку целиком
                                details.Add(new DetailItem { Name = $"{r.Псг} ({r.Пч})", Value = val });
                            }
                        }
                    }

                    prop.SetValue(row, total);
                    row.CellDetails[prop.Name] = details;
                }
            }

            // Для итоговых строк эти поля пустые
            row.Начкар = "";
            row.Datafilled = "";
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
            fpsRows = fpsRows.Where(r => r.Псг != "Прионежский").ToList();

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
                .Where(n => n.Isitog != 1)
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

            // 2. другие && kv.Key != "ЧПО"   && kv.Key != "ВПО" 
            var otherLeaves = leavesByType.Where(kv => kv.Key != "ФПС" && kv.Key != "ППС" && kv.Key != "ЧПО" && kv.Key != "ВПО" && kv.Key != "АСФ").SelectMany(kv => kv.Value).ToList();
            rows.Add(CreateCategoryRow(psgNode, "другие", otherLeaves));
            // 2. другие
            var otherLeaves1 = leavesByType.Where(kv => kv.Key != "ФПС" && kv.Key != "ППС" && kv.Key != "АСФ").SelectMany(kv => kv.Value).ToList();
            var другиеПСГRow = CreateCategoryRow(psgNode, "другиеПСГ", otherLeaves);
            rows.Add(другиеПСГRow);// это другие для ПСГ (не территориального, т.к. в том ВПО,ЧПО отдельно)
                                   // 3. всего
            var всегоRow = CreateTotalRow(psgNode, rows.Where(r => r.Category == "ГПС" || r.Category == "другие").ToList());


            var ВПО_ЧПО_АСФrows = new List<PivotRow>();
            // 4. ВПО, ЧПО, АСФ
            foreach (var cat in new[] { "ВПО", "ЧПО", "АСФ" })
            {
                if (leavesByType.TryGetValue(cat, out var catLeaves))
                {
                    var r = CreateCategoryRow(psgNode, cat, catLeaves);
                    ВПО_ЧПО_АСФrows.Add(r);
                }
            }
            rows.AddRange(ВПО_ЧПО_АСФrows);
            //Сформировать строку "всего" для районного ПСГ и занести все предыдущие итоговые в childes
            всегоRow.Childes.AddRange(new List<PivotRow> { всегоПСГrow, другиеПСГRow });
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
                Псг = psgNode.Name,
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
                row.Пч = displayNames[categoryName];
            else if (categoryName == "всего")
                row.Пч = psgNode.Name;
            else
                row.Пч = "Не определено";
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

            row.ВсегоОтс = (row.ПоСписку ?? 0) - (row.Налицо ?? 0);
            return row;
        }
        private PivotRow CreateLeafRow(ReportNode leaf)
        {
            var row = new PivotRow
            {
                Псг = GetPsgNameForNode(leaf),
                Пч = leaf.Name, //  это просто Name(psgstat) =  garnizon(psgdata)
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
            row.ВсегоОтс = (row.ПоСписку ?? 0) - (row.Налицо ?? 0);
            //row.Начкар = nachkarBySubdiv[leaf.Id].Nachkar;

            if (nachkarBySubdiv != null &&
    nachkarBySubdiv.TryGetValue(leaf.Id, out var nachkar) &&
    nachkar != null)
            {
                row.Начкар = nachkar.Nachkar ?? "";
            }
            else
            {
                row.Начкар = "";
            }

            //и Datafilled – как было
            // ...

            return row;
        }
        private PivotRow CreateTotalRow(ReportNode psgNode, List<PivotRow> rowsToSum)
        {
            var row = new PivotRow
            {
                Псг = psgNode.Name,
                Пч = psgNode.Name,
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
                                details.Add(new DetailItem { Name = r.Пч, Value = val });
                            }
                        }
                    }

                    prop.SetValue(row, total);
                    row.CellDetails[prop.Name] = details;
                }
            }
            // Дополнительно можно скопировать текстовые поля (если нужно)
            // Например, Nachkar, Datafilled – для итогов обычно пустые
            row.ВсегоОтс = (row.ПоСписку ?? 0) - (row.Налицо ?? 0);
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
            if ((node.Children.Count == 0) && (node.Isitog == 0))
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



        private void InitializeColumnConfigs()
        {
            List<string> lstВсего = new List<string>() { "ПНК_2 Боевой расчет", "КО_2 Боевой расчет", "Водители_2 Боевой расчет", "Пожарные_2 Боевой расчет" };
            List<string> lstГДЗС = new List<string>() { "НК_3 ГДЗС", "ПНК_3 ГДЗС", "КО_3 ГДЗС", "Водители_3 ГДЗС", "Пожарные_3 ГДЗС" };
            List<string> lstНалицо = lstВсего.Concat(new[] { "НК_2 Боевой расчет", "Диспетчер_2 Боевой расчет" }).ToList();
            List<string> lstВсегоОтс = lstВсего.Concat(new[] { "НК_2 Боевой расчет", "Диспетчер_2 Боевой расчет" }).ToList();
            columnConfigs = new Dictionary<string, ColumnConfig>
            {
                #region ---- Боевой расчёт (br), резерв (rezerv), ремонт (remont) для каждого типа техники ----
                // АЦ
                ["AcBr"] = new ColumnConfig
                {
                    PropertyName = "AcBr",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АЦ" },
                    AggregateField = "br"
                },
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

                // АЛ 7
                ["AlBr"] = new ColumnConfig { PropertyName = "AlBr", SourceTable = "sredstva", FilterValues = new List<string> { "АЛ-30", "АЛ-50" }, AggregateField = "br" },
                ["AlRezerv"] = new ColumnConfig { PropertyName = "AlRezerv", SourceTable = "sredstva", FilterValues = new List<string> { "АЛ-30", "АЛ-50" }, AggregateField = "rezerv" },
                ["AlRemont"] = new ColumnConfig { PropertyName = "AlRemont", SourceTable = "sredstva", FilterValues = new List<string> { "АЛ-30", "АЛ-50" }, AggregateField = "remont" },

                // КП
                ["КпBr"] = new ColumnConfig { PropertyName = "КпBr", SourceTable = "sredstva", FilterValues = new List<string> { "КП", "АКП" }, AggregateField = "br" },
                ["КпRezerv"] = new ColumnConfig { PropertyName = "КпRezerv", SourceTable = "sredstva", FilterValues = new List<string> { "КП", "АКП" }, AggregateField = "rezerv" },
                ["КпRemont"] = new ColumnConfig { PropertyName = "КпRemont", SourceTable = "sredstva", FilterValues = new List<string> { "КП", "АКП" }, AggregateField = "remont" },

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
                    FilterValues = new List<string> { "АЛ", "КП", "АР", "АСМП", "ПСА", "АШ", "АСМ", "АСМРХ", "АВС", "УКС", "АБГ", "АКП", "АЛ-30", "АЛ-50" },
                    AggregateField = "remont"
                },
                ["ПожарныйКорабльРемонт"] = new ColumnConfig
                {
                    PropertyName = "ПожарныйКорабльРемонт",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "Пожарный_корабль" },
                    Compute = fields => fields.GetValueOrDefault("rezerv", 0) + fields.GetValueOrDefault("remont", 0)

                },

                // ---- Спецсредства (суммы br+rezerv+remont) ----
                ["ПлавСредства"] = new ColumnConfig
                {
                    PropertyName = "ПлавСредства",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "Плав.средства" },
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
                    FilterValues = new List<string> { "Грузовой_автомобиль", "Автобусы", "Бензовозы", "Краны", "Инженерная", "Мототехника", "Иные", "Автомобиль аэродромный" },
                    Compute = fields => fields.GetValueOrDefault("br", 0) + fields.GetValueOrDefault("rezerv", 0)
                },

                #region ---- ТО ----
                ["Tofirst"] = new ColumnConfig
                {
                    PropertyName = "Tofirst",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АЦ" },
                    AggregateField = "to1" // если есть поле "to", иначе заглушка
                },
                ["Totow"] = new ColumnConfig
                {
                    PropertyName = "Totow",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "АЦ" },
                    AggregateField = "to2"
                },
                #endregion
                #endregion
                #region ---- СИЗОД ----
                //["ПенаРасчёт"] = new ColumnConfig { PropertyName = "ПенаРасчёт", SourceTable = "penas", FilterValues = new List<string> { "Пенообразователь" }, AggregateField = "inwork" },
                ["SizodBr"] = new ColumnConfig { PropertyName = "SizodBr", SourceTable = "sizod", Compute = fields => fields.GetValueOrDefault("raschet", 0) },
                ["SizodRezerv"] = new ColumnConfig { PropertyName = "SizodRezerv", SourceTable = "sizod", Compute = fields => fields.GetValueOrDefault("rezerv", 0) },

                #endregion
                #region ---- Костюмы ----
                ["КостюмыЛ1Таск"] = new ColumnConfig
                {
                    PropertyName = "КостюмыЛ1Таск",
                    SourceTable = "kostyms",
                    FilterValues = new List<string> { "Л-1", "ТАСК", "ОЗК" },
                    AggregateField = "n"
                },
                ["КостюмыТок"] = new ColumnConfig
                {
                    PropertyName = "КостюмыТок",
                    SourceTable = "kostyms",
                    FilterValues = new List<string> { "ТОК" },
                    AggregateField = "n"
                },
                ["КостюмыДругие"] = new ColumnConfig
                {
                    PropertyName = "КостюмыДругие",
                    SourceTable = "kostyms",
                    FilterValues = new List<string> { "ОЗК" },
                    AggregateField = "n"
                },
                #endregion
                #region ---- Личный состав (sostav) ----

                ["Нк"] = new ColumnConfig { PropertyName = "Нк", SourceTable = "sostav", FilterValues = new List<string> { "НК_2 Боевой расчет" }, AggregateField = "count" },
                ["Диспетчер"] = new ColumnConfig { PropertyName = "Диспетчер", SourceTable = "sostav", FilterValues = new List<string> { "Диспетчер_2 Боевой расчет" }, AggregateField = "count" },
                ["Пнк"] = new ColumnConfig { PropertyName = "Пнк", SourceTable = "sostav", FilterValues = new List<string> { "ПНК_2 Боевой расчет" }, AggregateField = "count" },
                ["Ко"] = new ColumnConfig { PropertyName = "Ко", SourceTable = "sostav", FilterValues = new List<string> { "КО_2 Боевой расчет" }, AggregateField = "count" },
                ["Водитель"] = new ColumnConfig { PropertyName = "Водитель", SourceTable = "sostav", FilterValues = new List<string> { "Водители_2 Боевой расчет" }, AggregateField = "count" },
                ["Пожарный"] = new ColumnConfig { PropertyName = "Пожарный", SourceTable = "sostav", FilterValues = new List<string> { "Пожарные_2 Боевой расчет" }, AggregateField = "count" },
                ["Гдзс"] = new ColumnConfig { PropertyName = "Гдзс", SourceTable = "sostav", FilterValues = lstГДЗС, AggregateField = "count" },
                ["ПоСписку"] = new ColumnConfig { PropertyName = "ПоСписку", SourceTable = "psgdata", FilterValues = new List<string> { "ПоСписку" }, AggregateField = "count" },
                ["Налицо"] = new ColumnConfig { PropertyName = "Налицо", SourceTable = "sostav", FilterValues = lstНалицо, AggregateField = "count" },
                ["Всего"] = new ColumnConfig { PropertyName = "Всего", SourceTable = "sostav", FilterValues = lstВсего, AggregateField = "count" },
                ["Резерв"] = new ColumnConfig { PropertyName = "Резерв", SourceTable = "sostav", FilterValues = new List<string> { "резерв" }, AggregateField = "count" },//TODO только отсутствующие 
                #endregion

                #region ---- Отсутствующие (sostav) ----
                ["ВсегоОтс"] = new ColumnConfig { PropertyName = "ВсегоОтс", SourceTable = "sostav", FilterValues = new List<string> { "Всего_4 Отсутствует" }, AggregateField = "count" },
                ["Отпуск"] = new ColumnConfig { PropertyName = "Отпуск", SourceTable = "sostav", FilterValues = new List<string> { "Отпуск_4 Отсутствует" }, AggregateField = "count" },
                ["ПоБольничному"] = new ColumnConfig { PropertyName = "ПоБольничному", SourceTable = "sostav", FilterValues = new List<string> { "По больничному_4 Отсутствует" }, AggregateField = "count" },
                ["Командировка"] = new ColumnConfig { PropertyName = "Командировка", SourceTable = "sostav", FilterValues = new List<string> { "Командировка_4 Отсутствует" }, AggregateField = "count" },
                ["Некомплект"] = new ColumnConfig { PropertyName = "Некомплект", SourceTable = "sostav", FilterValues = new List<string> { "Некомплект_4 Отсутствует" }, AggregateField = "count" },
                ["ПрочиеОтс"] = new ColumnConfig { PropertyName = "ПрочиеОтс", SourceTable = "sostav", FilterValues = new List<string> { "Прочее_4 Отсутствует" }, AggregateField = "count" },
                #endregion
                #region ---- Пена и порошок (детализированные) топливо ----
                ["ПенаРасчёт"] = new ColumnConfig { PropertyName = "ПенаРасчёт", SourceTable = "penas", FilterValues = new List<string> { "Пенообразователь" }, AggregateField = "inwork" },
                ["ПенаРезерв"] = new ColumnConfig { PropertyName = "ПенаРезерв", SourceTable = "penas", FilterValues = new List<string> { "Пенообразователь" }, AggregateField = "inrezerv" },
                ["ПорошокРасчёт"] = new ColumnConfig { PropertyName = "ПорошокРасчёт", SourceTable = "penasList", FilterValues = new List<string> { "Пенообразователь" }, AggregateField = "inwork" },
                ["ПорошокРезерв"] = new ColumnConfig { PropertyName = "ПорошокРезерв", SourceTable = "penasList", FilterValues = new List<string> { "Пенообразователь" }, AggregateField = "inrezerv" },
                #region ---- ГАСИ (пена/порошок) ----  'ГАСИ_ручной', 'ГАСИ_механизированный'
                ["ГасиРасчёт"] = new ColumnConfig
                {
                    PropertyName = "ГасиРасчёт",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "ГАСИ_ручной", "ГАСИ_механизированный" },
                    AggregateField = "br"
                },
                ["ГасиРезерв"] = new ColumnConfig
                {
                    PropertyName = "ГасиРезерв",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "ГАСИ_ручной", "ГАСИ_механизированный" },
                    AggregateField = "rezerv"
                },
                #endregion
                #region ---- Топливо ----
                ["Дт"] = new ColumnConfig
                {
                    PropertyName = "Дт",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "ДТ" },
                    Compute = fields => fields.GetValueOrDefault("br", 0) + fields.GetValueOrDefault("rezerv", 0)

                },
                ["Бензин"] = new ColumnConfig
                {
                    PropertyName = "Бензин",
                    SourceTable = "sredstva",
                    FilterValues = new List<string> { "Бензин" },
                    Compute = fields => fields.GetValueOrDefault("br", 0) + fields.GetValueOrDefault("rezerv", 0)

                }
                #endregion
                #endregion
            };
        }

        private static bool IsDecimalProperty(PropertyInfo prop)
        {
            if (!prop.CanWrite) return false;
            var type = prop.PropertyType;
            return type == typeof(decimal) || type == typeof(decimal?);
        }

    }
}




