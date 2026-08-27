using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class Vid
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? GarnizonId { get; set; }

        public virtual Garnizon? Garnizon { get; set; }
    }
}
