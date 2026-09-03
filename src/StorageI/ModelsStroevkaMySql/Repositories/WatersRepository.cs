// StorageI/Repositories/WatersRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using StorageI.ModelsStroevkaMySql;

namespace StorageI.Repositories
{
    public class WatersRepository : IDisposable
    {
        private readonly stroevkaContext _context;
        private bool _disposed;

        public WatersRepository(stroevkaContext context)
        {
            _context = context ?? new stroevkaContext();
        }

        public List<Water> LoadWaters(int subdivisionId)
        {
            return new List<Water>();// _context.Waters
                //.Where(w => w.SubdivisionId == subdivisionId)
                //.OrderBy(w => w.Norder)
                //.ToList();
        }

        public bool SaveWaters(List<Water> items)
        {
            try
            {
                foreach (var item in items)
                {
                    if (item.Id == 0)
                    {
                        _context.Waters.Add(item);
                    }
                    else
                    {
                        var existing = _context.Waters.Find(item.Id);
                        if (existing != null)
                        {
                            existing.Total = item.Total;
                            existing.Fault = item.Fault;
                            existing.EditTime = DateTime.Now;
                        }
                    }
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка сохранения воды: {ex.Message}");
                return false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context?.Dispose();
            }
            _disposed = true;
        }
    }
}