using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class Mtable
    {
        public Mtable()
        {
            InverseParentNavigation = new HashSet<Mtable>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Parent { get; set; }
        public string? Objectname { get; set; }
        public string? Setname { get; set; }
        public string? Repositoryname { get; set; }
        public short? MeChecked { get; set; }
        public byte[]? Icon { get; set; }
        public int? N { get; set; }
        public string? Filters { get; set; }
        /// <summary>
        /// Имя view в котором будет отображаться таблица
        /// </summary>
        public string? View { get; set; }
        public string? MapParams { get; set; }
        public string? PackageName { get; set; }

        public virtual Mtable? ParentNavigation { get; set; }
        public virtual ICollection<Mtable> InverseParentNavigation { get; set; }
    }
}
