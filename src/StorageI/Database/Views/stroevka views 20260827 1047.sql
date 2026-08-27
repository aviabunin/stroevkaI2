--
-- Скрипт сгенерирован Devart dbForge Studio for MySQL, Версия 7.3.131.0
-- Домашняя страница продукта: http://www.devart.com/ru/dbforge/mysql/studio
-- Дата скрипта: 27.08.2026 10:47:35
-- Версия сервера: 5.5.15
-- Версия клиента: 4.1
--


-- 
-- Отключение внешних ключей
-- 
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;

-- 
-- Установить режим SQL (SQL mode)
-- 
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- 
-- Установка кодировки, с использованием которой клиент будет посылать запросы на сервер
--
SET NAMES 'utf8';

--
-- Установка базы данных по умолчанию
--
USE stroevka;

--
-- Удалить представление "v_sostav_base"
--
DROP VIEW IF EXISTS v_sostav_base CASCADE;

--
-- Удалить представление "v_sostav"
--
DROP VIEW IF EXISTS v_sostav CASCADE;

--
-- Удалить представление "v_sizod"
--
DROP VIEW IF EXISTS v_sizod CASCADE;

--
-- Удалить представление "v_penas"
--
DROP VIEW IF EXISTS v_penas CASCADE;

--
-- Удалить представление "v_kostyms"
--
DROP VIEW IF EXISTS v_kostyms CASCADE;

--
-- Удалить представление "titogs"
--
DROP VIEW IF EXISTS titogs CASCADE;

--
-- Удалить представление "grp"
--
DROP VIEW IF EXISTS grp CASCADE;

--
-- Удалить представление "fire_psg_stat"
--
DROP VIEW IF EXISTS fire_psg_stat CASCADE;

--
-- Удалить представление "cpivot_copy"
--
DROP VIEW IF EXISTS cpivot_copy CASCADE;

--
-- Удалить представление "cpivot"
--
DROP VIEW IF EXISTS cpivot CASCADE;

--
-- Удалить представление "bpivot"
--
DROP VIEW IF EXISTS bpivot CASCADE;

--
-- Удалить представление "apivot"
--
DROP VIEW IF EXISTS apivot CASCADE;

--
-- Удалить представление "a_sredstva"
--
DROP VIEW IF EXISTS a_sredstva CASCADE;

--
-- Установка базы данных по умолчанию
--
USE stroevka;

--
-- Создать представление "a_sredstva"
--
CREATE
DEFINER = 'root'@'localhost'
VIEW a_sredstva
AS
SELECT
  `s`.`subdivision_id` AS `subdivision_id`,
  `s`.`garnizon_id` AS `parent`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АЦ') THEN `s`.`br` ELSE 0 END)) AS `ac_br`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АЦ') THEN `s`.`rezerv` ELSE 0 END)) AS `ac_rezerv`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АЦ') THEN `s`.`remont` ELSE 0 END)) AS `ac_remont`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АЦЛ') THEN `s`.`br` ELSE 0 END)) AS `acl_br`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АЦЛ') THEN `s`.`rezerv` ELSE 0 END)) AS `acl_rezerv`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АЦЛ') THEN `s`.`remont` ELSE 0 END)) AS `acl_remont`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АНР') THEN `s`.`br` ELSE 0 END)) AS `анр_br`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АНР') THEN `s`.`rezerv` ELSE 0 END)) AS `анр_rezerv`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АНР') THEN `s`.`remont` ELSE 0 END)) AS `анр_remont`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АСА') THEN `s`.`br` ELSE 0 END)) AS `аса_br`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АСА') THEN `s`.`rezerv` ELSE 0 END)) AS `аса_rezerv`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АСА') THEN `s`.`remont` ELSE 0 END)) AS `аса_remont`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АСО') THEN `s`.`br` ELSE 0 END)) AS `асо_br`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АСО') THEN `s`.`rezerv` ELSE 0 END)) AS `асо_rezerv`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АСО') THEN `s`.`remont` ELSE 0 END)) AS `асо_remont`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АВ') THEN `s`.`br` ELSE 0 END)) AS `ав_br`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АВ') THEN `s`.`rezerv` ELSE 0 END)) AS `ав_rezerv`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АВ') THEN `s`.`remont` ELSE 0 END)) AS `ав_remont`,
  SUM((CASE WHEN (`s`.`name_sredstvo` IN ('АСА', 'АПП', 'АСМ')) THEN `s`.`br` ELSE 0 END)) AS `аса_апп_асм_br`,
  SUM((CASE WHEN (`s`.`name_sredstvo` IN ('АСА', 'АПП', 'АСМ')) THEN `s`.`rezerv` ELSE 0 END)) AS `аса_апп_асм_rezerv`,
  SUM((CASE WHEN (`s`.`name_sredstvo` IN ('АСА', 'АПП', 'АСМ')) THEN `s`.`remont` ELSE 0 END)) AS `аса_апп_асм_remont`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'ПНС') THEN `s`.`br` ELSE 0 END)) AS `пнс_br`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'ПНС') THEN `s`.`rezerv` ELSE 0 END)) AS `пнс_rezerv`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'ПНС') THEN `s`.`remont` ELSE 0 END)) AS `пнс_remont`,
  SUM((CASE WHEN (`s`.`name_sredstvo` LIKE 'АЛ%') THEN `s`.`br` ELSE 0 END)) AS `al_br`,
  SUM((CASE WHEN (`s`.`name_sredstvo` LIKE 'АЛ%') THEN `s`.`rezerv` ELSE 0 END)) AS `al_rezerv`,
  SUM((CASE WHEN (`s`.`name_sredstvo` LIKE 'АЛ%') THEN `s`.`remont` ELSE 0 END)) AS `al_remont`,
  SUM((CASE WHEN (`s`.`name_sredstvo` IN ('АКП', 'КП')) THEN `s`.`br` ELSE 0 END)) AS `кп_br`,
  SUM((CASE WHEN (`s`.`name_sredstvo` IN ('АКП', 'КП')) THEN `s`.`rezerv` ELSE 0 END)) AS `кп_rezerv`,
  SUM((CASE WHEN (`s`.`name_sredstvo` IN ('АКП', 'КП')) THEN `s`.`remont` ELSE 0 END)) AS `кп_remont`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АР') THEN `s`.`br` ELSE 0 END)) AS `ар_br`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АР') THEN `s`.`rezerv` ELSE 0 END)) AS `ар_rezerv`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АР') THEN `s`.`remont` ELSE 0 END)) AS `ар_remont`,
  SUM((CASE WHEN (`s`.`name_sredstvo` IN ('АСМП', 'ПСА')) THEN `s`.`br` ELSE 0 END)) AS `асмп_пса_br`,
  SUM((CASE WHEN (`s`.`name_sredstvo` IN ('АСМП', 'ПСА')) THEN `s`.`rezerv` ELSE 0 END)) AS `асмп_пса_rezerv`,
  SUM((CASE WHEN (`s`.`name_sredstvo` IN ('АСМП', 'ПСА')) THEN `s`.`remont` ELSE 0 END)) AS `асмп_пса_remont`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АШ') THEN `s`.`br` ELSE 0 END)) AS `аш_br`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АШ') THEN `s`.`rezerv` ELSE 0 END)) AS `аш_rezerv`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АШ') THEN `s`.`remont` ELSE 0 END)) AS `аш_remont`,
  SUM((CASE WHEN (`s`.`name_sredstvo` IN ('УКС', 'АБГ')) THEN `s`.`br` ELSE 0 END)) AS `укс_абг_br`,
  SUM((CASE WHEN (`s`.`name_sredstvo` IN ('УКС', 'АБГ')) THEN `s`.`rezerv` ELSE 0 END)) AS `укс_абг_rezerv`,
  SUM((CASE WHEN (`s`.`name_sredstvo` IN ('УКС', 'АБГ')) THEN `s`.`remont` ELSE 0 END)) AS `укс_абг_remont`,
  SUM((CASE WHEN (`s`.`name_sredstvo` IN ('Пожарный_поезд', 'Пожарный_корабль')) THEN `s`.`br` ELSE 0 END)) AS `пож_поезд_корабль_br`,
  SUM((CASE WHEN (`s`.`name_sredstvo` IN ('Пожарный_поезд', 'Пожарный_корабль')) THEN `s`.`rezerv` ELSE 0 END)) AS `пож_поезд_корабль_rezerv`,
  SUM((CASE WHEN (`s`.`name_sredstvo` IN ('Пожарный_поезд', 'Пожарный_корабль')) THEN `s`.`remont` ELSE 0 END)) AS `пож_поезд_корабль_remont`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'Пожарный_поезд') THEN `s`.`br` ELSE 0 END)) AS `пож_поезд_br`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'Пожарный_поезд') THEN `s`.`rezerv` ELSE 0 END)) AS `пож_поезд_rezerv`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'Пожарный_поезд') THEN `s`.`remont` ELSE 0 END)) AS `пож_поезд_remont`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'Пожарный_корабль(катер)') THEN `s`.`br` ELSE 0 END)) AS `пож_корабль_катер_br`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'Пожарный_корабль(катер)') THEN `s`.`rezerv` ELSE 0 END)) AS `пож_корабль_катер_rezerv`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'Пожарный_корабль(катер)') THEN `s`.`remont` ELSE 0 END)) AS `пож_корабль_катер_remont`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АСМРХ') THEN `s`.`br` ELSE 0 END)) AS `АСМРХ_br`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АСМРХ') THEN `s`.`rezerv` ELSE 0 END)) AS `АСМРХ_rezerv`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АВС') THEN `s`.`br` ELSE 0 END)) AS `АВС_br`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'АВС') THEN `s`.`rezerv` ELSE 0 END)) AS `АВС_rezerv`,
  SUM((CASE WHEN (`s`.`name_sredstvo` IN ('АЦ', 'АЦЛ', 'АВ', 'АСА', 'АПП', 'ПНС', 'АНР')) THEN `s`.`remont` ELSE 0 END)) AS `ремонт_основной`,
  SUM((CASE WHEN ((`s`.`name_sredstvo` IN ('АЛ', 'КП', 'АР', 'АСМП', 'ПСА', 'АШ', 'АСМ', 'АСМРХ', 'АВС', 'УКС', 'АБГ', 'АКП')) OR
      (`s`.`name_sredstvo` LIKE 'АЛ%')) THEN `s`.`remont` ELSE 0 END)) AS `ремонт_специальной`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'Пожарный_корабль') THEN (`s`.`remont` + `s`.`rezerv`) ELSE 0 END)) AS `пожарный_корабль_ремонт`,
  SUM((CASE WHEN (`s`.`name_sredstvo` LIKE CAST('%Плав_средства%' AS char CHARSET binary)) THEN ((`s`.`br` + `s`.`remont`) + `s`.`rezerv`) ELSE 0 END)) AS `плав_средства`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'Болотоходы') THEN ((`s`.`br` + `s`.`remont`) + `s`.`rezerv`) ELSE 0 END)) AS `болотоходы`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'Мотопомпы') THEN ((`s`.`br` + `s`.`remont`) + `s`.`rezerv`) ELSE 0 END)) AS `мотопомпы`,
  SUM((CASE WHEN (`s`.`name_sredstvo` IN ('Грузовой_автомобиль', 'Автобусы', 'Бензовозы', 'Краны', 'Инженерная', 'Мототехника', 'Иные', 'Автомобиль аэродромный')) THEN (`s`.`br` + `s`.`rezerv`) ELSE 0 END)) AS `прочее`,
  SUM((CASE WHEN (`s`.`name_sredstvo` NOT IN ('ДТ', 'Бензин')) THEN `s`.`tofirst` ELSE 0 END)) AS `tofirst`,
  SUM((CASE WHEN (`s`.`name_sredstvo` NOT IN ('ДТ', 'Бензин')) THEN `s`.`totow` ELSE 0 END)) AS `totow`,
  SUM((CASE WHEN (CAST(`s`.`name_sredstvo` AS char CHARSET binary) IN ('ГАСИ_ручной', 'ГАСИ_механизированный')) THEN `s`.`br` ELSE 0 END)) AS `ГАСИ_расчёт`,
  SUM((CASE WHEN (CAST(`s`.`name_sredstvo` AS char CHARSET binary) IN ('ГАСИ_ручной', 'ГАСИ_механизированный')) THEN `s`.`rezerv` ELSE 0 END)) AS `ГАСИ_резерв`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'ДТ') THEN (`s`.`br` + `s`.`rezerv`) ELSE 0 END)) AS `ДТ`,
  SUM((CASE WHEN (`s`.`name_sredstvo` = 'Бензин') THEN (`s`.`br` + `s`.`rezerv`) ELSE 0 END)) AS `Бензин`
FROM `sredstva` `s`
GROUP BY `s`.`subdivision_id`;

