using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StorageI.ModelsStroevkaMySql;

namespace stroevkaI.Services.Tests
{
    /// <summary>
    /// Сервис для чтения Excel файлов строевой записки
    /// </summary>
    public class ExcelReaderService
    {
        private readonly Dictionary<string, int> _psgCodeMap;
        private readonly List<Pch> _pchList;
        private readonly List<Psg> _psgList;

        public ExcelReaderService(List<Pch> pchList = null, List<Psg> psgList = null)
        {
            _pchList = pchList ?? new List<Pch>();
            _psgList = psgList ?? new List<Psg>();

            // Инициализация справочников кодов ПСГ  todo - сформировать из БД
            _psgCodeMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { "Петрозаводский", 1 },
                { "Костомукшский", 3 },
                { "Сегежский", 8 },
                { "Сортавальский", 4 },
                { "Питкярантский", 5 },
                { "Муезерский", 6 },
                { "Кондопожский", 7 },
                { "Калевальский", 111 },
                { "Кемский", 122 },
                { "Пудожский", 138 },
                { "Суоярвский", 176 },
                { "Олонецкий", 201 },
                { "Лоухский", 229 },
                { "Беломорский", 248 },
                { "Пряжинский", 271 },
                { "Медвежьегорский", 291 },
                { "Лахденпохский", 324 },
                { "Прионежский", 1744 }
            };
        }

