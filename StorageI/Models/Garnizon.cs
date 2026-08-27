using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class Garnizon
    {
        public Garnizon()
        {
            InverseParentNavigation = new HashSet<Garnizon>();
            Report3guGarnizones = new HashSet<Report3gu>();
            Report3guPsgs = new HashSet<Report3gu>();
            Vids = new HashSet<Vid>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NameShort { get; set; }
        public string? Level { get; set; }
        public int? Parent { get; set; }
        public string? ForRep2 { get; set; }
        public int? Norder { get; set; }
        public string? Type { get; set; }
        public int? TypeId { get; set; }
        /// <summary>
        /// Данные заполнены
        /// </summary>
        public bool DataFilled { get; set; }
        /// <summary>
        /// Данные проверены
        /// </summary>
        public bool DataChecked { get; set; }
        public string? ExcelName { get; set; }
        public int? Order { get; set; }
        public string ForControl { get; set; } = null!;
        public string? Rank { get; set; }

        public virtual Garnizon? ParentNavigation { get; set; }
        public virtual ICollection<Garnizon> InverseParentNavigation { get; set; }
        public virtual ICollection<Report3gu> Report3guGarnizones { get; set; }
        public virtual ICollection<Report3gu> Report3guPsgs { get; set; }
        public virtual ICollection<Vid> Vids { get; set; }
    }
}
