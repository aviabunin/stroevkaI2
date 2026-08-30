using System;
using System.Collections.Generic;

namespace StorageI.ModelsStroevkaMySql
{
    public partial class Contact
    {
        public int Id { get; set; }
        public string? Post { get; set; }
        public string? Fio { get; set; }
        public DateTime? Mdate { get; set; }
        public string? TfMobil { get; set; }
        public string? TfWork { get; set; }
        public string? TfDom { get; set; }
        public string? Subdivision { get; set; }
        public string? Posyvnoy { get; set; }
        public string? NameGarnizone { get; set; }
        public DateTime? EditTime { get; set; }
        public int? GarnizonId { get; set; }
        public int? SubdivisionId { get; set; }
        public int? PostId { get; set; }
        public string? Excel { get; set; }
        public int Norder { get; set; }
        public int Karaul { get; set; }

        //public virtual Psgstat? Subdivision1 { get; set; }
        public virtual Psgdatum? SubdivisionNavigation { get; set; }
    }
}