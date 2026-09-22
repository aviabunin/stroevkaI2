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
            public int Updated { get; set; }
            public int Inserted { get; set; }
            public int Unchanged { get; set; }
            public int Total => Updated + Inserted + Unchanged;
        }

        /// <summary>
        /// Построчный upsert по pch_id.
        /// - Существующие строки с изменениями → UPDATE.
        /// - Существующие без изменений → пропуск.
        /// - Новые pch_id → INSERT (редко).
        /// Никаких DELETE.
        /// </summary>
        public static SaveResult SavePivotRows(List<PivotRow> newRows)
        {
            var result = new SaveResult();
            if (newRows == null || newRows.Count == 0) return result;

            using var ctx = new stroevkaContext();
            using var tx = ctx.Database.BeginTransaction();

            var newIds = newRows.Select(r => r.PchId).ToList();

            // Ключевое: AsNoTracking — не попадают в трекер EF
            var existing = ctx.PivotRows
                .AsNoTracking()
                .Where(r => newIds.Contains(r.PchId))
                .ToDictionary(r => r.PchId ?? 0, r => r);

            var toInsert = new List<PivotRow>();
            var toUpdate = new List<PivotRow>();

            foreach (var newRow in newRows)
            {
                var key = newRow.PchId ?? 0;

                if (existing.TryGetValue(key, out var oldRow))
                {
                    if (HasChanges(oldRow, newRow))
                    {
                        newRow.Id = oldRow.Id;                    // ключ из БД
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
                    newRow.LastModified = DateTime.Now;
                    toInsert.Add(newRow);
                }
            }

            if (toUpdate.Count > 0)
                ctx.PivotRows.UpdateRange(toUpdate);              // UpdateRange вместо foreach Update

            if (toInsert.Count > 0)
                ctx.PivotRows.AddRange(toInsert);

            ctx.SaveChanges();
            tx.Commit();

            result.Updated = toUpdate.Count;
            result.Inserted = toInsert.Count;
            return result;
        }

        /// <summary>
        /// Сравнивает все persistent-колонки, кроме служебных.
        /// </summary>
        private static bool HasChanges(PivotRow oldRow, PivotRow newRow)
        {
            foreach (var prop in typeof(PivotRow).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!prop.CanRead) continue;
                if (prop.GetCustomAttribute<NotMappedAttribute>() != null) continue;

                var name = prop.Name;
                if (name == nameof(PivotRow.Id)) continue;
                if (name == nameof(PivotRow.LastModified)) continue;
                if (name == nameof(PivotRow.SourceVersion)) continue;

                var vOld = prop.GetValue(oldRow);
                var vNew = prop.GetValue(newRow);

                if (!Equals(vOld, vNew)) return true;
            }
            return false;
        }
    }
}