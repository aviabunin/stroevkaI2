// StorageI/Repositories/SostavRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using StorageI.ModelsStroevkaMySql;

namespace StorageI.Repositories
{
    public class SostavRepository : IDisposable
    {
        private readonly stroevkaContext _context;
        private bool _disposed = false;

        public SostavRepository()
        {
            _context = new stroevkaContext();
        }

        public SostavRepository(stroevkaContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Исправлено: выборка по SubdivisionId (Id пожарной части)
        public List<Sostav> LoadSostav(int subdivisionId)
        {
            return _context.Sostavs
                .Where(s => s.SubdivisionId == subdivisionId)
                .OrderBy(s => s.Norder)
                .ToList();
        }

        public List<string> GetSostavGroups()
        {
            return _context.Sostavs
                .Select(s => s.SostavVid)
                .Distinct()
                .Where(v => !string.IsNullOrEmpty(v))
                .OrderBy(v => v)
                .ToList();
        }

        public List<Post> LoadPosts()
        {
            return _context.Posts.OrderBy(p => p.Norder).ToList();
        }

        public bool SaveSostav(List<Sostav> items)
        {
            try
            {
                foreach (var item in items)
                {
                    if (item.Id == 0)
                    {
                        item.EditTime = DateTime.Now;
                        _context.Sostavs.Add(item);
                    }
                    else
                    {
                        var existing = _context.Sostavs.Find(item.Id);
                        if (existing != null)
                        {
                            existing.Count = item.Count;
                            existing.EditTime = DateTime.Now;
                        }
                    }
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка сохранения ЛС: {ex.Message}");
                return false;
            }
        }

        public bool DeleteSostav(int id)
        {
            try
            {
                var item = _context.Sostavs.Find(id);
                if (item != null)
                {
                    _context.Sostavs.Remove(item);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка удаления ЛС: {ex.Message}");
                return false;
            }
        }

        public Sostav CreateSostav(int garnizonId, int subdivisionId, string name,
                                   string sostavVid, int norder, string garnizonName = "",
                                   string subdivisionName = "")
        {
            return new Sostav
            {
                GarnizoneId = garnizonId,
                SubdivisionId = subdivisionId,
                Name = name,
                SostavVid = sostavVid,
                Norder = norder,
                Count = 0,
                Mdate = DateTime.Now.Date,
                EditTime = DateTime.Now,
                NameGarnizone = garnizonName,
                Subdivision = subdivisionName
            };
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}