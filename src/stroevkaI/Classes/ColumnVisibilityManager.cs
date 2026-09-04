using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stroevkaI
{
    public class ColumnVisibilityManager
    {
        private readonly DataGridView _grid1; // PivotRowGrid
        private readonly DataGridView _grid2; // EquipmentDataGridView

        public ColumnVisibilityManager(DataGridView grid1, DataGridView grid2)
        {
            _grid1 = grid1;
            _grid2 = grid2;
        }

        // Применить список видимых колонок
        public void ApplyVisibility(List<string> visibleColumns)
        {
            // Всегда добавляем первую колонку (Псг)
            if (!visibleColumns.Contains("ПЧ"))
                visibleColumns.Insert(0, "ПЧ");

            ApplyToGrid(_grid1, visibleColumns);
            ApplyToGrid(_grid2, visibleColumns);
        }

        private void ApplyToGrid(DataGridView grid, List<string> visibleColumns)
        {
            if (grid.DataSource == null) return;
            foreach (DataGridViewColumn col in grid.Columns)
            {
                if (string.IsNullOrEmpty(col.DataPropertyName)) continue;
                bool shouldBeVisible = visibleColumns.Contains(col.DataPropertyName);
                col.Visible = shouldBeVisible;
                // Замораживаем колонку "Псг"
                if (col.DataPropertyName == "Псг")
                    col.Frozen = true;
            }
        }

        // Применить группу
        public void ApplyGroup(string groupName)
        {
            List<string> groupColumns = groupName switch
            {
                "БоевойРасчёт" => ColumnGroups.БоевойРасчёт,
                "ЛичныйСостав" => ColumnGroups.ЛичныйСостав,
                "ДополнительныйСписок" => ColumnGroups.ДополнительныйСписок,
                _ => ColumnGroups.ВсеКолонки // или null, обработать отдельно
            };
            ApplyVisibility(groupColumns);
        }

        // Получить текущий список видимых колонок
        public List<string> GetVisibleColumns()
        {
            // Берём из первого грида
            return _grid1.Columns
                .Cast<DataGridViewColumn>()
                .Where(c => c.Visible && !string.IsNullOrEmpty(c.DataPropertyName))
                .Select(c => c.DataPropertyName)
                .ToList();
        }

    }
    public static class ColumnGroups
    {
        // Группа "Средства в боевом расчёте" (br)
        public static readonly List<string> БоевойРасчёт = new List<string>
    {
        "AcBr", "AclBr", "АвBr", "АсаBr", "АсоBr", "ПнсBr", "AlBr", "КпBr",
        "АрBr", "АсмпПсаBr", "АшBr", "УксАбгBr", "ПожПоездКорабльBr",
        "ПожПоездBr", "ПожКорабльКатерBr", "АсмрхBr", "АвсBr",
        // и другие br-колонки (по вашему FirePsgStat)
    };

        // Группа "Личный состав"
        public static readonly List<string> ЛичныйСостав = new List<string>
    {
        "ПоСписку", "Налицо", "Всего", "Резерв", "Нк", "Диспетчер",
        "Пнк", "Ко", "Водитель", "Пожарный", "Гдзс", "ВсегоОтс",
        "Отпуск", "ПоБольничному", "Командировка", "Некомплект", "ПрочиеОтс"
    };

        // Группа "Дополнительный список" (например, СИЗОД, ГАСИ, пена, топливо)
        public static readonly List<string> ДополнительныйСписок = new List<string>
    {
        "SizodBr", "SizodRezerv", "КостюмыЛ1Таск", "КостюмыТок", "КостюмыДругие",
        "ГасиРасчёт", "ГасиРезерв", "ПенаРасчёт", "ПенаРезерв",
        "ПорошокРасчёт", "ПорошокРезерв", "Дт", "Бензин",
        "Tofirst", "Totow", "ПлавСредства", "Болотоходы", "Мотопомпы", "Прочее"
    };

        // Все колонки (для инициализации и чекбоксов)
        public static readonly List<string> ВсеКолонки = БоевойРасчёт
            .Concat(ЛичныйСостав)
            .Concat(ДополнительныйСписок)
            .Distinct()
            .ToList();
    }
}
