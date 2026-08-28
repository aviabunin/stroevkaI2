using System;
using System.Collections.Generic;

namespace StorageI.ModelsStroevkaMySql
{
    public partial class Psgstat
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Garntype { get; set; }
        public string? Displayname { get; set; }
        public int? Parent { get; set; }
        public int? Isitog { get; set; }
        public int? Inreport { get; set; }
        public int? Datafilled { get; set; }
        public int? Used { get; set; }
        public int? Norder { get; set; }
        public int? Rank { get; set; }
        public int? Karaul { get; set; }
        public DateTime Mdate { get; set; }
    }
}
