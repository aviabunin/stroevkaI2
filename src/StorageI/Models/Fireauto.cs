using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class Fireauto
    {
        public Fireauto()
        {
            Departures = new HashSet<Departure>();
            Fireworks = new HashSet<Firework>();
            InverseParentNavigation = new HashSet<Fireauto>();
        }

        public int Id { get; set; }
        public string? Car { get; set; }
        public string? Psch { get; set; }
        public string? Type { get; set; }
        public int? Parent { get; set; }
        public string? Комментарий { get; set; }
        public int? IdГарнизон { get; set; }

        public virtual Fireauto? ParentNavigation { get; set; }
        public virtual ICollection<Departure> Departures { get; set; }
        public virtual ICollection<Firework> Fireworks { get; set; }
        public virtual ICollection<Fireauto> InverseParentNavigation { get; set; }
    }
}
