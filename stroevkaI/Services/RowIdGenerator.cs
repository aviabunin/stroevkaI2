using System;
using System.Collections.Generic;
using System.Linq;

namespace StorageI.ModelsStroevkaMySql
{
    public static class RowIdGenerator
    {
        // Справочник кодов ПЧ (автоматически формируется из БД)
        private static readonly Dictionary<string, string> PchCodeMap = new()
        {
            { "ПЧ-1", "0002" },
            { "ПЧ-12 (Сегежа)", "0009" }
            // Остальные будут добавлены динамически из БД
        };

        /// <summary>
        /// Формирует уникальный ID строки на основе объекта FirePsgStat
        /// </summary>
        /// <param name="stat">Объект статистики</param>
        /// <param name="psgList">Список ПСГ из таблицы Psg</param>
        /// <param name="pchCodeMap">Справочник кодов ПЧ (из БД)</param>
        /// <returns>12-символьный уникальный ID</returns>
        public static string GenerateRowId(
            FirePsgStat stat,
            IEnumerable<Psg> psgList,
            Dictionary<string, string> pchCodeMap = null)
        {
            if (stat == null)
                throw new ArgumentNullException(nameof(stat));

            if (psgList == null)
                throw new ArgumentNullException(nameof(psgList));

            // 1. Признак итоговой строки (2 символа)
            string totalFlag = GetTotalFlag(stat.Isitog, stat.Псг, stat.Пч);

            // 2. Код ПСГ (4 символа) - берется из таблицы Psg
            string psgCode = GetPsgCode(stat.Псг, psgList);

            // 3. Код ПЧ (4 символа)
            string pchCode = GetPchCode(stat.Пч, stat.Isitog, pchCodeMap);

            // 4. Уровень/родитель (2 символа) - на основе Parent и Norder
            string levelCode = GetLevelCode(stat.Parent, (int)stat.Norder);

            // Сборка: 2 + 4 + 4 + 2 = 12 символов
            return totalFlag + psgCode + pchCode + levelCode;
        }

        /// <summary>
        /// Формирует уникальный ID для строки ПСГ (для итоговых строк)
        /// </summary>
        public static string GenerateRowIdForPsg(
            FirePsgStat stat,
            Psg psg,
            Dictionary<string, string> pchCodeMap = null)
        {
            if (stat == null)
                throw new ArgumentNullException(nameof(stat));

            if (psg == null)
                throw new ArgumentNullException(nameof(psg));

            // 1. Признак итоговой строки (2 символа)
            string totalFlag = GetTotalFlag(stat.Isitog, stat.Псг, stat.Пч);

            // 2. Код ПСГ (4 символа)
            string psgCode = GetPsgCodeFromPsg(psg);

            // 3. Код ПЧ (4 символа) - для итоговых = 0000
            string pchCode = "0000";

            // 4. Уровень (2 символа) - на основе Parent из Psg
            string levelCode = GetLevelCodeFromPsg(psg);

            return totalFlag + psgCode + pchCode + levelCode;
        }

        /// <summary>
        /// Определяет признак итоговой строки
        /// </summary>
        private static string GetTotalFlag(long? isItog, string psg, string pch)
        {
            if (isItog == null || isItog == 0)
                return "00"; // Не итоговая

            // Определяем тип итогов по контексту
            if (string.IsNullOrEmpty(pch) || pch == "Все ПЧ")
            {
                if (string.IsNullOrEmpty(psg) || psg == "Все ПСГ")
                    return "01"; // Итоги всего по ПСГ
                else
                    return "02"; // Итоги по ГПС (ППС+ФПС)
            }
            else
            {
                // Детализация по типам итогов
                if (pch.Contains("ФПС", StringComparison.OrdinalIgnoreCase))
                    return "03"; // в т.ч. по ФПС
                else if (pch.Contains("ВПО", StringComparison.OrdinalIgnoreCase))
                    return "05"; // по ВПО
                else if (pch.Contains("ЧПО", StringComparison.OrdinalIgnoreCase))
                    return "06"; // по ЧПО
                else if (pch.Contains("АСФ", StringComparison.OrdinalIgnoreCase))
                    return "07"; // по АСФ
                else if (pch.Contains("ДПК") || pch.Contains("ДПД") || pch.Contains("ДПО"))
                    return "04"; // по "другие"
                else
                    return "02"; // по умолчанию - ГПС
            }
        }

