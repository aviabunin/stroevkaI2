using System;
using System.Collections.Generic;
using System.Linq;
using StorageI.ModelsStroevkaMySql;
using stroevkaI.Models;

namespace stroevkaI.Services
{
    public class TreeBuilder
    {
        private readonly List<Pch> _pchs;
        private readonly List<Psg> _psgs;
        private readonly List<Sredstva> _sredstva;
        private readonly List<Sostav> _sostav;
        private readonly List<Sizod> _sizod;
        private readonly List<Pena> _penas;
        private readonly List<Kostym> _kostyms;
        // Словари для быстрого доступа
        private Dictionary<int, Pch> _pchDict;
        private Dictionary<int, Psg> _psgDict;

        public TreeBuilder(
            List<Pch> pchs,
            List<Psg> psgs,
            List<Sredstva> sredstva,
            List<Sostav> sostav,
            List<Sizod> sizod,
            List<Pena> penas,
            List<Kostym> kostyms)
        {
            _pchs = pchs;
            _psgs = psgs;
            _sredstva = sredstva;
            _sostav = sostav;
            _sizod = sizod;
            _penas = penas;
            _kostyms = kostyms;

            _pchDict = _pchs.ToDictionary(p => p.Id);
            _psgDict = _psgs.ToDictionary(p => p.Id);
        }

        public ReportNode BuildTree()
        {
            var root = new PsgNode
            {
                Id = 11,
                Name = "Территориальный",
                //IsTotal = true,
                //CategoryType = "territorial"
            };

            // Группируем ПЧ по ПСГ (parent)
            var pchByPsg = _pchs.GroupBy(p => p.Parent ?? 11);

            // Создаём узлы ПСГ
            foreach (var group in pchByPsg)
            {
                int psgId = group.Key;
                if (psgId == 11)
                {
                    // Таких быть не должно, но если есть – пропускаем (или можно создать отдельную обработку, но по условию не нужно)
                    continue;
                }

                if (!_psgDict.TryGetValue(psgId, out Psg psg))
                    continue;

                //Создаём узел ПСГ  категории psg
                var psgNode = new PsgNode
                {
                    Id = psgId,
                    Name = psg.Garnizon,
                    //Parent = root,
                    //IsTotal = true,
                    //CategoryType = "psg"
                };
                //root.Children.Add(psgNode);

                // Добавляем ПЧ этого ПСГ
                foreach (var pch in group)
                {
                    var pchNode = CreatePchNode(pch);
                    //psgNode.Children.Add(pchNode);
                }

                // Добавляем итоговые категории для этого ПСГ
                AddCategoryTotals(psgNode);
            }

            // Добавляем итоговые категории для территориального уровня
            // Они будут агрегировать данные по всем ПСГ (путём суммирования дочерних узлов)
            AddCategoryTotals(root);

            // Выполняем агрегацию снизу вверх
            AggregateTree(root);

            return root;
        }

        private PchNode CreatePchNode(Pch pch)
        {
            var node = new PchNode
            {
                Id = pch.Id,
                Name = pch.Name ?? pch.Fullname ?? "Без имени",
                PchId = pch.Id,
                Category = pch.Garntype ?? "Другое",
                Datafilled = pch.Datafilled == true,
                RowId = pch.RowId,
                Norder = pch.Norder,
                ParentId = pch.Parent ?? 11,
                //IsTotal = false
            };

            // Заполняем агрегированные значения для этой ПЧ
            FillPchValues(node);

            return node;
        }

