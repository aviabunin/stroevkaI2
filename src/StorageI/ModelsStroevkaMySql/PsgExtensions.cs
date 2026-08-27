using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageI.ModelsStroevkaMySql
{
    public static class PsgExtensions
    {
        /// <summary>
        /// Создаёт RowId для итоговой строки ПСГ по категории
        /// </summary>
        public static string GetPsgRowId(this Psg psg, string categoryType = "main")
        {
            if (psg == null) return null;

            return RowIdGenerator.GenerateTotalRowId(psg.Id, categoryType);
        }
    }
}
