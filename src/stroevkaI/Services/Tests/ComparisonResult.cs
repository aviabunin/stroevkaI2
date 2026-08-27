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
    public class ComparisonResult
    {
        public int RowNum { get; set; }
        public string RowId { get; set; }
        public string Наименование { get; set; }
        public string RowType { get; set; }
        public string Status { get; set; } // "Совпадает", "Различия", "Нет в БД"
        public List<FieldDifference> Differences { get; set; }
    }

    /// <summary>
    /// Различие в конкретном поле
    /// </summary>
    public class FieldDifference
    {
        public int ColumnNumber { get; set; }
        public string FieldName { get; set; }
        public string ExcelValue { get; set; }
        public string GridValue { get; set; }
    }




    /// <summary>
    /// Тип строки в отчете
    /// </summary>
    public enum RowType
    {
        Regular,    // Обычная строка (подразделение)
        TotalGps,   // Итог по ГПС
        TotalFps,   // Итог по ФПС
        TotalOther, // Итог по "другие"
        TotalChpo,  // Итог по ЧПО
        TotalVpo,   // Итог по ВПО
        TotalASF,    // Итог "по АСФ"
        TotalAll    // Итог "всего"
    }
}
