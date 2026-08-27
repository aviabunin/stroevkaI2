using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class Psgparam
    {
        public int Id { get; set; }
        public int Посписку { get; set; }
        public int ПовышКо { get; set; }
        public int ПовышВод { get; set; }
        public int ПовышПож { get; set; }
        public int ПовышГдзс { get; set; }
        public int ПовышАш { get; set; }
        public int? SubdivisionId { get; set; }
    }
}
