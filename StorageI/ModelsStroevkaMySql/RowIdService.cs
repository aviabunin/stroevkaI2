using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace StorageI.ModelsStroevkaMySql
{
    public class RowIdService
    {
        private readonly stroevkaContext _context;

        public RowIdService()
        {
            _context = new stroevkaContext();
        }

        /// <summary>
        /// Обновляет RowId для всех ПЧ
        /// </summary>
        public int UpdateAllPchRowIds()
        {
            var pchList = _context.Pchs.ToList();
            int updatedCount = 0;

            foreach (var pch in pchList)
            {
                string oldRowId = pch.RowId;
                pch.GenerateRowId();

                if (pch.RowId != oldRowId)
                    updatedCount++;
            }

            _context.SaveChanges();
            return updatedCount;
        }

        /// <summary>
        /// Обновляет RowId для всех ПСГ
        /// </summary>
        public int UpdateAllPsgRowIds()
        {
            var psgList = _context.Psgs.ToList();
            int updatedCount = 0;

            foreach (var psg in psgList)
            {
                string oldRowId = psg.RowId;
                psg.RowId = psg.GetPsgRowId();

                if (psg.RowId != oldRowId)
                    updatedCount++;
            }

            _context.SaveChanges();
            return updatedCount;
        }

        /// <summary>
        /// Обновляет RowId для всех итоговых строк
        /// </summary>
        public int UpdateAllTotalRowIds()
        {
            var totalRows = _context.PsgTotalRows.ToList();
            int updatedCount = 0;

            foreach (var row in totalRows)
            {
                string oldRowId = row.RowId;
                row.GenerateRowId();

                if (row.RowId != oldRowId)
                    updatedCount++;
            }

            _context.SaveChanges();
            return updatedCount;
        }

        /// <summary>
        /// Обновляет все RowId
        /// </summary>
        public (int pchUpdated, int totalUpdated) UpdateAllRowIds()
        {
            int totalUpdated = UpdateAllTotalRowIds();
            //int pchUpdated = UpdateAllPchRowIds();
            //var psgUpdated = UpdateAllPsgRowIds();


            return (1,1);// (pchUpdated, totalUpdated);
        }

        /// <summary>
        /// Получает статистику по RowId
        /// </summary>
        public object GetRowIdStatistics()
        {
            var totalRows = _context.PsgTotalRows
                .Select(r => new
                {
                    r.Id,
                    r.Name,
                    r.PsgId,
                    r.CategoryType,
                    r.TotalFlag,
                    r.RowId,
                    Parsed = r.RowId != null ? new
                    {
                        TotalFlag = r.RowId.Substring(0, 2),
                        PsgCode = r.RowId.Substring(2, 4),
                        PchCode = r.RowId.Substring(6, 4),
                        ParentCode = r.RowId.Substring(10, 4)
                    } : null
                })
                .ToList();

            var pchStats = _context.Pchs
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Parent,
                    p.RowId,
                    Parsed = p.RowId != null ? new
                    {
                        TotalFlag = p.RowId.Substring(0, 2),
                        PsgCode = p.RowId.Substring(2, 4),
                        PchCode = p.RowId.Substring(6, 4),
                        ParentCode = p.RowId.Substring(10, 4)
                    } : null
                })
                .ToList();

            return new
            {
                TotalRows = new
                {
                    Total = totalRows.Count,
                    WithRowId = totalRows.Count(r => !string.IsNullOrEmpty(r.RowId)),
                    Items = totalRows
                },
                Pch = new
                {
                    Total = pchStats.Count,
                    WithRowId = pchStats.Count(p => !string.IsNullOrEmpty(p.RowId)),
                    Items = pchStats
                }
            };
        }

        /// <summary>
        /// Проверяет корректность всех RowId
        /// </summary>
        public List<string> ValidateAllRowIds()
        {
            var errors = new List<string>();

            // Проверяем итоговые строки
            foreach (var row in _context.PsgTotalRows.ToList())
            {
                if (string.IsNullOrEmpty(row.RowId))
                {
                    errors.Add($"TotalRow Id={row.Id} ({row.Name}): RowId is empty");
                    continue;
                }

                if (row.RowId.Length != 14)
                {
                    errors.Add($"TotalRow Id={row.Id}: RowId length is {row.RowId.Length}, expected 14");
                    continue;
                }

                // Проверяем формат
                string expected = RowIdGenerator.GenerateTotalRowId((int)row.PsgId, row.CategoryType);
                if (row.RowId != expected)
                {
                    errors.Add($"TotalRow Id={row.Id} ({row.Name}): RowId '{row.RowId}' doesn't match expected '{expected}'");
                }
            }

            // Проверяем ПЧ
            foreach (var pch in _context.Pchs.ToList())
            {
                if (string.IsNullOrEmpty(pch.RowId))
                {
                    errors.Add($"PCH Id={pch.Id} ({pch.Name}): RowId is empty");
                    continue;
                }

                if (pch.RowId.Length != 14)
                {
                    errors.Add($"PCH Id={pch.Id}: RowId length is {pch.RowId.Length}, expected 14");
                    continue;
                }

                string expected = RowIdGenerator.GeneratePchRowId(pch.Id, pch.Parent);
                if (pch.RowId != expected)
                {
                    errors.Add($"PCH Id={pch.Id} ({pch.Name}): RowId '{pch.RowId}' doesn't match expected '{expected}'");
                }
            }

            return errors;
        }

        /// <summary>
        /// Пересоздает все итоговые строки
        /// </summary>
        public void RebuildTotalRows()
        {
            // Удаляем существующие
            _context.PsgTotalRows.RemoveRange(_context.PsgTotalRows);
            _context.SaveChanges();

            // Получаем все районные ПСГ
            var psgs = _context.Psgs.Where(p => p.Id != 11 && p.Parent == 11).ToList();

            // Создаем строки для каждого ПСГ
            var totalRows = new List<PsgTotalRow>();

            foreach (var psg in psgs)
            {
                // main - итог по ПСГ
                totalRows.Add(new PsgTotalRow
                {
                    Name = psg.Garnizon,
                    DisplayName = psg.Garnizon,
                    PsgId = psg.Id,
                    CategoryType = "main",
                    CategoryDisplay = "всего",
                    TotalFlag = "01",
                    Norder = -20,
                    IsActive = true
                });

                // gps - итог по ГПС
                totalRows.Add(new PsgTotalRow
                {
                    Name = "      в т.ч. ГПС",
                    DisplayName = "      в т.ч. ГПС",
                    PsgId = psg.Id,
                    CategoryType = "gps",
                    CategoryDisplay = "в т.ч. ГПС",
                    TotalFlag = "02",
                    Norder = -19,
                    IsActive = true
                });

                // fps - итог по ФПС (для всех)
                totalRows.Add(new PsgTotalRow
                {
                    Name = "             ФПС",
                    DisplayName = "             ФПС",
                    PsgId = psg.Id,
                    CategoryType = "fps",
                    CategoryDisplay = "ФПС",
                    TotalFlag = "03",
                    Norder = -18,
                    IsActive = true
                });

                // Специфичные категории
                if (psg.Garnizon == "Сортавальский" || psg.Garnizon == "Питкярантский")
                {
                    totalRows.Add(new PsgTotalRow
                    {
                        Name = "             ВПО",
                        DisplayName = "             ВПО",
                        PsgId = psg.Id,
                        CategoryType = "vpo",
                        CategoryDisplay = "ВПО",
                        TotalFlag = "05",
                        Norder = -15,
                        IsActive = true
                    });
                }

                if (psg.Garnizon == "Муезерский" || psg.Garnizon == "Кондопожский")
                {
                    totalRows.Add(new PsgTotalRow
                    {
                        Name = "             ЧПО",
                        DisplayName = "             ЧПО",
                        PsgId = psg.Id,
                        CategoryType = "chpo",
                        CategoryDisplay = "ЧПО",
                        TotalFlag = "06",
                        Norder = -13,
                        IsActive = true
                    });
                }

                if (new[] { "Калевальский", "Кемский", "Пудожский", "Суоярвский", "Олонецкий",
                            "Лоухский", "Беломорский", "Пряжинский", "Медвежьегорский", "Лахденпохский" }.Contains(psg.Garnizon))
                {
                    totalRows.Add(new PsgTotalRow
                    {
                        Name = "             другие",
                        DisplayName = "             другие",
                        PsgId = psg.Id,
                        CategoryType = "other",
                        CategoryDisplay = "другие",
                        TotalFlag = "04",
                        Norder = -17,
                        IsActive = true
                    });
                }

                if (psg.Garnizon == "Прионежский")
                {
                    totalRows.Add(new PsgTotalRow
                    {
                        Name = "АСФ а/п Петрозаводск",
                        DisplayName = "АСФ а/п Петрозаводск",
                        PsgId = psg.Id,
                        CategoryType = "asf",
                        CategoryDisplay = "АСФ",
                        TotalFlag = "07",
                        Norder = -10,
                        IsActive = true
                    });
                }
            }

            // Добавляем территориальный ПСГ
            // main
            totalRows.Add(new PsgTotalRow
            {
                Name = "Территориальный",
                DisplayName = "Территориальный",
                PsgId = 11,
                CategoryType = "main",
                CategoryDisplay = "всего",
                TotalFlag = "01",
                Norder = -225,
                IsActive = true
            });

            // gps для территориального
            totalRows.Add(new PsgTotalRow
            {
                Name = "  Итого по ГПС",
                DisplayName = "  Итого по ГПС",
                PsgId = 11,
                CategoryType = "gps",
                CategoryDisplay = "ГПС",
                TotalFlag = "02",
                Norder = -223,
                IsActive = true
            });

            // fps для территориального
            totalRows.Add(new PsgTotalRow
            {
                Name = "        в т.ч. по ФПС",
                DisplayName = "        в т.ч. по ФПС",
                PsgId = 11,
                CategoryType = "fps",
                CategoryDisplay = "ФПС",
                TotalFlag = "03",
                Norder = -221,
                IsActive = true
            });

            // other для территориального
            totalRows.Add(new PsgTotalRow
            {
                Name = "по  ДПО",
                DisplayName = "по  ДПО",
                PsgId = 11,
                CategoryType = "other",
                CategoryDisplay = "другие",
                TotalFlag = "04",
                Norder = -215,
                IsActive = true
            });

            // vpo для территориального
            totalRows.Add(new PsgTotalRow
            {
                Name = "по ВПО",
                DisplayName = "по ВПО",
                PsgId = 11,
                CategoryType = "vpo",
                CategoryDisplay = "ВПО",
                TotalFlag = "05",
                Norder = -219,
                IsActive = true
            });

            // chpo для территориального
            totalRows.Add(new PsgTotalRow
            {
                Name = "по ЧПО",
                DisplayName = "по ЧПО",
                PsgId = 11,
                CategoryType = "chpo",
                CategoryDisplay = "ЧПО",
                TotalFlag = "06",
                Norder = -217,
                IsActive = true
            });

            // asf для территориального
            totalRows.Add(new PsgTotalRow
            {
                Name = "АСФ а/п Петрозаводск",
                DisplayName = "АСФ а/п Петрозаводск",
                PsgId = 11,
                CategoryType = "asf",
                CategoryDisplay = "АСФ",
                TotalFlag = "07",
                Norder = -211,
                IsActive = true
            });

            // Генерируем RowId для всех строк
            foreach (var row in totalRows)
            {
                row.GenerateRowId();
            }

            _context.PsgTotalRows.AddRange(totalRows);
            _context.SaveChanges();
        }
    }
}
