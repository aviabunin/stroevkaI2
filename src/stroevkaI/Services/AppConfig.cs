using System;
using System.IO;
using System.Text.Json;

namespace stroevkaI.Services
{
    public enum AppMode { Central, Garrison, Standalone }

    public class AppConfig
    {
        public AppMode Mode { get; set; } = AppMode.Central;
        public int PchId { get; set; } = 0;
        public string JsonLocalPath { get; set; } = "psg_data";
        public string JsonNetworkPath { get; set; } = @"\\server\shared\psg_data";
        public string[] NetworkDrives { get; set; } = new[] { "X:", "Y:", "Z:" };

        public static AppConfig Load(string path = "appsettings.json")
        {
            try
            {
                if (File.Exists(path))
                {
                    var json = File.ReadAllText(path);
                    return JsonSerializer.Deserialize<AppConfig>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new AppConfig();
                }
            }
            catch { /* используем значения по умолчанию */ }
            return new AppConfig();
        }
    }
}