--
-- Создать представление "apivot"
--
CREATE
DEFINER = 'root'@'localhost'
VIEW apivot
AS
SELECT
  COALESCE(`psg`.`garnizon`, 'Без гарнизона') AS `ПСГ`,
  `pchs`.`name` AS `ПЧ`,
  `pchs`.`garntype` AS `category`,
  `pchs`.`id` AS `pch_id`,
  COALESCE(`pchs`.`parent`, 0) AS `parent`,
  `pchs`.`norder` AS `norder`,
  0 AS `isitog`,
  `pchs`.`datafilled` AS `datafilled`,
  `pchs`.`row_id` AS `row_id`,
  COALESCE(`s`.`ac_br`, 0) AS `ac_br`,
  COALESCE(`s`.`ac_rezerv`, 0) AS `ac_rezerv`,
  COALESCE(`s`.`ac_remont`, 0) AS `ac_remont`,
  COALESCE(`s`.`acl_br`, 0) AS `acl_br`,
  COALESCE(`s`.`acl_rezerv`, 0) AS `acl_rezerv`,
  COALESCE(`s`.`acl_remont`, 0) AS `acl_remont`,
  COALESCE(`s`.`ав_br`, 0) AS `ав_br`,
  COALESCE(`s`.`ав_rezerv`, 0) AS `ав_rezerv`,
  COALESCE(`s`.`ав_remont`, 0) AS `ав_remont`,
  COALESCE(`s`.`аса_апп_асм_br`, 0) AS `аса_апп_асм_br`,
  COALESCE(`s`.`аса_апп_асм_rezerv`, 0) AS `аса_апп_асм_rezerv`,
  COALESCE(`s`.`аса_апп_асм_remont`, 0) AS `аса_апп_асм_remont`,
  COALESCE(`s`.`пнс_br`, 0) AS `пнс_br`,
  COALESCE(`s`.`пнс_rezerv`, 0) AS `пнс_rezerv`,
  COALESCE(`s`.`пнс_remont`, 0) AS `пнс_remont`,
  COALESCE(`s`.`al_br`, 0) AS `al_br`,
  COALESCE(`s`.`al_rezerv`, 0) AS `al_rezerv`,
  COALESCE(`s`.`al_remont`, 0) AS `al_remont`,
  COALESCE(`s`.`кп_br`, 0) AS `кп_br`,
  COALESCE(`s`.`кп_rezerv`, 0) AS `кп_rezerv`,
  COALESCE(`s`.`кп_remont`, 0) AS `кп_remont`,
  COALESCE(`s`.`ар_br`, 0) AS `ар_br`,
  COALESCE(`s`.`ар_rezerv`, 0) AS `ар_rezerv`,
  COALESCE(`s`.`ар_remont`, 0) AS `ар_remont`,
  COALESCE(`s`.`асмп_пса_br`, 0) AS `асмп_пса_br`,
  COALESCE(`s`.`асмп_пса_rezerv`, 0) AS `асмп_пса_rezerv`,
  COALESCE(`s`.`асмп_пса_remont`, 0) AS `асмп_пса_remont`,
  COALESCE(`s`.`аш_br`, 0) AS `аш_br`,
  COALESCE(`s`.`аш_rezerv`, 0) AS `аш_rezerv`,
  COALESCE(`s`.`аш_remont`, 0) AS `аш_remont`,
  COALESCE(`s`.`асо_br`, 0) AS `асо_br`,
  COALESCE(`s`.`асо_rezerv`, 0) AS `асо_rezerv`,
  COALESCE(`s`.`асо_remont`, 0) AS `асо_remont`,
  COALESCE(`s`.`укс_абг_br`, 0) AS `укс_абг_br`,
  COALESCE(`s`.`укс_абг_rezerv`, 0) AS `укс_абг_rezerv`,
  COALESCE(`s`.`укс_абг_remont`, 0) AS `укс_абг_remont`,
  COALESCE(`s`.`АСМРХ_br`, 0) AS `АСМРХ_br`,
  COALESCE(`s`.`АСМРХ_rezerv`, 0) AS `АСМРХ_rezerv`,
  COALESCE(`s`.`АВС_br`, 0) AS `АВС_br`,
  COALESCE(`s`.`АВС_rezerv`, 0) AS `АВС_rezerv`,
  COALESCE(`s`.`пож_поезд_корабль_br`, 0) AS `пож_поезд_корабль_br`,
  COALESCE(`s`.`пож_поезд_корабль_rezerv`, 0) AS `пож_поезд_корабль_rezerv`,
  COALESCE(`s`.`пож_поезд_корабль_remont`, 0) AS `пож_поезд_корабль_remont`,
  COALESCE(`s`.`пож_поезд_br`, 0) AS `пож_поезд_br`,
  COALESCE(`s`.`пож_поезд_rezerv`, 0) AS `пож_поезд_rezerv`,
  COALESCE(`s`.`пож_поезд_remont`, 0) AS `пож_поезд_remont`,
  COALESCE(`s`.`пож_корабль_катер_br`, 0) AS `пож_корабль_катер_br`,
  COALESCE(`s`.`пож_корабль_катер_rezerv`, 0) AS `пож_корабль_катер_rezerv`,
  COALESCE(`s`.`пож_корабль_катер_remont`, 0) AS `пож_корабль_катер_remont`,
  COALESCE(`s`.`анр_br`, 0) AS `анр_br`,
  COALESCE(`s`.`анр_rezerv`, 0) AS `анр_rezerv`,
  COALESCE(`s`.`анр_remont`, 0) AS `анр_remont`,
  COALESCE(`s`.`аса_br`, 0) AS `аса_br`,
  COALESCE(`s`.`аса_rezerv`, 0) AS `аса_rezerv`,
  COALESCE(`s`.`аса_remont`, 0) AS `аса_remont`,
  COALESCE(`s`.`tofirst`, 0) AS `tofirst`,
  COALESCE(`s`.`totow`, 0) AS `totow`,
  COALESCE(`s`.`ремонт_основной`, 0) AS `ремонт_основной`,
  COALESCE(`s`.`ремонт_специальной`, 0) AS `ремонт_специальной`,
  COALESCE(`s`.`пожарный_корабль_ремонт`, 0) AS `пожарный_корабль_ремонт`,
  COALESCE(`s`.`плав_средства`, 0) AS `плав_средства`,
  COALESCE(`s`.`болотоходы`, 0) AS `болотоходы`,
  COALESCE(`s`.`мотопомпы`, 0) AS `мотопомпы`,
  COALESCE(`s`.`прочее`, 0) AS `прочее`,
  COALESCE(`z`.`sizod_br`, 0) AS `sizod_br`,
  COALESCE(`z`.`sizod_rezerv`, 0) AS `sizod_rezerv`,
  COALESCE(`k`.`костюмы_Л_1_ТАСК`, 0) AS `костюмы_Л-1_ТАСК`,
  COALESCE(`k`.`костюмы_ТОК`, 0) AS `костюмы_ТОК`,
  COALESCE(`k`.`костюмы_другие`, 0) AS `костюмы_другие`,
  COALESCE(`s`.`ГАСИ_расчёт`, 0) AS `ГАСИ_расчёт`,
  COALESCE(`s`.`ГАСИ_резерв`, 0) AS `ГАСИ_резерв`,
  COALESCE(`st`.`По_списку`, 0) AS `по_списку`,
  COALESCE(`st`.`Налицо`, 0) AS `Налицо`,
  COALESCE(`st`.`Всего`, 0) AS `всего`,
  COALESCE(`st`.`резерв`, 0) AS `резерв`,
  COALESCE(`st`.`НК`, 0) AS `НК`,
  COALESCE(`st`.`Диспетчер`, 0) AS `Диспетчер`,
  COALESCE(`st`.`ПНК`, 0) AS `ПНК`,
  COALESCE(`st`.`КО`, 0) AS `КО`,
  COALESCE(`st`.`Водитель`, 0) AS `Водитель`,
  COALESCE(`st`.`Пожарный`, 0) AS `Пожарный`,
  COALESCE(`st`.`ГДЗС`, 0) AS `ГДЗС`,
  COALESCE(`st`.`всего_отс`, 0) AS `всего_отс`,
  COALESCE(`st`.`Отпуск`, 0) AS `отпуск`,
  COALESCE(`st`.`По_больничному`, 0) AS `по_больничному`,
  COALESCE(`st`.`Командировка`, 0) AS `командировка`,
  COALESCE(`st`.`Некомплект`, 0) AS `некомплект`,
  COALESCE(`st`.`прочие_отс`, 0) AS `прочие_отс`,
  COALESCE(`p`.`пена_расчёт`, 0) AS `пена_расчёт`,
  COALESCE(`p`.`пена_резерв`, 0) AS `пена_резерв`,
  0 AS `порошок_расчёт`,
  0 AS `порошок_резерв`,
  COALESCE(`s`.`ДТ`, 0) AS `ДТ`,
  COALESCE(`s`.`Бензин`, 0) AS `Бензин`,
  COALESCE(`cn`.`nachkar`, 'не указан') AS `начкар`
FROM (((((((`pchs`
  LEFT JOIN `a_sredstva` `s`
    ON ((`pchs`.`id` = `s`.`subdivision_id`)))
  LEFT JOIN `v_sostav` `st`
    ON ((`pchs`.`id` = `st`.`subdivision_id`)))
  LEFT JOIN `v_kostyms` `k`
    ON ((`pchs`.`id` = `k`.`subdivision_id`)))
  LEFT JOIN `v_sizod` `z`
    ON ((`pchs`.`id` = `z`.`subdivision_id`)))
  LEFT JOIN `v_penas` `p`
    ON ((`pchs`.`id` = `p`.`subdivision_id`)))
  LEFT JOIN `psg`
    ON ((`pchs`.`parent` = `psg`.`id`)))
  LEFT JOIN `cache_nachkar` `cn`
    ON ((`cn`.`subdivision_id` = `pchs`.`id`)));

--
-- Создать представление "bpivot"
--
CREATE
DEFINER = 'root'@'localhost'
VIEW bpivot
AS
SELECT
  `grp`.`category_type` AS `category_type`,
  `apivot_mat`.`ПСГ` AS `ПСГ`,
  (CASE WHEN (`grp`.`category_type` = 'main') THEN `apivot_mat`.`ПСГ` WHEN (`grp`.`category_type` = 'gps') THEN '      в т.ч. ГПС' WHEN (`grp`.`category_type` = 'fps') THEN '        в т.ч. по ФПС' WHEN (`grp`.`category_type` = 'vpo') THEN '             ВПО' WHEN (`grp`.`category_type` = 'chpo') THEN '             ЧПО' WHEN (`grp`.`category_type` = 'other') THEN '             другие' WHEN (`grp`.`category_type` = 'asf') THEN 'АСФ' END) AS `ПЧ`,
  (CASE WHEN (`grp`.`category_type` = 'main') THEN 'всего' WHEN (`grp`.`category_type` = 'gps') THEN '      в т.ч. ГПС' WHEN (`grp`.`category_type` = 'fps') THEN '        в т.ч. по ФПС' WHEN (`grp`.`category_type` = 'vpo') THEN '             ВПО' WHEN (`grp`.`category_type` = 'chpo') THEN '             ЧПО' WHEN (`grp`.`category_type` = 'other') THEN '             другие' WHEN (`grp`.`category_type` = 'asf') THEN 'АСФ' END) AS `category`,
  (CASE WHEN (`grp`.`category_type` = 'main') THEN (MIN(`apivot_mat`.`parent`) * 1000) WHEN (`grp`.`category_type` = 'gps') THEN ((MIN(`apivot_mat`.`parent`) * 1000) + 1) WHEN (`grp`.`category_type` = 'fps') THEN ((MIN(`apivot_mat`.`parent`) * 1000) + 2) WHEN (`grp`.`category_type` = 'vpo') THEN ((MIN(`apivot_mat`.`parent`) * 1000) + 3) WHEN (`grp`.`category_type` = 'chpo') THEN ((MIN(`apivot_mat`.`parent`) * 1000) + 4) WHEN (`grp`.`category_type` = 'other') THEN ((MIN(`apivot_mat`.`parent`) * 1000) + 5) WHEN (`grp`.`category_type` = 'asf') THEN MIN(`apivot_mat`.`parent`) END) AS `pch_id`,
  (CASE WHEN (`grp`.`category_type` = 'main') THEN `apivot_mat`.`parent` ELSE MIN(`apivot_mat`.`parent`) END) AS `parent`,
  (CASE WHEN (`grp`.`category_type` = 'gps') THEN -(19) WHEN (`grp`.`category_type` = 'fps') THEN -(18) WHEN (`grp`.`category_type` = 'other') THEN -(17) WHEN (`grp`.`category_type` = 'vpo') THEN -(15) WHEN (`grp`.`category_type` = 'chpo') THEN -(13) WHEN (`grp`.`category_type` = 'main') THEN -(20) WHEN (`grp`.`category_type` = 'asf') THEN -(10) END) AS `norder`,
  1 AS `isitog`,
  NULL AS `datafilled`,
  NULL AS `row_id`,
  SUM(`apivot_mat`.`ac_br`) AS `ac_br`,
  SUM(`apivot_mat`.`ac_rezerv`) AS `ac_rezerv`,
  SUM(`apivot_mat`.`ac_remont`) AS `ac_remont`,
  SUM(`apivot_mat`.`acl_br`) AS `acl_br`,
  SUM(`apivot_mat`.`acl_rezerv`) AS `acl_rezerv`,
  SUM(`apivot_mat`.`acl_remont`) AS `acl_remont`,
  SUM(`apivot_mat`.`ав_br`) AS `ав_br`,
  SUM(`apivot_mat`.`ав_rezerv`) AS `ав_rezerv`,
  SUM(`apivot_mat`.`ав_remont`) AS `ав_remont`,
  SUM(`apivot_mat`.`аса_апп_асм_br`) AS `аса_апп_асм_br`,
  SUM(`apivot_mat`.`аса_апп_асм_rezerv`) AS `аса_апп_асм_rezerv`,
  SUM(`apivot_mat`.`аса_апп_асм_remont`) AS `аса_апп_асм_remont`,
  SUM(`apivot_mat`.`пнс_br`) AS `пнс_br`,
  SUM(`apivot_mat`.`пнс_rezerv`) AS `пнс_rezerv`,
  SUM(`apivot_mat`.`пнс_remont`) AS `пнс_remont`,
  SUM(`apivot_mat`.`al_br`) AS `al_br`,
  SUM(`apivot_mat`.`al_rezerv`) AS `al_rezerv`,
  SUM(`apivot_mat`.`al_remont`) AS `al_remont`,
  SUM(`apivot_mat`.`кп_br`) AS `кп_br`,
  SUM(`apivot_mat`.`кп_rezerv`) AS `кп_rezerv`,
  SUM(`apivot_mat`.`кп_remont`) AS `кп_remont`,
  SUM(`apivot_mat`.`ар_br`) AS `ар_br`,
  SUM(`apivot_mat`.`ар_rezerv`) AS `ар_rezerv`,
  SUM(`apivot_mat`.`ар_remont`) AS `ар_remont`,
  SUM(`apivot_mat`.`асмп_пса_br`) AS `асмп_пса_br`,
  SUM(`apivot_mat`.`асмп_пса_rezerv`) AS `асмп_пса_rezerv`,
  SUM(`apivot_mat`.`асмп_пса_remont`) AS `асмп_пса_remont`,
  SUM(`apivot_mat`.`аш_br`) AS `аш_br`,
  SUM(`apivot_mat`.`аш_rezerv`) AS `аш_rezerv`,
  SUM(`apivot_mat`.`аш_remont`) AS `аш_remont`,
  SUM(`apivot_mat`.`асо_br`) AS `асо_br`,
  SUM(`apivot_mat`.`асо_rezerv`) AS `асо_rezerv`,
  SUM(`apivot_mat`.`асо_remont`) AS `асо_remont`,
  SUM(`apivot_mat`.`укс_абг_br`) AS `укс_абг_br`,
  SUM(`apivot_mat`.`укс_абг_rezerv`) AS `укс_абг_rezerv`,
  SUM(`apivot_mat`.`укс_абг_remont`) AS `укс_абг_remont`,
  SUM(`apivot_mat`.`АСМРХ_br`) AS `АСМРХ_br`,
  SUM(`apivot_mat`.`АСМРХ_rezerv`) AS `АСМРХ_rezerv`,
  SUM(`apivot_mat`.`АВС_br`) AS `АВС_br`,
  SUM(`apivot_mat`.`АВС_rezerv`) AS `АВС_rezerv`,
  SUM(`apivot_mat`.`пож_поезд_корабль_br`) AS `пож_поезд_корабль_br`,
  SUM(`apivot_mat`.`пож_поезд_корабль_rezerv`) AS `пож_поезд_корабль_rezerv`,
  SUM(`apivot_mat`.`пож_поезд_корабль_remont`) AS `пож_поезд_корабль_remont`,
  SUM(`apivot_mat`.`пож_поезд_br`) AS `пож_поезд_br`,
  SUM(`apivot_mat`.`пож_поезд_rezerv`) AS `пож_поезд_rezerv`,
  SUM(`apivot_mat`.`пож_поезд_remont`) AS `пож_поезд_remont`,
  SUM(`apivot_mat`.`пож_корабль_катер_br`) AS `пож_корабль_катер_br`,
  SUM(`apivot_mat`.`пож_корабль_катер_rezerv`) AS `пож_корабль_катер_rezerv`,
  SUM(`apivot_mat`.`пож_корабль_катер_remont`) AS `пож_корабль_катер_remont`,
  SUM(`apivot_mat`.`анр_br`) AS `анр_br`,
  SUM(`apivot_mat`.`анр_rezerv`) AS `анр_rezerv`,
  SUM(`apivot_mat`.`анр_remont`) AS `анр_remont`,
  SUM(`apivot_mat`.`аса_br`) AS `аса_br`,
  SUM(`apivot_mat`.`аса_rezerv`) AS `аса_rezerv`,
  SUM(`apivot_mat`.`аса_remont`) AS `аса_remont`,
  SUM(`apivot_mat`.`tofirst`) AS `tofirst`,
  SUM(`apivot_mat`.`totow`) AS `totow`,
  SUM(`apivot_mat`.`ремонт_основной`) AS `ремонт_основной`,
  SUM(`apivot_mat`.`ремонт_специальной`) AS `ремонт_специальной`,
  SUM(`apivot_mat`.`пожарный_корабль_ремонт`) AS `пожарный_корабль_ремонт`,
  SUM(`apivot_mat`.`плав_средства`) AS `плав_средства`,
  SUM(`apivot_mat`.`болотоходы`) AS `болотоходы`,
  SUM(`apivot_mat`.`мотопомпы`) AS `мотопомпы`,
  SUM(`apivot_mat`.`прочее`) AS `прочее`,
  SUM(`apivot_mat`.`sizod_br`) AS `sizod_br`,
  SUM(`apivot_mat`.`sizod_rezerv`) AS `sizod_rezerv`,
  SUM(`apivot_mat`.`костюмы_Л-1_ТАСК`) AS `костюмы_Л-1_ТАСК`,
  SUM(`apivot_mat`.`костюмы_ТОК`) AS `костюмы_ТОК`,
  SUM(`apivot_mat`.`костюмы_другие`) AS `костюмы_другие`,
  SUM(`apivot_mat`.`ГАСИ_расчёт`) AS `ГАСИ_расчёт`,
  SUM(`apivot_mat`.`ГАСИ_резерв`) AS `ГАСИ_резерв`,
  SUM(`apivot_mat`.`по_списку`) AS `по_списку`,
  SUM(`apivot_mat`.`Налицо`) AS `Налицо`,
  SUM(`apivot_mat`.`всего`) AS `всего`,
  SUM(`apivot_mat`.`резерв`) AS `резерв`,
  SUM(`apivot_mat`.`НК`) AS `НК`,
  SUM(`apivot_mat`.`Диспетчер`) AS `Диспетчер`,
  SUM(`apivot_mat`.`ПНК`) AS `ПНК`,
  SUM(`apivot_mat`.`КО`) AS `КО`,
  SUM(`apivot_mat`.`Водитель`) AS `Водитель`,
  SUM(`apivot_mat`.`Пожарный`) AS `Пожарный`,
  SUM(`apivot_mat`.`ГДЗС`) AS `ГДЗС`,
  SUM(`apivot_mat`.`всего_отс`) AS `всего_отс`,
  SUM(`apivot_mat`.`отпуск`) AS `отпуск`,
  SUM(`apivot_mat`.`по_больничному`) AS `по_больничному`,
  SUM(`apivot_mat`.`командировка`) AS `командировка`,
  SUM(`apivot_mat`.`некомплект`) AS `некомплект`,
  SUM(`apivot_mat`.`прочие_отс`) AS `прочие_отс`,
  SUM(`apivot_mat`.`пена_расчёт`) AS `пена_расчёт`,
  SUM(`apivot_mat`.`пена_резерв`) AS `пена_резерв`,
  SUM(`apivot_mat`.`порошок_расчёт`) AS `порошок_расчёт`,
  SUM(`apivot_mat`.`порошок_резерв`) AS `порошок_резерв`,
  SUM(`apivot_mat`.`ДТ`) AS `ДТ`,
  SUM(`apivot_mat`.`Бензин`) AS `Бензин`,
  (CASE WHEN (`grp`.`category_type` = 'main') THEN COALESCE(`cn`.`nachkar`, 'не указан') WHEN (`grp`.`category_type` = 'gps') THEN ' ' WHEN (`grp`.`category_type` = 'fps') THEN '' WHEN (`grp`.`category_type` = 'vpo') THEN '' WHEN (`grp`.`category_type` = 'chpo') THEN '' WHEN (`grp`.`category_type` = 'other') THEN '' WHEN (`grp`.`category_type` = 'asf') THEN '' END) AS `начкар`
