// StorageI/Repositories/SizodsRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using StorageI.ModelsStroevkaMySql;

namespace StorageI.Repositories
{
    public class SizodsRepository : IDisposable
    {
        private readonly stroevkaContext _context;
        private bool _disposed;

        public SizodsRepository(stroevkaContext context)
        {
            _context = context ?? new stroevkaContext();
        }

        public List<Sizod> LoadSizods(int subdivisionId)
        {
            return _context.Sizods
                .Where(s => s.SubdivisionId == subdivisionId)
                .OrderBy(s => s.Norder)
                .ToList();
        }

        public bool SaveSizods(List<Sizod> items)
        {
            try
            {
                foreach (var item in items)
                {
                    if (item.Id == 0)
                    {
                        _context.Sizods.Add(item);
                    }
                    else
                    {
                        var existing = _context.Sizods.Find(item.Id);
                        if (existing != null)
                        {
                            existing.Raschet = item.Raschet;
                            existing.Rezerv = item.Rezerv;
                            existing.PostGdzs = item.PostGdzs;
                            existing.BazaGdzs = item.BazaGdzs;
                            existing.EditTime = DateTime.Now;
                        }
                    }
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка сохранения СИЗОД: {ex.Message}");
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