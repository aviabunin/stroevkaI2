// StorageI/Repositories/EquipmentRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using StorageI.ModelsStroevkaMySql;

namespace StorageI.Repositories
{
    public class EquipmentRepository
    {
        private readonly stroevkaContext _context;

        public EquipmentRepository(stroevkaContext context)
        {
            _context = context ?? new stroevkaContext();
        }

        #region Pena (Пенообразователи)

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
                        item.EditTime = DateTime.Now;
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

        public bool DeletePena(int id)
        {
            try
            {
                var item = _context.Penas.Find(id);
                if (item != null)
                {
                    _context.Penas.Remove(item);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка удаления пены: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Sizod (СИЗОД)

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
                        item.EditTime = DateTime.Now;
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

        public bool DeleteSizod(int id)
        {
            try
            {
                var item = _context.Sizods.Find(id);
                if (item != null)
                {
                    _context.Sizods.Remove(item);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка удаления СИЗОД: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Waters (Водоисточники)

        public List<Water> LoadWaters(int subdivisionId)
        {
            return _context.Waters
                .Where(w => w.SubdivisionId == subdivisionId)
                .OrderBy(w => w.Norder)
                .ToList();
        }

        public bool SaveWaters(List<Water> items)
        {
            try
            {
                foreach (var item in items)
                {
                    if (item.Id == 0)
                    {
                        item.EditTime = DateTime.Now;
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
                System.Diagnostics.Debug.WriteLine($"Ошибка сохранения водоисточников: {ex.Message}");
                return false;
            }
        }

        public bool DeleteWater(int id)
        {
            try
            {
                var item = _context.Waters.Find(id);
                if (item != null)
                {
                    _context.Waters.Remove(item);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка удаления водоисточника: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Kostym (Костюмы)

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
                        item.EditTime = DateTime.Now;
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

        public bool DeleteKostym(int id)
        {
            try
            {
                var item = _context.Kostyms.Find(id);
                if (item != null)
                {
                    _context.Kostyms.Remove(item);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка удаления костюма: {ex.Message}");
                return false;
            }
        }

        #endregion

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
