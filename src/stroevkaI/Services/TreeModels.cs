using System.Collections.Generic;

namespace stroevkaI.Models
{
    /// <summary>
    /// Базовый узел дерева
    /// </summary>
    public  class ReportNode
    {
        public int Id { get; set; }                             // Id ПЧ/ПСГ
        public string Name { get; set; }                        // Наименование 
        public string Category { get; set; }                    // ППС,ФПС, ДПО - для листьев gps,main...  для внутр узлов
        public int ParentId { get; set; }                       //  Id родителя
        public List<ReportNode> Children { get; set; } = new List<ReportNode>();  // дочки
        public int Isitog { get; set; }                       //  признак итогой строки

        // RawData: sourceType -> (itemName -> (fieldName -> value))
        public Dictionary<string, Dictionary<string, Dictionary<string, decimal>>> RawData { get; set; }
            = new Dictionary<string, Dictionary<string, Dictionary<string, decimal>>>();

        // Признак участия в территориальном итоге (по умолчанию true)
        public bool IncludeInTerritorial { get; set; } = true;
    }

    /// <summary>
    /// Узел ПСГ  not used yet
    /// </summary>
    public class PsgNode : ReportNode
    {
        // дополнительные свойства, если нужны
    }

    /// <summary>
    /// Узел ПЧ   - not used
    /// </summary>
    public class PchNode : ReportNode
    {
        public int PchId { get; set; }
        public string Category { get; set; } // "ФПС", "ППС", ...
        public bool Datafilled { get; set; }
        public string RowId { get; set; }
        public int Norder { get; set; }
        public int ParentId { get; set; } // id родительского ПСГ
    }

    /// <summary>
    /// Узел итога по категории (ГПС, ФПС, ВПО, ЧПО, другие, АСФ)
    /// </summary>
    public class CategoryTotalNode : ReportNode
    {
        public string CategoryType { get; set; } // "gps", "fps", etc.
    }
    public class ColumnConfig
    {
        public string ColumnId { get; set; }          // "ремонт_основной"
        public string SourceType { get; set; }        // "sredstva", "sostav", "sizod", ...
        public string AggregateField { get; set; }    // "remont", "br", "count", ...
        public List<string> FilterValues { get; set; } = new List<string>(); // ["АЦ", "АЦЛ", ...]
    }
    public class configRools { 
        public List<ColumnConfig> getRools()
        {
            var columnConfigs = new List<ColumnConfig>
            {
                new ColumnConfig { ColumnId = "ремонт_основной", SourceType = "sredstva", AggregateField = "remont", FilterValues = new List<string> { "АЦ", "АЦЛ", "АВ", "АСА", "АПП", "ПНС", "АНР" } },
                new ColumnConfig { ColumnId = "ac_br", SourceType = "sredstva", AggregateField = "br", FilterValues = new List<string> { "АЦ" } },
                new ColumnConfig { ColumnId = "sizod_br", SourceType = "sizod", AggregateField = "raschet", FilterValues = new List<string>() }, // все
                new ColumnConfig { ColumnId = "по_списку", SourceType = "sostav", AggregateField = "count", FilterValues = new List<string> { "По списку" } },
            };
            return columnConfigs;
        }
    }

}
