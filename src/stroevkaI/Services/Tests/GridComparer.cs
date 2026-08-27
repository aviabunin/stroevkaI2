using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace stroevkaI.Services.Tests
{
    /// <summary>
    /// Класс для сравнения данных из Excel с данными в гриде
    /// </summary>
    public class GridComparer
    {
        private readonly DataGridView _grid;

        public GridComparer(DataGridView grid)
        {
            _grid = grid;
        }

        /// <summary>
        /// Сравнивает все строки Excel с гридом
        /// </summary>
        public List<GridComparisonResult> CompareAll(Dictionary<string, StroevkaRowData> excelData)
        {
            var results = new List<GridComparisonResult>();

            foreach (var item in excelData)
            {
                string rowId = item.Key;
                StroevkaRowData excelRow = item.Value;

                // Находим строку в гриде
                DataGridViewRow gridRow = FindGridRowByRowId(rowId);

                var result = new GridComparisonResult
                {
                    RowId = rowId,
                    Наименование = excelRow.Наименование,
                    Differences = new List<FieldDifference>()
                };

                if (gridRow == null)
                {
                    result.Status = "Нет в гриде";
                    results.Add(result);
                    continue;
                }

                // Сравниваем строки
                result.Differences = CompareRow(excelRow, gridRow);

                // 5. Отображаем результаты
                //ShowComparisonResults(results);

   //     https://chat.deepseek.com/a/chat/s/4357603a-a319-4fae-9958-905f73a47a6f

                result.Status = result.Differences.Count == 0 ? "Совпадает" : "Различия";
                results.Add(result);
            }

            return results;
        }

        /// <summary>
        /// Находит строку в гриде по RowId
        /// </summary>
        private DataGridViewRow FindGridRowByRowId(string rowId)
        {
            if (string.IsNullOrEmpty(rowId))
                return null;

            int rowIdColumnIndex = _grid.Columns.Count - 1;

            if (rowIdColumnIndex < 0 || rowIdColumnIndex >= _grid.Columns.Count)
                return null;

            foreach (DataGridViewRow row in _grid.Rows)
            {
                if (row.IsNewRow) continue;

                object value = row.Cells[rowIdColumnIndex].Value;
                if (value != null && value.ToString() == rowId)
                {
                    return row;
                }
            }

            return null;
        }

        /// <summary>
        /// Сравнивает строку из Excel с строкой в гриде
        /// </summary>
        private List<FieldDifference> CompareRow(StroevkaRowData excelRow, DataGridViewRow gridRow)
        {
            var differences = new List<FieldDifference>();

            // Сравниваем числовые значения (колонки 2-67)
            for (int col = 2; col <= 67; col++)
            {
                int excelValue = excelRow.Values[col - 2];
                int gridValue = GetGridValueAsInt(gridRow, col);

                if (excelValue != gridValue)
                {
                    differences.Add(new FieldDifference
                    {
                        ColumnNumber = col,
                        FieldName = GetColumnDisplayName(col),
                        ExcelValue = excelValue.ToString(),
                        GridValue = gridValue.ToString()
                    });
                }
            }

            // Сравниваем ФИО начальника караула (колонка 68)   ПОКА ПРОПУСКАЕМ ИЗ-ЗА КАРАУЛА
            //string excelChief = excelRow.Начкар;
            //string gridChief = GetGridValue(gridRow, 68);

            //if (!CompareChiefNames(excelChief, gridChief))
            //{
            //    differences.Add(new FieldDifference
            //    {
            //        ColumnNumber = 68,
            //        FieldName = "Нач.караула (фамилия)",
            //        ExcelValue = ExtractLastName(excelChief),
            //        GridValue = ExtractLastName(gridChief)
            //    });
            //}

            return differences;
        }

        /// <summary>
        /// Получает значение из грида по номеру колонки как строку
        /// </summary>
        private string GetGridValue(DataGridViewRow row, int columnNumber)
        {
            int gridColumnIndex = columnNumber ;

            if (gridColumnIndex < 0 || gridColumnIndex >= _grid.Columns.Count)
                return "0";

            object value = row.Cells[gridColumnIndex].Value;
            return value?.ToString() ?? "0";
        }

        /// <summary>
        /// Получает значение из грида по номеру колонки как int
        /// </summary>
        private int GetGridValueAsInt(DataGridViewRow row, int columnNumber)
        {
            string value = GetGridValue(row, columnNumber);
            return int.TryParse(value, out int result) ? result : 0;
        }

        /// <summary>
        /// Сравнивает фамилии начальников караулов
        /// </summary>
        private bool CompareChiefNames(string chief1, string chief2)
        {
            string lastName1 = Regex.Replace(chief1, @"[ .]", ""); //chief1.    ExtractLastName(chief1);
            string lastName2 = Regex.Replace(chief2, @"[ .]", "");
            return string.Equals(lastName1, lastName2, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Извлекает фамилию из ФИО
        /// </summary>
        private string ExtractLastName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return string.Empty;

            fullName = fullName.Trim();
            int spaceIndex = fullName.IndexOf(' ');
            return spaceIndex > 0 ? fullName.Substring(0, spaceIndex) : fullName;
        }

        /// <summary>
        /// Возвращает отображаемое имя колонки
        /// </summary>
        private string GetColumnDisplayName(int column)
        {
            var displayNames = new Dictionary<int, string>
        {
            {2, "АЦ"}, {3, "АЦЛ"}, {4, "АВ"}, {5, "АСА/АПП"},
            {6, "ПНС"}, {7, "АЛ"}, {8, "КП"}, {9, "АР"},
            {10, "АСМП"}, {11, "АШ"}, {12, "УКС/АБГ"}, {13, "Пож.поезд"},{14, "Пож.корабль(катер)"},
            {15, "АЦ резерв"}, {16, "АЦЛ резерв"}, {17, "АНР резерв"},
            {18, "АВ резерв"}, {19, "АСА/АПП резерв"}, {20, "ПНС резерв"},
            {21, "АЛ резерв"}, {22, "КП резерв"}, {23, "АР резерв"},
            {24, "АСМП резерв"}, {25, "АШ резерв"},
            {26, "УКС/АБГ резерв"}, {27, "АСМРХ"}, {28, "АВС"},{29, "Пож.корабль(катер) резерв"},
            {30, "ТО-1"}, {31, "ТО-2"}, {32, "Ремонт основная"},
            {33, "Ремонт специальная"}, {34, "Пожарный корабль"},
            {35, "Плав.средство"}, {36, "Снегоход/болотоход"},
            {37, "Мотопомпа"}, {38, "Прочие"},
            {39, "СИЗОД расчет"}, {40, "СИЗОД резерв"},
            {41, "Л-1(ОЗК)/ТАСК"}, {42, "ТОК"},
            {43, "ГАСИ расчет"}, {44, "ГАСИ резерв"},
            {45, "По списку"}, {46, "Налицо"}, {47, "Всего"},
            {48, "Резерв ЛС"}, {49, "НК"}, {50, "Диспетчер"},
            {51, "ПНК"}, {52, "КО"}, {53, "Водитель"},
            {54, "Пожарный"}, {55, "ГДЗС"}, {56, "Всего отсутствует"},
            {57, "Отпуск"}, {58, "По болезни"}, {59, "Командировка"},
            {60, "Некомплект"}, {61, "Прочие"},
            {62, "Пена расчет"}, {63, "Порошок расчет"},
            {64, "Пена резерв"}, {65, "Порошок резерв"},
            {66, "ДТ"}, {67, "Бензин"},
            {68, "Нач.караула (фамилия)"}
        };

            return displayNames.TryGetValue(column, out string name) ? name : $"Колонка {column}";
        }
    }
}