FROM ((`apivot_mat`
  JOIN `grp`)
  LEFT JOIN `cache_nachkar` `cn`
    ON ((`cn`.`subdivision_id` = `apivot_mat`.`pch_id`)))
WHERE (((`grp`.`category_type` = 'main')
AND (`apivot_mat`.`category` <> 'АСФ'))
OR ((`grp`.`category_type` = 'gps')
AND (`apivot_mat`.`category` IN ('ППС', 'ФПС')))
OR ((`grp`.`category_type` = 'fps')
AND (((`apivot_mat`.`category` = 'ФПС')
AND (`apivot_mat`.`parent` <> 1744))
OR (`apivot_mat`.`ПЧ` = 'ПЧ-75')))
OR ((`grp`.`category_type` = 'vpo')
AND (`apivot_mat`.`category` = 'ВПО'))
OR ((`grp`.`category_type` = 'chpo')
AND (`apivot_mat`.`category` = 'ЧПО'))
OR ((`grp`.`category_type` = 'asf')
AND (`apivot_mat`.`category` = 'АСФ'))
OR ((`grp`.`category_type` = 'other')
AND (`apivot_mat`.`category` NOT IN ('ППС', 'ФПС', 'АСФ'))))
GROUP BY `apivot_mat`.`ПСГ`,
         `grp`.`category_type`;

--
-- Создать представление "cpivot"
--
CREATE
DEFINER = 'root'@'localhost'
VIEW cpivot
AS
SELECT
  'main' AS `category_type`,
  'Территориальный' AS `ПСГ`,
  `ptr`.`display_name` AS `ПЧ`,
  `ptr`.`category_display` AS `category`,
  `ptr`.`Id` AS `pch_id`,
  `ptr`.`psg_id` AS `parent`,
  `ptr`.`norder` AS `norder`,
  1 AS `isitog`,
  NULL AS `datafilled`,
  `ptr`.`row_id` AS `row_id`,
  SUM(`bpivot`.`ac_br`) AS `ac_br`,
  SUM(`bpivot`.`ac_rezerv`) AS `ac_rezerv`,
  SUM(`bpivot`.`ac_remont`) AS `ac_remont`,
  SUM(`bpivot`.`acl_br`) AS `acl_br`,
  SUM(`bpivot`.`acl_rezerv`) AS `acl_rezerv`,
  SUM(`bpivot`.`acl_remont`) AS `acl_remont`,
  SUM(`bpivot`.`ав_br`) AS `ав_br`,
  SUM(`bpivot`.`ав_rezerv`) AS `ав_rezerv`,
  SUM(`bpivot`.`ав_remont`) AS `ав_remont`,
  SUM(`bpivot`.`аса_апп_асм_br`) AS `аса_апп_асм_br`,
  SUM(`bpivot`.`аса_апп_асм_rezerv`) AS `аса_апп_асм_rezerv`,
  SUM(`bpivot`.`аса_апп_асм_remont`) AS `аса_апп_асм_remont`,
  SUM(`bpivot`.`пнс_br`) AS `пнс_br`,
  SUM(`bpivot`.`пнс_rezerv`) AS `пнс_rezerv`,
  SUM(`bpivot`.`пнс_remont`) AS `пнс_remont`,
  SUM(`bpivot`.`al_br`) AS `al_br`,
  SUM(`bpivot`.`al_rezerv`) AS `al_rezerv`,
  SUM(`bpivot`.`al_remont`) AS `al_remont`,
  SUM(`bpivot`.`кп_br`) AS `кп_br`,
  SUM(`bpivot`.`кп_rezerv`) AS `кп_rezerv`,
  SUM(`bpivot`.`кп_remont`) AS `кп_remont`,
  SUM(`bpivot`.`ар_br`) AS `ар_br`,
  SUM(`bpivot`.`ар_rezerv`) AS `ар_rezerv`,
  SUM(`bpivot`.`ар_remont`) AS `ар_remont`,
  SUM(`bpivot`.`асмп_пса_br`) AS `асмп_пса_br`,
  SUM(`bpivot`.`асмп_пса_rezerv`) AS `асмп_пса_rezerv`,
  SUM(`bpivot`.`асмп_пса_remont`) AS `асмп_пса_remont`,
  SUM(`bpivot`.`аш_br`) AS `аш_br`,
  SUM(`bpivot`.`аш_rezerv`) AS `аш_rezerv`,
  SUM(`bpivot`.`аш_remont`) AS `аш_remont`,
  SUM(`bpivot`.`асо_br`) AS `асо_br`,
  SUM(`bpivot`.`асо_rezerv`) AS `асо_rezerv`,
  SUM(`bpivot`.`асо_remont`) AS `асо_remont`,
  SUM(`bpivot`.`укс_абг_br`) AS `укс_абг_br`,
  SUM(`bpivot`.`укс_абг_rezerv`) AS `укс_абг_rezerv`,
  SUM(`bpivot`.`укс_абг_remont`) AS `укс_абг_remont`,
  SUM(`bpivot`.`АСМРХ_br`) AS `АСМРХ_br`,
  SUM(`bpivot`.`АСМРХ_rezerv`) AS `АСМРХ_rezerv`,
  SUM(`bpivot`.`АВС_br`) AS `АВС_br`,
  SUM(`bpivot`.`АВС_rezerv`) AS `АВС_rezerv`,
  SUM(`bpivot`.`пож_поезд_корабль_br`) AS `пож_поезд_корабль_br`,
  SUM(`bpivot`.`пож_поезд_корабль_rezerv`) AS `пож_поезд_корабль_rezerv`,
  SUM(`bpivot`.`пож_поезд_корабль_remont`) AS `пож_поезд_корабль_remont`,
  SUM(`bpivot`.`пож_поезд_br`) AS `пож_поезд_br`,
  SUM(`bpivot`.`пож_поезд_rezerv`) AS `пож_поезд_rezerv`,
  SUM(`bpivot`.`пож_поезд_remont`) AS `пож_поезд_remont`,
  SUM(`bpivot`.`пож_корабль_катер_br`) AS `пож_корабль_катер_br`,
  SUM(`bpivot`.`пож_корабль_катер_rezerv`) AS `пож_корабль_катер_rezerv`,
  SUM(`bpivot`.`пож_корабль_катер_remont`) AS `пож_корабль_катер_remont`,
  SUM(`bpivot`.`анр_br`) AS `анр_br`,
  SUM(`bpivot`.`анр_rezerv`) AS `анр_rezerv`,
  SUM(`bpivot`.`анр_remont`) AS `анр_remont`,
  SUM(`bpivot`.`аса_br`) AS `аса_br`,
  SUM(`bpivot`.`аса_rezerv`) AS `аса_rezerv`,
  SUM(`bpivot`.`аса_remont`) AS `аса_remont`,
  SUM(`bpivot`.`tofirst`) AS `tofirst`,
  SUM(`bpivot`.`totow`) AS `totow`,
  SUM(`bpivot`.`ремонт_основной`) AS `ремонт_основной`,
  SUM(`bpivot`.`ремонт_специальной`) AS `ремонт_специальной`,
  SUM(`bpivot`.`пожарный_корабль_ремонт`) AS `пожарный_корабль_ремонт`,
  SUM(`bpivot`.`плав_средства`) AS `плав_средства`,
  SUM(`bpivot`.`болотоходы`) AS `болотоходы`,
  SUM(`bpivot`.`мотопомпы`) AS `мотопомпы`,
  SUM(`bpivot`.`прочее`) AS `прочее`,
  SUM(`bpivot`.`sizod_br`) AS `sizod_br`,
  SUM(`bpivot`.`sizod_rezerv`) AS `sizod_rezerv`,
  SUM(`bpivot`.`костюмы_Л-1_ТАСК`) AS `костюмы_Л-1_ТАСК`,
  SUM(`bpivot`.`костюмы_ТОК`) AS `костюмы_ТОК`,
  SUM(`bpivot`.`костюмы_другие`) AS `костюмы_другие`,
  SUM(`bpivot`.`ГАСИ_расчёт`) AS `ГАСИ_расчёт`,
  SUM(`bpivot`.`ГАСИ_резерв`) AS `ГАСИ_резерв`,
  SUM(`bpivot`.`по_списку`) AS `по_списку`,
  SUM(`bpivot`.`Налицо`) AS `Налицо`,
  SUM(`bpivot`.`всего`) AS `всего`,
  SUM(`bpivot`.`резерв`) AS `резерв`,
  SUM(`bpivot`.`НК`) AS `НК`,
  SUM(`bpivot`.`Диспетчер`) AS `Диспетчер`,
  SUM(`bpivot`.`ПНК`) AS `ПНК`,
  SUM(`bpivot`.`КО`) AS `КО`,
  SUM(`bpivot`.`Водитель`) AS `Водитель`,
  SUM(`bpivot`.`Пожарный`) AS `Пожарный`,
  SUM(`bpivot`.`ГДЗС`) AS `ГДЗС`,
  SUM(`bpivot`.`всего_отс`) AS `всего_отс`,
  SUM(`bpivot`.`отпуск`) AS `отпуск`,
  SUM(`bpivot`.`по_больничному`) AS `по_больничному`,
  SUM(`bpivot`.`командировка`) AS `командировка`,
  SUM(`bpivot`.`некомплект`) AS `некомплект`,
  SUM(`bpivot`.`прочие_отс`) AS `прочие_отс`,
  SUM(`bpivot`.`пена_расчёт`) AS `пена_расчёт`,
  SUM(`bpivot`.`пена_резерв`) AS `пена_резерв`,
  SUM(`bpivot`.`порошок_расчёт`) AS `порошок_расчёт`,
  SUM(`bpivot`.`порошок_резерв`) AS `порошок_резерв`,
  SUM(`bpivot`.`ДТ`) AS `ДТ`,
  SUM(`bpivot`.`Бензин`) AS `Бензин`,
  'не указан' AS `начкар`
