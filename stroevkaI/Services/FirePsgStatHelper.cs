using System;
using System.Collections.Generic;
using System.Reflection;
using StorageI.ModelsStroevkaMySql;

public static class FirePsgStatHelper
{
    // Список имён свойств в строгом порядке от AcBr до Начкар
    private static readonly List<string> ColumnOrder = new List<string>
    {
        "AcBr", "AcRezerv", "AcRemont", "AclBr", "AclRezerv", "AclRemont",
        "АнрBr", "АнрRezerv", "АнрRemont", "АсаBr", "АсаRezerv", "АсаRemont",
        "АсоBr", "АсоRezerv", "АсоRemont", "АвBr", "АвRezerv", "АвRemont",
        "АсаАппАсмBr", "АсаАппАсмRezerv", "АсаАппАсмRemont", "ПнсBr", "ПнсRezerv", "ПнсRemont",
        "AlBr", "AlRezerv", "AlRemont", "КпBr", "КпRezerv", "КпRemont",
        "АрBr", "АрRezerv", "АрRemont", "АсмпПсаBr", "АсмпПсаRezerv", "АсмпПсаRemont",
        "АшBr", "АшRezerv", "АшRemont", "УксАбгBr", "УксАбгRezerv", "УксАбгRemont",
        "ПожПоездКорабльBr", "ПожПоездКорабльRezerv", "ПожПоездКорабльRemont",
        "АсмрхBr", "АсмрхRezerv", "АвсBr", "АвсRezerv", "РемонтОсновной", "РемонтСпециальной",
        "ПожарныйКорабльРемонт", "ПлавСредства", "Болотоходы", "Мотопомпы", "Прочее",
        "Tofirst", "Totow", "SizodBr", "SizodRezerv",
        "КостюмыЛ1Таск", "КостюмыТок", "КостюмыДругие",
        "Нк", "Диспетчер", "Пнк", "Ко", "Водитель", "Пожарный", "Гдзс",
        "ПоСписку", "Налицо", "Всего", "Резерв", "ГасиРасчёт", "ГасиРезерв",
        "ВсегоОтс", "Отпуск", "ПоБольничному", "Командировка", "Некомплект", "ПрочиеОтс",
        "ПенаРасчёт", "ПенаРезерв", "ПорошокРасчёт", "ПорошокРезерв",
        "Дт", "Бензин", "Начкар"
    };

    /// <summary>
    /// Возвращает значение колонки по её индексу (0 = AcBr).
    /// </summary>
    /// <param name="record">Экземпляр FirePsgStat</param>
    /// <param name="columnIndex">Индекс колонки, начиная с 0</param>
    /// <returns>Значение в виде строки, для null – пустая строка</returns>
    public static string GetColumnValue(FirePsgStat record, int columnIndex)
    {
        if (record == null)
            throw new ArgumentNullException(nameof(record));

        if (columnIndex < 0 || columnIndex >= ColumnOrder.Count)
            throw new ArgumentOutOfRangeException(nameof(columnIndex), $"Индекс должен быть от 0 до {ColumnOrder.Count - 1}");

        string propertyName = ColumnOrder[columnIndex];
        PropertyInfo prop = typeof(FirePsgStat).GetProperty(propertyName);

        if (prop == null)
            return string.Empty; // свойство не найдено (на всякий случай)

        object value = prop.GetValue(record);
        return value?.ToString() ?? string.Empty;
    }
}
