using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    /// <summary>
    /// Выезда
    /// </summary>
    public partial class Call
    {
        public Call()
        {
            Departures = new HashSet<Departure>();
        }

        public int Id { get; set; }
        public DateTime? Дата { get; set; }
        public DateTime? Время { get; set; }
        public string? Фабула { get; set; }
        public string? Адрес { get; set; }
        public string? Результат { get; set; }
        public DateTime? ВремяИзм { get; set; }
        public string? Mguid { get; set; }
        public string? Выезды { get; set; }

        public virtual ICollection<Departure> Departures { get; set; }
    }
}