FROM (`bpivot`
  JOIN `psg_total_rows` `ptr`
    ON (((`ptr`.`psg_id` = 11)
    AND (`ptr`.`category_type` = `bpivot`.`category_type`))))
WHERE (`ptr`.`category_type` IN ('gps', 'fps', 'vpo', 'chpo', 'asf'))
GROUP BY `ptr`.`Id`
UNION ALL
SELECT
  'main' AS `category_type`,
  'Территориальный' AS `ПСГ`,
  `ptr`.`display_name` AS `ПЧ`,
  `ptr`.`category_display` AS `category`,
  `ptr`.`Id` AS `pch_id`,
  `ptr`.`psg_id` AS `parent`,
  `ptr`.`norder` AS `norder`,
  1 AS `isitog`,
  NULL AS `datafilled`,
  `ptr`.`row_id` AS `row_id`,
  SUM(`apivot_mat`.`ac_br`) AS `ac_br`,
  SUM(`apivot_mat`.`ac_rezerv`) AS `ac_rezerv`,
  SUM(`apivot_mat`.`ac_remont`) AS `ac_remont`,
  SUM(`apivot_mat`.`acl_br`) AS `acl_br`,
  SUM(`apivot_mat`.`acl_rezerv`) AS `acl_rezerv`,
  SUM(`apivot_mat`.`acl_remont`) AS `acl_remont`,
  SUM(`apivot_mat`.`ав_br`) AS `ав_br`,
  SUM(`apivot_mat`.`ав_rezerv`) AS `ав_rezerv`,
  SUM(`apivot_mat`.`ав_remont`) AS `ав_remont`,
  SUM(`apivot_mat`.`аса_апп_асм_br`) AS `аса_апп_асм_br`,
  SUM(`apivot_mat`.`аса_апп_асм_rezerv`) AS `аса_апп_асм_rezerv`,
  SUM(`apivot_mat`.`аса_апп_асм_remont`) AS `аса_апп_асм_remont`,
  SUM(`apivot_mat`.`пнс_br`) AS `пнс_br`,
  SUM(`apivot_mat`.`пнс_rezerv`) AS `пнс_rezerv`,
  SUM(`apivot_mat`.`пнс_remont`) AS `пнс_remont`,
  SUM(`apivot_mat`.`al_br`) AS `al_br`,
  SUM(`apivot_mat`.`al_rezerv`) AS `al_rezerv`,
  SUM(`apivot_mat`.`al_remont`) AS `al_remont`,
  SUM(`apivot_mat`.`кп_br`) AS `кп_br`,
  SUM(`apivot_mat`.`кп_rezerv`) AS `кп_rezerv`,
  SUM(`apivot_mat`.`кп_remont`) AS `кп_remont`,
  SUM(`apivot_mat`.`ар_br`) AS `ар_br`,
  SUM(`apivot_mat`.`ар_rezerv`) AS `ар_rezerv`,
  SUM(`apivot_mat`.`ар_remont`) AS `ар_remont`,
  SUM(`apivot_mat`.`асмп_пса_br`) AS `асмп_пса_br`,
  SUM(`apivot_mat`.`асмп_пса_rezerv`) AS `асмп_пса_rezerv`,
  SUM(`apivot_mat`.`асмп_пса_remont`) AS `асмп_пса_remont`,
  SUM(`apivot_mat`.`аш_br`) AS `аш_br`,
  SUM(`apivot_mat`.`аш_rezerv`) AS `аш_rezerv`,
  SUM(`apivot_mat`.`аш_remont`) AS `аш_remont`,
  SUM(`apivot_mat`.`асо_br`) AS `асо_br`,
  SUM(`apivot_mat`.`асо_rezerv`) AS `асо_rezerv`,
  SUM(`apivot_mat`.`асо_remont`) AS `асо_remont`,
  SUM(`apivot_mat`.`укс_абг_br`) AS `укс_абг_br`,
  SUM(`apivot_mat`.`укс_абг_rezerv`) AS `укс_абг_rezerv`,
  SUM(`apivot_mat`.`укс_абг_remont`) AS `укс_абг_remont`,
  SUM(`apivot_mat`.`АСМРХ_br`) AS `АСМРХ_br`,
  SUM(`apivot_mat`.`АСМРХ_rezerv`) AS `АСМРХ_rezerv`,
  SUM(`apivot_mat`.`АВС_br`) AS `АВС_br`,
  SUM(`apivot_mat`.`АВС_rezerv`) AS `АВС_rezerv`,
  SUM(`apivot_mat`.`пож_поезд_корабль_br`) AS `пож_поезд_корабль_br`,
  SUM(`apivot_mat`.`пож_поезд_корабль_rezerv`) AS `пож_поезд_корабль_rezerv`,
  SUM(`apivot_mat`.`пож_поезд_корабль_remont`) AS `пож_поезд_корабль_remont`,
  SUM(`apivot_mat`.`пож_поезд_br`) AS `пож_поезд_br`,
  SUM(`apivot_mat`.`пож_поезд_rezerv`) AS `пож_поезд_rezerv`,
  SUM(`apivot_mat`.`пож_поезд_remont`) AS `пож_поезд_remont`,
  SUM(`apivot_mat`.`пож_корабль_катер_br`) AS `пож_корабль_катер_br`,
  SUM(`apivot_mat`.`пож_корабль_катер_rezerv`) AS `пож_корабль_катер_rezerv`,
  SUM(`apivot_mat`.`пож_корабль_катер_remont`) AS `пож_корабль_катер_remont`,
  SUM(`apivot_mat`.`анр_br`) AS `анр_br`,
  SUM(`apivot_mat`.`анр_rezerv`) AS `анр_rezerv`,
  SUM(`apivot_mat`.`анр_remont`) AS `анр_remont`,
  SUM(`apivot_mat`.`аса_br`) AS `аса_br`,
  SUM(`apivot_mat`.`аса_rezerv`) AS `аса_rezerv`,
  SUM(`apivot_mat`.`аса_remont`) AS `аса_remont`,
  SUM(`apivot_mat`.`tofirst`) AS `tofirst`,
  SUM(`apivot_mat`.`totow`) AS `totow`,
  SUM(`apivot_mat`.`ремонт_основной`) AS `ремонт_основной`,
  SUM(`apivot_mat`.`ремонт_специальной`) AS `ремонт_специальной`,
  SUM(`apivot_mat`.`пожарный_корабль_ремонт`) AS `пожарный_корабль_ремонт`,
  SUM(`apivot_mat`.`плав_средства`) AS `плав_средства`,
  SUM(`apivot_mat`.`болотоходы`) AS `болотоходы`,
  SUM(`apivot_mat`.`мотопомпы`) AS `мотопомпы`,
  SUM(`apivot_mat`.`прочее`) AS `прочее`,
  SUM(`apivot_mat`.`sizod_br`) AS `sizod_br`,
  SUM(`apivot_mat`.`sizod_rezerv`) AS `sizod_rezerv`,
  SUM(`apivot_mat`.`костюмы_Л-1_ТАСК`) AS `костюмы_Л-1_ТАСК`,
  SUM(`apivot_mat`.`костюмы_ТОК`) AS `костюмы_ТОК`,
  SUM(`apivot_mat`.`костюмы_другие`) AS `костюмы_другие`,
  SUM(`apivot_mat`.`ГАСИ_расчёт`) AS `ГАСИ_расчёт`,
  SUM(`apivot_mat`.`ГАСИ_резерв`) AS `ГАСИ_резерв`,
  SUM(`apivot_mat`.`по_списку`) AS `по_списку`,
  SUM(`apivot_mat`.`Налицо`) AS `Налицо`,
  SUM(`apivot_mat`.`всего`) AS `всего`,
  SUM(`apivot_mat`.`резерв`) AS `резерв`,
  SUM(`apivot_mat`.`НК`) AS `НК`,
  SUM(`apivot_mat`.`Диспетчер`) AS `Диспетчер`,
  SUM(`apivot_mat`.`ПНК`) AS `ПНК`,
  SUM(`apivot_mat`.`КО`) AS `КО`,
  SUM(`apivot_mat`.`Водитель`) AS `Водитель`,
  SUM(`apivot_mat`.`Пожарный`) AS `Пожарный`,
  SUM(`apivot_mat`.`ГДЗС`) AS `ГДЗС`,
  SUM(`apivot_mat`.`всего_отс`) AS `всего_отс`,
  SUM(`apivot_mat`.`отпуск`) AS `отпуск`,
  SUM(`apivot_mat`.`по_больничному`) AS `по_больничному`,
  SUM(`apivot_mat`.`командировка`) AS `командировка`,
  SUM(`apivot_mat`.`некомплект`) AS `некомплект`,
  SUM(`apivot_mat`.`прочие_отс`) AS `прочие_отс`,
  SUM(`apivot_mat`.`пена_расчёт`) AS `пена_расчёт`,
  SUM(`apivot_mat`.`пена_резерв`) AS `пена_резерв`,
  SUM(`apivot_mat`.`порошок_расчёт`) AS `порошок_расчёт`,
  SUM(`apivot_mat`.`порошок_резерв`) AS `порошок_резерв`,
  SUM(`apivot_mat`.`ДТ`) AS `ДТ`,
  SUM(`apivot_mat`.`Бензин`) AS `Бензин`,
  'не указан' AS `начкар`
FROM (`apivot_mat`
  JOIN `psg_total_rows` `ptr`
    ON (((`ptr`.`psg_id` = 11)
    AND (`ptr`.`category_type` = 'other'))))
