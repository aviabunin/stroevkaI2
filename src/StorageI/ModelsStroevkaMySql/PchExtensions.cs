using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageI.ModelsStroevkaMySql
{
    public static class PchExtensions
    {
        /// <summary>
        /// Генерирует и устанавливает RowId для ПЧ
        /// </summary>
        public static void GenerateRowId(this Pch pch)
        {
            if (pch == null) return;

            pch.RowId = RowIdGenerator.GeneratePchRowId(pch.Id, pch.Parent);
        }
    }
}
