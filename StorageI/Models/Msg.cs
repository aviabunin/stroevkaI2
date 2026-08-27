using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class Msg
    {
        public int Id { get; set; }
        public string? Tema { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public string? Text { get; set; }
        public string? Cmd { get; set; }
        public string? Comment { get; set; }
        public string? Visible { get; set; }
        public DateTime? Mdate { get; set; }
        public DateTime? Lastedit { get; set; }
        public string? Status { get; set; }
        public bool? IsNew { get; set; }
        public int? StatusId { get; set; }
    }
}
