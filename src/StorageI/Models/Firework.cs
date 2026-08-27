using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    /// <summary>
    /// Выезда
    /// </summary>
    public partial class Firework
    {
        public int Id { get; set; }
        public int N { get; set; }
        public DateTime? ВремяСообщения { get; set; }
        public string? Фабула { get; set; }
        public string? Адрес { get; set; }
        public string? Автомобиль { get; set; }
        public DateTime? ВремяВыезда { get; set; }
        public DateTime? ВремяКМестуВызова { get; set; }
        public DateTime? ВремяЛокализации { get; set; }
        public DateTime? ВремяЛиквидации { get; set; }
        public DateTime? ВремяВозвращенияВДепо { get; set; }
        public string? Результат { get; set; }
        public int? IdГарнизона { get; set; }
        public int? IdАвтомобиля { get; set; }
        public int Nall { get; set; }
        public int Nown { get; set; }

        public virtual Fireauto? IdАвтомобиляNavigation { get; set; }
        public virtual Psgdatum? IdГарнизонаNavigation { get; set; }
    }
}
