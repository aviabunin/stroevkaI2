using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageI.ModelsStroevkaMySql
{
    public  class FireEquipsPivotRepository
    {
        public static stroevkaContext context = new stroevkaContext();

        public static List<Sredstva> GetAllSredstva()
        {
            return context.Sredstvas.OrderBy(c=>c.SubdivisionId).OrderBy(c=>c.EditTime).ToList();
        }
        public static List<Sostav> GetAllSostav()
        {
            return context.Sostavs.OrderBy(c => c.SubdivisionId).OrderBy(c => c.EditTime).ToList();
        }
        public static List<Sizod> GetAllSizod()
        {
            return context.Sizods.OrderBy(c => c.SubdivisionId).OrderBy(c => c.EditTime).ToList();
        }
        public static List<Pena> GetAllPenas()
        {
            return context.Penas.OrderBy(c => c.SubdivisionId).OrderBy(c => c.EditTime).ToList();
        }
        public static List<Kostym> GetAllKostyms()
        {
            return context.Kostyms.OrderBy(c => c.SubdivisionId).OrderBy(c => c.EditTime).ToList();
        }
        public static List<Contact> GetAllContacts()
        {
            return context.Contacts.OrderBy(c => c.SubdivisionId).OrderBy(c => c.EditTime).ToList();
        }
        public static List<Water> GetAllWaters()
        {
            return context.Waters.OrderBy(c => c.SubdivisionId).OrderBy(c => c.EditTime).ToList();
        }





        public static List<Psgdatum> loadAllPsgdata(string psgName) {

            Psgdatum root = context.Psgdata.Where(c => (c.Garnizon==psgName)).FirstOrDefault(); //((c.Old == true) &&  
            if (root == null) return null;
            List<Psgdatum> allPsg = context.Psgdata.Where(c => ((c.Parent == root.Id) && (c.Old == true))).ToList();
            allPsg.Add(root);


            return allPsg;
        }
        public static List<FirePsgStat> loadAllFps(string psgName)
        {
            FirePsgStat rootFps = context.FirePsgStats.Where(C=>C.Пч.Trim() == psgName).FirstOrDefault();
            if (rootFps == null) return null;
            List<FirePsgStat> allFps = context.FirePsgStats.Where(c => c.Parent == rootFps.Parent).ToList();
            allFps = allFps.Where(c => !(c.Пч.Contains("ВПО") && (c.Isitog==1) )).ToList();
            allFps = allFps.Where(c => !(c.Пч.Contains("АСФ") && (c.Isitog == 1))).OrderBy(c=>c.Norder).ToList();
            //allFps.Add(rootFps);// т.к. у root parent = 3
            return allFps;
        }
        /// <summary>
        /// обновление CacheNachkars: для каждого гарнизона делаем Update - если есть то обновляем, если нет, то создаём новый
        /// </summary>
        /// <param name="karaulNumber"></param>
        /// 

        // выбираем все гарнизоны firepsgstat  и в цикле вызываем Update               
        // ЦИКЛ
        //   если это isitog
        //      if   - районный общий - опер_деж_по гарн (код post_id=2)
        //      else - возврат с " "               
        //   иначе  (это обычная из 309 ПЧ)
        //      если есть начкар (код post_id=1) возврат его
        //      если есть опер_деж_по гарн (код post_id=2) возврат его 
        //      если есть начкар (код post_id=5) возврат его
        //      иначе - возвра

        // save контекста
        public static void UpdateCacheNachkar(int karaulNumber)
        {
            context = new stroevkaContext();
            string fioNachKar="";
            List<FirePsgStat> lst = context.FirePsgStats.ToList();
            try
            {
                foreach (FirePsgStat fps in lst) {

   
                    //if (fps.PchId != 6000)
                    //    continue;

                    if (fps.Isitog == 1)
                        fioNachKar = начкарИтоговаяСтрока(fps, context, karaulNumber);// там формируем cashNachkar
                    else
                        fioNachKar = начкарПЧ(fps, context, karaulNumber);
                    updateNachcar(context, (int)fps.PchId,karaulNumber,fioNachKar,(int)fps.Parent);
         
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка обновления cache_nachkar: {ex.Message}");
            }
        }
        private static string начкарИтоговаяСтрока(FirePsgStat fps, stroevkaContext context,int karaul) {

            if(!fps.Category.StartsWith("Общие"))
                return "";

             var psg = context.Psgs.Where(c => c.Garnizon.StartsWith(fps.Псг.Substring(0, 5))).FirstOrDefault();// ищем гарнизон по 5 первым буквам

            if (psg == null)
                return "-";

            int mainPchId = (int)psg.MainPchId;   // Найти основную ПЧ и взять начкар оттуда

            var cont = context.Contacts.Where(c => ((c.SubdivisionId == mainPchId) && (c.Karaul ==karaul ))).FirstOrDefault();
            if ((cont == null) || (cont.Fio.Trim().ToLower()=="нет"))
                return "Не указан";
            string a = cont.Fio != null ? cont.Fio : "";
            return a;  // dВозможно завести поле, чтобы оператор сам формировал нужное сокращение
        }
        private static string начкарПЧ(FirePsgStat it, stroevkaContext context, int karaul)
        {
            List<Contact> contacts = context.Contacts.Where(c => ((c.SubdivisionId == it.PchId) && (c.Karaul == karaul))).ToList();
            if ((contacts == null) || (contacts.Count<=0))
                return "";
            //Если есть начкар (post_id = 1) - то возврат его , иначе если есть ДПО (post_id = 5) то его, иначе "нет"
            var cont = contacts.Where(c => c.PostId == 1).FirstOrDefault();
            if((cont!=null) && cont.Fio.Trim() !="" && cont.Fio.Trim() != "нет")
                    return cont.Fio.Trim();
            cont = contacts.Where(c => c.PostId == 5).FirstOrDefault();
            if ((cont != null) && cont.Fio.Trim() != "" && cont.Fio.Trim() != "нет")
                return cont.Fio.Trim();
            return "Не указан"; 

        }
       
        public static CacheNachkar updateNachcar(stroevkaContext context, int pchId, int karaulNumber, string fioNachKar, int psgid) {


            //stroevkaContext context1 = new stroevkaContext();
            var cn = context.CacheNachkars.Where(c => ((c.SubdivisionId == pchId) )).FirstOrDefault();
            if (cn == null)
            {
                cn = new CacheNachkar();
                cn.SubdivisionId = (int)pchId;
                cn.PsgId = psgid; 
                context.Add(cn);
            }
            cn.Karaul = karaulNumber;
            cn.LastUpdated = DateTime.Now;
            cn.Nachkar = createIO(fioNachKar);
            return cn;// можно ничего не возращать или true/false
        }
        static private string createIO(string FIO) {

            //разобьем по пробелу
            string[] fios = FIO.Split(new char[] { ' ', '.' });
            string имя = "";
            string отчество = "";
            if (FIO.Contains('.'))
                return FIO;
            if (FIO.ToLower().StartsWith("не"))
                return FIO;
            try
            {
                if(fios.Length>1)
                    if(fios[1].Trim()!="")
                       имя = fios[1].Substring(0, 1) + ". ";
                if (fios.Length > 2)
                    if (fios[2].Trim() != "")
                        отчество = fios[2].Substring(0, 1) + ".";
            }
            catch (Exception)
            {
                return FIO;
            }

            return fios[0] + " " + имя + отчество;


        }



        public static void SetPchDatafilled(int pchId, bool isFilled)
        {
            using (var context = new stroevkaContext())
            {
                var pch = context.FirePsgStats.FirstOrDefault(p => p.PchId == pchId);
                if (pch != null)
                {
                    //pch.Datafilled = isFilled ? 1 : 0;
                    context.SaveChanges();
                }
            }
        }
        public static List<FirePsgStat> LoadAllPsgs()
        {
            using (var context = new stroevkaContext())
            {
                try
                {
                    // Загружаем все записи, где есть название гарнизона
                    var psgs = context.FirePsgStats
                        .Where(p => p.Псг != null && p.Псг != "")
                        .OrderBy(p => p.Псг)
                        .ThenBy(p => p.Пч)
                        .ToList();

                    return psgs;
                }
                catch (Exception ex)
                {
                    return new List<FirePsgStat>();
                }
            }
        }

        /// <summary>
        /// Загрузка средств для конкретного подразделения (ПЧ)
        /// </summary>
        public static List<Sredstva> LoadSredstva(int subdivisionId)
        {
            context = new stroevkaContext();
            return context.Sredstvas
                .Where(s => s.SubdivisionId == subdivisionId)
                .OrderBy(s => s.SredstvoVid)
                .ThenBy(s => s.Norder)
                .ToList();
        }

        /// <summary>
        /// Получение ПЧ по ID
        /// </summary>
        public static FirePsgStat getPchById(int subdivisionId)
        {
            context = new stroevkaContext();
            return context.FirePsgStats
                .FirstOrDefault(p => p.PchId == subdivisionId);
        }

        /// <summary>
        /// Сохранение средства
        /// </summary>
        public static bool SaveSredstva(Sredstva item)
        {
            context = new stroevkaContext();
            try
            {
                if (item.Id == 0)
                {
                    context.Sredstvas.Add(item);
                }
                else
                {
                    var existing = context.Sredstvas.FirstOrDefault(s => s.Id == item.Id);
                    if (existing != null)
                    {
                        context.Entry(existing).CurrentValues.SetValues(item);
                    }
                }
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SaveSredstva error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Удаление средства
        /// </summary>
        public static bool DeleteSredstva(int id)
        {
            context = new stroevkaContext();
            try
            {
                var item = context.Sredstvas.FirstOrDefault(s => s.Id == id);
                if (item != null)
                {
                    context.Sredstvas.Remove(item);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DeleteSredstva error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Добавление нового средства
        /// </summary>
        public static bool AddSredstva(Sredstva newItem)
        {
            context = new stroevkaContext();
            try
            {
                context.Sredstvas.Add(newItem);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AddSredstva error: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Получение списка уникальных видов средств для ПЧ
        /// </summary>
        public static List<string> GetSredstvoVids(int pchId)
        {
            context = new stroevkaContext();
            return context.Sredstvas
                .Where(s => s.SubdivisionId == pchId)
                .Select(s => s.SredstvoVid)
                .Distinct()
                .OrderBy(v => v)
                .ToList();
        }



        public static List<FirePsgStat> LoadEquips()
        {
            context = new stroevkaContext();
            var v = context.FirePsgStats.ToList();
            return v;// context.FireEquipsPivots.ToList();
        }

        // главное пользуйся автоподсказкой - набрал точку и она покажет и Contacts,
        //  и SubdivisionId  и c.Karaul
        /// <summary>
        /// Возвращает список из таблицы Contacts
        /// обращение из вызывающей программы  FireEquipsPivotRepository.GetContacts(id,karaul)
        /// Чтобы увидела репозитарий нужно вверху там добавить using torageI.ModelsStroevkaMySql
        /// т.е. namespace который указан здесь вверху в 7 строке
        /// </summary>
        /// <param name="garn_id"> id пожарной части</param>
        /// <param name="karaul"> номер караула</param>
        /// <returns></returns>
        public static List<Contact> GetContacts(int garn_id, int karaul)
        {
            context = new stroevkaContext();
            var v = context.Contacts.Where(c=>c.SubdivisionId == garn_id && c.Karaul==karaul).ToList();
            return v;// context.FireEquipsPivots.ToList();
        }
        /// <summary>
        /// Возвращает список контактов основной ПЧ для районного ПСГ по его Id и номеру караула
        /// </summary>
        /// <param name="psg_id">Id текущего ПСГ</param>
        /// <param name="karaul">Номер караула</param>
        /// <returns>список таблицы контактов</returns>
        public static List<Contact> GetContactsMestn(int psg_id, int karaul)
        {
            context = new stroevkaContext();

            int garn_id = context.Psgs.Where(c => c.Id == psg_id).Select(c => (int)c.MainPchId).FirstOrDefault();
            var v = context.Contacts.Where(c => c.SubdivisionId == garn_id && c.Karaul == 1).ToList();
            return v;// context.FireEquipsPivots.ToList();
        }



        public static List<FirePsgStat> LoadEquipsByPsg(string _psgname)
        {
            List<FirePsgStat> lst = new List<FirePsgStat>();

            if (!_psgname.Contains("Террит"))
            {
                // Для районных ПСГ
                Psg psg = context.Psgs.Where(c => c.Garnizon == _psgname).FirstOrDefault();
                if (psg == null) return lst;

                var stats = context.FirePsgStats
                    .Where(c => c.Parent == psg.Id)
                    .ToList();

                lst = stats
                    .OrderBy(c => c.Category == "всего" ? 0 : 1)
                    .ThenBy(c => c.Category == "всего" ? psg.Norder : c.Norder)
                    .ThenBy(c => c.Norder)
                    .ToList();

                return lst;
            }
            else
            {
                // Для территориального
                var allStats = context.FirePsgStats
                    .Where(c => (c.PchId == 11 || c.Parent == 11 || c.Category.ToLower().Contains("всего")))
                    .ToList();

                // Словарь для сортировки ПСГ по их Norder
                var psgOrder = context.Psgs
                    .Where(p => p.Parent == 11)
                    .ToDictionary(p => p.Garnizon, p => p.Norder);

                // Сортируем: сначала территориальный итог (по своему Norder),
                // затем остальные "всего" по порядку ПСГ, потом остальные строки
                lst = allStats
                    .OrderBy(c => c.Псг == "Территориальный" ? 0 : 1)
                    .ThenBy(c => c.Category == "всего" ? (psgOrder.ContainsKey(c.Псг) ? psgOrder[c.Псг] : 999) : 999)
                    .ThenBy(c => c.Norder)
                    .ToList();

                return lst;
            }
        }


        public static Psg GetPsgByName(string psgName)//TODO
        {
            context = new stroevkaContext();
            return context.Psgs.Where(c => c.Garnizon.Trim() == psgName).FirstOrDefault();
        }
        public static FirePsgStat GetPsgByName2(string psgName)
        {
            context = new stroevkaContext();
            return context.FirePsgStats.Where(c => c.Псг.Trim() == psgName).FirstOrDefault();
        }

 
        public static int GetPsgIdByName(string psgName)
        {
            context = new stroevkaContext();
            return context.Psgs.Where(c => c.Garnizon == psgName).Select(c=>c.Id).FirstOrDefault();
        }

        public static List<Contact> GetContactsByPsg(string psgName, int karaul)
        {
            context = new stroevkaContext();

            context = new stroevkaContext();
            int Id = context.Psgs.Where(c => c.Garnizon == psgName).Select(c => c.Id).FirstOrDefault();

            int garn_id = context.Psgs.Where(c => c.Id == Id).Select(c => (int)c.MainPchId).FirstOrDefault();
            var v = context.Contacts.Where(c => c.SubdivisionId == garn_id && c.Karaul == karaul).ToList();
            return v;// context.FireEquipsPivots.ToList();
        }

        public static List<Pch> getPchList() {
            context = new stroevkaContext();
            return context.Pchs.ToList();
        }
        public static List<Psg> getPsgList()
        {
            context = new stroevkaContext();
            return context.Psgs.ToList();
        }
        
         public static bool rowNameIsPsgName(string name)
        {
            context = new stroevkaContext();
            var v = context.Psgs.Where(c => c.Garnizon.Contains(name)).FirstOrDefault();
            return v != null;
        }
    }
}
//UNION ALL
//SELECT
//  'asf' AS `asf`
