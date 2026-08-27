using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class SredstvaVid
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Fullname { get; set; }
        public string? Comment { get; set; }
        public string? Group { get; set; }
        public int? GroupId { get; set; }
    }
}
