using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class SredstvaДоУдал
    {
        public int Id { get; set; }
        /// <summary>
        /// На какую дату
        /// </summary>
        public DateTime? Mdate { get; set; }
        public string? NameGarnizone { get; set; }
        public string? Subdivision { get; set; }
        /// <summary>
        /// Наименование средства
        /// </summary>
        public string? NameSredstvo { get; set; }
        public int? Parent { get; set; }
        /// <summary>
        /// Боевой расчет
        /// </summary>
        public int? Br { get; set; }
        /// <summary>
        /// В резерве
        /// </summary>
        public int? Rezerv { get; set; }
        /// <summary>
        /// В ремонте
        /// </summary>
        public int? Remont { get; set; }
        /// <summary>
        /// Время последнего изменения
        /// </summary>
        public DateTime? EditTime { get; set; }
        public int? SredstvoId { get; set; }
        public int? GarnizonId { get; set; }
        public int? SubdivisionId { get; set; }
        public int? SredstvoVidId { get; set; }
        public string? SredstvoVid { get; set; }
        public string? Excel { get; set; }
        public int Norder { get; set; }
    }
}
