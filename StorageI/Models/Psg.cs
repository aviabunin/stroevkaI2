using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class Psg
    {
        public int Id { get; set; }
        public int? Parent { get; set; }
        public string? Garnizon { get; set; }
        public int Norder { get; set; }
        public bool? Old { get; set; }
        public bool Datafilled { get; set; }
    }
}