WHERE (`apivot_mat`.`category` NOT IN ('ППС', 'ФПС', 'АСФ', 'ВПО', 'ЧПО'))
GROUP BY `ptr`.`Id`
UNION ALL
SELECT
  'main' AS `category_type`,
  'Территориальный' AS `ПСГ`,
  `ptr_main`.`display_name` AS `ПЧ`,
  `ptr_main`.`category_display` AS `category`,
  `ptr_main`.`Id` AS `pch_id`,
  `ptr_main`.`psg_id` AS `parent`,
  `ptr_main`.`norder` AS `norder`,
  1 AS `isitog`,
  NULL AS `datafilled`,
  `ptr_main`.`row_id` AS `row_id`,
  SUM(`apivot_mat`.`ac_br`) AS `ac_br`,
  SUM(`apivot_mat`.`ac_rezerv`) AS `ac_rezerv`,
  SUM(`apivot_mat`.`ac_remont`) AS `ac_remont`,
  SUM(`apivot_mat`.`acl_br`) AS `acl_br`,
  SUM(`apivot_mat`.`acl_rezerv`) AS `acl_rezerv`,
  SUM(`apivot_mat`.`acl_remont`) AS `acl_remont`,
  SUM(`apivot_mat`.`ав_br`) AS `ав_br`,
  SUM(`apivot_mat`.`ав_rezerv`) AS `ав_rezerv`,
  SUM(`apivot_mat`.`ав_remont`) AS `ав_remont`,
  SUM(`apivot_mat`.`аса_апп_асм_br`) AS `аса_апп_асм_br`,
  SUM(`apivot_mat`.`аса_апп_асм_rezerv`) AS `аса_апп_асм_rezerv`,
  SUM(`apivot_mat`.`аса_апп_асм_remont`) AS `аса_апп_асм_remont`,
  SUM(`apivot_mat`.`пнс_br`) AS `пнс_br`,
  SUM(`apivot_mat`.`пнс_rezerv`) AS `пнс_rezerv`,
  SUM(`apivot_mat`.`пнс_remont`) AS `пнс_remont`,
  SUM(`apivot_mat`.`al_br`) AS `al_br`,
  SUM(`apivot_mat`.`al_rezerv`) AS `al_rezerv`,
  SUM(`apivot_mat`.`al_remont`) AS `al_remont`,
  SUM(`apivot_mat`.`кп_br`) AS `кп_br`,
  SUM(`apivot_mat`.`кп_rezerv`) AS `кп_rezerv`,
  SUM(`apivot_mat`.`кп_remont`) AS `кп_remont`,
  SUM(`apivot_mat`.`ар_br`) AS `ар_br`,
  SUM(`apivot_mat`.`ар_rezerv`) AS `ар_rezerv`,
  SUM(`apivot_mat`.`ар_remont`) AS `ар_remont`,
  SUM(`apivot_mat`.`асмп_пса_br`) AS `асмп_пса_br`,
  SUM(`apivot_mat`.`асмп_пса_rezerv`) AS `асмп_пса_rezerv`,
  SUM(`apivot_mat`.`асмп_пса_remont`) AS `асмп_пса_remont`,
  SUM(`apivot_mat`.`аш_br`) AS `аш_br`,
  SUM(`apivot_mat`.`аш_rezerv`) AS `аш_rezerv`,
  SUM(`apivot_mat`.`аш_remont`) AS `аш_remont`,
  SUM(`apivot_mat`.`асо_br`) AS `асо_br`,
  SUM(`apivot_mat`.`асо_rezerv`) AS `асо_rezerv`,
  SUM(`apivot_mat`.`асо_remont`) AS `асо_remont`,
  SUM(`apivot_mat`.`укс_абг_br`) AS `укс_абг_br`,
  SUM(`apivot_mat`.`укс_абг_rezerv`) AS `укс_абг_rezerv`,
  SUM(`apivot_mat`.`укс_абг_remont`) AS `укс_абг_remont`,
  SUM(`apivot_mat`.`АСМРХ_br`) AS `АСМРХ_br`,
  SUM(`apivot_mat`.`АСМРХ_rezerv`) AS `АСМРХ_rezerv`,
  SUM(`apivot_mat`.`АВС_br`) AS `АВС_br`,
  SUM(`apivot_mat`.`АВС_rezerv`) AS `АВС_rezerv`,
  SUM(`apivot_mat`.`пож_поезд_корабль_br`) AS `пож_поезд_корабль_br`,
  SUM(`apivot_mat`.`пож_поезд_корабль_rezerv`) AS `пож_поезд_корабль_rezerv`,
  SUM(`apivot_mat`.`пож_поезд_корабль_remont`) AS `пож_поезд_корабль_remont`,
  SUM(`apivot_mat`.`пож_поезд_br`) AS `пож_поезд_br`,
  SUM(`apivot_mat`.`пож_поезд_rezerv`) AS `пож_поезд_rezerv`,
  SUM(`apivot_mat`.`пож_поезд_remont`) AS `пож_поезд_remont`,
  SUM(`apivot_mat`.`пож_корабль_катер_br`) AS `пож_корабль_катер_br`,
  SUM(`apivot_mat`.`пож_корабль_катер_rezerv`) AS `пож_корабль_катер_rezerv`,
  SUM(`apivot_mat`.`пож_корабль_катер_remont`) AS `пож_корабль_катер_remont`,
  SUM(`apivot_mat`.`анр_br`) AS `анр_br`,
  SUM(`apivot_mat`.`анр_rezerv`) AS `анр_rezerv`,
  SUM(`apivot_mat`.`анр_remont`) AS `анр_remont`,
  SUM(`apivot_mat`.`аса_br`) AS `аса_br`,
  SUM(`apivot_mat`.`аса_rezerv`) AS `аса_rezerv`,
  SUM(`apivot_mat`.`аса_remont`) AS `аса_remont`,
  SUM(`apivot_mat`.`tofirst`) AS `tofirst`,
  SUM(`apivot_mat`.`totow`) AS `totow`,
  SUM(`apivot_mat`.`ремонт_основной`) AS `ремонт_основной`,
  SUM(`apivot_mat`.`ремонт_специальной`) AS `ремонт_специальной`,
  SUM(`apivot_mat`.`пожарный_корабль_ремонт`) AS `пожарный_корабль_ремонт`,
  SUM(`apivot_mat`.`плав_средства`) AS `плав_средства`,
  SUM(`apivot_mat`.`болотоходы`) AS `болотоходы`,
  SUM(`apivot_mat`.`мотопомпы`) AS `мотопомпы`,
  SUM(`apivot_mat`.`прочее`) AS `прочее`,
  SUM(`apivot_mat`.`sizod_br`) AS `sizod_br`,
  SUM(`apivot_mat`.`sizod_rezerv`) AS `sizod_rezerv`,
  SUM(`apivot_mat`.`костюмы_Л-1_ТАСК`) AS `костюмы_Л-1_ТАСК`,
  SUM(`apivot_mat`.`костюмы_ТОК`) AS `костюмы_ТОК`,
  SUM(`apivot_mat`.`костюмы_другие`) AS `костюмы_другие`,
  SUM(`apivot_mat`.`ГАСИ_расчёт`) AS `ГАСИ_расчёт`,
  SUM(`apivot_mat`.`ГАСИ_резерв`) AS `ГАСИ_резерв`,
  SUM(`apivot_mat`.`по_списку`) AS `по_списку`,
  SUM(`apivot_mat`.`Налицо`) AS `Налицо`,
  SUM(`apivot_mat`.`всего`) AS `всего`,
  SUM(`apivot_mat`.`резерв`) AS `резерв`,
  SUM(`apivot_mat`.`НК`) AS `НК`,
  SUM(`apivot_mat`.`Диспетчер`) AS `Диспетчер`,
  SUM(`apivot_mat`.`ПНК`) AS `ПНК`,
  SUM(`apivot_mat`.`КО`) AS `КО`,
  SUM(`apivot_mat`.`Водитель`) AS `Водитель`,
  SUM(`apivot_mat`.`Пожарный`) AS `Пожарный`,
  SUM(`apivot_mat`.`ГДЗС`) AS `ГДЗС`,
  SUM(`apivot_mat`.`всего_отс`) AS `всего_отс`,
  SUM(`apivot_mat`.`отпуск`) AS `отпуск`,
  SUM(`apivot_mat`.`по_больничному`) AS `по_больничному`,
  SUM(`apivot_mat`.`командировка`) AS `командировка`,
  SUM(`apivot_mat`.`некомплект`) AS `некомплект`,
  SUM(`apivot_mat`.`прочие_отс`) AS `прочие_отс`,
  SUM(`apivot_mat`.`пена_расчёт`) AS `пена_расчёт`,
  SUM(`apivot_mat`.`пена_резерв`) AS `пена_резерв`,
  SUM(`apivot_mat`.`порошок_расчёт`) AS `порошок_расчёт`,
  SUM(`apivot_mat`.`порошок_резерв`) AS `порошок_резерв`,
  SUM(`apivot_mat`.`ДТ`) AS `ДТ`,
  SUM(`apivot_mat`.`Бензин`) AS `Бензин`,
  COALESCE(`cn`.`nachkar`, 'не указан') AS `начкар`
FROM ((`apivot_mat`
  JOIN `psg_total_rows` `ptr_main`
    ON (((`ptr_main`.`psg_id` = 11)
    AND (`ptr_main`.`category_type` = 'main'))))
  LEFT JOIN `cache_nachkar` `cn`
    ON ((`cn`.`subdivision_id` = `apivot_mat`.`pch_id`)))
WHERE (NOT ((`apivot_mat`.`ПЧ` LIKE '%АСФ%')));

--
-- Создать представление "cpivot_copy"
--
CREATE
DEFINER = 'root'@'localhost'
VIEW cpivot_copy
AS
SELECT
  'main' AS `category_type`,
  'Территориальный' AS `ПСГ`,
  `ptr`.`display_name` AS `ПЧ`,
  `ptr`.`category_display` AS `category`,
  `ptr`.`Id` AS `pch_id`,
  `ptr`.`psg_id` AS `parent`,
  `ptr`.`norder` AS `norder`,
  1 AS `isitog`,
  NULL AS `datafilled`,
  `ptr`.`row_id` AS `row_id`,
  SUM(`bpivot`.`ac_br`) AS `ac_br`,
  SUM(`bpivot`.`ac_rezerv`) AS `ac_rezerv`,
  SUM(`bpivot`.`ac_remont`) AS `ac_remont`,
  SUM(`bpivot`.`acl_br`) AS `acl_br`,
  SUM(`bpivot`.`acl_rezerv`) AS `acl_rezerv`,
  SUM(`bpivot`.`acl_remont`) AS `acl_remont`,
  SUM(`bpivot`.`ав_br`) AS `ав_br`,
  SUM(`bpivot`.`ав_rezerv`) AS `ав_rezerv`,
  SUM(`bpivot`.`ав_remont`) AS `ав_remont`,
  SUM(`bpivot`.`аса_апп_асм_br`) AS `аса_апп_асм_br`,
  SUM(`bpivot`.`аса_апп_асм_rezerv`) AS `аса_апп_асм_rezerv`,
  SUM(`bpivot`.`аса_апп_асм_remont`) AS `аса_апп_асм_remont`,
  SUM(`bpivot`.`пнс_br`) AS `пнс_br`,
  SUM(`bpivot`.`пнс_rezerv`) AS `пнс_rezerv`,
  SUM(`bpivot`.`пнс_remont`) AS `пнс_remont`,
  SUM(`bpivot`.`al_br`) AS `al_br`,
  SUM(`bpivot`.`al_rezerv`) AS `al_rezerv`,
  SUM(`bpivot`.`al_remont`) AS `al_remont`,
  SUM(`bpivot`.`кп_br`) AS `кп_br`,
  SUM(`bpivot`.`кп_rezerv`) AS `кп_rezerv`,
  SUM(`bpivot`.`кп_remont`) AS `кп_remont`,
  SUM(`bpivot`.`ар_br`) AS `ар_br`,
  SUM(`bpivot`.`ар_rezerv`) AS `ар_rezerv`,
  SUM(`bpivot`.`ар_remont`) AS `ар_remont`,
  SUM(`bpivot`.`асмп_пса_br`) AS `асмп_пса_br`,
  SUM(`bpivot`.`асмп_пса_rezerv`) AS `асмп_пса_rezerv`,
  SUM(`bpivot`.`асмп_пса_remont`) AS `асмп_пса_remont`,
  SUM(`bpivot`.`аш_br`) AS `аш_br`,
  SUM(`bpivot`.`аш_rezerv`) AS `аш_rezerv`,
  SUM(`bpivot`.`аш_remont`) AS `аш_remont`,
  SUM(`bpivot`.`асо_br`) AS `асо_br`,
  SUM(`bpivot`.`асо_rezerv`) AS `асо_rezerv`,
  SUM(`bpivot`.`асо_remont`) AS `асо_remont`,
  SUM(`bpivot`.`укс_абг_br`) AS `укс_абг_br`,
  SUM(`bpivot`.`укс_абг_rezerv`) AS `укс_абг_rezerv`,
  SUM(`bpivot`.`укс_абг_remont`) AS `укс_абг_remont`,
  SUM(`bpivot`.`АСМРХ_br`) AS `АСМРХ_br`,
  SUM(`bpivot`.`АСМРХ_rezerv`) AS `АСМРХ_rezerv`,
  SUM(`bpivot`.`АВС_br`) AS `АВС_br`,
  SUM(`bpivot`.`АВС_rezerv`) AS `АВС_rezerv`,
  SUM(`bpivot`.`пож_поезд_корабль_br`) AS `пож_поезд_корабль_br`,
  SUM(`bpivot`.`пож_поезд_корабль_rezerv`) AS `пож_поезд_корабль_rezerv`,
  SUM(`bpivot`.`пож_поезд_корабль_remont`) AS `пож_поезд_корабль_remont`,
  SUM(`bpivot`.`анр_br`) AS `анр_br`,
  SUM(`bpivot`.`анр_rezerv`) AS `анр_rezerv`,
  SUM(`bpivot`.`анр_remont`) AS `анр_remont`,
  SUM(`bpivot`.`аса_br`) AS `аса_br`,
  SUM(`bpivot`.`аса_rezerv`) AS `аса_rezerv`,
  SUM(`bpivot`.`аса_remont`) AS `аса_remont`,
  SUM(`bpivot`.`tofirst`) AS `tofirst`,
  SUM(`bpivot`.`totow`) AS `totow`,
  SUM(`bpivot`.`ремонт_основной`) AS `ремонт_основной`,
  SUM(`bpivot`.`ремонт_специальной`) AS `ремонт_специальной`,
  SUM(`bpivot`.`пожарный_корабль_ремонт`) AS `пожарный_корабль_ремонт`,
  SUM(`bpivot`.`плав_средства`) AS `плав_средства`,
  SUM(`bpivot`.`болотоходы`) AS `болотоходы`,
  SUM(`bpivot`.`мотопомпы`) AS `мотопомпы`,
  SUM(`bpivot`.`прочее`) AS `прочее`,
  SUM(`bpivot`.`sizod_br`) AS `sizod_br`,
  SUM(`bpivot`.`sizod_rezerv`) AS `sizod_rezerv`,
  SUM(`bpivot`.`костюмы_Л-1_ТАСК`) AS `костюмы_Л-1_ТАСК`,
  SUM(`bpivot`.`костюмы_ТОК`) AS `костюмы_ТОК`,
  SUM(`bpivot`.`костюмы_другие`) AS `костюмы_другие`,
  SUM(`bpivot`.`ГАСИ_расчёт`) AS `ГАСИ_расчёт`,
  SUM(`bpivot`.`ГАСИ_резерв`) AS `ГАСИ_резерв`,
  SUM(`bpivot`.`по_списку`) AS `по_списку`,
  SUM(`bpivot`.`Налицо`) AS `Налицо`,
  SUM(`bpivot`.`всего`) AS `всего`,
  SUM(`bpivot`.`резерв`) AS `резерв`,
  SUM(`bpivot`.`НК`) AS `НК`,
  SUM(`bpivot`.`Диспетчер`) AS `Диспетчер`,
  SUM(`bpivot`.`ПНК`) AS `ПНК`,
  SUM(`bpivot`.`КО`) AS `КО`,
  SUM(`bpivot`.`Водитель`) AS `Водитель`,
  SUM(`bpivot`.`Пожарный`) AS `Пожарный`,
  SUM(`bpivot`.`ГДЗС`) AS `ГДЗС`,
  SUM(`bpivot`.`всего_отс`) AS `всего_отс`,
  SUM(`bpivot`.`отпуск`) AS `отпуск`,
  SUM(`bpivot`.`по_больничному`) AS `по_больничному`,
  SUM(`bpivot`.`командировка`) AS `командировка`,
  SUM(`bpivot`.`некомплект`) AS `некомплект`,
  SUM(`bpivot`.`прочие_отс`) AS `прочие_отс`,
  SUM(`bpivot`.`пена_расчёт`) AS `пена_расчёт`,
  SUM(`bpivot`.`пена_резерв`) AS `пена_резерв`,
  SUM(`bpivot`.`порошок_расчёт`) AS `порошок_расчёт`,
  SUM(`bpivot`.`порошок_резерв`) AS `порошок_резерв`,
  SUM(`bpivot`.`ДТ`) AS `ДТ`,
  SUM(`bpivot`.`Бензин`) AS `Бензин`,
  'не указан' AS `начкар`
FROM (`bpivot`
  JOIN `psg_total_rows` `ptr`
    ON (((`ptr`.`psg_id` = 11)
    AND (`ptr`.`category_type` = `bpivot`.`category_type`))))
