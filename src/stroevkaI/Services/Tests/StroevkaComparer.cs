using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageI.ModelsStroevkaMySql;
using System.Reflection;

namespace stroevkaI.Services.Tests
{
    /// <summary>
    /// Сравнивает данные из Excel с данными в гриде
    /// </summary>
    public class StroevkaComparer
    {
        private readonly DataGridView _grid;
        private  Dictionary<int, string> _columnDisplayNames;
        #region Инициализация
        public StroevkaComparer(DataGridView grid)
        {
            _grid = grid;
            InitializeColumnNames();
        }

        /// <summary>
        /// Инициализирует отображаемые имена колонок
        /// </summary>
        private void InitializeColumnNames()
        {
            _columnDisplayNames = new Dictionary<int, string>
        {
            // 2-13: Пожарная техника (боевой расчет)
            {2, "АЦ"}, {3, "АЦЛ"}, {4, "АВ"}, {5, "АСА/АПП"},
            {6, "ПНС"}, {7, "АЛ"}, {8, "КП"}, {9, "АР"},
            {10, "АСМП"}, {11, "АШ"}, {12, "УКС/АБГ"}, {13, "Пож.поезд/корабль"},
            
            // 14-28: Резерв
            {14, "АЦ резерв"}, {15, "АЦЛ резерв"}, {16, "АНР резерв"},
            {17, "АВ резерв"}, {18, "АСА/АПП резерв"}, {19, "ПНС резерв"},
            {20, "АЛ резерв"}, {21, "КП резерв"}, {22, "АР резерв"},
            {23, "АСМП резерв"}, {24, "АШ резерв"}, {25, "АСО резерв"},
            {26, "УКС/АБГ резерв"}, {27, "АСМРХ"}, {28, "АВС"},
            
            // 29-37: Не в расчете
            {29, "ТО-1"}, {30, "ТО-2"}, {31, "Ремонт основная"},
            {32, "Ремонт специальная"}, {33, "Пожарный корабль"},
            {34, "Плав.средство"}, {35, "Снегоход/болотоход"},
            {36, "Мотопомпа"}, {37, "Прочие"},
            
            // 38-39: СИЗОД
            {38, "СИЗОД расчет"}, {39, "СИЗОД резерв"},
            
            // 40-42: Защитные костюмы
            {40, "Л-1(ОЗК)/ТАСК"}, {41, "ТОК"}, {42, "Костюмы другие"},
            
            // 43-44: ГАСИ
            {43, "ГАСИ расчет"}, {44, "ГАСИ резерв"},
            
            // 45-61: Личный состав
            {45, "По списку"}, {46, "Налицо"}, {47, "Всего"},
            {48, "Резерв ЛС"}, {49, "НК"}, {50, "Диспетчер"},
            {51, "ПНК"}, {52, "КО"}, {53, "Водитель"},
            {54, "Пожарный"}, {55, "ГДЗС"}, {56, "Всего отсутствует"},
            {57, "Отпуск"}, {58, "По болезни"}, {59, "Командировка"},
            {60, "Некомплект"}, {61, "Прочие"},
            
            // 62-65: Огнетушащие вещества
            {62, "Пена расчет"}, {63, "Порошок расчет"},
            {64, "Пена резерв"}, {65, "Порошок резерв"},
            
            // 66-67: Запас топлива
            {66, "ДТ"}, {67, "Бензин"},
            
            // 68
            {68, "Нач.караула (фамилия)"}
        };
        }
        #endregion

