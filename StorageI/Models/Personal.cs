using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class Personal
    {
        public int Id { get; set; }
        public string? F { get; set; }
        public string? I { get; set; }
        public string? O { get; set; }
        public string? Post { get; set; }
        public int? PostId { get; set; }
        /// <summary>
        /// Сегодня на смене (из графика)
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// Работает 1 отгул 2 уволен 0
        /// </summary>
        public int? Inwork { get; set; }
        public int? PsgId { get; set; }
        public string? PsgName { get; set; }
        public int? SubdivisionId { get; set; }
        public string? Subdivision { get; set; }
        public string? Zvanie { get; set; }
        public string? TfMobil { get; set; }
        public string? TfWork { get; set; }
        public string? TfDom { get; set; }
        public string? Posyvnoy { get; set; }
        public int? Order { get; set; }
        public DateTime? OtpBeg { get; set; }
        public DateTime? OtpEnd { get; set; }
        public string? Otdel { get; set; }
        public int? OtdelId { get; set; }
        public string? Comment { get; set; }

        public virtual Post? PostNavigation { get; set; }
        public virtual Psgdatum? SubdivisionNavigation { get; set; }
    }
}
