using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class Post
    {
        public Post()
        {
            Personals = new HashSet<Personal>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        /// <summary>
        /// 0 - территориальный 1 - ПСГ  2 - местный гарнизон
        /// </summary>
        public int? Level { get; set; }
        public int Norder { get; set; }

        public virtual ICollection<Personal> Personals { get; set; }
    }
}
