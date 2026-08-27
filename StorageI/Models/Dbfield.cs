using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class Dbfield
    {
        public int Id { get; set; }
        public string? MName { get; set; }
        /// <summary>
        /// Тип столбца в гриде - comboBox, Text, int ...
        /// </summary>
        public string? MType { get; set; }
        public string? MGroup { get; set; }
        public string? MSubgroup { get; set; }
        public string? MTable { get; set; }
        public int MLength { get; set; }
        public string? MDisplay { get; set; }
        public bool? MVisible { get; set; }
        public int MOrder { get; set; }
        /// <summary>
        /// 1 - запрет редактирования в форме 0 - разрешено 
        /// </summary>
        public int MEditenable { get; set; }
        /// <summary>
        /// 1 - есть форма редактирования 0 - нет 
        /// </summary>
        public int MEditForm { get; set; }
        public int? NTable { get; set; }
        public int? NGroup { get; set; }
        public int? Parent { get; set; }
        /// <summary>
        /// Не показывать вообще
        /// </summary>
        public bool? Enabled { get; set; }
        public bool? Grouppingfield { get; set; }
        public int? ParentGrouppingfield { get; set; }
        public bool? GrouppingOn { get; set; }
        public bool FilterOn { get; set; }
        public string? FilterExpr { get; set; }
    }
}