        /// <summary>
        /// Сравнивает строки Excel с данными в гриде
        /// </summary>
        public List<ComparisonResult> Compare(List<StroevkaExcelRow> excelRows)
        {
            var results = new List<ComparisonResult>();

            foreach (var excelRow in excelRows)
            {
                // Ищем строку в гриде по RowId (последняя, невидимая колонка)
                DataGridViewRow gridRow = FindRowByRowId(excelRow.RowId);

                var result = new ComparisonResult
                {
                    RowNum = excelRow.RowNum,
                    RowId = excelRow.RowId,
                    Наименование = excelRow.Пч,
                    RowType = excelRow.Type.ToString(),
                    Differences = new List<FieldDifference>()
                };

                if (gridRow == null)
                {
                    result.Status = "Нет в гриде";
                    results.Add(result);
                    continue;
                }

                // Сравниваем колонки 2-67
                for (int col = 2; col <= 67; col++)
                {
                    // Получаем значение из Excel
                    string excelValue = excelRow.ColumnValues.TryGetValue(col, out string val) ? val : "0";

                    // Получаем значение из грида
                    string gridValue = GetGridValue(gridRow, col);

                    // Сравниваем как числа
                    if (int.TryParse(excelValue, out int excelInt) && int.TryParse(gridValue, out int gridInt))
                    {
                        if (excelInt != gridInt)
                        {
                            result.Differences.Add(new FieldDifference
                            {
                                ColumnNumber = col,
                                FieldName = GetColumnDisplayName(col),
                                ExcelValue = excelInt.ToString(),
                                GridValue = gridInt.ToString()
                            });
                        }
                    }
                    else
                    {
                        if (excelValue != gridValue)
                        {
                            result.Differences.Add(new FieldDifference
                            {
                                ColumnNumber = col,
                                FieldName = GetColumnDisplayName(col),
                                ExcelValue = excelValue,
                                GridValue = gridValue
                            });
                        }
                    }
                }

                // Сравниваем ФИО начальника караула (колонка 68)
                string gridChief = GetGridValue(gridRow, 68);
                CompareChiefLastName(excelRow.Начкар, gridChief, result);

                result.Status = result.Differences.Count == 0 ? "Совпадает" : "Различия";
                results.Add(result);
            }

            return results;
        }

        /// <summary>
        /// Находит строку в гриде по RowId (последняя, невидимая колонка)
        /// </summary>
        private DataGridViewRow FindRowByRowId(string rowId)
        {
            if (string.IsNullOrEmpty(rowId))
                return null;

            // Предполагаем, что последняя колонка (индекс = Columns.Count - 1) содержит RowId
            int rowIdColumnIndex = _grid.Columns.Count - 1;

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
        /// Получает значение из грида по номеру колонки
        /// </summary>
        private string GetGridValue(DataGridViewRow row, int columnNumber)
        {
            // Номера колонок в гриде: колонка 0 - это Наименование (1 в Excel)
            // Поэтому номер колонки в Excel = columnNumber, в гриде = columnNumber - 1
            int gridColumnIndex = columnNumber - 1;

            if (gridColumnIndex < 0 || gridColumnIndex >= _grid.Columns.Count)
                return "0";

            object value = row.Cells[gridColumnIndex].Value;
            return value?.ToString() ?? "0";
        }

        /// <summary>
        /// Возвращает отображаемое имя колонки
        /// </summary>
        private string GetColumnDisplayName(int column)
        {
            return _columnDisplayNames.TryGetValue(column, out string name) ? name : $"Колонка {column}";
        }

        /// <summary>
        /// Сравнивает фамилию начальника караула (без инициалов)
        /// </summary>
        private void CompareChiefLastName(string excelChief, string gridChief, ComparisonResult result)
        {
            string excelLastName = ExtractLastName(excelChief);
            string gridLastName = ExtractLastName(gridChief);

            if (!string.Equals(excelLastName, gridLastName, StringComparison.OrdinalIgnoreCase))
            {
                result.Differences.Add(new FieldDifference
                {
                    ColumnNumber = 68,
                    FieldName = "Нач.караула (фамилия)",
                    ExcelValue = excelLastName,
                    GridValue = gridLastName
                });
            }
        }

        /// <summary>
        /// Извлекает фамилию из ФИО (все, что до первого пробела)
        /// </summary>
        private string ExtractLastName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return string.Empty;

            fullName = fullName.Trim();
            int spaceIndex = fullName.IndexOf(' ');
            return spaceIndex > 0 ? fullName.Substring(0, spaceIndex) : fullName;
        }
    }
}
