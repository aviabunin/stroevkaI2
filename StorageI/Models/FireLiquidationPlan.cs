using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class FireLiquidationPlan
    {
        public int Id { get; set; }
        public string? ТипДокумента { get; set; }
        public int? НомерДокумента { get; set; }
        public string? КороткоеНазваниеОрганизации { get; set; }
        public string? Псо { get; set; }
        public string? АдресОбъекта { get; set; }
        public DateTime? ДатаУтверждения { get; set; }
        public DateTime? СрокОтработки { get; set; }
        public bool? Исполнен { get; set; }
        public string? ОтметкаОбИсполнении { get; set; }
        public string? СоставлениеИлиКорректировка { get; set; }
        public string? Исполнитель { get; set; }
        public DateTime ВремяРедактирования { get; set; }
        public string? НаличиеЭлектронногоВарианта { get; set; }
        public string? Организация { get; set; }
        public int? IdOrganization { get; set; }
        public int? IdПсо { get; set; }
        public int? IdИсполнителя { get; set; }
        public string? Документ { get; set; }

        public virtual Organization? IdOrganizationNavigation { get; set; }
    }
}
