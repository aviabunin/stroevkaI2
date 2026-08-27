using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageI.ModelsStroevkaMySql
{
    public static class PsgTotalRowExtensions
    {
        /// <summary>
        /// Генерирует и устанавливает RowId для итоговой строки
        /// </summary>
        public static void GenerateRowId(this PsgTotalRow totalRow)
        {
            if (totalRow == null) return;

            totalRow.RowId = RowIdGenerator.GenerateTotalRowId(
                (int)totalRow.PsgId,
                totalRow.CategoryType
            );
        }
    }
}
