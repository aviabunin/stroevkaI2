using System;
using System.Collections.Generic;

namespace StorageI.ModelsStroevkaMySql
{
    /// <summary>
    /// Итоговые строки для ПСГ
    /// </summary>
    public partial class PsgTotalRow
    {
        public int Id { get; set; }
        /// <summary>
        ///  Уникальный ID строки (14 символов): признак(2)+кодПСГ(4)+0000(4)+кодТерриториального(4)
        /// </summary>
        public string? RowId { get; set; }
        /// <summary>
        /// Наименование итоговой строки в представлении
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Отображаемое наименование (с отступами)
        /// </summary>
        public string? DisplayName { get; set; }
        /// <summary>
        /// ID ПСГ (из таблицы psg или 11 для территориального)
        /// </summary>
        public int? PsgId { get; set; }
        /// <summary>
        /// Тип категории: main, gps, fps, other, vpo, chpo, asf
        /// </summary>
        public string? CategoryType { get; set; }
        public string? CategoryDisplay { get; set; }
        /// <summary>
        /// Признак итога: 01-итог по ПСГ, 02-ГПС, 03-ФПС, 04-другие, 05-ВПО, 06-ЧПО, 07-АСФ
        /// </summary>
        public string? TotalFlag { get; set; }
        /// <summary>
        /// Порядок сортировки (отрицательные числа для группировки)
        /// </summary>
        public int? Norder { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
