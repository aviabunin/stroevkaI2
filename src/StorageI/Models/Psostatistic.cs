using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class Psostatistic
    {
        public int Id { get; set; }
        public DateTime? НаДату { get; set; }
        public int ВсегоОрганизаций { get; set; }
        public int СоСроком { get; set; }
        public int СКаталогом { get; set; }
        public int Просрочено { get; set; }
        public int СделаноСначалаГода { get; set; }
        public int СделаноЗаМесяц { get; set; }
        public int СделаноЗаНеделю { get; set; }
        public int ОсталосьЗаМесяц { get; set; }
        public int ОсталосьЗаГод { get; set; }
        public string Комментарий { get; set; } = null!;
        public int ЗаполненоСрокЗаДень { get; set; }
        public int ЗаполненоСрокЗаНеделю { get; set; }
        public int ЗаполненоСрокЗаМесяц { get; set; }
        public int КаталоговЗаДень { get; set; }
        public int КаталоговЗаНеделю { get; set; }
        public int КаталоговЗаМесяц { get; set; }
        public DateTime ВремяИзменения { get; set; }
        public string? Псг { get; set; }
        public string? Псо { get; set; }
        public int? IdПсг { get; set; }
        public int? IdПсо { get; set; }
    }
}
