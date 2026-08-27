using stroevkaI.Models;
public static class PivotConfigs
{
    public static LevelConfig PsgLevelConfig { get; } = new LevelConfig
    {
        LevelId = "psg",
        Categories = new List<CategoryRule>
        {
            new CategoryRule { CategoryId = "main", Condition = n => n.Category != "АСФ" },
            new CategoryRule { CategoryId = "gps", Condition = n => n.Category == "ППС" || n.Category == "ФПС" },
            new CategoryRule { CategoryId = "fps", Condition = n => n.Category == "ФПС" },
            new CategoryRule { CategoryId = "vpo", Condition = n => n.Category == "ВПО" },
            new CategoryRule { CategoryId = "chpo", Condition = n => n.Category == "ЧПО" },
            new CategoryRule { CategoryId = "asf", Condition = n => n.Category == "АСФ" },
            new CategoryRule { CategoryId = "other", Condition = n => !new[] { "ППС", "ФПС", "АСФ", "ВПО", "ЧПО" }.Contains(n.Category) }
        }
    };

    public static LevelConfig TerritorialLevelConfig { get; } = new LevelConfig
    {
        LevelId = "territorial",
        Categories = new List<CategoryRule>
        {
            new CategoryRule { CategoryId = "main", Condition = n => n.Category != "АСФ" },
            new CategoryRule { CategoryId = "gps", Condition = n => n.Category == "ППС" || n.Category == "ФПС" },
            new CategoryRule { CategoryId = "fps", Condition = n => (n.Category == "ФПС" && n.ParentId != 1744) || n.Name == "ПЧ-75" },
            new CategoryRule { CategoryId = "vpo", Condition = n => n.Category == "ВПО" },
            new CategoryRule { CategoryId = "chpo", Condition = n => n.Category == "ЧПО" },
            new CategoryRule { CategoryId = "asf", Condition = n => n.Category == "АСФ" },
            new CategoryRule { CategoryId = "other", Condition = n => !new[] { "ППС", "ФПС", "АСФ", "ВПО", "ЧПО" }.Contains(n.Category) }
        }
    };
}
// ============================================================
// 2. ВСПОМОГАТЕЛЬНЫЕ КЛАССЫ ДЛЯ КОНФИГУРАЦИЙ
// ============================================================
public class ColumnConfig
{
    public string PropertyName { get; set; }          // Имя свойства в PivotRow
    public string SourceTable { get; set; }           // "sredstva", "sostav", ...
    public List<string> FilterValues { get; set; } = new List<string>(); // Для фильтрации по имени (например, "АЦ")

    // ---- НОВОЕ: делегат вычисления ----
    public Func<Dictionary<string, decimal>, decimal> Compute { get; set; }

    // ---- Для обратной совместимости (простой случай) ----
    public string AggregateField { get; set; }        // Одно поле для суммирования

    // Вспомогательный метод для получения значения
    public decimal GetValue(Dictionary<string, decimal> fields)
    {
        if (Compute != null)
            return Compute(fields);

        if (!string.IsNullOrEmpty(AggregateField))
            return fields.GetValueOrDefault(AggregateField, 0);

        return 0;
    }
}

public class CategoryRule
{
    public string CategoryId { get; set; }
    public Func<ReportNode, bool> Condition { get; set; }
}

public class LevelConfig
{
    public string LevelId { get; set; } // "psg" или "territorial"
    public List<CategoryRule> Categories { get; set; } = new List<CategoryRule>();
}

