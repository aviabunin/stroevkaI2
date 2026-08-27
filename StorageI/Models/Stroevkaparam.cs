using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class Stroevkaparam
    {
        public Stroevkaparam()
        {
            InverseParentNavigation = new HashSet<Stroevkaparam>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Mtable { get; set; }
        public string? DisplayName { get; set; }
        public int? Level { get; set; }
        public int? Parent { get; set; }
        public int? InitValue { get; set; }
        public string? Mtype { get; set; }
        public string? ExcelCol { get; set; }
        public string? ExcelRow { get; set; }
        public string? Comment { get; set; }
        public string? RezervExcelCol { get; set; }
        public string? ReservVal { get; set; }
        public string? RemontExcelCol { get; set; }
        public string? RemontVal { get; set; }
        /// <summary>
        /// Порядок
        /// </summary>
        public int? Norder { get; set; }
        public int? Operation { get; set; }

        public virtual Stroevkaparam? ParentNavigation { get; set; }
        public virtual ICollection<Stroevkaparam> InverseParentNavigation { get; set; }
    }
}
