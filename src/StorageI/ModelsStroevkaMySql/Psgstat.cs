using System;
using System.Collections.Generic;

namespace StorageI.ModelsStroevkaMySql
{
    public partial class Psgstat
    {
        public Psgstat()
        {
            Contacts = new HashSet<Contact>();
            InverseParentNavigation = new HashSet<Psgstat>();
            Kostyms = new HashSet<Kostym>();
            Penas = new HashSet<Pena>();
            Sizods = new HashSet<Sizod>();
            Sostavs = new HashSet<Sostav>();
            Sredstvas = new HashSet<Sredstva>();
            Waters = new HashSet<Water>();
        }

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

        public virtual Psgstat? ParentNavigation { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Psgstat> InverseParentNavigation { get; set; }
        public virtual ICollection<Kostym> Kostyms { get; set; }
        public virtual ICollection<Pena> Penas { get; set; }
        public virtual ICollection<Sizod> Sizods { get; set; }
        public virtual ICollection<Sostav> Sostavs { get; set; }
        public virtual ICollection<Sredstva> Sredstvas { get; set; }
        public virtual ICollection<Water> Waters { get; set; }
    }
}
