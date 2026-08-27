using System;
using System.Collections.Generic;
using System.Text;

public static class ViewGenerator
{
    // Список всех числовых полей (кроме служебных)
    private static readonly List<string> NumberFields = new List<string>
    {
        "ac_br", "ac_rezerv", "ac_remont",
        "acl_br", "acl_rezerv", "acl_remont",
        "ав_br", "ав_rezerv", "ав_remont",
        "аса_апп_асм_br", "аса_апп_асм_rezerv", "аса_апп_асм_remont",
        "пнс_br", "пнс_rezerv", "пнс_remont",
        "al_br", "al_rezerv", "al_remont",
        "кп_br", "кп_rezerv", "кп_remont",
        "ар_br", "ар_rezerv", "ар_remont",
        "асмп_пса_br", "асмп_пса_rezerv", "асмп_пса_remont",
        "аш_br", "аш_rezerv", "аш_remont",
        "асо_br", "асо_rezerv", "асо_remont",
        "укс_абг_br", "укс_абг_rezerv", "укс_абг_remont",
        "АСМРХ_br", "АСМРХ_rezerv",
        "АВС_br", "АВС_rezerv",
        "пож_поезд_корабль_br", "пож_поезд_корабль_rezerv", "пож_поезд_корабль_remont",
        "анр_br", "анр_rezerv", "анр_remont",
        "аса_br", "аса_rezerv", "аса_remont",
        "tofirst", "totow",
        "ремонт_основной", "ремонт_специальной", "пожарный_корабль_ремонт",
        "плав_средства", "болотоходы", "мотопомпы", "прочее",
        "sizod_br", "sizod_rezerv",
        "костюмы_Л-1_ТАСК", "костюмы_ТОК", "костюмы_другие",
        "ГАСИ_расчёт", "ГАСИ_резерв",
        "по_списку", "Налицо", "всего", "резерв",
        "НК", "Диспетчер", "ПНК", "КО", "Водитель", "Пожарный", "ГДЗС",
        "всего_отс", "отпуск", "по_больничному", "командировка", "некомплект", "прочие_отс",
        "пена_расчёт", "пена_резерв", "порошок_расчёт", "порошок_резерв",
        "ДТ", "Бензин"
    };

    // Маппинг имен полей (имя в представлении -> имя в таблице)
    private static readonly Dictionary<string, string> FieldNameMapping = new Dictionary<string, string>
    {
        { "костюмы_Л-1_ТАСК", "костюмы_Л_1_ТАСК" }
    };

    // Поля, которые берутся из разных источников (для apivot)
    private static readonly Dictionary<string, string> FieldSources = new Dictionary<string, string>
    {
        { "sizod_br", "z" },
        { "sizod_rezerv", "z" },
        { "костюмы_Л-1_ТАСК", "k" },
        { "костюмы_ТОК", "k" },
        { "костюмы_другие", "k" },
        { "ГАСИ_расчёт", "s" },
        { "ГАСИ_резерв", "s" },
        { "НК", "st" },
        { "Диспетчер", "st" },
        { "ПНК", "st" },
        { "КО", "st" },
        { "Водитель", "st" },
        { "Пожарный", "st" },
        { "ГДЗС", "st" },
        { "по_списку", "st" },
        { "Налицо", "st" },
        { "всего", "st" },
        { "резерв", "st" },
        { "всего_отс", "st" },
        { "отпуск", "st" },
        { "по_больничному", "st" },
        { "командировка", "st" },
        { "некомплект", "st" },
        { "прочие_отс", "st" },
        { "пена_расчёт", "p" },
        { "пена_резерв", "p" },
        { "ДТ", "s" },
        { "Бензин", "s" }
    };

    // Поля с особыми значениями (для apivot)
    private static readonly Dictionary<string, string> SpecialFields = new Dictionary<string, string>
    {
        { "порошок_расчёт", "0" },
        { "порошок_резерв", "0" }
    };

    // ===================== МЕТОДЫ ДЛЯ ROW_ID =====================