        private void FillPchValues(PchNode node)
        {
            int pchId = node.PchId;

            // --- Техника (sredstva) ---
            var pchSredstva = _sredstva.Where(s => s.SubdivisionId == pchId).ToList();

            // Суммируем по каждому типу
            // Для простоты используем вспомогательный метод
            AddSredstvaValue(node, pchSredstva, "АЦ", "AcBr", "br");
            AddSredstvaValue(node, pchSredstva, "АЦ", "AcRezerv", "rezerv");
            AddSredstvaValue(node, pchSredstva, "АЦ", "AcRemont", "remont");
            AddSredstvaValue(node, pchSredstva, "АЦЛ", "AclBr", "br");
            AddSredstvaValue(node, pchSredstva, "АЦЛ", "AclRezerv", "rezerv");
            // ... аналогично для всех типов (можно сделать циклом по конфигурации)
            // Ниже сокращённый пример:

            // Для остальных типов используем обобщённый метод
            var types = new[]
            {
                new { Name = "АНР", BrKey = "АнрBr", RezKey = "АнрRezerv", RemKey = "АнрRemont" },
                new { Name = "АСА", BrKey = "АсаBr", RezKey = "АсаRezerv", RemKey = "АсаRemont" },
                new { Name = "АСО", BrKey = "АсоBr", RezKey = "АсоRezerv", RemKey = "АсоRemont" },
                new { Name = "АВ", BrKey = "АвBr", RezKey = "АвRezerv", RemKey = "АвRemont" },
                new { Name = "ПНС", BrKey = "ПнсBr", RezKey = "ПнсRezerv", RemKey = "ПнсRemont" },
                new { Name = "АР", BrKey = "АрBr", RezKey = "АрRezerv", RemKey = "АрRemont" },
                new { Name = "АШ", BrKey = "АшBr", RezKey = "АшRezerv", RemKey = "АшRemont" },
                // ... остальные
            };

            foreach (var t in types)
            {
                var items = pchSredstva.Where(s => s.NameSredstvo == t.Name);
                //node.AggregatedValues[t.BrKey] = items.Sum(s => s.Br ?? 0);
                //node.AggregatedValues[t.RezKey] = items.Sum(s => s.Rezerv ?? 0);
                //node.AggregatedValues[t.RemKey] = items.Sum(s => s.Remont ?? 0);
                //// Сохраняем исходные записи для детализации
                //if (items.Any())
                //{
                //    node.SourceRecords[t.BrKey] = items.Where(s => (s.Br ?? 0) > 0).Cast<object>().ToList();
                //    node.SourceRecords[t.RezKey] = items.Where(s => (s.Rezerv ?? 0) > 0).Cast<object>().ToList();
                //    node.SourceRecords[t.RemKey] = items.Where(s => (s.Remont ?? 0) > 0).Cast<object>().ToList();
                //}
            }

            // Специальные поля: ремонт основной, специальной и т.д.
            // Это можно сделать позже, пока для примера.

            // --- Личный состав (sostav) ---
            var pchSostav = _sostav.Where(s => s.SubdivisionId == pchId).ToList();
            // Суммируем по должностям и категориям
            var nk = pchSostav.Where(s => s.Name == "НК" && s.SostavVid == "2 Боевой расчет").Sum(s => s.Count ?? 0);
            //node.AggregatedValues["Нк"] = nk;
            //node.SourceRecords["Нк"] = pchSostav.Where(s => s.Name == "НК" && s.SostavVid == "2 Боевой расчет" && (s.Count ?? 0) > 0).Cast<object>().ToList();

            // Аналогично для Диспетчер, ПНК, КО, Водитель, Пожарный, ГДЗС, Отпуск, Больничный и т.д.
            // Для краткости оставим так.

            // --- СИЗОД ---
            //var pchSizod = _sizod.Where(s => s.SubdivisionId == pchId).ToList();
            //node.AggregatedValues["SizodBr"] = pchSizod.Sum(s => s.Raschet ?? 0);
            //node.SourceRecords["SizodBr"] = pchSizod.Where(s => (s.Raschet ?? 0) > 0).Cast<object>().ToList();

            //// --- Пенообразователь ---
            //var pchPenas = _penas.Where(p => p.SubdivisionId == pchId && p.Mname == "Пенообразователь").ToList();
            //node.AggregatedValues["ПенаРасчёт"] = pchPenas.Sum(p => p.Inwork ?? 0);
            //node.SourceRecords["ПенаРасчёт"] = pchPenas.Where(p => (p.Inwork ?? 0) > 0).Cast<object>().ToList();

            // --- Костюмы ---
            var pchKostyms = _kostyms.Where(k => k.SubdivisionId == pchId).ToList();
            // Л-1/ТАСК/ОЗК
            var l1 = pchKostyms.Where(k => new[] { "Л-1", "ТАСК", "ОЗК" }.Contains(k.Mname)).Sum(k => k.N ?? 0);
            //node.AggregatedValues["КостюмыЛ1Таск"] = l1;
            //node.SourceRecords["КостюмыЛ1Таск"] = pchKostyms.Where(k => new[] { "Л-1", "ТАСК", "ОЗК" }.Contains(k.Mname) && (k.N ?? 0) > 0).Cast<object>().ToList();
            //// ТОК
            //var tok = pchKostyms.Where(k => k.Mname == "ТОК").Sum(k => k.N ?? 0);
            //node.AggregatedValues["КостюмыТок"] = tok;
            //node.SourceRecords["КостюмыТок"] = pchKostyms.Where(k => k.Mname == "ТОК" && (k.N ?? 0) > 0).Cast<object>().ToList();

            // Добавим другие поля (tofirst, totow, ГАСИ и т.д.) по аналогии
            // Для вычисляемых полей (Налицо, Всего, ВсегоОтс, ПрочиеОтс) пока не заполняем,
            // они будут вычислены при агрегации родительских узлов, либо отдельно.
        }