WHERE (`ptr`.`category_type` IN ('gps', 'fps', 'vpo', 'chpo', 'other', 'asf'))
GROUP BY `ptr`.`Id`
UNION ALL
SELECT
  'main' AS `category_type`,
  'Территориальный' AS `ПСГ`,
  `ptr_main`.`display_name` AS `ПЧ`,
  `ptr_main`.`category_display` AS `category`,
  `ptr_main`.`Id` AS `pch_id`,
  `ptr_main`.`psg_id` AS `parent`,
  `ptr_main`.`norder` AS `norder`,
  1 AS `isitog`,
  NULL AS `datafilled`,
  `ptr_main`.`row_id` AS `row_id`,
  SUM(`apivot_mat`.`ac_br`) AS `ac_br`,
  SUM(`apivot_mat`.`ac_rezerv`) AS `ac_rezerv`,
  SUM(`apivot_mat`.`ac_remont`) AS `ac_remont`,
  SUM(`apivot_mat`.`acl_br`) AS `acl_br`,
  SUM(`apivot_mat`.`acl_rezerv`) AS `acl_rezerv`,
  SUM(`apivot_mat`.`acl_remont`) AS `acl_remont`,
  SUM(`apivot_mat`.`ав_br`) AS `ав_br`,
  SUM(`apivot_mat`.`ав_rezerv`) AS `ав_rezerv`,
  SUM(`apivot_mat`.`ав_remont`) AS `ав_remont`,
  SUM(`apivot_mat`.`аса_апп_асм_br`) AS `аса_апп_асм_br`,
  SUM(`apivot_mat`.`аса_апп_асм_rezerv`) AS `аса_апп_асм_rezerv`,
  SUM(`apivot_mat`.`аса_апп_асм_remont`) AS `аса_апп_асм_remont`,
  SUM(`apivot_mat`.`пнс_br`) AS `пнс_br`,
  SUM(`apivot_mat`.`пнс_rezerv`) AS `пнс_rezerv`,
  SUM(`apivot_mat`.`пнс_remont`) AS `пнс_remont`,
  SUM(`apivot_mat`.`al_br`) AS `al_br`,
  SUM(`apivot_mat`.`al_rezerv`) AS `al_rezerv`,
  SUM(`apivot_mat`.`al_remont`) AS `al_remont`,
  SUM(`apivot_mat`.`кп_br`) AS `кп_br`,
  SUM(`apivot_mat`.`кп_rezerv`) AS `кп_rezerv`,
  SUM(`apivot_mat`.`кп_remont`) AS `кп_remont`,
  SUM(`apivot_mat`.`ар_br`) AS `ар_br`,
  SUM(`apivot_mat`.`ар_rezerv`) AS `ар_rezerv`,
  SUM(`apivot_mat`.`ар_remont`) AS `ар_remont`,
  SUM(`apivot_mat`.`асмп_пса_br`) AS `асмп_пса_br`,
  SUM(`apivot_mat`.`асмп_пса_rezerv`) AS `асмп_пса_rezerv`,
  SUM(`apivot_mat`.`асмп_пса_remont`) AS `асмп_пса_remont`,
  SUM(`apivot_mat`.`аш_br`) AS `аш_br`,
  SUM(`apivot_mat`.`аш_rezerv`) AS `аш_rezerv`,
  SUM(`apivot_mat`.`аш_remont`) AS `аш_remont`,
  SUM(`apivot_mat`.`асо_br`) AS `асо_br`,
  SUM(`apivot_mat`.`асо_rezerv`) AS `асо_rezerv`,
  SUM(`apivot_mat`.`асо_remont`) AS `асо_remont`,
  SUM(`apivot_mat`.`укс_абг_br`) AS `укс_абг_br`,
  SUM(`apivot_mat`.`укс_абг_rezerv`) AS `укс_абг_rezerv`,
  SUM(`apivot_mat`.`укс_абг_remont`) AS `укс_абг_remont`,
  SUM(`apivot_mat`.`АСМРХ_br`) AS `АСМРХ_br`,
  SUM(`apivot_mat`.`АСМРХ_rezerv`) AS `АСМРХ_rezerv`,
  SUM(`apivot_mat`.`АВС_br`) AS `АВС_br`,
  SUM(`apivot_mat`.`АВС_rezerv`) AS `АВС_rezerv`,
  SUM(`apivot_mat`.`пож_поезд_корабль_br`) AS `пож_поезд_корабль_br`,
  SUM(`apivot_mat`.`пож_поезд_корабль_rezerv`) AS `пож_поезд_корабль_rezerv`,
  SUM(`apivot_mat`.`пож_поезд_корабль_remont`) AS `пож_поезд_корабль_remont`,
  SUM(`apivot_mat`.`анр_br`) AS `анр_br`,
  SUM(`apivot_mat`.`анр_rezerv`) AS `анр_rezerv`,
  SUM(`apivot_mat`.`анр_remont`) AS `анр_remont`,
  SUM(`apivot_mat`.`аса_br`) AS `аса_br`,
  SUM(`apivot_mat`.`аса_rezerv`) AS `аса_rezerv`,
  SUM(`apivot_mat`.`аса_remont`) AS `аса_remont`,
  SUM(`apivot_mat`.`tofirst`) AS `tofirst`,
  SUM(`apivot_mat`.`totow`) AS `totow`,
  SUM(`apivot_mat`.`ремонт_основной`) AS `ремонт_основной`,
  SUM(`apivot_mat`.`ремонт_специальной`) AS `ремонт_специальной`,
  SUM(`apivot_mat`.`пожарный_корабль_ремонт`) AS `пожарный_корабль_ремонт`,
  SUM(`apivot_mat`.`плав_средства`) AS `плав_средства`,
  SUM(`apivot_mat`.`болотоходы`) AS `болотоходы`,
  SUM(`apivot_mat`.`мотопомпы`) AS `мотопомпы`,
  SUM(`apivot_mat`.`прочее`) AS `прочее`,
  SUM(`apivot_mat`.`sizod_br`) AS `sizod_br`,
  SUM(`apivot_mat`.`sizod_rezerv`) AS `sizod_rezerv`,
  SUM(`apivot_mat`.`костюмы_Л-1_ТАСК`) AS `костюмы_Л-1_ТАСК`,
  SUM(`apivot_mat`.`костюмы_ТОК`) AS `костюмы_ТОК`,
  SUM(`apivot_mat`.`костюмы_другие`) AS `костюмы_другие`,
  SUM(`apivot_mat`.`ГАСИ_расчёт`) AS `ГАСИ_расчёт`,
  SUM(`apivot_mat`.`ГАСИ_резерв`) AS `ГАСИ_резерв`,
  SUM(`apivot_mat`.`по_списку`) AS `по_списку`,
  SUM(`apivot_mat`.`Налицо`) AS `Налицо`,
  SUM(`apivot_mat`.`всего`) AS `всего`,
  SUM(`apivot_mat`.`резерв`) AS `резерв`,
  SUM(`apivot_mat`.`НК`) AS `НК`,
  SUM(`apivot_mat`.`Диспетчер`) AS `Диспетчер`,
  SUM(`apivot_mat`.`ПНК`) AS `ПНК`,
  SUM(`apivot_mat`.`КО`) AS `КО`,
  SUM(`apivot_mat`.`Водитель`) AS `Водитель`,
  SUM(`apivot_mat`.`Пожарный`) AS `Пожарный`,
  SUM(`apivot_mat`.`ГДЗС`) AS `ГДЗС`,
  SUM(`apivot_mat`.`всего_отс`) AS `всего_отс`,
  SUM(`apivot_mat`.`отпуск`) AS `отпуск`,
  SUM(`apivot_mat`.`по_больничному`) AS `по_больничному`,
  SUM(`apivot_mat`.`командировка`) AS `командировка`,
  SUM(`apivot_mat`.`некомплект`) AS `некомплект`,
  SUM(`apivot_mat`.`прочие_отс`) AS `прочие_отс`,
  SUM(`apivot_mat`.`пена_расчёт`) AS `пена_расчёт`,
  SUM(`apivot_mat`.`пена_резерв`) AS `пена_резерв`,
  SUM(`apivot_mat`.`порошок_расчёт`) AS `порошок_расчёт`,
  SUM(`apivot_mat`.`порошок_резерв`) AS `порошок_резерв`,
  SUM(`apivot_mat`.`ДТ`) AS `ДТ`,
  SUM(`apivot_mat`.`Бензин`) AS `Бензин`,
  COALESCE(`cn`.`nachkar`, 'не указан') AS `начкар`
FROM ((`apivot_mat`
  JOIN `psg_total_rows` `ptr_main`
    ON (((`ptr_main`.`psg_id` = 11)
    AND (`ptr_main`.`category_type` = 'main'))))
  LEFT JOIN `cache_nachkar` `cn`
    ON ((`cn`.`subdivision_id` = `apivot_mat`.`pch_id`)))
WHERE (NOT ((`apivot_mat`.`ПЧ` LIKE '%АСФ%')));

--
-- Создать представление "fire_psg_stat"
--
CREATE
DEFINER = 'root'@'localhost'
VIEW fire_psg_stat
AS
SELECT
  `apivot_mat`.`ПСГ` AS `ПСГ`,
  `apivot_mat`.`ПЧ` AS `ПЧ`,
  `apivot_mat`.`category` AS `category`,
  `apivot_mat`.`pch_id` AS `pch_id`,
  `apivot_mat`.`row_id` AS `row_id1`,
  `apivot_mat`.`parent` AS `parent`,
  `apivot_mat`.`norder` AS `norder`,
  `apivot_mat`.`isitog` AS `isitog`,
  `apivot_mat`.`datafilled` AS `datafilled`,
  `apivot_mat`.`ac_br` AS `ac_br`,
  `apivot_mat`.`ac_rezerv` AS `ac_rezerv`,
  `apivot_mat`.`ac_remont` AS `ac_remont`,
  `apivot_mat`.`acl_br` AS `acl_br`,
  `apivot_mat`.`acl_rezerv` AS `acl_rezerv`,
  `apivot_mat`.`acl_remont` AS `acl_remont`,
  `apivot_mat`.`анр_br` AS `анр_br`,
  `apivot_mat`.`анр_rezerv` AS `анр_rezerv`,
  `apivot_mat`.`анр_remont` AS `анр_remont`,
  `apivot_mat`.`аса_br` AS `аса_br`,
  `apivot_mat`.`аса_rezerv` AS `аса_rezerv`,
  `apivot_mat`.`аса_remont` AS `аса_remont`,
  `apivot_mat`.`асо_br` AS `асо_br`,
  `apivot_mat`.`асо_rezerv` AS `асо_rezerv`,
  `apivot_mat`.`асо_remont` AS `асо_remont`,
  `apivot_mat`.`ав_br` AS `ав_br`,
  `apivot_mat`.`ав_rezerv` AS `ав_rezerv`,
  `apivot_mat`.`ав_remont` AS `ав_remont`,
  `apivot_mat`.`аса_апп_асм_br` AS `аса_апп_асм_br`,
  `apivot_mat`.`аса_апп_асм_rezerv` AS `аса_апп_асм_rezerv`,
  `apivot_mat`.`аса_апп_асм_remont` AS `аса_апп_асм_remont`,
  `apivot_mat`.`пнс_br` AS `пнс_br`,
  `apivot_mat`.`пнс_rezerv` AS `пнс_rezerv`,
  `apivot_mat`.`пнс_remont` AS `пнс_remont`,
  `apivot_mat`.`al_br` AS `al_br`,
  `apivot_mat`.`al_rezerv` AS `al_rezerv`,
  `apivot_mat`.`al_remont` AS `al_remont`,
  `apivot_mat`.`кп_br` AS `кп_br`,
  `apivot_mat`.`кп_rezerv` AS `кп_rezerv`,
  `apivot_mat`.`кп_remont` AS `кп_remont`,
  `apivot_mat`.`ар_br` AS `ар_br`,
  `apivot_mat`.`ар_rezerv` AS `ар_rezerv`,
  `apivot_mat`.`ар_remont` AS `ар_remont`,
  `apivot_mat`.`асмп_пса_br` AS `асмп_пса_br`,
  `apivot_mat`.`асмп_пса_rezerv` AS `асмп_пса_rezerv`,
  `apivot_mat`.`асмп_пса_remont` AS `асмп_пса_remont`,
  `apivot_mat`.`аш_br` AS `аш_br`,
  `apivot_mat`.`аш_rezerv` AS `аш_rezerv`,
  `apivot_mat`.`аш_remont` AS `аш_remont`,
  `apivot_mat`.`укс_абг_br` AS `укс_абг_br`,
  `apivot_mat`.`укс_абг_rezerv` AS `укс_абг_rezerv`,
  `apivot_mat`.`укс_абг_remont` AS `укс_абг_remont`,
  `apivot_mat`.`пож_поезд_корабль_br` AS `пож_поезд_корабль_br`,
  `apivot_mat`.`пож_поезд_корабль_rezerv` AS `пож_поезд_корабль_rezerv`,
  `apivot_mat`.`пож_поезд_корабль_remont` AS `пож_поезд_корабль_remont`,
  `apivot_mat`.`пож_поезд_br` AS `пож_поезд_br`,
  `apivot_mat`.`пож_поезд_rezerv` AS `пож_поезд_rezerv`,
  `apivot_mat`.`пож_поезд_remont` AS `пож_поезд_remont`,
  `apivot_mat`.`пож_корабль_катер_br` AS `пож_корабль_катер_br`,
  `apivot_mat`.`пож_корабль_катер_rezerv` AS `пож_корабль_катер_rezerv`,
  `apivot_mat`.`пож_корабль_катер_remont` AS `пож_корабль_катер_remont`,
  `apivot_mat`.`АСМРХ_br` AS `АСМРХ_br`,
  `apivot_mat`.`АСМРХ_rezerv` AS `АСМРХ_rezerv`,
  `apivot_mat`.`АВС_br` AS `АВС_br`,
  `apivot_mat`.`АВС_rezerv` AS `АВС_rezerv`,
  `apivot_mat`.`ремонт_основной` AS `ремонт_основной`,
  `apivot_mat`.`ремонт_специальной` AS `ремонт_специальной`,
  `apivot_mat`.`пожарный_корабль_ремонт` AS `пожарный_корабль_ремонт`,
  `apivot_mat`.`плав_средства` AS `плав_средства`,
  `apivot_mat`.`болотоходы` AS `болотоходы`,
  `apivot_mat`.`мотопомпы` AS `мотопомпы`,
  `apivot_mat`.`прочее` AS `прочее`,
  `apivot_mat`.`tofirst` AS `tofirst`,
  `apivot_mat`.`totow` AS `totow`,
  `apivot_mat`.`sizod_br` AS `sizod_br`,
  `apivot_mat`.`sizod_rezerv` AS `sizod_rezerv`,
  `apivot_mat`.`костюмы_Л-1_ТАСК` AS `костюмы_Л-1_ТАСК`,
  `apivot_mat`.`костюмы_ТОК` AS `костюмы_ТОК`,
  `apivot_mat`.`костюмы_другие` AS `костюмы_другие`,
  `apivot_mat`.`НК` AS `НК`,
  `apivot_mat`.`Диспетчер` AS `Диспетчер`,
  `apivot_mat`.`ПНК` AS `ПНК`,
  `apivot_mat`.`КО` AS `КО`,
  `apivot_mat`.`Водитель` AS `Водитель`,
  `apivot_mat`.`Пожарный` AS `Пожарный`,
  `apivot_mat`.`ГДЗС` AS `ГДЗС`,
  `apivot_mat`.`по_списку` AS `по_списку`,
  `apivot_mat`.`Налицо` AS `Налицо`,
  `apivot_mat`.`всего` AS `всего`,
  `apivot_mat`.`резерв` AS `резерв`,
  `apivot_mat`.`ГАСИ_расчёт` AS `ГАСИ_расчёт`,
  `apivot_mat`.`ГАСИ_резерв` AS `ГАСИ_резерв`,
  `apivot_mat`.`всего_отс` AS `всего_отс`,
  `apivot_mat`.`отпуск` AS `отпуск`,
  `apivot_mat`.`по_больничному` AS `по_больничному`,
  `apivot_mat`.`командировка` AS `командировка`,
  `apivot_mat`.`некомплект` AS `некомплект`,
  `apivot_mat`.`прочие_отс` AS `прочие_отс`,
  `apivot_mat`.`пена_расчёт` AS `пена_расчёт`,
  `apivot_mat`.`пена_резерв` AS `пена_резерв`,
  `apivot_mat`.`порошок_расчёт` AS `порошок_расчёт`,
  `apivot_mat`.`порошок_резерв` AS `порошок_резерв`,
  `apivot_mat`.`ДТ` AS `ДТ`,
  `apivot_mat`.`Бензин` AS `Бензин`,
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
  `t`.`ac_br` AS `ac_br`,
  `t`.`ac_rezerv` AS `ac_rezerv`,
  `t`.`ac_remont` AS `ac_remont`,
  `t`.`acl_br` AS `acl_br`,
  `t`.`acl_rezerv` AS `acl_rezerv`,
  `t`.`acl_remont` AS `acl_remont`,
  `t`.`анр_br` AS `анр_br`,
  `t`.`анр_rezerv` AS `анр_rezerv`,
  `t`.`анр_remont` AS `анр_remont`,
  `t`.`аса_br` AS `аса_br`,
  `t`.`аса_rezerv` AS `аса_rezerv`,
  `t`.`аса_remont` AS `аса_remont`,
  `t`.`асо_br` AS `асо_br`,
  `t`.`асо_rezerv` AS `асо_rezerv`,
  `t`.`асо_remont` AS `асо_remont`,
  `t`.`ав_br` AS `ав_br`,
  `t`.`ав_rezerv` AS `ав_rezerv`,
  `t`.`ав_remont` AS `ав_remont`,
  `t`.`аса_апп_асм_br` AS `аса_апп_асм_br`,
  `t`.`аса_апп_асм_rezerv` AS `аса_апп_асм_rezerv`,
  `t`.`аса_апп_асм_remont` AS `аса_апп_асм_remont`,
  `t`.`пнс_br` AS `пнс_br`,
  `t`.`пнс_rezerv` AS `пнс_rezerv`,
  `t`.`пнс_remont` AS `пнс_remont`,
  `t`.`al_br` AS `al_br`,
  `t`.`al_rezerv` AS `al_rezerv`,
  `t`.`al_remont` AS `al_remont`,
  `t`.`кп_br` AS `кп_br`,
  `t`.`кп_rezerv` AS `кп_rezerv`,
  `t`.`кп_remont` AS `кп_remont`,
  `t`.`ар_br` AS `ар_br`,
  `t`.`ар_rezerv` AS `ар_rezerv`,
  `t`.`ар_remont` AS `ар_remont`,
  `t`.`асмп_пса_br` AS `асмп_пса_br`,
  `t`.`асмп_пса_rezerv` AS `асмп_пса_rezerv`,
  `t`.`асмп_пса_remont` AS `асмп_пса_remont`,
  `t`.`аш_br` AS `аш_br`,
  `t`.`аш_rezerv` AS `аш_rezerv`,
  `t`.`аш_remont` AS `аш_remont`,
  `t`.`укс_абг_br` AS `укс_абг_br`,
  `t`.`укс_абг_rezerv` AS `укс_абг_rezerv`,
  `t`.`укс_абг_remont` AS `укс_абг_remont`,
  `t`.`пож_поезд_корабль_br` AS `пож_поезд_корабль_br`,
  `t`.`пож_поезд_корабль_rezerv` AS `пож_поезд_корабль_rezerv`,
  `t`.`пож_поезд_корабль_remont` AS `пож_поезд_корабль_remont`,
  `t`.`пож_поезд_br` AS `пож_поезд_br`,
  `t`.`пож_поезд_rezerv` AS `пож_поезд_rezerv`,
  `t`.`пож_поезд_remont` AS `пож_поезд_remont`,
  `t`.`пож_корабль_катер_br` AS `пож_корабль_катер_br`,
  `t`.`пож_корабль_катер_rezerv` AS `пож_корабль_катер_rezerv`,
  `t`.`пож_корабль_катер_remont` AS `пож_корабль_катер_remont`,
  `t`.`АСМРХ_br` AS `АСМРХ_br`,
  `t`.`АСМРХ_rezerv` AS `АСМРХ_rezerv`,
  `t`.`АВС_br` AS `АВС_br`,
  `t`.`АВС_rezerv` AS `АВС_rezerv`,
  `t`.`ремонт_основной` AS `ремонт_основной`,
  `t`.`ремонт_специальной` AS `ремонт_специальной`,
  `t`.`пожарный_корабль_ремонт` AS `пожарный_корабль_ремонт`,
  `t`.`плав_средства` AS `плав_средства`,
  `t`.`болотоходы` AS `болотоходы`,
  `t`.`мотопомпы` AS `мотопомпы`,
  `t`.`прочее` AS `прочее`,
  `t`.`tofirst` AS `tofirst`,
  `t`.`totow` AS `totow`,
  `t`.`sizod_br` AS `sizod_br`,
  `t`.`sizod_rezerv` AS `sizod_rezerv`,
  `t`.`костюмы_Л-1_ТАСК` AS `костюмы_Л-1_ТАСК`,
  `t`.`костюмы_ТОК` AS `костюмы_ТОК`,
  `t`.`костюмы_другие` AS `костюмы_другие`,
  `t`.`НК` AS `НК`,
  `t`.`Диспетчер` AS `Диспетчер`,
  `t`.`ПНК` AS `ПНК`,
  `t`.`КО` AS `КО`,
  `t`.`Водитель` AS `Водитель`,
  `t`.`Пожарный` AS `Пожарный`,
  `t`.`ГДЗС` AS `ГДЗС`,
  `t`.`по_списку` AS `по_списку`,
  `t`.`Налицо` AS `Налицо`,
  `t`.`всего` AS `всего`,
  `t`.`резерв` AS `резерв`,
  `t`.`ГАСИ_расчёт` AS `ГАСИ_расчёт`,
  `t`.`ГАСИ_резерв` AS `ГАСИ_резерв`,
  `t`.`всего_отс` AS `всего_отс`,
  `t`.`отпуск` AS `отпуск`,
  `t`.`по_больничному` AS `по_больничному`,
  `t`.`командировка` AS `командировка`,
  `t`.`некомплект` AS `некомплект`,
  `t`.`прочие_отс` AS `прочие_отс`,
  `t`.`пена_расчёт` AS `пена_расчёт`,
  `t`.`пена_резерв` AS `пена_резерв`,
  `t`.`порошок_расчёт` AS `порошок_расчёт`,
  `t`.`порошок_резерв` AS `порошок_резерв`,
  `t`.`ДТ` AS `ДТ`,
  `t`.`Бензин` AS `Бензин`,
  `t`.`начкар` AS `начкар`
