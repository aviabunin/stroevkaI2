using stroevkaI;
using stroevkaI.Forms;
using stroevkaI.Services;
using StorageI.ModelsStroevkaMySql;

namespace stroevkaI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Один раз определяем, куда подключаться
            StorageI.Services.DbConfig.Detect();
            Log.Write($"[Start] DB host = {StorageI.Services.DbConfig.Host}, " +
                      $"remote = {StorageI.Services.DbConfig.IsRemote}");

            // Прогрев EF-модели в фоне, чтобы первое открытие редактора было быстрым
            Task.Run(() =>
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                try
                {
                    using var warmup = new stroevkaContext();
                    _ = warmup.Psgs.FirstOrDefault();   // любое обращение — построит модель
                    Log.Write($"[Warmup] EF model built in {sw.ElapsedMilliseconds} ms");
                }
                catch (Exception ex)
                {
                    Log.Write($"[Warmup] error: {ex.Message}");
                }
            });

            var config = AppConfig.Load();




            switch (config.Mode)
            {
                case AppMode.Standalone:
                    Application.Run(new PivotRowEditor(config.PchId));
                    break;
                case AppMode.Garrison:
                case AppMode.Central:
                default:
                    Application.Run(new Form1());
                    break;
            }
        }
    }
}