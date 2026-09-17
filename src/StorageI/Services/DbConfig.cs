using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace StorageI.Services
{
    /// <summary>
    /// Единая точка определения параметров подключения к MySQL.
    /// Работает и для EF (stroevkaContext), и для ADO.NET (FastPivotLoader).
    /// </summary>
    public static class DbConfig
    {
        private const string RemoteHost = "10.37.128.123";
        private const string LocalHost = "127.0.0.1";
        private const int Port = 3306;
        private const string User = "root";
        private const string Password = "Djkjlz1";
        private const string Database = "stroevka";

        /// <summary>
        /// Текущий хост. По умолчанию LocalHost, меняется после Detect().
        /// </summary>
        public static string Host { get; private set; } = RemoteHost;

        /// <summary>
        /// Признак: работаем с удалённым сервером.
        /// </summary>
        public static bool IsRemote => Host == RemoteHost;

        /// <summary>
        /// Признак: обнаружение уже выполнялось.
        /// </summary>
        public static bool IsDetected { get; private set; }

        /// <summary>
        /// Разово определяет доступность удалённого сервера.
        /// Если Detect() уже вызывался — ничего не делает.
        /// </summary>
        public static void Detect()
        {
            if (IsDetected) return;

            var sw = Stopwatch.StartNew();
            bool remoteOk = TryTcpConnect(RemoteHost, Port, timeoutMs: 700);

//            Host = RemoteHost;// remoteOk ? RemoteHost : LocalHost;
            Host = remoteOk ? RemoteHost : LocalHost;

            IsDetected = true;

            Debug.WriteLine($"[DbConfig] host = {Host} (remote check: {remoteOk}, {sw.ElapsedMilliseconds} ms)");
        }

        /// <summary>
        /// Возвращает готовую строку подключения для MySqlConnection / UseMySQL.
        /// Если Detect() ещё не вызывался — использует LocalHost.
        /// </summary>
        public static string BuildConnectionString()
        {
            return $"server={Host};port={Port};user={User};password={Password};" +
                   $"database={Database};Character Set=utf8;" +
                   "Convert Zero Datetime=True;Allow Zero Datetime=True;" +
                   "Pooling=true;MinimumPoolSize=1;MaximumPoolSize=10;";
        }

        private static bool TryTcpConnect(string host, int port, int timeoutMs)
        {
            try
            {
                using var client = new TcpClient();
                var task = client.ConnectAsync(host, port);
                return task.Wait(timeoutMs) && client.Connected;
            }
            catch
            {
                return false;
            }
        }
    }
}
