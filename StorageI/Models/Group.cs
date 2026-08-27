using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class Group
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Comment { get; set; }
    }
}
