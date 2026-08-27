using StorageI.ModelsStroevkaMySql;
using ClosedXML.Excel;
using System.Diagnostics;

namespace stroevkaI.Services.Reports
{
    public  class psgReport
    {
        public static  void printLocal(string PsgName, DataGridView EquipmentDataGridView)
        {
            //имя шаблона = каталогПроги\отчеты\шаблоны\psgTemplates\"Шаблон_Беломорский.xlsx"
            string dirTemplatePath = Directory.GetCurrentDirectory() + @"\отчеты\";
            string имяШаблона = dirTemplatePath + @"шаблоны\psgTemplates\Шаблон_" + PsgName.Trim() + @".xlsx";


            //имя дневного отчёта = каталогПроги\отчеты\days_stroevka\Беломорский строевка 27-06-26.xlsx"
            string fileName = PsgName+" строевка от " +DateTime.Now.ToString("dd-MM-yy") + ".xlsx";
            // имя сформированного отчёта с приформированной датой и пишется в каталог days_stroevka
             string newFileName = dirTemplatePath + @"days_stroevka\" + fileName;

            using (var workbook = new XLWorkbook(имяШаблона))
            {
                var sheet = workbook.Worksheet(1);
                int n_row = 13;

                foreach (DataGridViewRow row in EquipmentDataGridView.Rows)
                {
                    object pchValue = row.Cells[0].Value;
                    var nameCell = sheet.Cell(n_row, 1);
                    nameCell.Clear();
                    nameCell.SetValue(pchValue?.ToString().Trim());
                    nameCell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    nameCell.Style.Border.SetOutsideBorderColor(XLColor.Black);

                    for (int i = 2; i < row.Cells.Count; i++)
                    {
                        var cellValue = row.Cells[i].Value?.ToString() ?? "";
                        var cell = sheet.Cell(n_row, i);

                        //Очищаем ячейку перед записью
                        cell.Clear();

                        if (double.TryParse(cellValue, out double numericValue))
                        {
                            cell.SetValue(numericValue);
                            cell.Style.NumberFormat.SetFormat("0");
                        }
                        else
                        {
                            cell.SetValue(cellValue);
                        }

                        //Применяем границы для всех ячеек
                        cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        cell.Style.Border.SetOutsideBorderColor(XLColor.Black);
                    }
                    n_row++;
                }

                PrintDetails2(sheet, n_row + 3, 31, PsgName);
                workbook.SaveAs(newFileName);
                if (File.Exists(newFileName))
                {
                    //Открывает файл в приложении по умолчанию для.xlsx файлов
                    Process.Start(new ProcessStartInfo(newFileName) { UseShellExecute = true });
                }
            }
        }


        static void PrintDetails2(IXLWorksheet currentSheet, int n_row, int n_col, string psgName)
        {
            DateTime karaul1date = new DateTime(2018, 07, 31);
            int караул = ((DateTime.Now.AddHours(-8).Date - karaul1date).Days) % 4 + 1;

            string[] months = { "января", "февраля", "марта", "апреля", "мая", "июня",
                           "июля", "августа", "сентября", "октября", "ноября", "декабря" };

            DateTime dt = DateTime.Now;
            string dateTimeStroevka = $"на {dt.Hour} час {dt.Minute} мин {dt.Day:D2}.{dt.Month:D2}.{dt.Year} года. №";
            string dateTime = $"\"{dt.Day}\" {months[dt.Month - 1]} {dt.Year} года.";


            //List<Contact> lstOfContacts = StorageI.ModelsStroevkaMySql.FireEquipsPivotRepository.context.Contacts.Where(c => (c.Karaul == 3) && (c.SubdivisionId == 339)).ToList();
            //StorageI.ModelsStroevkaMySql.Contacts.Where(c => (c.karaul == 3) && (c.subdivision_id == 339)).ToList();

            if (psgName == "Территориальный")
            {
                MessageBox.Show("Выберите местный гарнизон");
                return;
                List<Contact> lstOfContacts = StorageI.ModelsStroevkaMySql.FireEquipsPivotRepository.context.Contacts.Where(c => (c.Karaul == караул) && (c.SubdivisionId == 339)).ToList();
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
            else
            {

                currentSheet.Cell("СтроевкаДата").Value = dateTimeStroevka;
                currentSheet.Cell("РайонПСГ").Value = psgName.Substring(0, psgName.Length - 2) + "ого пожарно-спасательного гарнизона";


                List<Contact> lstOfContacts2 = StorageI.ModelsStroevkaMySql.FireEquipsPivotRepository.GetContactsByPsg(psgName, караул);

                // Начальник дежурной смены
                Contact начместПСГ = lstOfContacts2.Where(c => c.Post == "Начальник местного ПСГ").FirstOrDefault();
                currentSheet.Cell(n_row, 31).Value = "Начальник местного ПСГ : " + начместПСГ.Fio;
                currentSheet.Cell(n_row + 2, 31).Value = dateTime; // Дата

                // Старший помощник : слева
                Contact ОдПСГ = lstOfContacts2.Where(c => c.Post == "Оперативный дежурный по гарнизону").FirstOrDefault();
                currentSheet.Cell(n_row + 4, 31).Value = "Оперативный дежурный";
                currentSheet.Cell(n_row + 5, 31).Value = "Старший помощник : " + ОдПСГ.Fio;
                currentSheet.Cell(n_row + 7, 31).Value = dateTime; // Дата

                // "Диспетчер ПСГ слева
                Contact диспетчерПСГ = lstOfContacts2.Where(c => c.Post == "Диспетчер ПСГ").FirstOrDefault();
                currentSheet.Cell(n_row + 9, 31).Value = "Диспетчер пожарно-спасательного гарнизона : " + диспетчерПСГ.Fio;
                currentSheet.Cell(n_row + 11, 31).Value = dateTime; // Дата

            }


            //// "Оперативный дежурный ПСГ СПРАВА     
            //currentSheet.Cell("BJ51").Value = "_______________ " + начДежСмены.Fio;// тот же сотрудник что и слева выше
            //currentSheet.Cell("BJ53").Value = dateTime; // Дата

            //// "Диспетчер ПСГ СПРАВА
            //currentSheet.Cell("BJ58").Value = "_______________ " + диспетчерПСГ.Fio;
            //currentSheet.Cell("BJ60").Value = dateTime; // Дата           

        }

    }
}
