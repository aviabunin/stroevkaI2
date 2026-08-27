using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class Apch
    {
        public int Id { get; set; }
        public int? Parent { get; set; }
        public bool Datafilled { get; set; }
        public string? Name { get; set; }
        public string? Garnizon { get; set; }
        public bool? Old { get; set; }
        public int Norder { get; set; }
        public string? Rank { get; set; }
        public string? Fullname { get; set; }
        public int? Karaul { get; set; }
        public string? ForControl { get; set; }
        public bool? Visibility { get; set; }
        public int Dataerr { get; set; }
        public string? ForRep2 { get; set; }
        public DateTime Mdate { get; set; }
        /// <summary>
        /// подразделение, местный, территориальный
        /// </summary>
        public int? GarnTypeid { get; set; }
        public string? Garntype { get; set; }
    }
}
