using System;
using System.Collections.Generic;

namespace StorageI.ModelsStroevkaMySql
{
    public partial class CacheNachkar
    {
        public int SubdivisionId { get; set; }
        public string? Nachkar { get; set; }
        public int? Karaul { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int Id { get; set; }
        public int? PsgId { get; set; }
    }
}
