using System;
using System.Collections.Generic;

namespace StorageI.ModelsStroevkaMySql
{
    public partial class FirePsg
    {
        public int Id { get; set; }
        public int? Parent { get; set; }
        public string? Garnizon { get; set; }
        public int Norder { get; set; }
        public bool? Old { get; set; }
        public bool Datafilled { get; set; }
    }
}
