using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class Firecar
    {
        public int Id { get; set; }
        public string? Автомобиль { get; set; }
        public int? Parent { get; set; }
        public string? Гарнизон { get; set; }
        public int? IdГарнизон { get; set; }
        public string? Комментарий { get; set; }
    }
}
