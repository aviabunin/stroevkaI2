using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class FireEquip
    {
        public string? Sredstvo { get; set; }
        public decimal? Br { get; set; }
        public decimal? Rezerv { get; set; }
        public decimal? Remont { get; set; }
        public decimal? Tofirst { get; set; }
        public decimal? Totow { get; set; }
        public int? IdPsg { get; set; }
        public int? IdPsgunit { get; set; }
        public string? Category { get; set; }
    }
}
