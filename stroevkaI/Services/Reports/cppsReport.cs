using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClosedXML.Excel;
using System.IO;
using System.Diagnostics;
using StorageI.ModelsStroevkaMySql;


namespace stroevkaI.Services.Reports;

class cppsReport
{
    #region Параметры отчёта

    // эти 2 параметра (караул и начГУ) будем передавать через параметры отчёта, пока здесь
    static int номер_караула = 3;  // Пока просто взял из текущей БД
    static string начГУ = "Клинг А.В.";// 

    static string dirTemplatePath = Directory.GetCurrentDirectory() + @"\отчеты\";
    static string имяШаблона = dirTemplatePath + @"шаблоны\psgTemplates\Шаблон_Территориальный.xlsx";

    static string fileName = "Территориальный строевка от "+DateTime.Now.ToString("dd-MM-yy")+".xlsx";
    // имя сформированного отчёта с приформированной датой и пишется в каталог days_stroevka
    static string dirOutPath = dirTemplatePath + @"days_stroevka\" + fileName;


    static string цппсTemplatePath = dirTemplatePath + @"шаблоны\строевка ТПСГ шаблон.xlsx";

    static Dictionary<string, int> rowNumbers = new Dictionary<string, int>(){
        {"Петрозаводский" ,21},
        {"Прионежский"    ,22},
        {"Беломорский"    ,23},
        {"Калевальский"   ,24},
        {"Кемский"        ,25},
        {"Кондопожский"   ,26},
        {"Костомукшский"  ,27},
        {"Лахденпохский"  ,28},
        {"Лоухский"       ,29},
        {"Медвежьегорский",30},
        {"Муезерский"     ,31},
        {"Олонецкий"      ,32},
        {"Питкярантский"  ,33},
        {"Пряжинский"     ,34},
        {"Пудожский"      ,35},
        {"Сегежский"      ,36},
        {"Сортавальский"  ,37},
        {"Суоярвский",     38},
        {"Итого по ГПС",39},
        {"в т.ч. по ФПС",40},
        {"по ВПО"              ,41},
        {"по  ДПО",42},
        {"по ЧПО"             ,43},
        {"Территориальный"                          ,44},
        {"АСФ а/п 'Петрозаводск'"             ,45}
    };


    static string[] months = { "января", "февраля", "марта", "апреля", "мая", "июня", "июля", "августа", "сентября", "октября", "ноября", "декабря" };

    #endregion

    static List<FirePsgStat> reportData; // Список строк отчёта ЦППС с номерами строк в шаблоне Excel 
    
    // шаблон можно посмотреть предварительно
             // а затем при входе в программу посмотреть список - и как формируется каждая строка

    /// <summary>
    /// Печать отчёта ЦППС по списку элементов DataGrid из основной формы.
    ///    Номера строк задаются в словаре rowNumbers, 
    ///      каждая строка из списка ищется по названию гарнизона 
    ///      колонки выводятся , начиная с третьей , в которой задано АЦ по последнюю - начкар
    ///      заполняются только те, у которых найдено значение ключа в словаре. Итоговые видимо пока не найдутся, потом сделаем
    /// </summary>
    /// <param name="_reportData"></param>