        /// <summary>
        /// Получает код ПСГ из таблицы Psg по наименованию
        /// </summary>
        private static string GetPsgCode(string psgName, IEnumerable<Psg> psgList)
        {
            if (string.IsNullOrEmpty(psgName))
                return "0000"; // Территориальный ПСГ

            // Ищем ПСГ по имени (с учетом возможных вариантов)
            var psg = psgList.FirstOrDefault(p =>
                p.Garnizon != null &&
                psgName.Contains(p.Garnizon, StringComparison.OrdinalIgnoreCase));

            if (psg != null)
                return GetPsgCodeFromPsg(psg);

            // Если не найден - пытаемся найти по частичному совпадению
            psg = psgList.FirstOrDefault(p =>
                p.Garnizon != null &&
                p.Garnizon.Contains(psgName, StringComparison.OrdinalIgnoreCase));

            if (psg != null)
                return GetPsgCodeFromPsg(psg);

            // Если не найден - возвращаем код по умолчанию
            return "0000";
        }

        /// <summary>
        /// Получает код ПСГ из объекта Psg
        /// </summary>
        private static string GetPsgCodeFromPsg(Psg psg)
        {
            // Территориальный ПСГ (Id = 11 или Parent = null)
            if (psg.Id == 11 || psg.Parent == null)
                return "0000";

            // Для остальных ПСГ код = Id (4 символа)
            return psg.Id.ToString("D4");
        }

        /// <summary>
        /// Получает код ПЧ из справочника или генерирует новый
        /// </summary>
        private static string GetPchCode(string pchName, long? isItog, Dictionary<string, string> pchCodeMap)
        {
            // Для итоговых строк код ПЧ = 0000
            if (isItog != null && isItog > 0)
                return "0000";

            if (string.IsNullOrEmpty(pchName))
                return "0000";

            // Используем переданный справочник или стандартный
            var map = pchCodeMap ?? PchCodeMap;

            // Поиск точного совпадения
            if (map.TryGetValue(pchName, out string code))
                return code;

            // Поиск по частичному совпадению
            var key = map.Keys.FirstOrDefault(k =>
                pchName.Contains(k, StringComparison.OrdinalIgnoreCase) ||
                k.Contains(pchName, StringComparison.OrdinalIgnoreCase));

            if (key != null)
                return map[key];

            // Если не найден - генерируем новый код
            return GenerateTempCode(pchName);
        }

        /// <summary>
        /// Определяет код уровня/родителя для строки статистики
        /// </summary>
        private static string GetLevelCode(long? parent, long norder)
        {
            // Если parent = 0 или null - это корневой уровень
            if (parent == null || parent == 0)
                return "00";

            // Используем Norder для определения уровня вложенности
            // или последние 2 цифры parent
            string parentStr = parent.Value.ToString("D4");
            return parentStr.Length >= 2 ? parentStr.Substring(2, 2) : "00";
        }

        /// <summary>
        /// Определяет код уровня на основе объекта Psg
        /// </summary>
        private static string GetLevelCodeFromPsg(Psg psg)
        {
            // Если Parent = null или 0 - корневой уровень
            if (psg.Parent == null || psg.Parent == 0)
                return "00";

            // Для остальных - последние 2 цифры Parent
            string parentStr = psg.Parent.Value.ToString("D4");
            return parentStr.Length >= 2 ? parentStr.Substring(2, 2) : "00";
        }

        /// <summary>
        /// Генерирует временный код для неизвестных наименований
        /// </summary>
        private static string GenerateTempCode(string name)
        {
            // Хеш-код + нормализация до 4 символов
            int hash = Math.Abs(name.GetHashCode());
            string code = (hash % 10000).ToString("D4");
            return code;
        }

