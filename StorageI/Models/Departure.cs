using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    /// <summary>
    /// Выезда
    /// </summary>
    public partial class Departure
    {
        public int Id { get; set; }
        public string? Гарнизон { get; set; }
        public string? Автомобиль { get; set; }
        public string? Адрес { get; set; }
        public DateTime? Выезд { get; set; }
        public DateTime? КМестуВызова { get; set; }
        public DateTime? Локализация { get; set; }
        public DateTime? Ликвидация { get; set; }
        public DateTime? Возвращение { get; set; }
        public int? IdГарнизона { get; set; }
        public int? IdАвтомобиля { get; set; }
        public int? IdВызова { get; set; }
        public string? Комментарий { get; set; }
        public string? Mguid { get; set; }
        public DateTime? Lastchanged { get; set; }
        public int? Idparent { get; set; }

        public virtual Fireauto? IdАвтомобиляNavigation { get; set; }
        public virtual Call? IdВызоваNavigation { get; set; }
    }
}