        private void AddSredstvaValue(PchNode node, List<Sredstva> items, string name, string key, string field)
        {
            var filtered = items.Where(s => s.NameSredstvo == name).ToList();
            decimal sum = 0;
            switch (field)
            {
                case "br": sum = filtered.Sum(s => s.Br ?? 0); break;
                case "rezerv": sum = filtered.Sum(s => s.Rezerv ?? 0); break;
                case "remont": sum = filtered.Sum(s => s.Remont ?? 0); break;
            }
            //node.AggregatedValues[key] = sum;
            //if (sum > 0)
            //    node.SourceRecords[key] = filtered.Cast<object>().ToList();
        }

        private void AddCategoryTotals(PsgNode psgNode)
        {
            // Создаём итоговые узлы для категорий:
            // "main" (всего), "gps", "fps", "vpo", "chpo", "other", "asf"
            var categoryTypes = new[] { "main", "gps", "fps", "vpo", "chpo", "other", "asf" };
            //foreach (var ct in categoryTypes)
            //{
            //    var catNode = new CategoryTotalNode
            //    {
            //        Id = -psgNode.Id * 100 - Array.IndexOf(categoryTypes, ct), // уникальный отрицательный id
            //        Name = GetCategoryDisplayName(ct),
            //        CategoryType = ct,
            //        IsTotal = true,
            //        Parent = psgNode
            //    };
            //    psgNode.Children.Add(catNode);
            //}
        }

        private string GetCategoryDisplayName(string ct)
        {
            switch (ct)
            {
                case "main": return "всего";
                case "gps": return "в т.ч. ГПС";
                case "fps": return "в т.ч. по ФПС";
                case "vpo": return "ВПО";
                case "chpo": return "ЧПО";
                case "other": return "другие";
                case "asf": return "АСФ";
                default: return ct;
            }
        }

        private void AggregateTree(ReportNode node)
        {
            // Рекурсивно агрегируем дочерние узлы
            //foreach (var child in node.Children)
            //    AggregateTree(child);

            //// Если узел итоговый (не ПЧ), суммируем значения дочерних узлов
            //if (!(node is PchNode))
            //{
            //    // Определим список ключей, которые нужно агрегировать
            //    var keys = node.Children.SelectMany(c => c.AggregatedValues.Keys).Distinct().ToList();
            //    foreach (var key in keys)
            //    {
            //        node.AggregatedValues[key] = node.Children.Sum(c => c.AggregatedValues.GetValueOrDefault(key, 0));
            //    }
            //}
        }
    }
}
