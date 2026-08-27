using System;
using System.Collections.Generic;

namespace StorageI.ModelsStroevkaMySql
{
    public partial class Psg
    {
        public int Id { get; set; }
        public int? Parent { get; set; }
        public string? Garnizon { get; set; }
        public int Norder { get; set; }
        public bool? Old { get; set; }
        public bool Datafilled { get; set; }
        public int? MainPchId { get; set; }
        public string RowId { get; set; } = null!;
    }
}
