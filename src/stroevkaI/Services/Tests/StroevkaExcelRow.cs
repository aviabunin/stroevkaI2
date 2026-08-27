// Модель данных для хранения одной строки из Excel
public class StroevkaExcelRow
{
    // Идентификаторы
    public int Id { get; set; }             // Id
    public string Пч { get; set; }          // ПЧ (наименование подразделения) - колонка 1
    public string Datafilled { get; set; }  // Datafilled

    // Поля Excel (колонки 2-67) - все числовые
    public int AcBr { get; set; }           // 2  - АЦ
    public int AclBr { get; set; }          // 3  - АЦЛ
    public int АвBr { get; set; }           // 4  - АВ
    public int АсаАппАсмBr { get; set; }    // 5  - АСА/АПП
    public int ПнсBr { get; set; }          // 6  - ПНС
    public int AlBr { get; set; }           // 7  - АЛ
    public int КпBr { get; set; }           // 8  - КП
    public int АрBr { get; set; }           // 9  - АР
    public int АсмпПсаBr { get; set; }      // 10 - АСМП
    public int АшBr { get; set; }           // 11 - АШ
    public int УксАбгBr { get; set; }       // 12 - УКС/АБГ
    public int fireTrainColumn { get; set; }// 13 - Пож. поезд / корабль

    public int AcRezerv { get; set; }           // 14 - АЦ резерв
    public int aclrezerv { get; set; }          // 15 - АЦЛ резерв
    public int АнрRezerv { get; set; }          // 16 - АНР резерв
    public int АvRezerv { get; set; }           // 17 - АВ резерв
    public int АсаАппАсмRezerv { get; set; }    // 18 - АСА/АПП резерв
    public int ПнсRezerv { get; set; }          // 19 - ПНС резерв
    public int AlRezerv { get; set; }           // 20 - АЛ резерв
    public int КпRezerv { get; set; }           // 21 - КП резерв
    public int АрRezerv { get; set; }           // 22 - АР резерв
    public int АсмпПсаRezerv { get; set; }      // 23 - АСМП резерв
    public int АшRezerv { get; set; }           // 24 - АШ резерв
    public int АсоRezerv { get; set; }          // 25 - АСО резерв
    public int УксАбгRezerv { get; set; }       // 26 - УКС/АБГ резерв
    public int АсмрхBr { get; set; }            // 27 - АСМРХ
    public int АвсRezerv { get; set; }          // 28 - АВС

    public int Tofirst { get; set; }            // 29 - ТО-1
    public int Totow { get; set; }              // 30 - ТО-2
    public int РемонтОсновной { get; set; }     // 31 - Ремонт основная
    public int РемонтСпециальной { get; set; }  // 32 - Ремонт специальная
    public int ПожПоездКорабльRezerv { get; set; } // 33 - Пож.корабль не в расч
    public int ПлавСредства { get; set; }       // 34 - Плав.средство
    public int Болотоходы { get; set; }         // 35 - Снегоход \\ болотоход
    public int Мотопомпы { get; set; }          // 36 - Мотопомпа
    public int Прочее { get; set; }             // 37 - Прочие

    public int SizodBr { get; set; }            // 38 - сизод расч
    public int SizodRezerv { get; set; }        // 39 - сизод рез
    public int КостюмыЛ1Таск { get; set; }      // 40 - Л1(ОЗК)/ТАСК
    public int КостюмыТок { get; set; }         // 41 - ТОК
    public int КостюмыДругие { get; set; }      // 42 - другие
    public int ГасиРасчёт { get; set; }         // 43 - гаси расч
    public int ГасиРезерв { get; set; }         // 44 - гаси рез

    public int ПоСписку { get; set; }           // 45 - По списку
    public int Налицо { get; set; }             // 46 - Налицо
    public int Всего { get; set; }              // 47 - Всего
    public int Резерв { get; set; }             // 48 - Резерв ЛС
    public int Нк { get; set; }                 // 49 - НК
    public int Диспетчер { get; set; }          // 50 - диспетчер
    public int Пнк { get; set; }                // 51 - ПНК
    public int Ко { get; set; }                 // 52 - КО
    public int Водитель { get; set; }           // 53 - Водитель
    public int Пожарный { get; set; }           // 54 - Пожарный
    public int Гдзс { get; set; }               // 55 - ГДЗС
    public int ВсегоОтс { get; set; }           // 56 - Всего отсутствует
    public int Отпуск { get; set; }             // 57 - Отпуск
    public int ПоБольничному { get; set; }      // 58 - По болезни
    public int Командировка { get; set; }       // 59 - Командировка
    public int Некомплект { get; set; }         // 60 - Некомплект
    public int ПрочиеОтс { get; set; }          // 61 - Прочие

    public int ПенаРасчёт { get; set; }         // 62 - Пена расч
    public int ПорошокРасчёт { get; set; }      // 63 - Порош расч
    public int ПенаРезерв { get; set; }         // 64 - Пена рез
    public int ПорошокРезерв { get; set; }      // 65 - Порош рез
    public int Дт { get; set; }                 // 66 - ДТ
    public int Бензин { get; set; }             // 67 - Бензин

    // Поле Excel (колонка 68) - ФИО начальника караула
    public string Начкар { get; set; }          // 68 - Начкар

    // Словарь для быстрого доступа по индексу (заполняется при парсинге)
    public Dictionary<int, string> ColumnValues { get; set; } = new Dictionary<int, string>();