FROM `titogs` `t`;

--
-- Создать представление "grp"
--
CREATE
DEFINER = 'root'@'localhost'
VIEW grp
AS
SELECT
  'main' AS `category_type`
UNION ALL
SELECT
  'gps' AS `gps`
UNION ALL
SELECT
  'fps' AS `fps`
UNION ALL
SELECT
  'vpo' AS `vpo`
UNION ALL
SELECT
  'chpo' AS `chpo`
UNION ALL
SELECT
  'other' AS `other`
UNION ALL
SELECT
  'asf' AS `asf`;

--
-- Создать представление "titogs"
--
CREATE
DEFINER = 'root'@'localhost'
VIEW titogs
AS
SELECT
  `b`.`category_type` AS `category_type`,
  `b`.`ПСГ` AS `ПСГ`,
  `b`.`ПЧ` AS `ПЧ`,
  `b`.`category` AS `category`,
  `b`.`pch_id` AS `pch_id`,
  `b`.`parent` AS `parent`,
  `b`.`norder` AS `norder`,
  `b`.`isitog` AS `isitog`,
  `b`.`datafilled` AS `datafilled`,
  COALESCE(`b`.`row_id`, `psg`.`row_id`) AS `row_id`,
  `b`.`ac_br` AS `ac_br`,
  `b`.`ac_rezerv` AS `ac_rezerv`,
  `b`.`ac_remont` AS `ac_remont`,
  `b`.`acl_br` AS `acl_br`,
  `b`.`acl_rezerv` AS `acl_rezerv`,
  `b`.`acl_remont` AS `acl_remont`,
  `b`.`ав_br` AS `ав_br`,
  `b`.`ав_rezerv` AS `ав_rezerv`,
  `b`.`ав_remont` AS `ав_remont`,
  `b`.`аса_апп_асм_br` AS `аса_апп_асм_br`,
  `b`.`аса_апп_асм_rezerv` AS `аса_апп_асм_rezerv`,
  `b`.`аса_апп_асм_remont` AS `аса_апп_асм_remont`,
  `b`.`пнс_br` AS `пнс_br`,
  `b`.`пнс_rezerv` AS `пнс_rezerv`,
  `b`.`пнс_remont` AS `пнс_remont`,
  `b`.`al_br` AS `al_br`,
  `b`.`al_rezerv` AS `al_rezerv`,
  `b`.`al_remont` AS `al_remont`,
  `b`.`кп_br` AS `кп_br`,
  `b`.`кп_rezerv` AS `кп_rezerv`,
  `b`.`кп_remont` AS `кп_remont`,
  `b`.`ар_br` AS `ар_br`,
  `b`.`ар_rezerv` AS `ар_rezerv`,
  `b`.`ар_remont` AS `ар_remont`,
  `b`.`асмп_пса_br` AS `асмп_пса_br`,
  `b`.`асмп_пса_rezerv` AS `асмп_пса_rezerv`,
  `b`.`асмп_пса_remont` AS `асмп_пса_remont`,
  `b`.`аш_br` AS `аш_br`,
  `b`.`аш_rezerv` AS `аш_rezerv`,
  `b`.`аш_remont` AS `аш_remont`,
  `b`.`асо_br` AS `асо_br`,
  `b`.`асо_rezerv` AS `асо_rezerv`,
  `b`.`асо_remont` AS `асо_remont`,
  `b`.`укс_абг_br` AS `укс_абг_br`,
  `b`.`укс_абг_rezerv` AS `укс_абг_rezerv`,
  `b`.`укс_абг_remont` AS `укс_абг_remont`,
  `b`.`АСМРХ_br` AS `АСМРХ_br`,
  `b`.`АСМРХ_rezerv` AS `АСМРХ_rezerv`,
  `b`.`АВС_br` AS `АВС_br`,
  `b`.`АВС_rezerv` AS `АВС_rezerv`,
  `b`.`пож_поезд_корабль_br` AS `пож_поезд_корабль_br`,
  `b`.`пож_поезд_корабль_rezerv` AS `пож_поезд_корабль_rezerv`,
  `b`.`пож_поезд_корабль_remont` AS `пож_поезд_корабль_remont`,
  `b`.`пож_поезд_br` AS `пож_поезд_br`,
  `b`.`пож_поезд_rezerv` AS `пож_поезд_rezerv`,
  `b`.`пож_поезд_remont` AS `пож_поезд_remont`,
  `b`.`пож_корабль_катер_br` AS `пож_корабль_катер_br`,
  `b`.`пож_корабль_катер_rezerv` AS `пож_корабль_катер_rezerv`,
  `b`.`пож_корабль_катер_remont` AS `пож_корабль_катер_remont`,
  `b`.`анр_br` AS `анр_br`,
  `b`.`анр_rezerv` AS `анр_rezerv`,
  `b`.`анр_remont` AS `анр_remont`,
  `b`.`аса_br` AS `аса_br`,
  `b`.`аса_rezerv` AS `аса_rezerv`,
  `b`.`аса_remont` AS `аса_remont`,
  `b`.`tofirst` AS `tofirst`,
  `b`.`totow` AS `totow`,
  `b`.`ремонт_основной` AS `ремонт_основной`,
  `b`.`ремонт_специальной` AS `ремонт_специальной`,
  `b`.`пожарный_корабль_ремонт` AS `пожарный_корабль_ремонт`,
  `b`.`плав_средства` AS `плав_средства`,
  `b`.`болотоходы` AS `болотоходы`,
  `b`.`мотопомпы` AS `мотопомпы`,
  `b`.`прочее` AS `прочее`,
  `b`.`sizod_br` AS `sizod_br`,
  `b`.`sizod_rezerv` AS `sizod_rezerv`,
  `b`.`костюмы_Л-1_ТАСК` AS `костюмы_Л-1_ТАСК`,
  `b`.`костюмы_ТОК` AS `костюмы_ТОК`,
  `b`.`костюмы_другие` AS `костюмы_другие`,
  `b`.`ГАСИ_расчёт` AS `ГАСИ_расчёт`,
  `b`.`ГАСИ_резерв` AS `ГАСИ_резерв`,
  `b`.`по_списку` AS `по_списку`,
  `b`.`Налицо` AS `Налицо`,
  `b`.`всего` AS `всего`,
  `b`.`резерв` AS `резерв`,
  `b`.`НК` AS `НК`,
  `b`.`Диспетчер` AS `Диспетчер`,
  `b`.`ПНК` AS `ПНК`,
  `b`.`КО` AS `КО`,
  `b`.`Водитель` AS `Водитель`,
  `b`.`Пожарный` AS `Пожарный`,
  `b`.`ГДЗС` AS `ГДЗС`,
  `b`.`всего_отс` AS `всего_отс`,
  `b`.`отпуск` AS `отпуск`,
  `b`.`по_больничному` AS `по_больничному`,
  `b`.`командировка` AS `командировка`,
  `b`.`некомплект` AS `некомплект`,
  `b`.`прочие_отс` AS `прочие_отс`,
  `b`.`пена_расчёт` AS `пена_расчёт`,
  `b`.`пена_резерв` AS `пена_резерв`,
  `b`.`порошок_расчёт` AS `порошок_расчёт`,
  `b`.`порошок_резерв` AS `порошок_резерв`,
  `b`.`ДТ` AS `ДТ`,
  `b`.`Бензин` AS `Бензин`,
  `b`.`начкар` AS `начкар`
FROM (`bpivot` `b`
  LEFT JOIN `psg_total_rows` `psg`
    ON (((`b`.`parent` = `psg`.`psg_id`)
    AND (`b`.`category_type` = `psg`.`category_type`)
    AND ISNULL(`b`.`row_id`))))
