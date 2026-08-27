using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class Dblog
    {
        public int Id { get; set; }
        public string? Logitem { get; set; }
        public int? Loglevel { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? UserName { get; set; }
        public string? Computer { get; set; }
        public string? Operation { get; set; }
        public string? PrimaryKeyName { get; set; }
        public string? KeyValue { get; set; }
        public string? SetName { get; set; }
        public int? IdRegion { get; set; }
        public int? IdSettle { get; set; }
        public int? IdPunct { get; set; }
    }
}
