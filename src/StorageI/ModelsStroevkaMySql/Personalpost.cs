using System;
using System.Collections.Generic;

namespace StorageI.ModelsStroevkaMySql
{
    public partial class Personalpost
    {
        public uint Id { get; set; }
        public int? PersonalId { get; set; }
        public int? PostId { get; set; }
        public string? Fio { get; set; }
        public string? Post { get; set; }

        public virtual Personal? Personal { get; set; }
        public virtual Post? PostNavigation { get; set; }
    }
}
