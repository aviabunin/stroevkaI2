// StorageI/Repositories/KostymsRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using StorageI.ModelsStroevkaMySql;

namespace StorageI.Repositories
{
    public class KostymsRepository : IDisposable
    {
        private readonly stroevkaContext _context;
        private bool _disposed;

        public KostymsRepository(stroevkaContext context)
        {
            _context = context ?? new stroevkaContext();
        }

        public List<Kostym> LoadKostyms(int subdivisionId)
        {
            return _context.Kostyms
                .Where(k => k.SubdivisionId == subdivisionId)
                .OrderBy(k => k.Norder)
                .ToList();
        }

        public bool SaveKostyms(List<Kostym> items)
        {
            try
            {
                foreach (var item in items)
                {
                    if (item.Id == 0)
                    {
                        _context.Kostyms.Add(item);
                    }
                    else
                    {
                        var existing = _context.Kostyms.Find(item.Id);
                        if (existing != null)
                        {
                            existing.N = item.N;
                            existing.EditTime = DateTime.Now;
                        }
                    }
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка сохранения костюмов: {ex.Message}");
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