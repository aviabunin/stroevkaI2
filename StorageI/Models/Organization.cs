using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class Organization
    {
        public Organization()
        {
            FireLiquidationPlans = new HashSet<FireLiquidationPlan>();
        }

        public int Id { get; set; }
        public string? Наименование { get; set; }
        public string? Псо { get; set; }
        public DateTime ВремяРедактирования { get; set; }
        public string? ПтпКтп { get; set; }
        public bool Исполнен { get; set; }
        public string? Каталог { get; set; }
        public string? ПланТушения { get; set; }
        public DateTime? Срок { get; set; }
        public int IdПсо { get; set; }
        public string? ТелефонРуководителя { get; set; }
        public string? Адрес { get; set; }
        public string? ТелефонОхраны { get; set; }
        public int? ЧислоЖилыхДомов { get; set; }
        public int? ЧислоЖизненноВажныхОбъектов { get; set; }
        public double? РасстояниеДоПч { get; set; }
        public string? МаршрутСледования { get; set; }
        public string? ТелефонОтвЗаПб { get; set; }
        public string? ТелефонДиспетчераЭнергослужбы { get; set; }
        public string? Комментарии { get; set; }
        public string? Район { get; set; }
        public string? Поселение { get; set; }
        public string? НасПункт { get; set; }
        public string? Номер { get; set; }
        public string Psg { get; set; } = null!;
        public int? IdPsg { get; set; }
        public int? IdRegion { get; set; }
        public int? IdSettle { get; set; }
        public int? IdPunct { get; set; }
        public string? ПтпСтатус { get; set; }
        public string? ТипДокумента { get; set; }
        /// <summary>
        /// id последнего документа птп/ктп или null
        /// </summary>
        public int? IdДокумента { get; set; }
        public int? Признак { get; set; }
        public string? Гарнизон { get; set; }
        public int? IdГарнизона { get; set; }
        public string? ДатаКонтроля { get; set; }

        public virtual Psgdatum IdПсоNavigation { get; set; } = null!;
        public virtual ICollection<FireLiquidationPlan> FireLiquidationPlans { get; set; }
    }
}
