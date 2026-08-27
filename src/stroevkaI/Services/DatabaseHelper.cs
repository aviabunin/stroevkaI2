using MySql.Data.MySqlClient; // или MySql.Data.MySqlClient
using System;
using System.Text;

public class DatabaseHelper
{
    private readonly string _connectionString;

    public DatabaseHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Удаляет комментарии из SQL
    /// </summary>
    private string RemoveComments(string sql)
    {
        var lines = sql.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var sb = new StringBuilder();

        foreach (var line in lines)
        {
            var trimmed = line.Trim();

            // Пропускаем пустые строки
            if (string.IsNullOrEmpty(trimmed))
                continue;

            // Пропускаем строки, начинающиеся с -- (комментарии)
            if (trimmed.StartsWith("--"))
                continue;

            // Убираем встроенные комментарии (если есть)
            var commentIndex = trimmed.IndexOf(" --");
            if (commentIndex >= 0)
            {
                trimmed = trimmed.Substring(0, commentIndex);
            }

            sb.AppendLine(trimmed);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Выполняет SQL скрипт
    /// </summary>
    public void ExecuteSql(string sql)
    {
        // Удаляем комментарии
        var cleanSql = RemoveComments(sql);

        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        // Разбиваем по точке с запятой
        var commands = cleanSql.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var cmd in commands)
        {
            var trimmedCmd = cmd.Trim();

            // Пропускаем пустые строки
            if (string.IsNullOrWhiteSpace(trimmedCmd))
                continue;

            try
            {
                using var command = new MySqlCommand(trimmedCmd, connection);
                command.ExecuteNonQuery();
                Console.WriteLine($"✅ Выполнено: {trimmedCmd.Substring(0, Math.Min(50, trimmedCmd.Length))}...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
                Console.WriteLine($"SQL: {trimmedCmd}");
                throw;
            }
        }
    }
}
