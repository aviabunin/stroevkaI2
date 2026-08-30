using System;
using System.Collections.Generic;

namespace StorageI.ModelsStroevkaMySql
{
    public partial class Water
    {
        public int Id { get; set; }
        public string? Mname { get; set; }
        public int? Total { get; set; }
        public int? Fault { get; set; }
        public DateTime? Mdate { get; set; }
        public string? Subdivision { get; set; }
        public string? NameGarnizone { get; set; }
        public DateTime? EditTime { get; set; }
        public int? GarnizonId { get; set; }
        public int? SubdivisionId { get; set; }
        public string? Excel { get; set; }
        public int Norder { get; set; }

        public virtual Psgdatum? SubdivisionNavigation { get; set; }
    }
}