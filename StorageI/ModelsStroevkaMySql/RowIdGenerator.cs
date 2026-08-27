using System;
using System.Collections.Generic;
using System.Linq;

namespace StorageI.ModelsStroevkaMySql
{
    public static class RowIdGenerator
    {
        /// <summary>
        /// Генерирует RowId для ПЧ (неитоговая строка)
        /// Формат: 00 + КодПСГ(4) + КодПЧ(4) + КодПСГ(4) = 14 символов
        /// </summary>
        public static string GeneratePchRowId(int pchId, int? psgId)
        {
            string psgCode = GetPsgCode(psgId);
            string pchCode = pchId.ToString("D4");
            // Для ПЧ код родителя = код ПСГ
            string parentCode = psgCode;

            return $"00{psgCode}{pchCode}{parentCode}";
        }

        /// <summary>
        /// Генерирует RowId для итоговой строки из таблицы PsgTotalRow
        /// </summary>
        public static string GenerateTotalRowId(int psgId, string categoryType)
        {
            string psgCode = GetPsgCode(psgId);
            string totalFlag = GetTotalFlag(categoryType);

            // Для итоговых строк код родителя = код территориального ПСГ (0011)
            // Для территориального ПСГ (psgId=11) родитель = 0000
            string parentCode = psgId == 11 ? "0000" : "0011";

            return $"{totalFlag}{psgCode}0000{parentCode}";
        }

        /// <summary>
        /// Получает код ПСГ (4 символа)
        /// </summary>
        private static string GetPsgCode(int? psgId)
        {
            if (psgId == null || psgId == 11)
                return "0000";

            return psgId.Value.ToString("D4");
        }

        /// <summary>
        /// Получает признак итоговой строки на основе категории
        /// </summary>
        private static string GetTotalFlag(string categoryType)
        {
            return categoryType?.ToLower() switch
            {
                "main" => "01",
                "gps" => "02",
                "fps" => "03",
                "other" => "04",
                "vpo" => "05",
                "chpo" => "06",
                "asf" => "07",
                _ => "01"
            };
        }

        /// <summary>
        /// Получает категорию по отображаемому имени
        /// </summary>
        public static string GetCategoryTypeByDisplay(string display)
        {
            if (string.IsNullOrEmpty(display))
                return "main";

            if (display.Contains("всего") || display == "Территориальный")
                return "main";
            if (display.Contains("ГПС"))
                return "gps";
            if (display.Contains("ФПС"))
                return "fps";
            if (display.Contains("другие") || display.Contains("ДПО"))
                return "other";
            if (display.Contains("ВПО"))
                return "vpo";
            if (display.Contains("ЧПО"))
                return "chpo";
            if (display.Contains("АСФ"))
                return "asf";

            return "main";
        }
    }
}
