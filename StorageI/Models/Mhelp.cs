using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class Mhelp
    {
        public Mhelp()
        {
            InverseParentNavigation = new HashSet<Mhelp>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Parent { get; set; }
        public string? Description { get; set; }
        public string? Filename { get; set; }
        public string? Level { get; set; }
        public string? Catalog { get; set; }
        public DateTime? DateCreate { get; set; }
        public string? DateEdit { get; set; }
        public int? Author { get; set; }
        public string? DocType { get; set; }
        public string? Params { get; set; }
        public string? Video { get; set; }
        public string? Images { get; set; }
        public int? Descrexist { get; set; }
        public int? Enabled { get; set; }

        public virtual Mhelp? ParentNavigation { get; set; }
        public virtual ICollection<Mhelp> InverseParentNavigation { get; set; }
    }
}
