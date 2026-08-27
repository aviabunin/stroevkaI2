using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stroevkaI.Services.Tests
{
    /// <summary>
    /// Результат сравнения строки
    /// </summary>
    public class GridComparisonResult
    {
        public string RowId { get; set; }
        public string Наименование { get; set; }
        public string Status { get; set; } // "Совпадает", "Различия", "Нет в гриде"
        public List<FieldDifference> Differences { get; set; }
    }
}