    // Для удобства - массив значений
    public string[] Values { get; set; }

    // Служебные поля
    public int RowNum { get; set; }
    public string RowId { get; set; }
    public RowType Type { get; set; }
    public string RowTypeDescription { get; set; }
    public int? PchId { get; set; }
    public int? PsgId { get; set; }
    public int? PsgDatumId { get; set; }
    public int? ParentId { get; set; }

    /// <summary>
    /// Заполняет словарь ColumnValues значениями из свойств
    /// </summary>
    public void FillColumnValues()
    {
        ColumnValues.Clear();

        var columnMapping = new Dictionary<int, Func<string>>
        {
            // 2-13
            [2] = () => AcBr.ToString(),
            [3] = () => AclBr.ToString(),
            [4] = () => АвBr.ToString(),
            [5] = () => АсаАппАсмBr.ToString(),
            [6] = () => ПнсBr.ToString(),
            [7] = () => AlBr.ToString(),
            [8] = () => КпBr.ToString(),
            [9] = () => АрBr.ToString(),
            [10] = () => АсмпПсаBr.ToString(),
            [11] = () => АшBr.ToString(),
            [12] = () => УксАбгBr.ToString(),
            [13] = () => fireTrainColumn.ToString(),

            // 14-28
            [14] = () => AcRezerv.ToString(),
            [15] = () => aclrezerv.ToString(),
            [16] = () => АнрRezerv.ToString(),
            [17] = () => АvRezerv.ToString(),
            [18] = () => АсаАппАсмRezerv.ToString(),
            [19] = () => ПнсRezerv.ToString(),
            [20] = () => AlRezerv.ToString(),
            [21] = () => КпRezerv.ToString(),
            [22] = () => АрRezerv.ToString(),
            [23] = () => АсмпПсаRezerv.ToString(),
            [24] = () => АшRezerv.ToString(),
            [25] = () => АсоRezerv.ToString(),
            [26] = () => УксАбгRezerv.ToString(),
            [27] = () => АсмрхBr.ToString(),
            [28] = () => АвсRezerv.ToString(),

            // 29-37
            [29] = () => Tofirst.ToString(),
            [30] = () => Totow.ToString(),
            [31] = () => РемонтОсновной.ToString(),
            [32] = () => РемонтСпециальной.ToString(),
            [33] = () => ПожПоездКорабльRezerv.ToString(),
            [34] = () => ПлавСредства.ToString(),
            [35] = () => Болотоходы.ToString(),
            [36] = () => Мотопомпы.ToString(),
            [37] = () => Прочее.ToString(),

            // 38-44
            [38] = () => SizodBr.ToString(),
            [39] = () => SizodRezerv.ToString(),
            [40] = () => КостюмыЛ1Таск.ToString(),
            [41] = () => КостюмыТок.ToString(),
            [42] = () => КостюмыДругие.ToString(),
            [43] = () => ГасиРасчёт.ToString(),
            [44] = () => ГасиРезерв.ToString(),

            // 45-61
            [45] = () => ПоСписку.ToString(),
            [46] = () => Налицо.ToString(),
            [47] = () => Всего.ToString(),
            [48] = () => Резерв.ToString(),
            [49] = () => Нк.ToString(),
            [50] = () => Диспетчер.ToString(),
            [51] = () => Пнк.ToString(),
            [52] = () => Ко.ToString(),
            [53] = () => Водитель.ToString(),
            [54] = () => Пожарный.ToString(),
            [55] = () => Гдзс.ToString(),
            [56] = () => ВсегоОтс.ToString(),
            [57] = () => Отпуск.ToString(),
            [58] = () => ПоБольничному.ToString(),
            [59] = () => Командировка.ToString(),
            [60] = () => Некомплект.ToString(),
            [61] = () => ПрочиеОтс.ToString(),

            // 62-67
            [62] = () => ПенаРасчёт.ToString(),
            [63] = () => ПорошокРасчёт.ToString(),
            [64] = () => ПенаРезерв.ToString(),
            [65] = () => ПорошокРезерв.ToString(),
            [66] = () => Дт.ToString(),
            [67] = () => Бензин.ToString(),
        };

        foreach (var kvp in columnMapping)
        {
            ColumnValues[kvp.Key] = kvp.Value();
        }
    }
}

/// <summary>
/// Тип строки в отчете
/// </summary>
public enum RowType
{
    Regular,    // Обычная строка (подразделение)
    TotalGps,   // Итог по ГПС
    TotalOther, // Итог по "другие"
    TotalChpo,  // Итог по ЧПО
    TotalVpo,   // Итог по ВПО
    TotalFps,   // Итог по ФПС
    TotalAll    // Итог "всего"
}
// Класс для хранения всего отчета
public class StroevkaExcelReport
{
    public List<StroevkaExcelRow> Rows { get; set; } = new List<StroevkaExcelRow>();
    public DateTime ExportDate { get; set; }
    public string FileName { get; set; }
    public int TotalRows { get; set; }

    // Дополнительные метаданные для районного ПСГ
    public string GarrisonName { get; set; }           // Название гарнизона
    public DateTime ReportDateTime { get; set; }       // Дата и время отчета
    public string ReportNumber { get; set; }           // Номер отчета
    public string ChiefName { get; set; }              // Начальник ПСГ
    public string DutyOfficer { get; set; }            // Оперативный дежурный
    public string Dispatcher { get; set; }             // Диспетчер
}
