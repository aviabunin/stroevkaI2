using Microsoft.EntityFrameworkCore;
using StorageI.ModelsStroevkaMySql;
using System.Linq;

namespace stroevkaI.Services
{
    public enum ResourceStatus { Unknown, Available, Unavailable }
    public enum DataSourceKind { Database, JsonLocal, JsonNetwork }

    public class AppStatus
    {
        public ResourceStatus DatabaseStatus { get; set; } = ResourceStatus.Unknown;
        public Dictionary<string, ResourceStatus> NetworkDrives { get; } = new()
        {
            ["X:"] = ResourceStatus.Unknown,
            ["Y:"] = ResourceStatus.Unknown,
            ["Z:"] = ResourceStatus.Unknown,
        };

        public DataSourceKind ActiveSource { get; set; } = DataSourceKind.Database;
        public DateTime LastCheck { get; set; } = DateTime.MinValue;
        public string JsonLocalPath { get; set; } = Path.Combine(AppContext.BaseDirectory, "psg_data");
        public string JsonNetworkPath { get; set; } = "";

        public bool IsDatabaseOnline => DatabaseStatus == ResourceStatus.Available;
        public bool IsNetworkAvailable => NetworkDrives.Values.Any(v => v == ResourceStatus.Available);
    }

    public class AppStatusService
    {
        private readonly AppStatus _status = new();
        public AppStatus Status => _status;

        public AppStatusService(string jsonLocalPath, string jsonNetworkPath, IEnumerable<string> drives)
        {
            _status.JsonLocalPath = jsonLocalPath;
            _status.JsonNetworkPath = jsonNetworkPath;
            _status.NetworkDrives.Clear();
            foreach (var d in drives)
                _status.NetworkDrives[d] = ResourceStatus.Unknown;
        }

        public async Task RefreshAsync(stroevkaContext ctx)
        {
            _status.DatabaseStatus = await CheckDatabaseAsync(ctx)
                ? ResourceStatus.Available
                : ResourceStatus.Unavailable;

            foreach (var drive in new List<string>(_status.NetworkDrives.Keys))
            {
                bool ok;
                try { ok = Directory.Exists(drive); }
                catch { ok = false; }
                _status.NetworkDrives[drive] = ok ? ResourceStatus.Available : ResourceStatus.Unavailable;
            }

            _status.ActiveSource = _status.DatabaseStatus == ResourceStatus.Available
                ? DataSourceKind.Database
                : DataSourceKind.JsonLocal;

            _status.LastCheck = DateTime.Now;
        }

        private static async Task<bool> CheckDatabaseAsync(stroevkaContext ctx)
        {
            try
            {
                // Таймаут ~2 сек: не ждём стандартные 15 сек при недоступном сервере
                using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(2));
                await ctx.Database.ExecuteSqlRawAsync("SELECT 1", cts.Token);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}