WHERE (NOT ((`b`.`category_type` LIKE '%asf%')))
UNION ALL
SELECT
  `c`.`category_type` AS `category_type`,
  `c`.`ПСГ` AS `ПСГ`,
  `c`.`ПЧ` AS `ПЧ`,
  `c`.`category` AS `category`,
  `c`.`pch_id` AS `pch_id`,
  `c`.`parent` AS `parent`,
  `c`.`norder` AS `norder`,
  `c`.`isitog` AS `isitog`,
  `c`.`datafilled` AS `datafilled`,
  `c`.`row_id` AS `row_id`,
  `c`.`ac_br` AS `ac_br`,
  `c`.`ac_rezerv` AS `ac_rezerv`,
  `c`.`ac_remont` AS `ac_remont`,
  `c`.`acl_br` AS `acl_br`,
  `c`.`acl_rezerv` AS `acl_rezerv`,
  `c`.`acl_remont` AS `acl_remont`,
  `c`.`ав_br` AS `ав_br`,
  `c`.`ав_rezerv` AS `ав_rezerv`,
  `c`.`ав_remont` AS `ав_remont`,
  `c`.`аса_апп_асм_br` AS `аса_апп_асм_br`,
  `c`.`аса_апп_асм_rezerv` AS `аса_апп_асм_rezerv`,
  `c`.`аса_апп_асм_remont` AS `аса_апп_асм_remont`,
  `c`.`пнс_br` AS `пнс_br`,
  `c`.`пнс_rezerv` AS `пнс_rezerv`,
  `c`.`пнс_remont` AS `пнс_remont`,
  `c`.`al_br` AS `al_br`,
  `c`.`al_rezerv` AS `al_rezerv`,
  `c`.`al_remont` AS `al_remont`,
  `c`.`кп_br` AS `кп_br`,
  `c`.`кп_rezerv` AS `кп_rezerv`,
  `c`.`кп_remont` AS `кп_remont`,
  `c`.`ар_br` AS `ар_br`,
  `c`.`ар_rezerv` AS `ар_rezerv`,
  `c`.`ар_remont` AS `ар_remont`,
  `c`.`асмп_пса_br` AS `асмп_пса_br`,
  `c`.`асмп_пса_rezerv` AS `асмп_пса_rezerv`,
  `c`.`асмп_пса_remont` AS `асмп_пса_remont`,
  `c`.`аш_br` AS `аш_br`,
  `c`.`аш_rezerv` AS `аш_rezerv`,
  `c`.`аш_remont` AS `аш_remont`,
  `c`.`асо_br` AS `асо_br`,
  `c`.`асо_rezerv` AS `асо_rezerv`,
  `c`.`асо_remont` AS `асо_remont`,
  `c`.`укс_абг_br` AS `укс_абг_br`,
  `c`.`укс_абг_rezerv` AS `укс_абг_rezerv`,
  `c`.`укс_абг_remont` AS `укс_абг_remont`,
  `c`.`АСМРХ_br` AS `АСМРХ_br`,
  `c`.`АСМРХ_rezerv` AS `АСМРХ_rezerv`,
  `c`.`АВС_br` AS `АВС_br`,
  `c`.`АВС_rezerv` AS `АВС_rezerv`,
  `c`.`пож_поезд_корабль_br` AS `пож_поезд_корабль_br`,
  `c`.`пож_поезд_корабль_rezerv` AS `пож_поезд_корабль_rezerv`,
  `c`.`пож_поезд_корабль_remont` AS `пож_поезд_корабль_remont`,
  `c`.`пож_поезд_br` AS `пож_поезд_br`,
  `c`.`пож_поезд_rezerv` AS `пож_поезд_rezerv`,
  `c`.`пож_поезд_remont` AS `пож_поезд_remont`,
  `c`.`пож_корабль_катер_br` AS `пож_корабль_катер_br`,
  `c`.`пож_корабль_катер_rezerv` AS `пож_корабль_катер_rezerv`,
  `c`.`пож_корабль_катер_remont` AS `пож_корабль_катер_remont`,
  `c`.`анр_br` AS `анр_br`,
  `c`.`анр_rezerv` AS `анр_rezerv`,
  `c`.`анр_remont` AS `анр_remont`,
  `c`.`аса_br` AS `аса_br`,
  `c`.`аса_rezerv` AS `аса_rezerv`,
  `c`.`аса_remont` AS `аса_remont`,
  `c`.`tofirst` AS `tofirst`,
  `c`.`totow` AS `totow`,
  `c`.`ремонт_основной` AS `ремонт_основной`,
  `c`.`ремонт_специальной` AS `ремонт_специальной`,
  `c`.`пожарный_корабль_ремонт` AS `пожарный_корабль_ремонт`,
  `c`.`плав_средства` AS `плав_средства`,
  `c`.`болотоходы` AS `болотоходы`,
  `c`.`мотопомпы` AS `мотопомпы`,
  `c`.`прочее` AS `прочее`,
  `c`.`sizod_br` AS `sizod_br`,
  `c`.`sizod_rezerv` AS `sizod_rezerv`,
  `c`.`костюмы_Л-1_ТАСК` AS `костюмы_Л-1_ТАСК`,
  `c`.`костюмы_ТОК` AS `костюмы_ТОК`,
  `c`.`костюмы_другие` AS `костюмы_другие`,
  `c`.`ГАСИ_расчёт` AS `ГАСИ_расчёт`,
  `c`.`ГАСИ_резерв` AS `ГАСИ_резерв`,
  `c`.`по_списку` AS `по_списку`,
  `c`.`Налицо` AS `Налицо`,
  `c`.`всего` AS `всего`,
  `c`.`резерв` AS `резерв`,
  `c`.`НК` AS `НК`,
  `c`.`Диспетчер` AS `Диспетчер`,
  `c`.`ПНК` AS `ПНК`,
  `c`.`КО` AS `КО`,
  `c`.`Водитель` AS `Водитель`,
  `c`.`Пожарный` AS `Пожарный`,
  `c`.`ГДЗС` AS `ГДЗС`,
  `c`.`всего_отс` AS `всего_отс`,
  `c`.`отпуск` AS `отпуск`,
  `c`.`по_больничному` AS `по_больничному`,
  `c`.`командировка` AS `командировка`,
  `c`.`некомплект` AS `некомплект`,
  `c`.`прочие_отс` AS `прочие_отс`,
  `c`.`пена_расчёт` AS `пена_расчёт`,
  `c`.`пена_резерв` AS `пена_резерв`,
  `c`.`порошок_расчёт` AS `порошок_расчёт`,
  `c`.`порошок_резерв` AS `порошок_резерв`,
  `c`.`ДТ` AS `ДТ`,
  `c`.`Бензин` AS `Бензин`,
  `c`.`начкар` AS `начкар`
FROM `cpivot` `c`;

--
-- Создать представление "v_kostyms"
--
CREATE
DEFINER = 'root'@'localhost'
VIEW v_kostyms
AS
SELECT
  `k`.`subdivision_id` AS `subdivision_id`,
  SUM((CASE WHEN (`k`.`mname` IN ('Л-1', 'ТАСК', 'ОЗК')) THEN `k`.`n` ELSE 0 END)) AS `костюмы_Л_1_ТАСК`,
  SUM((CASE WHEN (`k`.`mname` = 'ТОК') THEN `k`.`n` ELSE 0 END)) AS `костюмы_ТОК`,
  SUM((CASE WHEN (`k`.`mname` NOT IN ('Л-1', 'ТАСК', 'ОЗК', 'ТОК')) THEN `k`.`n` ELSE 0 END)) AS `костюмы_другие`
FROM (`kostyms` `k`
  JOIN `pchs` `p`
    ON ((`k`.`subdivision_id` = `p`.`id`)))
GROUP BY `k`.`subdivision_id`;

--
-- Создать представление "v_penas"
--
CREATE
DEFINER = 'root'@'localhost'
VIEW v_penas
AS
SELECT
  `pe`.`subdivision_id` AS `subdivision_id`,
  SUM((CASE WHEN (`pe`.`mname` = 'Пенообразователь') THEN `pe`.`inwork` ELSE 0 END)) AS `пена_расчёт`,
  SUM((CASE WHEN (`pe`.`mname` = 'Пенообразователь') THEN `pe`.`inrezerv` ELSE 0 END)) AS `пена_резерв`
FROM (`penas` `pe`
  JOIN `pchs` `p`
    ON ((`pe`.`subdivision_id` = `p`.`id`)))
GROUP BY `pe`.`subdivision_id`;

--
-- Создать представление "v_sizod"
--
CREATE
DEFINER = 'root'@'localhost'
VIEW v_sizod
AS
SELECT
  `z`.`subdivision_id` AS `subdivision_id`,
  SUM(`z`.`raschet`) AS `sizod_br`,
  SUM(`z`.`rezerv`) AS `sizod_rezerv`
FROM (`sizod` `z`
  JOIN `pchs` `p`
    ON ((`z`.`subdivision_id` = `p`.`id`)))
GROUP BY `z`.`subdivision_id`;

--
-- Создать представление "v_sostav"
--
CREATE
DEFINER = 'root'@'localhost'
VIEW v_sostav
AS
SELECT
  `v_sostav_base`.`subdivision_id` AS `subdivision_id`,
  `v_sostav_base`.`НК` AS `НК`,
  `v_sostav_base`.`Диспетчер` AS `Диспетчер`,
  `v_sostav_base`.`ПНК` AS `ПНК`,
  `v_sostav_base`.`КО` AS `КО`,
  `v_sostav_base`.`Водитель` AS `Водитель`,
  `v_sostav_base`.`Пожарный` AS `Пожарный`,
  `v_sostav_base`.`ГДЗС` AS `ГДЗС`,
  `v_sostav_base`.`Отпуск` AS `Отпуск`,
  `v_sostav_base`.`По_больничному` AS `По_больничному`,
  `v_sostav_base`.`Командировка` AS `Командировка`,
  `v_sostav_base`.`Некомплект` AS `Некомплект`,
  `v_sostav_base`.`По_списку` AS `По_списку`,
  `v_sostav_base`.`резерв` AS `резерв`,
  `v_sostav_base`.`sum_boevoi` AS `Всего`,
  ((`v_sostav_base`.`sum_boevoi` + `v_sostav_base`.`НК`) + `v_sostav_base`.`Диспетчер`) AS `Налицо`,
  (`v_sostav_base`.`По_списку` - ((`v_sostav_base`.`sum_boevoi` + `v_sostav_base`.`НК`) + `v_sostav_base`.`Диспетчер`)) AS `всего_отс`,
  ((((`v_sostav_base`.`По_списку` - ((`v_sostav_base`.`sum_boevoi` + `v_sostav_base`.`НК`) + `v_sostav_base`.`Диспетчер`)) - `v_sostav_base`.`Отпуск`) - `v_sostav_base`.`По_больничному`) - `v_sostav_base`.`Командировка`) AS `прочие_отс`
FROM `v_sostav_base`;

--
-- Создать представление "v_sostav_base"
--
CREATE
DEFINER = 'root'@'localhost'
VIEW v_sostav_base
AS
SELECT
  `st`.`subdivision_id` AS `subdivision_id`,
  MAX((CASE WHEN ((`st`.`name` = 'НК') AND
      (`st`.`sostav_vid` = '2 Боевой расчет')) THEN `st`.`count` ELSE 0 END)) AS `НК`,
  MAX((CASE WHEN ((`st`.`name` = 'Диспетчер') AND
      (`st`.`sostav_vid` = '2 Боевой расчет')) THEN `st`.`count` ELSE 0 END)) AS `Диспетчер`,
  MAX((CASE WHEN ((`st`.`name` = 'ПНК') AND
      (`st`.`sostav_vid` = '2 Боевой расчет')) THEN `st`.`count` ELSE 0 END)) AS `ПНК`,
  MAX((CASE WHEN ((`st`.`name` = 'КО') AND
      (`st`.`sostav_vid` = '2 Боевой расчет')) THEN `st`.`count` ELSE 0 END)) AS `КО`,
  MAX((CASE WHEN ((`st`.`name` = 'Водители') AND
      (`st`.`sostav_vid` = '2 Боевой расчет')) THEN `st`.`count` ELSE 0 END)) AS `Водитель`,
  MAX((CASE WHEN ((`st`.`name` = 'Пожарные') AND
      (`st`.`sostav_vid` = '2 Боевой расчет')) THEN `st`.`count` ELSE 0 END)) AS `Пожарный`,
  SUM((CASE WHEN ((`st`.`sostav_vid` = '3 ГДЗС') AND
      (`st`.`name` IN ('НК', 'ПНК', 'КО', 'Водители', 'Пожарные'))) THEN `st`.`count` ELSE 0 END)) AS `ГДЗС`,
  MAX((CASE WHEN ((`st`.`name` = 'Отпуск') AND
      (`st`.`sostav_vid` = '4 Отсутствует')) THEN `st`.`count` ELSE 0 END)) AS `Отпуск`,
  MAX((CASE WHEN ((`st`.`name` = 'По больничному') AND
      (`st`.`sostav_vid` = '4 Отсутствует')) THEN `st`.`count` ELSE 0 END)) AS `По_больничному`,
  MAX((CASE WHEN ((`st`.`name` = 'Командировка') AND
      (`st`.`sostav_vid` = '4 Отсутствует')) THEN `st`.`count` ELSE 0 END)) AS `Командировка`,
  MAX((CASE WHEN ((`st`.`name` = 'Некомплект') AND
      (`st`.`sostav_vid` = '4 Отсутствует')) THEN `st`.`count` ELSE 0 END)) AS `Некомплект`,
  MAX((CASE WHEN ((`st`.`name` = 'резерв') AND
      (`st`.`sostav_vid` = '4 Отсутствует')) THEN `st`.`count` ELSE 0 END)) AS `резерв`,
  MAX((CASE WHEN (`st`.`name` = 'По списку') THEN `p`.`По_списку` ELSE 0 END)) AS `По_списку`,
  SUM((CASE WHEN ((`st`.`sostav_vid` = '2 Боевой расчет') AND
      (`st`.`name` IN ('ПНК', 'КО', 'Водители', 'Пожарные'))) THEN `st`.`count` ELSE 0 END)) AS `sum_boevoi`
FROM (`sostav` `st`
  JOIN `psgdata` `p`
    ON ((`st`.`subdivision_id` = `p`.`id`)))
GROUP BY `st`.`subdivision_id`;
-- 
-- Восстановить предыдущий режим SQL (SQL mode)
-- 
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;

-- 
-- Включение внешних ключей
-- 
/*!40014 SET FOREIGN_KEY_CHECKS = @OLD_FOREIGN_KEY_CHECKS */;
