using System;
using System.Collections.Generic;
using System.Linq;

namespace stroevkaI.Services
{
    /// <summary>
    /// Единый in-memory лог изменений, сделанных в текущей сессии.
    /// После сохранения очищается.
    /// </summary>
    public sealed class ChangeLogService
    {
        public static ChangeLogService Instance { get; } = new ChangeLogService();
        private ChangeLogService() { }

        private readonly List<ChangeLogEntry> _entries = new();
        private readonly object _lock = new();

        /// <summary>Уведомление UI: список изменился.</summary>
        public event EventHandler Changed;

        public IReadOnlyList<ChangeLogEntry> Entries
        {
            get { lock (_lock) return _entries.ToList(); }
        }

        public int Count
        {
            get { lock (_lock) return _entries.Count; }
        }

        public bool HasChanges => Count > 0;

        public void Add(ChangeLogEntry entry)
        {
            if (entry == null) return;
            lock (_lock) _entries.Add(entry);
            Changed?.Invoke(this, EventArgs.Empty);
        }

        public void Add(int pchId, string pchName, string table, string field,
                        object oldValue, object newValue)
        {
            Add(new ChangeLogEntry
            {
                PchId = pchId,
                PchName = pchName,
                TableName = table,
                FieldName = field,
                OldValue = oldValue,
                NewValue = newValue,
                ChangedAt = DateTime.Now
            });
        }

        public void Clear()
        {
            lock (_lock) _entries.Clear();
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}
