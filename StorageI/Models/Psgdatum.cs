using System;
using System.Collections.Generic;

namespace StorageI.Models
{
    public partial class Psgdatum
    {
        public Psgdatum()
        {
            Contacts = new HashSet<Contact>();
            Fireworks = new HashSet<Firework>();
            InverseParentNavigation = new HashSet<Psgdatum>();
            Kostyms = new HashSet<Kostym>();
            Organizations = new HashSet<Organization>();
            Penas = new HashSet<Pena>();
            Personals = new HashSet<Personal>();
            Sizods = new HashSet<Sizod>();
            Sostavs = new HashSet<Sostav>();
            Sredstvas = new HashSet<Sredstva>();
            Waters = new HashSet<Waters>();
        }

        public int Id { get; set; }
        public int? Parent { get; set; }
        public bool Datafilled { get; set; }
        public string? Garnizon { get; set; }
        public bool? Old { get; set; }
        public int Norder { get; set; }
        public string? Rank { get; set; }
        public string? Garntype { get; set; }
        public string? Fullname { get; set; }
        public int? Karaul { get; set; }
        public string? ForControl { get; set; }
        public bool? Visibility { get; set; }
        public int Dataerr { get; set; }
        public string? ForRep2 { get; set; }
        public DateTime Mdate { get; set; }
        /// <summary>
        /// подразделение, местный, территориальный
        /// </summary>
        public int? GarnTypeid { get; set; }
        public int АцBr { get; set; }
        public int АцRezerv { get; set; }
        public int АцRemont { get; set; }
        public int ПнсBr { get; set; }
        public int ПнсRezerv { get; set; }
        public int ПнсRemont { get; set; }
        public int АцлBr { get; set; }
        public int АцлRezerv { get; set; }
        public int АцлRemont { get; set; }
        public int АсаBr { get; set; }
        public int АсаRezerv { get; set; }
        public int АсаRemont { get; set; }
        public int АнрBr { get; set; }
        public int АнрRezerv { get; set; }
        public int АнрRemont { get; set; }
        public int АвBr { get; set; }
        public int АвRezerv { get; set; }
        public int АвRemont { get; set; }
        public int Ал30Br { get; set; }
        public int Ал30Rezerv { get; set; }
        public int Ал30Remont { get; set; }
        public int Ал50Br { get; set; }
        public int Ал50Rezerv { get; set; }
        public int Ал50Remont { get; set; }
        public int? АкпBr { get; set; }
        public int? АкпRezerv { get; set; }
        public int? АкпRemont { get; set; }
        public int? АрBr { get; set; }
        public int? АрRezerv { get; set; }
        public int? АрRemont { get; set; }
        public int? АмпBr { get; set; }
        public int? АмпRezerv { get; set; }
        public int? АмпRemont { get; set; }
        public int? АгдзсBr { get; set; }
        public int? АгдзсRezerv { get; set; }
        public int? АгдзсRemont { get; set; }
        public int? ПсаBr { get; set; }
        public int? ПсаRezerv { get; set; }
        public int? ПсаRemont { get; set; }
        public int? УксBr { get; set; }
        public int? УксRezerv { get; set; }
        public int? УксRemont { get; set; }
        public int? АсмBr { get; set; }
        public int? АсмRezerv { get; set; }
        public int? АсмRemont { get; set; }
        public int? АппBr { get; set; }
        public int? АппRezerv { get; set; }
        public int? АппRemont { get; set; }
        public int? ОперативнаяГруппаBr { get; set; }
        public int? ОперативнаяГруппаRezerv { get; set; }
        public int? ОперативнаяГруппаRemont { get; set; }
        public int? МотопомпыBr { get; set; }
        public int? МотопомпыRezerv { get; set; }
        public int? МотопомпыRemont { get; set; }
        public int? Арс14Br { get; set; }
        public int? Арс14Rezerv { get; set; }
        public int? Арс14Remont { get; set; }
        public int? ПриспособленныеДляПеревозкиОвBr { get; set; }
        public int? ПриспособленныеДляПеревозкиОвRezerv { get; set; }
        public int? ПриспособленныеДляПеревозкиОвRemont { get; set; }
        public int? СнегоходыBr { get; set; }
        public int? СнегоходыRezerv { get; set; }
        public int? СнегоходыRemont { get; set; }
        public int? КвадроциклыBr { get; set; }
        public int? КвадроциклыRezerv { get; set; }
        public int? КвадроциклыRemont { get; set; }
        public int? КатераЛодкиBr { get; set; }
        public int? КатераЛодкиRezerv { get; set; }
        public int? КатераЛодкиRemont { get; set; }
        public int? СвпBr { get; set; }
        public int? СвпRezerv { get; set; }
        public int? СвпRemont { get; set; }
        public int? Ав1Br { get; set; }
        public int? Ав1Rezerv { get; set; }
        public int? Ав1Remont { get; set; }
        public int? ГрузовойАвтомобильBr { get; set; }
        public int? ГрузовойАвтомобильRezerv { get; set; }
        public int? ГрузовойАвтомобильRemont { get; set; }
        public int? ПожарныйПоездBr { get; set; }
        public int? ПожарныйПоездRezerv { get; set; }
        public int? ПожарныйПоездRemont { get; set; }
        public string? МаркаБпла1 { get; set; }
        public int? Бпла1Br { get; set; }
        public int? Бпла1Rezerv { get; set; }
        public int? Бпла1Remont { get; set; }
        public int? МаркаБпла2 { get; set; }
        public int? Бпла2Br { get; set; }
        public int? Бпла2Rezerv { get; set; }
        public int? Бпла2Remont { get; set; }
        public int? ВодолазноеСнаряжениеBr { get; set; }
        public int? ВодолазноеСнаряжениеRezerv { get; set; }
        public int? ВодолазноеСнаряжениеRemont { get; set; }
        public int? ВодолазноеСнаряжениеКомплектBr { get; set; }
        public int? ВодолазноеСнаряжениеКомплектRezerv { get; set; }
        public int? ВодолазноеСнаряжениеКомплектRemont { get; set; }
        public string? МаркаГасиРучной { get; set; }
        public int? ГасиРучнойBr { get; set; }
        public int? ГасиРучнойRemont { get; set; }
        public int? ГасиРучнойRezerv { get; set; }
        public int? МаркаГасиМех { get; set; }
        public int? ГасиМеханизированныйBr { get; set; }
        public int? ГасиМеханизированныйRezerv { get; set; }
        public int? ГасиМеханизированныйRemont { get; set; }
        public int? МедКомплектBr { get; set; }
        public int? МедКомплектRezerv { get; set; }
        public int? МедКомплектRemont { get; set; }
        public int? БензорезыBr { get; set; }
        public int? БензорезыRezerv { get; set; }
        public int? БензорезыRemont { get; set; }
        public int? БензопилыBr { get; set; }
        public int? БензопилыRezerv { get; set; }
        public int? БензопилыRemont { get; set; }
        public int? ИглаBr { get; set; }
        public int? ИглаRezerv { get; set; }
        public int? ИглаRemont { get; set; }
        public int? РанцевыеОгнетушителиBr { get; set; }
        public int? РанцевыеОгнетушителиRezerv { get; set; }
        public int? РанцевыеОгнетушителиRemont { get; set; }
        public string? SizodsMname { get; set; }
        public int? SizodsRaschet { get; set; }
        public int? SizodsRezerv { get; set; }
        public int? SizodsPostGdzs { get; set; }
        public int? SizodsBazaGdzs { get; set; }
        public int? Ток { get; set; }
        public int? Таск { get; set; }
        public int? ПоСписку { get; set; }
        public int? Налицо { get; set; }
        public int? Всего { get; set; }
        public int? Нк { get; set; }
        public int? Диспетчер { get; set; }
        public int? Пнк { get; set; }
        public int? Ко { get; set; }
        public int? Водители { get; set; }
        public int? Пожарные { get; set; }
        public int? ЛсВБр { get; set; }
        public int? Водолазы { get; set; }
        public int? Гимс { get; set; }
        public int? Крпсс { get; set; }
        public int? ПнкГдзс { get; set; }
        public int? НкГдзс { get; set; }
        public int? КоГдзс { get; set; }
        public int? ВодителиГдзс { get; set; }
        public int? ПожарныеГдзс { get; set; }
        public int? ВсегоОтсутствуют { get; set; }
        public int? Отпуск { get; set; }
        public int? ПоБольничному { get; set; }
        public int? Командировка { get; set; }
        public int? Прочее { get; set; }
        public int? ПгTotal { get; set; }
        public int? ПгFault { get; set; }
        public int? ПвTotal { get; set; }
        public int? ПвFault { get; set; }
        public int? ПпTotal { get; set; }
        public int? ПпFault { get; set; }
        public int? ПенообразовательInwork { get; set; }
        public int? ПенообразовательInrezerv { get; set; }
        public string НачальникКараула { get; set; } = null!;
        public string ОперативныйДежурныйПоГарнизону { get; set; } = null!;
        public string ДежурныйОтГпн { get; set; } = null!;
        public string ДежурныйОтГимс { get; set; } = null!;
        public string ОтветственныйЗаСборДпо { get; set; } = null!;
        public string ДиспетчерПсг { get; set; } = null!;
        public string НачальникДежурнойСменыТпсг { get; set; } = null!;
        public string РуководительСменыТпсг { get; set; } = null!;
        public string СтаршийПомошникТпсг { get; set; } = null!;
        public string НачальникТпсг { get; set; } = null!;
        public int Isgps { get; set; }
        public int АмбулансBr { get; set; }
        public int АмбулансRezerv { get; set; }
        public int АмбулансRemont { get; set; }
        public int АшBr { get; set; }
        public int АшRezerv { get; set; }
        public int АшRemont { get; set; }
        public int НожницыBr { get; set; }
        public int НожницыRezerv { get; set; }
        public int НожницыRemont { get; set; }
        public int РазжимBr { get; set; }
        public int РазжимRezerv { get; set; }
        public int РазжимRemont { get; set; }
        public string Errcolumns { get; set; } = null!;
        public int Tofirst { get; set; }
        public int Totwo { get; set; }
        public int? Spisok { get; set; }
        public int ПовышАш { get; set; }
        public int ПовышГдзс { get; set; }
        public int ПовышКо { get; set; }
        public int ПовышВод { get; set; }
        public int ПовышПож { get; set; }
        public int ПовышИтогоЛс { get; set; }
        public int Посписку1 { get; set; }
        public int Посписку2 { get; set; }
        public int Посписку3 { get; set; }
        public int Посписку4 { get; set; }
        public int ЛсПоШтату { get; set; }
        public int ЛсПоСписку { get; set; }
        public int ЛсНаДежурстве { get; set; }
        public int ТехникаВсегоПоШтату { get; set; }
        public int ТехникаВсегоНаДежурстве { get; set; }
        public int АвиацияПоШтату { get; set; }
        public int АвиацияНаДежурстве { get; set; }
        public int АвтомобилиПоШтату { get; set; }
        public int АвтомобилиНаДежурстве { get; set; }
        public int ПлавсредстваПоШтату { get; set; }
        public int ПлавсредстваНаДежурстве { get; set; }
        public int СпецтехникаПоШтату { get; set; }
        public long СпецтехникаНаДежурстве { get; set; }
        public int АсоBr { get; set; }
        public int АсоRezerv { get; set; }
        public int АсоRemont { get; set; }
        public int АсмпхBr { get; set; }
        public int АсмпхRezerv { get; set; }
        public int АсмпхRemont { get; set; }
        public int БолотоходыBr { get; set; }
        public int БолотоходыRezerv { get; set; }
        public int БолотоходыRemont { get; set; }
        public int ПожарныйКорабльBr { get; set; }
        public int ПожарныйКорабльRezerv { get; set; }
        public int ПожарныйКорабльRemont { get; set; }
        public int КпBr { get; set; }
        public int КпRezerv { get; set; }
        public int КпRemont { get; set; }
        public int ПлавсредствоBr { get; set; }
        public int ПлавсредствоRezerv { get; set; }
        public int ПлавсредствоRemont { get; set; }
        public int ПлавсрBr { get; set; }
        public int ПлавсрRezerv { get; set; }
        public int ПлавсрRemont { get; set; }
        public int АбгBr { get; set; }
        public int АбгRezerv { get; set; }
        public int АбгRemont { get; set; }

        public virtual Psgdatum? ParentNavigation { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Firework> Fireworks { get; set; }
        public virtual ICollection<Psgdatum> InverseParentNavigation { get; set; }
        public virtual ICollection<Kostym> Kostyms { get; set; }
        public virtual ICollection<Organization> Organizations { get; set; }
        public virtual ICollection<Pena> Penas { get; set; }
        public virtual ICollection<Personal> Personals { get; set; }
        public virtual ICollection<Sizod> Sizods { get; set; }
        public virtual ICollection<Sostav> Sostavs { get; set; }
        public virtual ICollection<Sredstva> Sredstvas { get; set; }
        public virtual ICollection<Waters> Waters { get; set; }
    }
}
