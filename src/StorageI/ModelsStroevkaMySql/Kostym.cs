using System;
using System.Collections.Generic;

namespace StorageI.ModelsStroevkaMySql
{
    public partial class Kostym
    {
        public int Id { get; set; }
        public string? Mname { get; set; }
        public int? N { get; set; }
        public DateTime? Mdate { get; set; }
        public string? Subdivision { get; set; }
        public string? NameGarnizione { get; set; }
        public DateTime? EditTime { get; set; }
        public int? GarnizionId { get; set; }
        public int? SubdivisionId { get; set; }
        public string? Excel { get; set; }
        public int Norder { get; set; }

        public virtual Psgstat? Subdivision1 { get; set; }
        public virtual Psgdatum? SubdivisionNavigation { get; set; }
    }
}