        /// <summary>
        /// Инициализация справочника ПЧ из базы данных
        /// </summary>
        public static Dictionary<string, string> InitializePchCodes(IEnumerable<FirePsgStat> stats)
        {
            var map = new Dictionary<string, string>();
            int counter = 1;

            foreach (var stat in stats.Where(s => !string.IsNullOrEmpty(s.Пч) && s.Isitog == 0))
            {
                string pchName = stat.Пч;
                if (!map.ContainsKey(pchName))
                {
                    // Генерируем код на основе PchId или порядкового номера
                    string code = stat.PchId != null
                        ? stat.PchId.Value.ToString("D4")
                        : (counter++).ToString("D4");
                    map[pchName] = code;
                }
            }

            return map;
        }

        /// <summary>
        /// Обновляет существующий справочник ПЧ
        /// </summary>
        public static void UpdatePchCodes(Dictionary<string, string> existingMap, IEnumerable<FirePsgStat> stats)
        {
            int maxCode = existingMap.Values
                .Select(v => int.TryParse(v, out int val) ? val : 0)
                .Max();

            foreach (var stat in stats.Where(s => !string.IsNullOrEmpty(s.Пч) && s.Isitog == 0))
            {
                string pchName = stat.Пч;
                if (!existingMap.ContainsKey(pchName))
                {
                    maxCode++;
                    existingMap[pchName] = maxCode.ToString("D4");
                }
            }
        }

        /// <summary>
        /// Создает справочник ПСГ из таблицы Psg
        /// </summary>
        public static Dictionary<string, string> CreatePsgCodeMap(IEnumerable<Psg> psgList)
        {
            var map = new Dictionary<string, string>();

            foreach (var psg in psgList)
            {
                if (psg.Garnizon != null)
                {
                    string code = psg.Id == 11 || psg.Parent == null
                        ? "0000"
                        : psg.Id.ToString("D4");

                    map[psg.Garnizon] = code;
                }
            }

            return map;
        }
    }

    // Расширения для FirePsgStat
    public static class FirePsgStatExtensions
    {
        /// <summary>
        /// Генерирует RowId для объекта статистики
        /// </summary>
        public static string GenerateRowId(
            this FirePsgStat stat,
            IEnumerable<Psg> psgList,
            Dictionary<string, string> pchCodeMap = null)
        {
            return RowIdGenerator.GenerateRowId(stat, psgList, pchCodeMap);
        }

        /// <summary>
        /// Проверяет, является ли строка итоговой
        /// </summary>
        public static bool IsTotalRow(this FirePsgStat stat)
        {
            return stat.Isitog != null && stat.Isitog > 0;
        }

        /// <summary>
        /// Получает тип итоговой строки
        /// </summary>
        public static string GetTotalType(this FirePsgStat stat)
        {
            if (!stat.IsTotalRow())
                return "00";

            if (string.IsNullOrEmpty(stat.Пч))
                return "01"; // Итоги по ПСГ

            if (stat.Пч.Contains("ФПС"))
                return "03";
            if (stat.Пч.Contains("ВПО"))
                return "05";
            if (stat.Пч.Contains("ЧПО"))
                return "06";
            if (stat.Пч.Contains("АСФ"))
                return "07";

            return "02"; // ГПС
        }
    }

    // Расширения для Psg
    public static class PsgExtensions
    {
        /// <summary>
        /// Получает код ПСГ из объекта Psg
        /// </summary>
        public static string GetCode(this Psg psg)
        {
            if (psg == null)
                return "0000";

            // Территориальный ПСГ (Id = 11 или Parent = null)
            if (psg.Id == 11 || psg.Parent == null)
                return "0000";

            return psg.Id.ToString("D4");
        }

        /// <summary>
        /// Получает код уровня из объекта Psg
        /// </summary>
        public static string GetLevelCode(this Psg psg)
        {
            if (psg == null || psg.Parent == null || psg.Parent == 0)
                return "00";

            string parentStr = psg.Parent.Value.ToString("D4");
            return parentStr.Length >= 2 ? parentStr.Substring(2, 2) : "00";
        }
    }
}
