using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorageI.ModelsStroevkaMySql
{
    public partial class PivotRow
    {
        /// <summary>
        /// Дочерние строки для формирования дерева и подсказок (tooltip).
        /// В БД не сохраняется.
        /// </summary>
        [NotMapped]
        public List<PivotRow> Childes { get; set; } = new List<PivotRow>();

        /// <summary>
        /// Детали для отображения составляющих суммы в tooltip.
        /// В БД не сохраняется.
        /// </summary>
        [NotMapped]
        public Dictionary<string, List<DetailItem>> CellDetails { get; set; }
            = new Dictionary<string, List<DetailItem>>();
    }

    /// <summary>
    /// Класс для хранения информации о составляющей суммы.
    /// Не мапится на БД.
    /// </summary>
    [NotMapped]
    public class DetailItem
    {
        public string Name { get; set; }        // например, "ПЧ-1" или "ПСГ Беломорский"
        public decimal Value { get; set; }
        public string Category { get; set; }    // опционально, для группировки
    }
}