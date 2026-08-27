// StorageI/Repositories/PenasRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using StorageI.ModelsStroevkaMySql;

namespace StorageI.Repositories
{
    public class PenasRepository : IDisposable
    {
        private readonly stroevkaContext _context;
        private bool _disposed;

        public PenasRepository(stroevkaContext context)
        {
            _context = context ?? new stroevkaContext();
        }

        public List<Pena> LoadPenas(int subdivisionId)
        {
            return _context.Penas
                .Where(p => p.SubdivisionId == subdivisionId)
                .OrderBy(p => p.Norder)
                .ToList();
        }

        public bool SavePenas(List<Pena> items)
        {
            try
            {
                foreach (var item in items)
                {
                    if (item.Id == 0)
                    {
                        _context.Penas.Add(item);
                    }
                    else
                    {
                        var existing = _context.Penas.Find(item.Id);
                        if (existing != null)
                        {
                            existing.Inwork = item.Inwork;
                            existing.Inrezerv = item.Inrezerv;
                            existing.EditTime = DateTime.Now;
                        }
                    }
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка сохранения пены: {ex.Message}");
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