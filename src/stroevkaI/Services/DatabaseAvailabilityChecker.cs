using System;
using Microsoft.EntityFrameworkCore;
using StorageI.ModelsStroevkaMySql;

namespace stroevkaI.Services
{
    public static class DatabaseAvailabilityChecker
    {
        public static bool IsDatabaseAvailable(stroevkaContext context)
        {
            try
            {
                // Простой тестовый запрос
                return context.Database.CanConnect();
            }
            catch
            {
                return false;
            }
        }
    }
}
