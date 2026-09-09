using System;
using System.Threading.Tasks;
using StorageI.ModelsStroevkaMySql;

namespace stroevkaI.Services
{
    public class DataSyncManager
    {
        private readonly stroevkaContext _context;
        private readonly JsonDataService _jsonService;
        private readonly int _pchId;
        private bool _isOnline;

        public DataSyncManager(stroevkaContext context, JsonDataService jsonService, int pchId)
        {
            _context = context;
            _jsonService = jsonService;
            _pchId = pchId;
            _isOnline = DatabaseAvailabilityChecker.IsDatabaseAvailable(context);
        }

        public bool IsOnline => _isOnline;

        // Загрузка данных (автоматически выбирает источник)
        public async Task<PchData> LoadDataAsync()
        {
            if (_isOnline)
            {
                return await LoadFromDatabaseAsync();
            }
            else
            {
                return await _jsonService.LoadDataAsync(_pchId) ?? new PchData { PchId = _pchId };
            }
        }

        // Сохранение данных
        public async Task SaveDataAsync(PchData data)
        {
            data.LastModified = DateTime.Now;
            if (_isOnline)
            {
                await SaveToDatabaseAsync(data);
            }
            await _jsonService.SaveDataAsync(data); // всегда сохраняем в JSON
        }

        // Периодическая проверка доступности БД (вызывать по таймеру)
        public void CheckConnectivity()
        {
            _isOnline = DatabaseAvailabilityChecker.IsDatabaseAvailable(_context);
        }

        // Загрузка из БД (реализация через репозитории)
        private async Task<PchData> LoadFromDatabaseAsync()
        {
            // Используем существующие репозитории для загрузки данных
            var data = new PchData { PchId = _pchId };
            // Пример: загрузка через репозитории (нужно адаптировать)
            // data.SredstvaList = await _context.Sredstvas.Where(s => s.SubdivisionId == _pchId).ToListAsync();
            // ... аналогично для других таблиц
            return data;
        }

        private async Task SaveToDatabaseAsync(PchData data)
        {
            // Сохранение в БД через репозитории
            // Например: обновление, вставка, удаление
            // Используйте UnitOfWork или транзакции
        }
    }
}
