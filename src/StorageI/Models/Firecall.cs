using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    /// <summary>
    /// Выезда
    /// </summary>
    public partial class Firecall
    {
        public int Id { get; set; }
        public DateTime? Calldata { get; set; }
        public DateTime? Calltime { get; set; }
        public DateTime? Timedepart { get; set; }
        public DateTime? Timearrive { get; set; }
        public DateTime? Returntime { get; set; }
        public string? Fabula { get; set; }
        public string? Adres { get; set; }
        public string? Result { get; set; }
        public string? Rang { get; set; }
        public string? Objtype { get; set; }
        public string? Obj { get; set; }
        public string? Objproperties { get; set; }
        public string? Firearea { get; set; }
        public int? Peopleinfire { get; set; }
        public int? Gdzs { get; set; }
        public int? Stvolnum { get; set; }
        public string? Gidrants { get; set; }
        public string? Additionalsr { get; set; }
        public string? Locarea { get; set; }
        public string? Comment { get; set; }
        public DateTime? Tchange { get; set; }
        public string? Mguid { get; set; }
        public int? Storeynum { get; set; }
        public string? Stepenognest { get; set; }
    }
}