        /// <summary>
        /// Чтение Excel файла и возврат словаря <кодСтроки, ДанныеСтроки>
        /// </summary>
        /// <param name="filePath">Полный путь к Excel файлу</param>
        /// <param name="rootpsg">Корневой ПСГ (объект из таблицы psg)</param>
        /// <returns>Словарь с кодом строки и данными</returns>
        public Dictionary<string, StroevkaRowData> ReadExcelFile(string filePath, Psg rootpsg, int rowsCount)//todo добавить количСтрок
        {
            int firstColumn = 2;
            int lastColumn = 68;// всегда 68

            string rootpsgName = rootpsg == null ? "Территориальный" : rootpsg.Garnizon.Trim();

            string dirTemplatePath = Directory.GetCurrentDirectory() + @"\отчеты\days_stroevka\";
             //filePath = dirTemplatePath + @"OLD\"+ rootpsgName + ".xlsx";

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл не найден: {filePath}");

            var result = new Dictionary<string, StroevkaRowData>();


            try
            {
                using (var workbook = new XLWorkbook(filePath))
                {
                    var worksheet = workbook.Worksheet(1);
                    if (worksheet == null)
                        throw new Exception("Не найден лист с данными");


                    int firstRow = rootpsgName == "Территориальный" ? 21 : 14;
                    int lastRow = firstRow + rowsCount;//   rootpsgName == "Территориальный" ? 45 : 32;

                    // Пропускаем заголовки, начинаем с первой строки данных   todo - для территориал - своя нач строка
                    for (int row = firstRow; row <= lastRow; row++)  
                    {
                        var rowData = new StroevkaRowData();
                        var values = new List<string>();

                        // Читаем все колонки
                        for (int col = 1; col <= lastColumn; col++)
                        {
                            var cell = worksheet.Cell(row, col); // проверить с какой кол начинать
                            string stringValue = GetCellValueAsString(cell);
                            values.Add(stringValue);
                        }

                        // Пропускаем пустые строки
                        if (values.Count == 0 || string.IsNullOrEmpty(values[0]))
                            continue;

                        // 1. Наименование (колонка 1)
                        rowData.Наименование = values[0]?.Trim() ?? string.Empty;

                        // 2. Числовые значения (колонки 2-67)
                        rowData.Values = new int[66]; // 66 значений (2-67)
                        for (int i = 0; i < 66; i++)
                        {
                            int index = i + 1; // индекс в values (2-67)
                            if (index < values.Count)
                            {
                                rowData.Values[i] = ParseInt(values[index]);
                            }
                            else
                            {
                                rowData.Values[i] = 0;
                            }
                        }

                        // 3. ФИО начальника караула (колонка 68)
                        rowData.Начкар =  values[67];

                        // 4. Формируем код строки
                        bool isTerritorial = rootpsgName == "Территориальный";
                        int psgId = rootpsg == null ? 11 : rootpsg.Id;
                        string rowId = GenerateRowId(rowData.Наименование, rootpsgName, psgId,isTerritorial);
                        rowData.RowId = rowId;

                        // Добавляем в словарь (если такой ключ уже есть, пропускаем)
                        if (!result.ContainsKey(rowId))
                        {
                            result[rowId] = rowData;
                        }
                        //else
                        //    result[rowId] = rowData;

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при чтении Excel файла: {ex.Message}", ex);
            }

            return result;
        }

        /// <summary>
        /// Определяет, является ли ПСГ территориальным 
        /// </summary>
        private bool IsTerritorialPsg(Psg psg)
        {
            if (psg == null)
                return false;

            // Проверяем по имени
            if (!string.IsNullOrEmpty(psg.Garnizon) &&
                psg.Garnizon.Contains("Территориальный", StringComparison.OrdinalIgnoreCase))
                return true;

            // Проверяем по ID (обычно 11 - территориальный)
            if (psg.Id == 11)
                return true;

            // Проверяем по Parent (если Parent == null, значит корневой)
            if (psg.Parent == null || psg.Parent == 0)
                return true;

            return false;
        }

        /// <summary>
        /// Генерирует код строки по наименованию и корневому ПСГ
        /// </summary>
        private string GenerateRowId(string rowName, string psgName, int rootpsgId, bool isTerritorial)
        {
            var rowType = DetermineRowType(rowName, psgName, isTerritorial); // 1. Определяем тип строки



            // Если это территориальный ПСГ, а имя строки - название ПСГ - то код определён
            if (isTerritorial) // значит это либо 
            {
                Psg psgInTerritorial = FireEquipsPivotRepository.GetPsgByName(rowName);
                bool isTerritItogs = psgInTerritorial == null;
                string psgCode = isTerritItogs ? "0000" : psgInTerritorial.Id.ToString("D4"); 
                return GetTotalFlag(rowType) + psgCode + "0000"+psgCode;
            }
            else
                return get_row_id_rpsg(rowName,rowType, rootpsgId);

        }

        /// <summary>
        /// Определяет тип строки
        /// </summary>
        private RowType DetermineRowType(string rowName, string PsgName, bool isTerritorial)
        {
            if (string.IsNullOrEmpty(rowName))
                return RowType.Regular;

            // Проверяем итоговые строки
            if (rowName.Contains("ГПС", StringComparison.OrdinalIgnoreCase))
                return RowType.TotalGps;
            if (rowName.Contains("другие", StringComparison.OrdinalIgnoreCase) )
                return RowType.TotalOther;
            if (rowName.Contains("ДПО", StringComparison.OrdinalIgnoreCase))
                return RowType.TotalOther; 
            if (rowName.Contains("ЧПО", StringComparison.OrdinalIgnoreCase))
                return RowType.TotalChpo;
            if (rowName.Contains("по ВПО", StringComparison.OrdinalIgnoreCase))
                return RowType.TotalVpo;
            if (rowName.Contains("ФПС", StringComparison.OrdinalIgnoreCase))
                return RowType.TotalFps;
            if (rowName.Contains("АСФ", StringComparison.OrdinalIgnoreCase) && isTerritorial)
                return RowType.TotalASF;
            if (rowName.Contains("по ТПСГ", StringComparison.OrdinalIgnoreCase))
                return RowType.TotalAll;
            // Проверяем, является ли строка итогом "всего"
            if (!string.IsNullOrEmpty(PsgName))
            {
                // Для территориального ПСГ
                if (isTerritorial && rowName.Contains("Территориальный", StringComparison.OrdinalIgnoreCase))
                    return RowType.TotalAll;

                // Для районного ПСГ - если наименование содержит только имя ПСГ
                if (  FireEquipsPivotRepository.rowNameIsPsgName(rowName.Trim()))
                    //rowName.Equals(, StringComparison.OrdinalIgnoreCase))
                    return RowType.TotalAll;
            }

            // Если строка содержит имя ПЧ - обычная
            var pch = FindPchByName(rowName);
            if (pch != null)
                return RowType.Regular;

            // Если содержит название ПСГ, но не является итогом "всего"
            if (!string.IsNullOrEmpty(PsgName) &&
                rowName.Contains(PsgName, StringComparison.OrdinalIgnoreCase))
                return RowType.Regular;

            return RowType.Regular;
        }



        //Если это Районный ПСГ, то это - либо общий итог, либо частный итог, либо ПЧ
        string get_row_id_rpsg(string rowName, RowType type, int rootpsgId) {

            string row_id = "";
            // 4. Получаем признак итоговой строки
            string totalFlag = GetTotalFlag(type);
            string psgCode = rootpsgId.ToString("D4");
            string pchCode = "0000";
            string parentCode = psgCode;
            if (type == RowType.Regular) // Если обычная строка , т.е. строка для ПЧ  - ищем в таблице pchs
            {  
                var pch = FindPchByName(rowName,rootpsgId);
                pchCode = pch.Id.ToString("D4");
                parentCode = pch.Parent.Value.ToString("D4");
            }
            row_id = totalFlag + psgCode + pchCode + parentCode; // Сборка: 2 + 4 + 4 + 4 = 14 символов
            return row_id;      
        }
        // row_id для строк для  ТПСГ
        string get_row_id_tpsg(string rowName, RowType type, int rootpsgId)
        {
            // Если не "Территориальный"
            return  GetTotalFlag(type) + "000000000000";   
        }


        /// <summary>
        /// Получает код ПСГ (4 символа)
        /// </summary>
        private string GetPsgCode(string psgName, int psgId, bool isTerritorial)
        {
            return isTerritorial ? "0000": psgId.ToString("D4");
        }

        /// <summary>
        /// Получает признак итоговой строки (2 символа)
        /// </summary>
        private string GetTotalFlag(RowType type)
        {
                return type switch
                {
                    RowType.Regular => "00",
                    RowType.TotalAll => "01",
                    RowType.TotalGps => "02",
                    RowType.TotalFps => "03",
                    RowType.TotalOther => "04",
                    RowType.TotalVpo => "05",
                    RowType.TotalChpo => "06",
                    RowType.TotalASF => "07",
                    _ => "00"
                };                                   
        }
        

        /// <summary>
        /// Находит ПЧ по наименованию
        /// </summary>
        private Pch FindPchByName(string name)
        {
            
            if (string.IsNullOrEmpty(name))
                return null;

            // Точное совпадение
            var pch = _pchList.FirstOrDefault(p =>
                !string.IsNullOrEmpty(p.Name) &&
                p.Name.TrimEnd('\r', '\n').Equals(name.TrimEnd('\r', '\n'), StringComparison.OrdinalIgnoreCase));


            if (pch != null)
                return pch;
            else
                return null;//***** удалить

            string _name = name.Replace(" ", "");
             pch = _pchList.Where(c => c.Name.Replace(" ", "") == _name).FirstOrDefault();

            if (pch != null)
                return pch;

            // Частичное совпадение (содержит название ПЧ)
            return _pchList.FirstOrDefault(p =>
                !string.IsNullOrEmpty(p.Name) &&
                name.Contains(p.Name, StringComparison.OrdinalIgnoreCase));
        }
        private Pch FindPchByName(string name,int psgId)
        {

            if (string.IsNullOrEmpty(name))
                return null;

            // Точное совпадение
            var pch = _pchList.Where(c=>c.Parent==psgId).FirstOrDefault(p =>
                !string.IsNullOrEmpty(p.Name) &&
                p.Name.TrimEnd('\r', '\n').Equals(name.TrimEnd('\r', '\n'), StringComparison.OrdinalIgnoreCase));


            if (pch != null)
                return pch;

            string _name = name.Replace(" ", "");
            pch = _pchList.Where(c => c.Name.Replace(" ", "") == _name).FirstOrDefault();

            if (pch != null)
                return pch;

            // Частичное совпадение (содержит название ПЧ)
            return _pchList.FirstOrDefault(p =>
                !string.IsNullOrEmpty(p.Name) &&
                name.Contains(p.Name, StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// Генерирует 4-символьный код из строки
        /// </summary>
        private string GenerateCodeFromString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "0000";

            int hash = Math.Abs(input.GetHashCode());
            return (hash % 10000).ToString("D4");
        }

        /// <summary>
        /// Безопасный парсинг int
        /// </summary>
        private int ParseInt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;
            return int.TryParse(value, out int result) ? result : 0;
        }

        /// <summary>
        /// Получение строкового значения ячейки
        /// </summary>
        private string GetCellValueAsString(IXLCell cell)
        {
            if (cell == null || cell.IsEmpty())
                return string.Empty;

            try
            {
                var dataType = cell.DataType;
                switch (dataType)
                {
                    case XLDataType.Text:
                        return cell.GetString()?.Trim() ?? string.Empty;
                    case XLDataType.Number:
                        return cell.GetDouble().ToString(System.Globalization.CultureInfo.InvariantCulture);
                    case XLDataType.Boolean:
                        return cell.GetBoolean().ToString();
                    case XLDataType.DateTime:
                        return cell.GetDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                    case XLDataType.TimeSpan:
                        return cell.GetTimeSpan().ToString();
                    default:
                        return cell.GetString()?.Trim() ?? string.Empty;
                }
            }
            catch
            {
                try
                {
                    return cell.GetString()?.Trim() ?? string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
    }

    /// <summary>
    /// Данные строки из Excel
    /// </summary>
    public class StroevkaRowData
    {
        /// <summary>
        /// Код строки (14 символов)
        /// </summary>
        public string RowId { get; set; }

        /// <summary>
        /// Наименование (ПЧ, ПСГ или итог)
        /// </summary>
        public string Наименование { get; set; }

        /// <summary>
        /// Массив числовых значений (колонки 2-67)
        /// </summary>
        public int[] Values { get; set; }

        /// <summary>
        /// ФИО начальника караула (колонка 68)
        /// </summary>
        public string Начкар { get; set; }
    }


}