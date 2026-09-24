using Microsoft.EntityFrameworkCore;
using StorageI.ModelsStroevkaMySql;

namespace stroevkaI.Services
{
    /// <summary>
    /// Единый in-memory кэш всех справочных таблиц.
    /// Заполняется один раз в фоне при старте + при явном Refresh().
    /// После загрузки — read-only, кроме Update* методов (для редакторов).
    /// </summary>
    public sealed class AppDataCache
    {
        public static AppDataCache Instance { get; } = new AppDataCache();
        private AppDataCache() { }

        // ---- состояние ----
        private readonly object _lock = new();

        public Dictionary<int, Psgstat> PsgstatById { get; private set; } = new();
        public Dictionary<int, List<Sredstva>> SredstvaByPch { get; private set; } = new();
        public Dictionary<int, List<Sostav>> SostavByPch { get; private set; } = new();
        public Dictionary<int, List<Contact>> ContactsByPch { get; private set; } = new();
        public Dictionary<int, List<Water>> WatersByPch { get; private set; } = new();
        public Dictionary<int, List<Pena>> PenasByPch { get; private set; } = new();
        public Dictionary<int, List<Sizod>> SizodsByPch { get; private set; } = new();
        public Dictionary<int, List<Kostym>> KostymsByPch { get; private set; } = new();
        public Dictionary<int, List<CacheNachkar>> NachkarBySubdiv { get; private set; } = new();

        public List<PivotRow> PivotRows { get; private set; } = new();
        public DateTime LastLoadedAt { get; private set; } = DateTime.MinValue;
        public bool IsLoaded { get; private set; }

        /// <summary>Событие: кэш перезагружен (для UI — перерисовать).</summary>
        public event EventHandler Reloaded;

        // ---------------------------------------------------------------
        // ЗАГРУЗКА (вызывается из фона)
        // ---------------------------------------------------------------
        public void LoadAll()
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();

            // Всё грузим через один контекст — так быстрее и консистентнее
            using var ctx = new stroevkaContext();

            var psgstat = ctx.Psgstats.AsNoTracking().Where(p => p.Used == 1).ToList();
            var sredstva = ctx.Sredstvas.AsNoTracking().ToList();
            var sostav = ctx.Sostavs.AsNoTracking().ToList();
            var contacts = ctx.Contacts.AsNoTracking().ToList();
            var waters = ctx.Waters.AsNoTracking().ToList();
            var penas = ctx.Penas.AsNoTracking().ToList();
            var sizods = ctx.Sizods.AsNoTracking().ToList();
            var kostyms = ctx.Kostyms.AsNoTracking().ToList();
            var nachkars = ctx.CacheNachkars.AsNoTracking().ToList();
            var pivots = ctx.PivotRows.AsNoTracking().ToList();

            // Группировки
            var psgDict = psgstat.ToDictionary(p => p.Id, p => p);
            var sredstvaDict = Group(sredstva, s => s.SubdivisionId);
            var sostavDict = Group(sostav, s => s.SubdivisionId);
            var contactsDict = Group(contacts, s => s.SubdivisionId);
            var watersDict = Group(waters, s => s.SubdivisionId);
            var penasDict = Group(penas, s => s.SubdivisionId);
            var sizodsDict = Group(sizods, s => s.SubdivisionId);
            var kostymsDict = Group(kostyms, s => s.SubdivisionId);
            var nachkarDict = nachkars
                .Where(n => n.SubdivisionId != 0)
                .GroupBy(n => n.SubdivisionId)
                .ToDictionary(g => g.Key, g => g.ToList());

            // ---- атомарная замена ----
            lock (_lock)
            {
                PsgstatById = psgDict;
                SredstvaByPch = sredstvaDict;
                SostavByPch = sostavDict;
                ContactsByPch = contactsDict;
                WatersByPch = watersDict;
                PenasByPch = penasDict;
                SizodsByPch = sizodsDict;
                KostymsByPch = kostymsDict;
                NachkarBySubdiv = nachkarDict;
                PivotRows = pivots;
                LastLoadedAt = DateTime.Now;
                IsLoaded = true;
            }

            sw.Stop();
            System.Diagnostics.Debug.WriteLine($"[Cache] LoadAll: {sw.ElapsedMilliseconds} ms");
            Reloaded?.Invoke(this, EventArgs.Empty);
        }

        private static Dictionary<int, List<T>> Group<T>(
            IEnumerable<T> src, Func<T, int?> key)
        {
            return src
                .Where(x => key(x).HasValue)
                .GroupBy(x => key(x)!.Value)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        // ---------------------------------------------------------------
        // ДОСТУП
        // ---------------------------------------------------------------
        public IReadOnlyList<Sredstva> GetSredstva(int pchId)
            => SredstvaByPch.TryGetValue(pchId, out var l) ? l : new List<Sredstva>();

        public IReadOnlyList<Sostav> GetSostav(int pchId)
            => SostavByPch.TryGetValue(pchId, out var l) ? l : new List<Sostav>();

        public IReadOnlyList<Contact> GetContacts(int pchId)
            => ContactsByPch.TryGetValue(pchId, out var l) ? l : new List<Contact>();

        public IReadOnlyList<Water> GetWaters(int pchId)
            => WatersByPch.TryGetValue(pchId, out var l) ? l : new List<Water>();

        public IReadOnlyList<Pena> GetPenas(int pchId)
            => PenasByPch.TryGetValue(pchId, out var l) ? l : new List<Pena>();

        public IReadOnlyList<Sizod> GetSizods(int pchId)
            => SizodsByPch.TryGetValue(pchId, out var l) ? l : new List<Sizod>();

        public IReadOnlyList<Kostym> GetKostyms(int pchId)
            => KostymsByPch.TryGetValue(pchId, out var l) ? l : new List<Kostym>();

        public IReadOnlyList<CacheNachkar> GetNachkars(int pchId)
            => NachkarBySubdiv.TryGetValue(pchId, out var l) ? l : new List<CacheNachkar>();

        // ---------------------------------------------------------------
        // ОБНОВЛЕНИЕ ИЗ РЕДАКТОРОВ
        // ---------------------------------------------------------------
        public void UpdateSredstvo(Sredstva item)
        {
            if (item?.SubdivisionId == null) return;
            lock (_lock)
            {
                int key = item.SubdivisionId.Value;
                if (!SredstvaByPch.TryGetValue(key, out var list))
                    SredstvaByPch[key] = list = new List<Sredstva>();

                int idx = list.FindIndex(s => s.Id == item.Id);
                if (idx >= 0) list[idx] = item; else list.Add(item);
            }
        }

        public void RemoveSredstvo(int pchId, int sredstvoId)
        {
            lock (_lock)
            {
                if (SredstvaByPch.TryGetValue(pchId, out var list))
                    list.RemoveAll(s => s.Id == sredstvoId);
            }
        }

        public void UpdatePivotRows(List<PivotRow> rows)
        {
            lock (_lock) { PivotRows = rows; }
        }
    }
}
