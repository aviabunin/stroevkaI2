using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using StorageI.ModelsStroevkaMySql;

namespace StorageI.Services
{
    public class PivotRowsRepository
    {
        public class SaveResult
        {
            public int Inserted { get; set; }
            public int Updated { get; set; }
            public int Deleted { get; set; }
            public int Unchanged { get; set; }
            public int Total => Inserted + Updated + Deleted + Unchanged;
        }

        /// <summary>
        /// Естественный ключ строки.
        /// </summary>
        private static string GetKey(PivotRow r)
            => $"{r.PsgId}|{r.Category}|{r.Isitog}|{r.PchId}";

        /// <summary>
        /// Синхронизирует строки одного ПСГ с БД. Обновляет только изменённые.
        /// </summary>
        public static SaveResult SavePivotRows(int psgId, List<PivotRow> newRows)
        {
            var result = new SaveResult();
            if (newRows == null) newRows = new List<PivotRow>();

            using var ctx = new stroevkaContext();
            using var tx = ctx.Database.BeginTransaction();

            // 1. Старые строки этого ПСГ
            var oldRows = ctx.PivotRows
                .Where(r => r.PsgId == psgId)
                .AsNoTracking()
                .ToList();

            var oldByKey = oldRows
                .GroupBy(GetKey)
                .ToDictionary(g => g.Key, g => g.First());
            var matchedKeys = new HashSet<string>();

            var toInsert = new List<PivotRow>();
            var toUpdate = new List<PivotRow>();

            // 2. Обходим новые строки
            foreach (var newRow in newRows)
            {
                newRow.PsgId = psgId;
                var key = GetKey(newRow);

                if (oldByKey.TryGetValue(key, out var oldRow))
                {
                    matchedKeys.Add(key);

                    if (!AreEqual(oldRow, newRow))
                    {
                        newRow.Id = oldRow.Id;               // сохраняем Id
                        newRow.LastModified = DateTime.Now;
                        toUpdate.Add(newRow);
                    }
                    else
                    {
                        result.Unchanged++;
                    }
                }
                else
                {
                    newRow.Id = 0;                           // БД выдаст
                    newRow.LastModified = DateTime.Now;
                    toInsert.Add(newRow);
                }
            }

            // 3. Удаляем старые, которых нет в новых
            var toDelete = oldRows
                .Where(r => !matchedKeys.Contains(GetKey(r)))
                .ToList();

            // 4. Применяем
            if (toDelete.Count > 0) ctx.PivotRows.RemoveRange(toDelete);
            if (toInsert.Count > 0) ctx.PivotRows.AddRange(toInsert);
            if (toUpdate.Count > 0) ctx.PivotRows.UpdateRange(toUpdate);

            ctx.SaveChanges();
            tx.Commit();

            result.Inserted = toInsert.Count;
            result.Updated = toUpdate.Count;
            result.Deleted = toDelete.Count;
            return result;
        }

        /// <summary>
        /// Сравнивает все persistent-колонки, кроме служебных.
        /// </summary>
        private static bool AreEqual(PivotRow a, PivotRow b)
        {
            foreach (var prop in typeof(PivotRow).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!prop.CanRead) continue;
                // пропускаем [NotMapped] (Childes, CellDetails)
                if (prop.GetCustomAttribute<NotMappedAttribute>() != null) continue;

                var name = prop.Name;
                if (name == nameof(PivotRow.Id)) continue;
                if (name == nameof(PivotRow.LastModified)) continue;
                if (name == nameof(PivotRow.SourceVersion)) continue;

                var va = prop.GetValue(a);
                var vb = prop.GetValue(b);
                if (!Equals(va, vb)) return false;
            }
            return true;
        }
    }
}
