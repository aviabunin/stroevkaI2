using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using StorageI.ModelsStroevkaMySql;

namespace stroevkaI.Services
{
    public class JsonDataService
    {
        private readonly string _basePath;

        public JsonDataService(string basePath)
        {
            _basePath = basePath;
        }

        private string GetFilePath(int pchId)
        {
            return Path.Combine(_basePath, pchId.ToString(), "data.json");
        }

        public async Task<PchData> LoadDataAsync(int pchId)
        {
            var filePath = GetFilePath(pchId);
            if (!File.Exists(filePath))
                return null;

            var json = await File.ReadAllTextAsync(filePath);
            var data = JsonSerializer.Deserialize<PchData>(json);
            return data;
        }

        public async Task SaveDataAsync(PchData data)
        {
            var filePath = GetFilePath(data.PchId);
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, json);
        }
        public string GetBasePath() => _basePath;
    }

    public class PchData
    {
        public int PchId { get; set; }
        public DateTime LastModified { get; set; }
        public List<Sredstva> SredstvaList { get; set; } = new();
        public List<Sostav> SostavList { get; set; } = new();
        public List<Contact> ContactsList { get; set; } = new();
        public List<Water> WatersList { get; set; } = new();
        public List<Pena> PenasList { get; set; } = new();
        public List<Sizod> SizodsList { get; set; } = new();
        public List<Kostym> KostymsList { get; set; } = new();
    }
}
