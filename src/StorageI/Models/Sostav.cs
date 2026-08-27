using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class Sostav
    {
        public int Id { get; set; }
        public DateTime? Mdate { get; set; }
        public string? NameGarnizone { get; set; }
        public string? Subdivision { get; set; }
        public string? Name { get; set; }
        public int? Count { get; set; }
        public string? SostavVid { get; set; }
        public int? GarnizoneId { get; set; }
        public int? SubdivisionId { get; set; }
        public int? SostavVidId { get; set; }
        public DateTime? EditTime { get; set; }
        public string? Parent { get; set; }
        public string? NameFull { get; set; }
        public string? Excel { get; set; }
        public int Norder { get; set; }

        public virtual Psgdatum? SubdivisionNavigation { get; set; }
    }
}
