using System;

namespace stroevkaI.Services
{
    /// <summary>
    /// Одна запись в логе изменений: какой параметр, где, что было, что стало.
    /// </summary>
    public class ChangeLogEntry
    {
        public int PchId { get; set; }              // ID ПЧ (psgstat.Id)
        public string PchName { get; set; }          // отображаемое имя ПЧ
        public string TableName { get; set; }        // "sredstva", "waters", "sostav", ...
        public string FieldName { get; set; }        // "АЦ: br → rezerv", "По списку (1 Общие)"
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.Now;
        public string UserName { get; set; } = Environment.UserName;
    }
}