    /// <summary>
    /// Генерирует список полей для SELECT с добавлением row_id (без последней запятой)
    /// </summary>
    public static string GenerateSelectFieldsWithRowId(string source, bool includeRowId = true)
    {
        var sb = new StringBuilder();

        // Добавляем row_id первым полем, если нужно
        if (includeRowId)
        {
            sb.AppendLine($"  `{source}`.`row_id` AS `row_id`,");
        }

        int count = NumberFields.Count;
        for (int i = 0; i < count; i++)
        {
            var field = NumberFields[i];
            if (i == count - 1)
                sb.Append($"  `{source}`.`{field}` AS `{field}`");
            else
                sb.AppendLine($"  `{source}`.`{field}` AS `{field}`,");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Генерирует список SUM полей с добавлением row_id (без последней запятой)
    /// </summary>
    public static string GenerateSumFieldsWithRowId(string source = "apivot_mat", bool includeRowId = true)
    {
        var sb = new StringBuilder();

        // Добавляем row_id как NULL (для итоговых строк)
        if (includeRowId)
        {
            sb.AppendLine($"  NULL AS `row_id`,");
        }

        int count = NumberFields.Count;
        for (int i = 0; i < count; i++)
        {
            var field = NumberFields[i];
            if (i == count - 1)
                sb.Append($"  SUM(`{source}`.`{field}`) AS `{field}`");
            else
                sb.AppendLine($"  SUM(`{source}`.`{field}`) AS `{field}`,");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Генерирует список COALESCE полей для apivot (без последней запятой)
    /// </summary>
    public static string GenerateApivotFields()
    {
        var sb = new StringBuilder();
        int count = NumberFields.Count;

        for (int i = 0; i < count; i++)
        {
            var field = NumberFields[i];
            bool isLast = (i == count - 1);

            // Проверяем маппинг имени поля
            string tableFieldName = FieldNameMapping.ContainsKey(field) ? FieldNameMapping[field] : field;

            if (SpecialFields.ContainsKey(field))
            {
                if (isLast)
                    sb.Append($"  {SpecialFields[field]} AS `{field}`");
                else
                    sb.AppendLine($"  {SpecialFields[field]} AS `{field}`,");
                continue;
            }

            string source = FieldSources.ContainsKey(field) ? FieldSources[field] : "s";

            if (isLast)
                sb.Append($"  COALESCE(`{source}`.`{tableFieldName}`, 0) AS `{field}`");
            else
                sb.AppendLine($"  COALESCE(`{source}`.`{tableFieldName}`, 0) AS `{field}`,");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Генерирует полный SQL для apivot (добавлен row_id)
    /// </summary>
    public static string GenerateApivot()
    {
        var fields = GenerateApivotFields();

        return $@"CREATE OR REPLACE VIEW `apivot` AS
SELECT
  COALESCE(`psg`.`garnizon`, 'Без гарнизона') AS `ПСГ`,
  `pchs`.`name` AS `ПЧ`,
  `pchs`.`garntype` AS `category`,
  `pchs`.`id` AS `pch_id`,
  COALESCE(`pchs`.`parent`, 0) AS `parent`,
  `pchs`.`norder` AS `norder`,
  0 AS `isitog`,
  `pchs`.`datafilled` AS `datafilled`,
  `pchs`.`row_id` AS `row_id`,  -- ← Добавлено
{fields},
  COALESCE(`cn`.`nachkar`, 'не указан') AS `начкар`
FROM (((((((`pchs`
  LEFT JOIN `a_sredstva` `s` ON (`pchs`.`id` = `s`.`subdivision_id`))
  LEFT JOIN `v_sostav` `st` ON (`pchs`.`id` = `st`.`subdivision_id`))
  LEFT JOIN `v_kostyms` `k` ON (`pchs`.`id` = `k`.`subdivision_id`))
  LEFT JOIN `v_sizod` `z` ON (`pchs`.`id` = `z`.`subdivision_id`))
  LEFT JOIN `v_penas` `p` ON (`pchs`.`id` = `p`.`subdivision_id`))
  LEFT JOIN `psg` ON (`pchs`.`parent` = `psg`.`id`))
  LEFT JOIN `cache_nachkar` `cn` ON (`cn`.`subdivision_id` = `pchs`.`id`));";
    }

    /// <summary>
    /// Генерирует полный SQL для bpivot (добавлен row_id = NULL)
    /// </summary>
    public static string GenerateBpivot()
    {
        var sumFields = GenerateSumFieldsWithRowId("apivot_mat", false);

        return $@"CREATE OR REPLACE VIEW `bpivot` AS
SELECT
  `grp`.`category_type` AS `category_type`,
  `apivot_mat`.`ПСГ` AS `ПСГ`,
  CASE 
    WHEN `grp`.`category_type` = 'main' THEN `apivot_mat`.`ПСГ` 
    WHEN `grp`.`category_type` = 'gps' THEN '      в т.ч. ГПС' 
    WHEN `grp`.`category_type` = 'fps' THEN '        в т.ч. по ФПС'
    WHEN `grp`.`category_type` = 'vpo' THEN '             ВПО' 
    WHEN `grp`.`category_type` = 'chpo' THEN '             ЧПО' 
    WHEN `grp`.`category_type` = 'other' THEN '             другие' 
    WHEN `grp`.`category_type` = 'asf' THEN 'АСФ' 
  END AS `ПЧ`,
  CASE 
    WHEN `grp`.`category_type` = 'main' THEN 'всего' 
    WHEN `grp`.`category_type` = 'gps' THEN '      в т.ч. ГПС' 
    WHEN `grp`.`category_type` = 'fps' THEN '        в т.ч. по ФПС'
    WHEN `grp`.`category_type` = 'vpo' THEN '             ВПО' 
    WHEN `grp`.`category_type` = 'chpo' THEN '             ЧПО' 
    WHEN `grp`.`category_type` = 'other' THEN '             другие' 
    WHEN `grp`.`category_type` = 'asf' THEN 'АСФ' 
  END AS `category`,
  CASE 
    WHEN `grp`.`category_type` = 'main' THEN (MIN(`apivot_mat`.`parent`) * 1000) 
    WHEN `grp`.`category_type` = 'gps' THEN ((MIN(`apivot_mat`.`parent`) * 1000) + 1) 
    WHEN `grp`.`category_type` = 'fps' THEN ((MIN(`apivot_mat`.`parent`) * 1000) + 2)
    WHEN `grp`.`category_type` = 'vpo' THEN ((MIN(`apivot_mat`.`parent`) * 1000) + 3) 
    WHEN `grp`.`category_type` = 'chpo' THEN ((MIN(`apivot_mat`.`parent`) * 1000) + 4) 
    WHEN `grp`.`category_type` = 'other' THEN ((MIN(`apivot_mat`.`parent`) * 1000) + 5) 
    WHEN `grp`.`category_type` = 'asf' THEN MIN(`apivot_mat`.`parent`)
  END AS `pch_id`,
  CASE 
    WHEN `grp`.`category_type` = 'main' THEN `apivot_mat`.`parent` 
    ELSE MIN(`apivot_mat`.`parent`) 
  END AS `parent`,
  CASE 
    WHEN `grp`.`category_type` = 'gps' THEN -19 
    WHEN `grp`.`category_type` = 'fps' THEN -18
    WHEN `grp`.`category_type` = 'other' THEN -17 
    WHEN `grp`.`category_type` = 'vpo' THEN -15 
    WHEN `grp`.`category_type` = 'chpo' THEN -13 
    WHEN `grp`.`category_type` = 'main' THEN -20 
    WHEN `grp`.`category_type` = 'asf' THEN -10 
  END AS `norder`,
  1 AS `isitog`,
  NULL AS `datafilled`,
   NULL AS `row_id`,
{sumFields},
  CASE 
    WHEN `grp`.`category_type` = 'main' THEN COALESCE(`cn`.`nachkar`, 'не указан') 
    WHEN `grp`.`category_type` = 'gps' THEN ' ' 
    WHEN `grp`.`category_type` = 'fps' THEN ''
    WHEN `grp`.`category_type` = 'vpo' THEN '' 
    WHEN `grp`.`category_type` = 'chpo' THEN '' 
    WHEN `grp`.`category_type` = 'other' THEN '' 
    WHEN `grp`.`category_type` = 'asf' THEN '' 
  END AS `начкар`
FROM `apivot_mat`
JOIN `grp`
LEFT JOIN `cache_nachkar` `cn` ON `cn`.`subdivision_id` = `apivot_mat`.`pch_id`
WHERE 
  (`grp`.`category_type` = 'main' AND `apivot_mat`.`category` <> 'АСФ')
  OR (`grp`.`category_type` = 'gps' AND `apivot_mat`.`category` IN ('ППС', 'ФПС'))
  OR (`grp`.`category_type` = 'fps' AND 
      ((`apivot_mat`.`category` = 'ФПС' AND `apivot_mat`.`parent` != 1744) 
       OR `apivot_mat`.`ПЧ` = 'ПЧ-75'))
  OR (`grp`.`category_type` = 'vpo' AND `apivot_mat`.`category` = 'ВПО')
  OR (`grp`.`category_type` = 'chpo' AND `apivot_mat`.`category` = 'ЧПО')
  OR (`grp`.`category_type` = 'other' AND `apivot_mat`.`category` NOT IN ('ППС', 'ФПС', 'ЧПО', 'ВПО', 'АСФ') AND `apivot_mat`.`category` <> '')
  OR (`grp`.`category_type` = 'asf' AND `apivot_mat`.`ПЧ` LIKE '%АСФ%')
GROUP BY `apivot_mat`.`ПСГ`, `grp`.`category_type`;";
    }

    /// <summary>
    /// Генерирует полный SQL для cpivot (row_id из psg_total_rows)
    /// </summary>
    public static string GenerateCpivot()
    {
        var sumFieldsBpivot = GenerateSumFieldsWithRowId("bpivot", false);
        var sumFieldsApivot = GenerateSumFieldsWithRowId("apivot_mat", false);

        return $@"CREATE OR REPLACE VIEW `cpivot` AS
SELECT
  'main' AS `category_type`,
  'Территориальный' AS `ПСГ`,
  `ptr`.`display_name` AS `ПЧ`,
  `ptr`.`category_display` AS `category`,
  `ptr`.`id` AS `pch_id`,
  `ptr`.`psg_id` AS `parent`,
  `ptr`.`norder` AS `norder`,
  1 AS `isitog`,
  NULL AS `datafilled`,
  `ptr`.`row_id` AS `row_id`,  -- ← Добавлено (из psg_total_rows)
{sumFieldsBpivot},
  'не указан' AS `начкар`
FROM `bpivot`
JOIN `psg_total_rows` `ptr` 
    ON `ptr`.`psg_id` = 11 
    AND `ptr`.`category_type` = `bpivot`.`category_type`
WHERE `ptr`.`category_type` IN ('gps', 'fps', 'vpo', 'chpo', 'other', 'asf')
GROUP BY `ptr`.`id`

UNION ALL

SELECT
  'main' AS `category_type`,
  'Территориальный' AS `ПСГ`,
  `ptr_main`.`display_name` AS `ПЧ`,
  `ptr_main`.`category_display` AS `category`,
  `ptr_main`.`id` AS `pch_id`,
  `ptr_main`.`psg_id` AS `parent`,
  `ptr_main`.`norder` AS `norder`,
  1 AS `isitog`,
  NULL AS `datafilled`,
  `ptr_main`.`row_id` AS `row_id`,  -- ← Добавлено (из psg_total_rows)
{sumFieldsApivot},
  COALESCE(`cn`.`nachkar`, 'не указан') AS `начкар`
FROM `apivot_mat`
JOIN `psg_total_rows` `ptr_main` 
    ON `ptr_main`.`psg_id` = 11 
    AND `ptr_main`.`category_type` = 'main'
LEFT JOIN `cache_nachkar` `cn` 
    ON `cn`.`subdivision_id` = `apivot_mat`.`pch_id`
WHERE `apivot_mat`.`ПЧ` NOT LIKE '%АСФ%';";
    }

    /// <summary>
    /// Генерирует полный SQL для FirePsgStat (добавлен row_id)
    /// </summary>
    public static string GenerateFirePsgStat()
    {
        var selectFields1 = GenerateSelectFieldsWithRowId("apivot_mat", false);
        var selectFields2 = GenerateSelectFieldsWithRowId("t", false);

        return $@"CREATE OR REPLACE VIEW `FirePsgStat` AS
SELECT
  `apivot_mat`.`ПСГ` AS `ПСГ`,
  `apivot_mat`.`ПЧ` AS `ПЧ`,
  `apivot_mat`.`category` AS `category`,
  `apivot_mat`.`pch_id` AS `pch_id`,
  `apivot_mat`.`row_id` AS `row_id`,
  `apivot_mat`.`parent` AS `parent`,
  `apivot_mat`.`norder` AS `norder`,
  `apivot_mat`.`isitog` AS `isitog`,
  `apivot_mat`.`datafilled` AS `datafilled`,
{selectFields1},
  `apivot_mat`.`начкар` AS `начкар`
FROM `apivot_mat`

UNION ALL

SELECT
  `t`.`ПСГ` AS `ПСГ`,
  `t`.`ПЧ` AS `ПЧ`,
  `t`.`category` AS `category`,
  `t`.`pch_id` AS `pch_id`,
  `t`.`row_id` AS `row_id`,
  `t`.`parent` AS `parent`,
  `t`.`norder` AS `norder`,
  `t`.`isitog` AS `isitog`,
  `t`.`datafilled` AS `datafilled`,
{selectFields2},
  `t`.`начкар` AS `начкар`
FROM `titogs` `t`;";
    }

    /// <summary>
    /// Генерирует полный SQL для titogs (добавлен row_id)
    /// </summary>
    public static string GenerateTitogs()
    {
        var selectFieldsB = GenerateSelectFieldsWithRowId("b", false);
        var selectFieldsC = GenerateSelectFieldsWithRowId("c", true);

        return $@"CREATE OR REPLACE VIEW `titogs` AS
SELECT
  `b`.`category_type`,
  `b`.`ПСГ` AS `ПСГ`,
  `b`.`ПЧ` AS `ПЧ`,
  `b`.`category` AS `category`,
  `b`.`pch_id` AS `pch_id`,
  `b`.`parent` AS `parent`,
  `b`.`norder` AS `norder`,
  `b`.`isitog` AS `isitog`,
  `b`.`datafilled` AS `datafilled`,
    COALESCE(b.row_id, psg.row_id) AS row_id,
{selectFieldsB},
  `b`.`начкар` AS `начкар`
FROM `bpivot` `b`
LEFT JOIN psg_total_rows psg 
    ON b.parent = psg.psg_id
    AND b.category_type = psg.category_type
    AND b.row_id IS NULL
WHERE `b`.`category` NOT LIKE '%АСФ%'

UNION ALL

SELECT
  `c`.`category_type`,
  `c`.`ПСГ` AS `ПСГ`,
  `c`.`ПЧ` AS `ПЧ`,
  `c`.`category` AS `category`,
  `c`.`pch_id` AS `pch_id`,
  `c`.`parent` AS `parent`,
  `c`.`norder` AS `norder`,
  `c`.`isitog` AS `isitog`,
  `c`.`datafilled` AS `datafilled`,
{selectFieldsC},
  `c`.`начкар` AS `начкар`
FROM `cpivot` `c`;";
    }

    /// <summary>
    /// Генерирует все представления одним скриптом
    /// </summary>
    public static string GenerateAllViews()
    {
        var grp = @"CREATE OR REPLACE VIEW `grp` AS
SELECT 'main' AS `category_type`
UNION ALL SELECT 'gps'
UNION ALL SELECT 'fps'
UNION ALL SELECT 'vpo'
UNION ALL SELECT 'chpo'
UNION ALL SELECT 'other'
UNION ALL SELECT 'asf';";

        var apivot = GenerateApivot();
        var bpivot = GenerateBpivot();
        var cpivot = GenerateCpivot();
        var titogs = GenerateTitogs();
        var firePsgStat = GenerateFirePsgStat();

        return $@"{grp}

{apivot}

{bpivot}

{cpivot}

{titogs}

{firePsgStat}";
    }
}