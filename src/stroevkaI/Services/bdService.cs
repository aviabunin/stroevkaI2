using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageI.ModelsStroevkaMySql;
using System.Reflection;
using stroevkaI.Services.Tests;

namespace stroevkaI.Services
{
    /// <summary>
    /// Сравнение с psgdata
    /// </summary>
    public  class bdService
    {
        static List<Psgdatum> psgdataList = new List<Psgdatum>();  //текущее состояние psgdata
        static List<FirePsgStat> fpsList = new List<FirePsgStat>();  //текущее состояние psgdata
        static List<StroevkaExcelRow> psgReportList = new List<StroevkaExcelRow>();  //список строк отчёта в старой проге

        static DataGridView _grid;
        static int rowCount = 0;
         
  
        public static string psgdataCompare(string psgName, DataGridView grid)
        {
            // Здесь будет логика сравнения с действующей БД
            List<Pch> pchList = FireEquipsPivotRepository.getPchList();
            List<Psg> psgList = FireEquipsPivotRepository.getPsgList();
            var excelReader = new ExcelReaderService(pchList,psgList);
            Psg psg = FireEquipsPivotRepository.GetPsgByName(psgName);
            _grid = grid;
            rowCount= grid.Rows.Cast<DataGridViewRow>().Count(row => row.Visible);

            // Чтение файла
            string dateStr = DateTime.Now.AddDays(-1).ToString("dd-MM-yy");
            string excelFilePath = @"D:\stroevka_reports\" + psgName + "_" + dateStr + ".xlsx";
            Dictionary<string, StroevkaRowData> data = excelReader.ReadExcelFile(excelFilePath, psg, rowCount);//(psgName);//resultMsg
                                                                                              // 4. Выводим результаты
            foreach (var item in data)
            {
                Console.WriteLine($"RowId: {item.Key}");
                Console.WriteLine($"Наименование: {item.Value.Наименование}");
                Console.WriteLine($"Начкар: {item.Value.Начкар}");
                Console.WriteLine($"Значения: {string.Join(", ", item.Value.Values.Take(10))}...");
                Console.WriteLine();
            }
            // 5. Сравниваем с гридом
            var gridComparar = new GridComparer(_grid);
            var results = gridComparar.CompareAll(data);

            // 7. Показываем результаты в отдельной форме
            var resultsForm = new stroevkaI.Forms.CompareResultsForm();
            resultsForm.ShowResults(results);
            resultsForm.ShowDialog();


            string rezstr = "";// rez ? "Успешно" : "Ошибка"; TODO
            return rezstr;


            //if (errors.Count > 0)
            //{
            //    string allErrors = string.Join(Environment.NewLine, errors.Take(10)); // Показываем первые 10 ошибок
            //    MessageBox.Show(allErrors);
            //    return $"Найдено ошибок: {errors.Count}. Первые 10: {Environment.NewLine}{allErrors}";
            //}

            //return $"Проверено {cnt} записей. Ошибок не найдено.";
        }

    }
}