    static public void ModifyDataRange(List<FirePsgStat> _reportData)
    {
        reportData = _reportData;
        DateTime date = DateTime.Now;
        string formattedDate = string.Format(@"""{0:dd}""  {1}  {2}  года", date, months[date.Month - 1], date.Year);

        using (var workbook = new XLWorkbook(цппсTemplatePath))      // 1. Открываем существующую книгу
        {
            var sheet = workbook.Worksheet(1);                       // 2. Получаем нужный лист (по индексу или имени)
            int номерСтрокиExcel;
            int cnt = 0;
            List<string> lst = new List<string>();
            // цикл формирования строк в окончательном отчёте
            foreach (FirePsgStat row in reportData)
            {
                string rowName = row.Пч.Trim();
                if (!rowNumbers.ContainsKey(rowName.Trim()))
                    continue;
                lst.Add(rowName);
                cnt++;
             
                номерСтрокиExcel = rowNumbers[rowName];//Определяем номер строки
                formPSGitogRow(sheet, row, номерСтрокиExcel++);// пишем в копию шаблона данные для сохранения под новым именем
            }
            #region Здесь будет формирование разных полей , пока убрал. Можно посмотреть в monitor_stroevka. но лучше я потом напишу, когда это отладишь

            #endregion

            #region Сохранить и показать отчёт оператору
            try
            {
                workbook.SaveAs(dirOutPath);            // 5. Сохраняем изменения
            }
            catch (Exception e)
            {}
            if (File.Exists(dirOutPath))
            {
                // Открывает файл в приложении по умолчанию для .xlsx файлов
                Process.Start(new ProcessStartInfo(dirOutPath) { UseShellExecute = true });
            }
            #endregion
        }
    }


    /// <summary>
    /// Копирует значения колонок из грида в Excel файл
    /// </summary>
    /// <param name="sheet"> строка excel</param>
    /// <param name="psgdata_row">строка из переданного списка грида</param>
    /// <param name="iRow">номер строки в Excel</param>
    static private void formPSGitogRow(IXLWorksheet sheet, FirePsgStat psgdata_row, int iRow) //перенос строки из таблицы в excel
    {

        for (int col = 2; col <= 68; col++)             //колонки , начиная с АЦ по начкар
        {
            var cell = sheet.Cell(iRow, col);
            cell.Value = FirePsgStatHelper.GetColumnValue(psgdata_row, col-2);
        }
    }

    static public void myReport(DataGridView dgv)
    {
        Dictionary<string, int> Garnizons = new Dictionary<string, int>(){
        {"Петрозаводский" ,21},
        {"Прионежский"    ,22},
        {"Беломорский"    ,23},
        {"Калевальский"   ,24},
        {"Кемский"        ,25},
        {"Кондопожский"   ,26},
        {"Костомукшский"  ,27},
        {"Лахденпохский"  ,28},
        {"Лоухский"       ,29},
        {"Медвежьегорский",30},
        {"Муезерский"     ,31},
        {"Олонецкий"      ,32},
        {"Питкярантский"  ,33},
        {"Пряжинский"     ,34},
        {"Пудожский"      ,35},
        {"Сегежский"      ,36},
        {"Сортавальский"  ,37},
        {"Суоярвский",     38},
        {"Итого по ГПС",   39},
        {"в т.ч. по ФПС" ,        40},
        {"по ВПО",41},
        {"по  ДПО",42},
        {"по ЧПО" ,43},
        {"Территориальный"                          ,44},
        {"АСФ а/п Петрозаводск"             ,45}
    };
        
        OpenShablon(Garnizons,dgv);
    }

    static void  OpenShablon( Dictionary<string,int> Garn, DataGridView dgv2)
    {

        string newFileName = dirOutPath;// @"D:\CopyZ5\" + "2_Отчёт для печати__ " + DateTime.Now.ToString("dd.MM.yy") + " строевка ТПСГ.xlsx";
        string docPath = имяШаблона;


        using (var workbook = new XLWorkbook(docPath))
        {

            //int rowIndex = 0;
            var sheet = workbook.Worksheet(1);
            FirePsgStat psgdata_row2;
            //foreach (var pair in Garn)
            //{
            //rowIndex = pair.Value;

            string ErrorGarn = "";

            foreach (DataGridViewRow row in dgv2.Rows)
            {
                object pchValue = row.Cells[0].Value;// Определяем имя строки в словаре
                 string pchName = pchValue?.ToString().Trim();
                if (string.IsNullOrEmpty(pchName)) continue;  // это ds делает аккуратно

                // Определяем строку Excel по словарю
                if (!Garn.TryGetValue(pchName, out int NumberOfexcelRow))  // здесь твой словарь поставь
                    continue; // или можно пропустить, или записать в лог
                for (int i = 2; i < row.Cells.Count; i++)
                {
                    var cellValue = row.Cells[i].Value.ToString();// Достаём значение колонки АЦ_br из DGV - проверь

                    // Пробуем преобразовать в число
                    if (double.TryParse(cellValue, out double numericValue))
                    {
                        sheet.Cell(NumberOfexcelRow, i).SetValue(numericValue); // Записываем как число
                        sheet.Cell(NumberOfexcelRow, i).Style.NumberFormat.SetFormat("0");

                        sheet.Cell(NumberOfexcelRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        sheet.Cell(NumberOfexcelRow, i).Style.Border.OutsideBorderColor = XLColor.Black;

                    }
                    else
                    {
                        // Если не число - записываем как текст
                        sheet.Cell(NumberOfexcelRow, i).SetValue(cellValue);


                        sheet.Cell(NumberOfexcelRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        sheet.Cell(NumberOfexcelRow, i).Style.Border.OutsideBorderColor = XLColor.Black;
                    }


                    //try
                    //{
                    //   sheet.Cell(NumberOfexcelRow, i).Value = cellValue; // поставь номер колонки 
                    //   sheet.Cell(NumberOfexcelRow, i).Style.NumberFormat.SetFormat("0");
                    //    break;
                    //}
                    //catch (Exception errmsg)
                    //{
                    //     ErrorGarn = row.Cells[0].ToString() + "строка: " + i + "   /n errmsg: " + errmsg + "   /n Гарнизон: " + pchName;
                    //     MessageBox.Show(" Ошибка в  " + ErrorGarn);
                    //     throw;
                    //}
                
                              
                }
                //break;
            }


            //sheet.Cell(21, 2).Value = dgv2.Rows[10].Cells[2].Value.ToString();

            //};

            PrintDetails(sheet);

            workbook.SaveAs(newFileName);

            if (File.Exists(newFileName))
            {
                // Открывает файл в приложении по умолчанию для .xlsx файлов
                Process.Start(new ProcessStartInfo(newFileName) { UseShellExecute = true });
            }

        }
    }

    static void PrintDetails(IXLWorksheet currentSheet)
    {
        string[] months = { "января", "февраля", "марта", "апреля", "мая", "июня",
                "июля", "августа", "сентября", "октября", "ноября", "декабря" };

        DateTime dt = DateTime.Now;
        string dateTime = $"\"{dt.Day}\" {months[dt.Month - 1]} {dt.Year} года.";

        currentSheet.Cell("BK7").Value = dateTime;

        List<Contact> lstOfContacts =  StorageI.ModelsStroevkaMySql.FireEquipsPivotRepository.context.Contacts.Where(c => (c.Karaul == 3) && (c.SubdivisionId == 339)).ToList();
        //StorageI.ModelsStroevkaMySql.Contacts.Where(c => (c.karaul == 3) && (c.subdivision_id == 339)).ToList();

        // Начальник дежурной смены
        Contact начДежСмены = lstOfContacts.Where(c => c.Post == "Начальник дежурной смены").FirstOrDefault();
        currentSheet.Cell("B51").Value = "Начальник дежурной смены : " + начДежСмены.Fio;

        // Старший помощник : слева
        Contact старПом = lstOfContacts.Where(c => c.Post == "Старший помошник").FirstOrDefault();
        currentSheet.Cell("B53").Value = "Старший помощник : " + старПом.Fio;

        // "Диспетчер ПСГ слева
        Contact диспетчерПСГ = lstOfContacts.Where(c => c.Post == "Диспетчер ПСГ").FirstOrDefault();
        currentSheet.Cell("B58").Value = "Диспетчер ПСГ : " + диспетчерПСГ.Fio;

        // "Оперативный дежурный ПСГ СПРАВА     
        currentSheet.Cell("BJ51").Value = "_______________ " + начДежСмены.Fio;// тот же сотрудник что и слева выше
        currentSheet.Cell("BJ53").Value = dateTime; // Дата

        // "Диспетчер ПСГ СПРАВА
        currentSheet.Cell("BJ58").Value = "_______________ " + диспетчерПСГ.Fio;
        currentSheet.Cell("BJ60").Value = dateTime; // Дата           

    }

}


