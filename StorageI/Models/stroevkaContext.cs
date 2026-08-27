using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace StorageI.Models
{
    public partial class stroevkaContext : DbContext
    {
        public stroevkaContext()
        {
        }

        public stroevkaContext(DbContextOptions<stroevkaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Apch> Apchs { get; set; } = null!;
        public virtual DbSet<ApchsView> ApchsViews { get; set; } = null!;
        public virtual DbSet<Bpivot> Bpivots { get; set; } = null!;
        public virtual DbSet<CacheNachkar> CacheNachkars { get; set; } = null!;
        public virtual DbSet<Call> Calls { get; set; } = null!;
        public virtual DbSet<Contact> Contacts { get; set; } = null!;
        public virtual DbSet<Dbfield> Dbfields { get; set; } = null!;
        public virtual DbSet<Dblog> Dblogs { get; set; } = null!;
        public virtual DbSet<Departure> Departures { get; set; } = null!;
        public virtual DbSet<Dpd> Dpds { get; set; } = null!;
        public virtual DbSet<Dpk> Dpks { get; set; } = null!;
        public virtual DbSet<FireEquip> FireEquips { get; set; } = null!;
        public virtual DbSet<FireEquipsPivot> FireEquipsPivots { get; set; } = null!;
        public virtual DbSet<FireGarn> FireGarns { get; set; } = null!;
        public virtual DbSet<FireLiquidationPlan> FireLiquidationPlans { get; set; } = null!;
        public virtual DbSet<FirePsgItog> FirePsgItogs { get; set; } = null!;
        public virtual DbSet<FirePsgStat> FirePsgStats { get; set; } = null!;
        public virtual DbSet<FirePsgStatByCategory> FirePsgStatByCategories { get; set; } = null!;
        public virtual DbSet<FireTpsgStat> FireTpsgStats { get; set; } = null!;
        public virtual DbSet<Fireauto> Fireautos { get; set; } = null!;
        public virtual DbSet<Firecall> Firecalls { get; set; } = null!;
        public virtual DbSet<Firecar> Firecars { get; set; } = null!;
        public virtual DbSet<Firework> Fireworks { get; set; } = null!;
        public virtual DbSet<Garndatum> Garndata { get; set; } = null!;
        public virtual DbSet<Garnizon> Garnizons { get; set; } = null!;
        public virtual DbSet<Group> Groups { get; set; } = null!;
        public virtual DbSet<Kostym> Kostyms { get; set; } = null!;
        public virtual DbSet<Mhelp> Mhelps { get; set; } = null!;
        public virtual DbSet<Msg> Msgs { get; set; } = null!;
        public virtual DbSet<Mtable> Mtables { get; set; } = null!;
        public virtual DbSet<Organization> Organizations { get; set; } = null!;
        public virtual DbSet<Pena> Penas { get; set; } = null!;
        public virtual DbSet<Personal> Personals { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<Psg> Psgs { get; set; } = null!;
        public virtual DbSet<Psgdatum> Psgdata { get; set; } = null!;
        public virtual DbSet<Psgparam> Psgparams { get; set; } = null!;
        public virtual DbSet<Psostatistic> Psostatistics { get; set; } = null!;
        public virtual DbSet<Report3gu> Report3gus { get; set; } = null!;
        public virtual DbSet<Reportstroevka> Reportstroevkas { get; set; } = null!;
        public virtual DbSet<Sizod> Sizods { get; set; } = null!;
        public virtual DbSet<Sostav> Sostavs { get; set; } = null!;
        public virtual DbSet<SostavVid> SostavVids { get; set; } = null!;
        public virtual DbSet<Sredstva> Sredstvas { get; set; } = null!;
        public virtual DbSet<SredstvaVid> SredstvaVids { get; set; } = null!;
        public virtual DbSet<SredstvaДоУдал> SredstvaДоУдалs { get; set; } = null!;
        public virtual DbSet<Stroevkaparam> Stroevkaparams { get; set; } = null!;
        public virtual DbSet<Vid> Vids { get; set; } = null!;
        public virtual DbSet<Waters> Waters { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=Djkjlz1; database=stroevka; Character Set=utf8; Convert Zero Datetime=True; Allow Zero Datetime=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Apch>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("apchs");

                entity.Property(e => e.Dataerr)
                    .HasColumnType("int(11)")
                    .HasColumnName("dataerr");

                entity.Property(e => e.Datafilled).HasColumnName("datafilled");

                entity.Property(e => e.ForControl)
                    .HasMaxLength(255)
                    .HasColumnName("forControl");

                entity.Property(e => e.ForRep2)
                    .HasMaxLength(255)
                    .HasColumnName("forRep2");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(511)
                    .HasColumnName("fullname")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.GarnTypeid)
                    .HasColumnType("int(11)")
                    .HasColumnName("garn_typeid")
                    .HasComment("подразделение, местный, территориальный");

                entity.Property(e => e.Garnizon)
                    .HasMaxLength(127)
                    .HasColumnName("garnizon");

                entity.Property(e => e.Garntype)
                    .HasMaxLength(31)
                    .HasColumnName("garntype");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Karaul)
                    .HasColumnType("int(11)")
                    .HasColumnName("karaul")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Mdate)
                    .HasColumnType("timestamp")
                    .HasColumnName("mdate")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.Name)
                    .HasMaxLength(127)
                    .HasColumnName("name");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder")
                    .HasDefaultValueSql("'100'");

                entity.Property(e => e.Old)
                    .IsRequired()
                    .HasColumnName("old")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Parent)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent");

                entity.Property(e => e.Rank)
                    .HasMaxLength(31)
                    .HasColumnName("rank");

                entity.Property(e => e.Visibility)
                    .IsRequired()
                    .HasColumnName("visibility")
                    .HasDefaultValueSql("'1'");
            });

            modelBuilder.Entity<ApchsView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("apchs_view");

                entity.Property(e => e.Dataerr)
                    .HasColumnType("int(11)")
                    .HasColumnName("dataerr");

                entity.Property(e => e.Datafilled).HasColumnName("datafilled");

                entity.Property(e => e.ForControl)
                    .HasMaxLength(255)
                    .HasColumnName("forControl");

                entity.Property(e => e.ForRep2)
                    .HasMaxLength(255)
                    .HasColumnName("forRep2");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(511)
                    .HasColumnName("fullname")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.GarnTypeid)
                    .HasColumnType("int(11)")
                    .HasColumnName("garn_typeid")
                    .HasComment("подразделение, местный, территориальный");

                entity.Property(e => e.Garnizon)
                    .HasMaxLength(127)
                    .HasColumnName("garnizon");

                entity.Property(e => e.Garntype)
                    .HasMaxLength(31)
                    .HasColumnName("garntype");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Karaul)
                    .HasColumnType("int(11)")
                    .HasColumnName("karaul")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Mdate)
                    .HasColumnType("timestamp")
                    .HasColumnName("mdate")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.Name)
                    .HasMaxLength(127)
                    .HasColumnName("name");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder")
                    .HasDefaultValueSql("'100'");

                entity.Property(e => e.Old)
                    .IsRequired()
                    .HasColumnName("old")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Parent)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent");

                entity.Property(e => e.Rank)
                    .HasMaxLength(31)
                    .HasColumnName("rank");

                entity.Property(e => e.Visibility)
                    .IsRequired()
                    .HasColumnName("visibility")
                    .HasDefaultValueSql("'1'");
            });

            modelBuilder.Entity<Bpivot>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("bpivot");

                entity.Property(e => e.AcBr)
                    .HasPrecision(32)
                    .HasColumnName("ac_br");

                entity.Property(e => e.AcRemont)
                    .HasPrecision(32)
                    .HasColumnName("ac_remont");

                entity.Property(e => e.AcRezerv)
                    .HasPrecision(32)
                    .HasColumnName("ac_rezerv");

                entity.Property(e => e.AclBr)
                    .HasPrecision(32)
                    .HasColumnName("acl_br");

                entity.Property(e => e.AclRemont)
                    .HasPrecision(32)
                    .HasColumnName("acl_remont");

                entity.Property(e => e.AclRezerv)
                    .HasPrecision(32)
                    .HasColumnName("acl_rezerv");

                entity.Property(e => e.AlBr)
                    .HasPrecision(32)
                    .HasColumnName("al_br");

                entity.Property(e => e.AlRemont)
                    .HasPrecision(32)
                    .HasColumnName("al_remont");

                entity.Property(e => e.AlRezerv)
                    .HasPrecision(32)
                    .HasColumnName("al_rezerv");

                entity.Property(e => e.Category)
                    .HasMaxLength(31)
                    .HasColumnName("category");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Isitog)
                    .HasColumnType("int(1)")
                    .HasColumnName("isitog");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder")
                    .HasDefaultValueSql("'100'");

                entity.Property(e => e.Parent)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent");

                entity.Property(e => e.SizodBr)
                    .HasPrecision(32)
                    .HasColumnName("sizod_br");

                entity.Property(e => e.SizodRezerv)
                    .HasPrecision(32)
                    .HasColumnName("sizod_rezerv");

                entity.Property(e => e.Tofirst)
                    .HasPrecision(32)
                    .HasColumnName("tofirst");

                entity.Property(e => e.Totow)
                    .HasPrecision(32)
                    .HasColumnName("totow");

                entity.Property(e => e.АвBr)
                    .HasPrecision(32)
                    .HasColumnName("ав_br");

                entity.Property(e => e.АвRemont)
                    .HasPrecision(32)
                    .HasColumnName("ав_remont");

                entity.Property(e => e.АвRezerv)
                    .HasPrecision(32)
                    .HasColumnName("ав_rezerv");

                entity.Property(e => e.АвсBr)
                    .HasPrecision(32)
                    .HasColumnName("АВС_br");

                entity.Property(e => e.АвсRezerv)
                    .HasPrecision(32)
                    .HasColumnName("АВС_rezerv");

                entity.Property(e => e.АнрBr)
                    .HasPrecision(32)
                    .HasColumnName("анр_br");

                entity.Property(e => e.АнрRemont)
                    .HasPrecision(32)
                    .HasColumnName("анр_remont");

                entity.Property(e => e.АнрRezerv)
                    .HasPrecision(32)
                    .HasColumnName("анр_rezerv");

                entity.Property(e => e.АрBr)
                    .HasPrecision(32)
                    .HasColumnName("ар_br");

                entity.Property(e => e.АрRemont)
                    .HasPrecision(32)
                    .HasColumnName("ар_remont");

                entity.Property(e => e.АрRezerv)
                    .HasPrecision(32)
                    .HasColumnName("ар_rezerv");

                entity.Property(e => e.АсаBr)
                    .HasPrecision(32)
                    .HasColumnName("аса_br");

                entity.Property(e => e.АсаRemont)
                    .HasPrecision(32)
                    .HasColumnName("аса_remont");

                entity.Property(e => e.АсаRezerv)
                    .HasPrecision(32)
                    .HasColumnName("аса_rezerv");

                entity.Property(e => e.АсаАппАсмBr)
                    .HasPrecision(32)
                    .HasColumnName("аса_апп_асм_br");

                entity.Property(e => e.АсаАппАсмRemont)
                    .HasPrecision(32)
                    .HasColumnName("аса_апп_асм_remont");

                entity.Property(e => e.АсаАппАсмRezerv)
                    .HasPrecision(32)
                    .HasColumnName("аса_апп_асм_rezerv");

                entity.Property(e => e.АсмпПсаBr)
                    .HasPrecision(32)
                    .HasColumnName("асмп_пса_br");

                entity.Property(e => e.АсмпПсаRemont)
                    .HasPrecision(32)
                    .HasColumnName("асмп_пса_remont");

                entity.Property(e => e.АсмпПсаRezerv)
                    .HasPrecision(32)
                    .HasColumnName("асмп_пса_rezerv");

                entity.Property(e => e.АсмрхBr)
                    .HasPrecision(32)
                    .HasColumnName("АСМРХ_br");

                entity.Property(e => e.АсмрхRezerv)
                    .HasPrecision(32)
                    .HasColumnName("АСМРХ_rezerv");

                entity.Property(e => e.АсоBr)
                    .HasPrecision(32)
                    .HasColumnName("асо_br");

                entity.Property(e => e.АсоRemont)
                    .HasPrecision(32)
                    .HasColumnName("асо_remont");

                entity.Property(e => e.АсоRezerv)
                    .HasPrecision(32)
                    .HasColumnName("асо_rezerv");

                entity.Property(e => e.АшBr)
                    .HasPrecision(32)
                    .HasColumnName("аш_br");

                entity.Property(e => e.АшRemont)
                    .HasPrecision(32)
                    .HasColumnName("аш_remont");

                entity.Property(e => e.АшRezerv)
                    .HasPrecision(32)
                    .HasColumnName("аш_rezerv");

                entity.Property(e => e.Бензин).HasPrecision(33);

                entity.Property(e => e.Болотоходы)
                    .HasPrecision(34)
                    .HasColumnName("болотоходы");

                entity.Property(e => e.Водитель).HasPrecision(32);

                entity.Property(e => e.Всего)
                    .HasPrecision(32)
                    .HasColumnName("всего");

                entity.Property(e => e.ВсегоОтс)
                    .HasPrecision(32)
                    .HasColumnName("всего_отс");

                entity.Property(e => e.ГасиРасчёт)
                    .HasPrecision(32)
                    .HasColumnName("ГАСИ_расчёт");

                entity.Property(e => e.ГасиРезерв)
                    .HasPrecision(32)
                    .HasColumnName("ГАСИ_резерв");

                entity.Property(e => e.Гдзс)
                    .HasPrecision(32)
                    .HasColumnName("ГДЗС");

                entity.Property(e => e.Диспетчер).HasPrecision(32);

                entity.Property(e => e.Дт)
                    .HasPrecision(33)
                    .HasColumnName("ДТ");

                entity.Property(e => e.Ко)
                    .HasPrecision(32)
                    .HasColumnName("КО");

                entity.Property(e => e.Командировка)
                    .HasPrecision(32)
                    .HasColumnName("командировка");

                entity.Property(e => e.КостюмыДругие)
                    .HasPrecision(32)
                    .HasColumnName("костюмы_другие");

                entity.Property(e => e.КостюмыЛ1Таск)
                    .HasPrecision(32)
                    .HasColumnName("костюмы_Л-1_ТАСК");

                entity.Property(e => e.КостюмыТок)
                    .HasPrecision(32)
                    .HasColumnName("костюмы_ТОК");

                entity.Property(e => e.КпBr)
                    .HasPrecision(32)
                    .HasColumnName("кп_br");

                entity.Property(e => e.КпRemont)
                    .HasPrecision(32)
                    .HasColumnName("кп_remont");

                entity.Property(e => e.КпRezerv)
                    .HasPrecision(32)
                    .HasColumnName("кп_rezerv");

                entity.Property(e => e.Мотопомпы)
                    .HasPrecision(34)
                    .HasColumnName("мотопомпы");

                entity.Property(e => e.Налицо).HasPrecision(32);

                entity.Property(e => e.Начкар)
                    .HasMaxLength(255)
                    .HasColumnName("начкар");

                entity.Property(e => e.Некомплект)
                    .HasPrecision(32)
                    .HasColumnName("некомплект");

                entity.Property(e => e.Нк)
                    .HasPrecision(32)
                    .HasColumnName("НК");

                entity.Property(e => e.Отпуск)
                    .HasPrecision(32)
                    .HasColumnName("отпуск");

                entity.Property(e => e.ПенаРасчёт)
                    .HasPrecision(32)
                    .HasColumnName("пена_расчёт");

                entity.Property(e => e.ПенаРезерв)
                    .HasPrecision(32)
                    .HasColumnName("пена_резерв");

                entity.Property(e => e.ПлавСредства)
                    .HasPrecision(34)
                    .HasColumnName("плав_средства");

                entity.Property(e => e.Пнк)
                    .HasPrecision(32)
                    .HasColumnName("ПНК");

                entity.Property(e => e.ПнсBr)
                    .HasPrecision(32)
                    .HasColumnName("пнс_br");

                entity.Property(e => e.ПнсRemont)
                    .HasPrecision(32)
                    .HasColumnName("пнс_remont");

                entity.Property(e => e.ПнсRezerv)
                    .HasPrecision(32)
                    .HasColumnName("пнс_rezerv");

                entity.Property(e => e.ПоБольничному)
                    .HasPrecision(32)
                    .HasColumnName("по_больничному");

                entity.Property(e => e.ПоСписку)
                    .HasPrecision(32)
                    .HasColumnName("по_списку");

                entity.Property(e => e.ПожПоездКорабльBr)
                    .HasPrecision(32)
                    .HasColumnName("пож_поезд_корабль_br");

                entity.Property(e => e.ПожПоездКорабльRemont)
                    .HasPrecision(32)
                    .HasColumnName("пож_поезд_корабль_remont");

                entity.Property(e => e.ПожПоездКорабльRezerv)
                    .HasPrecision(32)
                    .HasColumnName("пож_поезд_корабль_rezerv");

                entity.Property(e => e.Пожарный).HasPrecision(32);

                entity.Property(e => e.ПожарныйКорабльРемонт)
                    .HasPrecision(33)
                    .HasColumnName("пожарный_корабль_ремонт");

                entity.Property(e => e.ПорошокРасчёт)
                    .HasColumnType("int(1)")
                    .HasColumnName("порошок_расчёт");

                entity.Property(e => e.ПорошокРезерв)
                    .HasColumnType("int(1)")
                    .HasColumnName("порошок_резерв");

                entity.Property(e => e.Прочее)
                    .HasPrecision(33)
                    .HasColumnName("прочее");

                entity.Property(e => e.ПрочиеОтс)
                    .HasPrecision(32)
                    .HasColumnName("прочие_отс");

                entity.Property(e => e.Псг)
                    .HasMaxLength(127)
                    .HasColumnName("ПСГ");

                entity.Property(e => e.Пч)
                    .HasMaxLength(127)
                    .HasColumnName("ПЧ");

                entity.Property(e => e.Резерв)
                    .HasPrecision(32)
                    .HasColumnName("резерв");

                entity.Property(e => e.РемонтОсновной)
                    .HasPrecision(32)
                    .HasColumnName("ремонт_основной");

                entity.Property(e => e.РемонтСпециальной)
                    .HasPrecision(32)
                    .HasColumnName("ремонт_специальной");

                entity.Property(e => e.УксАбгBr)
                    .HasPrecision(32)
                    .HasColumnName("укс_абг_br");

                entity.Property(e => e.УксАбгRemont)
                    .HasPrecision(32)
                    .HasColumnName("укс_абг_remont");

                entity.Property(e => e.УксАбгRezerv)
                    .HasPrecision(32)
                    .HasColumnName("укс_абг_rezerv");
            });

            modelBuilder.Entity<CacheNachkar>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("cache_nachkar");

                entity.HasIndex(e => e.SubdivisionId, "idx_subdivision");

                entity.Property(e => e.Nachkar)
                    .HasMaxLength(255)
                    .HasColumnName("nachkar");

                entity.Property(e => e.SubdivisionId)
                    .HasColumnType("int(11)")
                    .HasColumnName("subdivision_id");
            });

            modelBuilder.Entity<Call>(entity =>
            {
                entity.ToTable("calls");

                entity.HasComment("Выезда");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Mguid)
                    .HasMaxLength(36)
                    .HasColumnName("mguid");

                entity.Property(e => e.Адрес).HasMaxLength(255);

                entity.Property(e => e.Время).HasColumnType("datetime");

                entity.Property(e => e.ВремяИзм)
                    .HasColumnType("timestamp")
                    .HasColumnName("времяИзм")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Выезды).HasMaxLength(255);

                entity.Property(e => e.Дата).HasColumnType("datetime");

                entity.Property(e => e.Результат).HasMaxLength(255);

                entity.Property(e => e.Фабула).HasMaxLength(255);
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("contacts");

                entity.HasIndex(e => e.GarnizonId, "FK_contacts_garnizon_id");

                entity.HasIndex(e => e.SubdivisionId, "FK_contacts_subdivision_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.EditTime)
                    .HasColumnType("timestamp")
                    .HasColumnName("edit_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Excel)
                    .HasMaxLength(255)
                    .HasColumnName("excel");

                entity.Property(e => e.Fio)
                    .HasMaxLength(255)
                    .HasColumnName("FIO");

                entity.Property(e => e.GarnizonId)
                    .HasColumnType("int(11)")
                    .HasColumnName("garnizon_id");

                entity.Property(e => e.Karaul)
                    .HasColumnType("int(11)")
                    .HasColumnName("karaul")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Mdate)
                    .HasColumnType("date")
                    .HasColumnName("mdate");

                entity.Property(e => e.NameGarnizone)
                    .HasMaxLength(255)
                    .HasColumnName("name_garnizone");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder");

                entity.Property(e => e.Post)
                    .HasMaxLength(255)
                    .HasColumnName("post");

                entity.Property(e => e.PostId)
                    .HasColumnType("int(11)")
                    .HasColumnName("post_id");

                entity.Property(e => e.Posyvnoy)
                    .HasMaxLength(255)
                    .HasColumnName("posyvnoy");

                entity.Property(e => e.Subdivision)
                    .HasMaxLength(255)
                    .HasColumnName("subdivision");

                entity.Property(e => e.SubdivisionId)
                    .HasColumnType("int(11)")
                    .HasColumnName("subdivision_id");

                entity.Property(e => e.TfDom)
                    .HasMaxLength(255)
                    .HasColumnName("tf_dom");

                entity.Property(e => e.TfMobil)
                    .HasMaxLength(255)
                    .HasColumnName("tf_mobil");

                entity.Property(e => e.TfWork)
                    .HasMaxLength(255)
                    .HasColumnName("tf_work");

                entity.HasOne(d => d.SubdivisionNavigation)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.SubdivisionId)
                    .HasConstraintName("FK_contacts_subdivision_id");
            });

            modelBuilder.Entity<Dbfield>(entity =>
            {
                entity.ToTable("dbfield");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Enabled)
                    .IsRequired()
                    .HasColumnName("enabled")
                    .HasDefaultValueSql("'1'")
                    .HasComment("Не показывать вообще");

                entity.Property(e => e.FilterExpr)
                    .HasMaxLength(255)
                    .HasColumnName("filter_expr")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.FilterOn).HasColumnName("filter_on");

                entity.Property(e => e.GrouppingOn).HasColumnName("groupping_on");

                entity.Property(e => e.Grouppingfield).HasColumnName("grouppingfield");

                entity.Property(e => e.MDisplay)
                    .HasMaxLength(50)
                    .HasColumnName("m_display");

                entity.Property(e => e.MEditForm)
                    .HasColumnType("int(11)")
                    .HasColumnName("m_editForm")
                    .HasComment("1 - есть форма редактирования 0 - нет ");

                entity.Property(e => e.MEditenable)
                    .HasColumnType("int(11)")
                    .HasColumnName("m_editenable")
                    .HasComment("1 - запрет редактирования в форме 0 - разрешено ");

                entity.Property(e => e.MGroup)
                    .HasMaxLength(63)
                    .HasColumnName("m_group");

                entity.Property(e => e.MLength)
                    .HasColumnType("int(11)")
                    .HasColumnName("m_length")
                    .HasDefaultValueSql("'70'");

                entity.Property(e => e.MName)
                    .HasMaxLength(50)
                    .HasColumnName("m_name");

                entity.Property(e => e.MOrder)
                    .HasColumnType("int(11)")
                    .HasColumnName("m_order")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.MSubgroup)
                    .HasMaxLength(63)
                    .HasColumnName("m_subgroup");

                entity.Property(e => e.MTable)
                    .HasMaxLength(50)
                    .HasColumnName("m_table");

                entity.Property(e => e.MType)
                    .HasMaxLength(255)
                    .HasColumnName("m_type")
                    .HasComment("Тип столбца в гриде - comboBox, Text, int ...");

                entity.Property(e => e.MVisible)
                    .IsRequired()
                    .HasColumnName("m_visible")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.NGroup)
                    .HasColumnType("int(11)")
                    .HasColumnName("n_group");

                entity.Property(e => e.NTable)
                    .HasColumnType("int(11)")
                    .HasColumnName("n_table");

                entity.Property(e => e.Parent)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent");

                entity.Property(e => e.ParentGrouppingfield)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent_grouppingfield");
            });

            modelBuilder.Entity<Dblog>(entity =>
            {
                entity.ToTable("dblog");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Computer)
                    .HasMaxLength(63)
                    .HasColumnName("computer");

                entity.Property(e => e.IdPunct)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_punct");

                entity.Property(e => e.IdRegion)
                    .HasColumnType("int(10)")
                    .HasColumnName("id_region");

                entity.Property(e => e.IdSettle)
                    .HasColumnType("int(10)")
                    .HasColumnName("id_settle");

                entity.Property(e => e.KeyValue)
                    .HasMaxLength(127)
                    .HasColumnName("keyValue");

                entity.Property(e => e.Logitem)
                    .HasMaxLength(2000)
                    .HasColumnName("logitem");

                entity.Property(e => e.Loglevel)
                    .HasColumnType("int(11)")
                    .HasColumnName("loglevel");

                entity.Property(e => e.Operation)
                    .HasMaxLength(15)
                    .HasColumnName("operation");

                entity.Property(e => e.PrimaryKeyName)
                    .HasMaxLength(256)
                    .HasColumnName("primaryKeyName");

                entity.Property(e => e.SetName)
                    .HasMaxLength(127)
                    .HasColumnName("setName");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("timestamp")
                    .HasColumnName("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UserName)
                    .HasMaxLength(63)
                    .HasColumnName("userName");
            });

            modelBuilder.Entity<Departure>(entity =>
            {
                entity.ToTable("departures");

                entity.HasComment("Выезда");

                entity.HasIndex(e => e.IdВызова, "FK_departures_id_вызова");

                entity.HasIndex(e => e.IdАвтомобиля, "FK_fireworks_id_машины");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Idparent)
                    .HasColumnType("int(11)")
                    .HasColumnName("idparent");

                entity.Property(e => e.IdАвтомобиля)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_автомобиля");

                entity.Property(e => e.IdВызова)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_вызова");

                entity.Property(e => e.IdГарнизона)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_гарнизона");

                entity.Property(e => e.Lastchanged)
                    .HasColumnType("timestamp")
                    .HasColumnName("lastchanged")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Mguid)
                    .HasMaxLength(36)
                    .HasColumnName("mguid");

                entity.Property(e => e.Автомобиль).HasMaxLength(255);

                entity.Property(e => e.Адрес).HasMaxLength(255);

                entity.Property(e => e.Возвращение).HasColumnType("datetime");

                entity.Property(e => e.Выезд).HasColumnType("datetime");

                entity.Property(e => e.Гарнизон).HasMaxLength(255);

                entity.Property(e => e.КМестуВызова)
                    .HasColumnType("datetime")
                    .HasColumnName("К_месту_вызова");

                entity.Property(e => e.Комментарий)
                    .HasMaxLength(255)
                    .HasColumnName("комментарий");

                entity.Property(e => e.Ликвидация).HasColumnType("datetime");

                entity.Property(e => e.Локализация).HasColumnType("datetime");

                entity.HasOne(d => d.IdАвтомобиляNavigation)
                    .WithMany(p => p.Departures)
                    .HasForeignKey(d => d.IdАвтомобиля)
                    .HasConstraintName("FK_departures_id_автомобиля");

                entity.HasOne(d => d.IdВызоваNavigation)
                    .WithMany(p => p.Departures)
                    .HasForeignKey(d => d.IdВызова)
                    .HasConstraintName("FK_departures_id_вызова");
            });

            modelBuilder.Entity<Dpd>(entity =>
            {
                entity.ToTable("dpd");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Garnizon)
                    .HasMaxLength(255)
                    .HasColumnName("garnizon");

                entity.Property(e => e.Idpsgdata)
                    .HasColumnType("int(11)")
                    .HasColumnName("idpsgdata");

                entity.Property(e => e.Punct)
                    .HasMaxLength(100)
                    .HasColumnName("punct");

                entity.Property(e => e.Количорг)
                    .HasColumnType("int(11)")
                    .HasColumnName("количорг");

                entity.Property(e => e.Лсвсего)
                    .HasColumnType("int(11)")
                    .HasColumnName("лсвсего");

                entity.Property(e => e.Пункт)
                    .HasMaxLength(255)
                    .HasColumnName("пункт");

                entity.Property(e => e.Район)
                    .HasMaxLength(255)
                    .HasColumnName("район");

                entity.Property(e => e.Тип)
                    .HasMaxLength(255)
                    .HasColumnName("тип");
            });

            modelBuilder.Entity<Dpk>(entity =>
            {
                entity.ToTable("dpk");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Garnizon)
                    .HasMaxLength(255)
                    .HasColumnName("garnizon");

                entity.Property(e => e.Idpsgdata)
                    .HasColumnType("int(11)")
                    .HasColumnName("idpsgdata");

                entity.Property(e => e.Punct)
                    .HasMaxLength(100)
                    .HasColumnName("punct");

                entity.Property(e => e.Дпкколич)
                    .HasColumnType("int(11)")
                    .HasColumnName("дпкколич");

                entity.Property(e => e.Лсвсего)
                    .HasColumnType("int(11)")
                    .HasColumnName("лсвсего");

                entity.Property(e => e.Лсдеж)
                    .HasColumnType("int(11)")
                    .HasColumnName("лсдеж");

                entity.Property(e => e.Лсрезерв)
                    .HasColumnType("int(11)")
                    .HasColumnName("лсрезерв");

                entity.Property(e => e.Пункт)
                    .HasMaxLength(255)
                    .HasColumnName("пункт");

                entity.Property(e => e.Район)
                    .HasMaxLength(255)
                    .HasColumnName("район");

                entity.Property(e => e.Техвсего)
                    .HasColumnType("int(11)")
                    .HasColumnName("техвсего");

                entity.Property(e => e.Техдеж)
                    .HasColumnType("int(11)")
                    .HasColumnName("техдеж");

                entity.Property(e => e.Тип)
                    .HasMaxLength(255)
                    .HasColumnName("тип");
            });

            modelBuilder.Entity<FireEquip>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("fire_equips");

                entity.Property(e => e.Br)
                    .HasPrecision(32)
                    .HasColumnName("br");

                entity.Property(e => e.Category)
                    .HasMaxLength(6)
                    .HasColumnName("category");

                entity.Property(e => e.IdPsg)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_psg");

                entity.Property(e => e.IdPsgunit)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_psgunit");

                entity.Property(e => e.Remont)
                    .HasPrecision(32)
                    .HasColumnName("remont");

                entity.Property(e => e.Rezerv)
                    .HasPrecision(32)
                    .HasColumnName("rezerv");

                entity.Property(e => e.Sredstvo)
                    .HasMaxLength(255)
                    .HasColumnName("sredstvo");

                entity.Property(e => e.Tofirst)
                    .HasPrecision(32)
                    .HasColumnName("tofirst");

                entity.Property(e => e.Totow)
                    .HasPrecision(32)
                    .HasColumnName("totow");
            });

            modelBuilder.Entity<FireEquipsPivot>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("fire_equips_pivot");

                entity.Property(e => e.AcBr)
                    .HasPrecision(54)
                    .HasColumnName("ac_br");

                entity.Property(e => e.AcRemont)
                    .HasPrecision(54)
                    .HasColumnName("ac_remont");

                entity.Property(e => e.AcRezerv)
                    .HasPrecision(54)
                    .HasColumnName("ac_rezerv");

                entity.Property(e => e.AclBr)
                    .HasPrecision(54)
                    .HasColumnName("acl_br");

                entity.Property(e => e.AclRemont)
                    .HasPrecision(54)
                    .HasColumnName("acl_remont");

                entity.Property(e => e.AclRezerv)
                    .HasPrecision(54)
                    .HasColumnName("acl_rezerv");

                entity.Property(e => e.AlBr)
                    .HasPrecision(54)
                    .HasColumnName("al_br");

                entity.Property(e => e.AlRemont)
                    .HasPrecision(54)
                    .HasColumnName("al_remont");

                entity.Property(e => e.AlRezerv)
                    .HasPrecision(54)
                    .HasColumnName("al_rezerv");

                entity.Property(e => e.Category)
                    .HasMaxLength(6)
                    .HasColumnName("category");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Isitog)
                    .HasColumnType("int(1)")
                    .HasColumnName("isitog");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder")
                    .HasDefaultValueSql("'100'");

                entity.Property(e => e.Parent)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent");

                entity.Property(e => e.SizodBr)
                    .HasPrecision(54)
                    .HasColumnName("sizod_br");

                entity.Property(e => e.SizodRezerv)
                    .HasPrecision(54)
                    .HasColumnName("sizod_rezerv");

                entity.Property(e => e.Tofirst)
                    .HasPrecision(54)
                    .HasColumnName("tofirst");

                entity.Property(e => e.Totow)
                    .HasPrecision(54)
                    .HasColumnName("totow");

                entity.Property(e => e.АвBr)
                    .HasPrecision(54)
                    .HasColumnName("ав_br");

                entity.Property(e => e.АвRemont)
                    .HasPrecision(54)
                    .HasColumnName("ав_remont");

                entity.Property(e => e.АвRezerv)
                    .HasPrecision(54)
                    .HasColumnName("ав_rezerv");

                entity.Property(e => e.АвсBr)
                    .HasPrecision(54)
                    .HasColumnName("АВС_br");

                entity.Property(e => e.АвсRezerv)
                    .HasPrecision(54)
                    .HasColumnName("АВС_rezerv");

                entity.Property(e => e.АнрBr)
                    .HasPrecision(54)
                    .HasColumnName("анр_br");

                entity.Property(e => e.АнрRemont)
                    .HasPrecision(54)
                    .HasColumnName("анр_remont");

                entity.Property(e => e.АнрRezerv)
                    .HasPrecision(54)
                    .HasColumnName("анр_rezerv");

                entity.Property(e => e.АрBr)
                    .HasPrecision(54)
                    .HasColumnName("ар_br");

                entity.Property(e => e.АрRemont)
                    .HasPrecision(54)
                    .HasColumnName("ар_remont");

                entity.Property(e => e.АрRezerv)
                    .HasPrecision(54)
                    .HasColumnName("ар_rezerv");

                entity.Property(e => e.АсаBr)
                    .HasPrecision(54)
                    .HasColumnName("аса_br");

                entity.Property(e => e.АсаRemont)
                    .HasPrecision(54)
                    .HasColumnName("аса_remont");

                entity.Property(e => e.АсаRezerv)
                    .HasPrecision(54)
                    .HasColumnName("аса_rezerv");

                entity.Property(e => e.АсаАппАсмBr)
                    .HasPrecision(54)
                    .HasColumnName("аса_апп_асм_br");

                entity.Property(e => e.АсаАппАсмRemont)
                    .HasPrecision(54)
                    .HasColumnName("аса_апп_асм_remont");

                entity.Property(e => e.АсаАппАсмRezerv)
                    .HasPrecision(54)
                    .HasColumnName("аса_апп_асм_rezerv");

                entity.Property(e => e.АсмпПсаBr)
                    .HasPrecision(54)
                    .HasColumnName("асмп_пса_br");

                entity.Property(e => e.АсмпПсаRemont)
                    .HasPrecision(54)
                    .HasColumnName("асмп_пса_remont");

                entity.Property(e => e.АсмпПсаRezerv)
                    .HasPrecision(54)
                    .HasColumnName("асмп_пса_rezerv");

                entity.Property(e => e.АсмрхBr)
                    .HasPrecision(54)
                    .HasColumnName("АСМРХ_br");

                entity.Property(e => e.АсмрхRezerv)
                    .HasPrecision(54)
                    .HasColumnName("АСМРХ_rezerv");

                entity.Property(e => e.АсоBr)
                    .HasPrecision(54)
                    .HasColumnName("асо_br");

                entity.Property(e => e.АсоRemont)
                    .HasPrecision(54)
                    .HasColumnName("асо_remont");

                entity.Property(e => e.АсоRezerv)
                    .HasPrecision(54)
                    .HasColumnName("асо_rezerv");

                entity.Property(e => e.АшBr)
                    .HasPrecision(54)
                    .HasColumnName("аш_br");

                entity.Property(e => e.АшRemont)
                    .HasPrecision(54)
                    .HasColumnName("аш_remont");

                entity.Property(e => e.АшRezerv)
                    .HasPrecision(54)
                    .HasColumnName("аш_rezerv");

                entity.Property(e => e.Бензин).HasPrecision(55);

                entity.Property(e => e.Болотоходы)
                    .HasPrecision(56)
                    .HasColumnName("болотоходы");

                entity.Property(e => e.Водитель).HasPrecision(54);

                entity.Property(e => e.Всего)
                    .HasPrecision(54)
                    .HasColumnName("всего");

                entity.Property(e => e.ВсегоОтс)
                    .HasPrecision(54)
                    .HasColumnName("всего_отс");

                entity.Property(e => e.ГасиРасчёт)
                    .HasPrecision(54)
                    .HasColumnName("ГАСИ_расчёт");

                entity.Property(e => e.ГасиРезерв)
                    .HasPrecision(54)
                    .HasColumnName("ГАСИ_резерв");

                entity.Property(e => e.Гдзс)
                    .HasPrecision(54)
                    .HasColumnName("ГДЗС");

                entity.Property(e => e.Диспетчер).HasPrecision(54);

                entity.Property(e => e.Дт)
                    .HasPrecision(55)
                    .HasColumnName("ДТ");

                entity.Property(e => e.Ко)
                    .HasPrecision(54)
                    .HasColumnName("КО");

                entity.Property(e => e.Командировка)
                    .HasPrecision(54)
                    .HasColumnName("командировка");

                entity.Property(e => e.КостюмыДругие)
                    .HasPrecision(54)
                    .HasColumnName("костюмы_другие");

                entity.Property(e => e.КостюмыЛ1Таск)
                    .HasPrecision(54)
                    .HasColumnName("костюмы_Л-1_ТАСК");

                entity.Property(e => e.КостюмыТок)
                    .HasPrecision(54)
                    .HasColumnName("костюмы_ТОК");

                entity.Property(e => e.КпBr)
                    .HasPrecision(54)
                    .HasColumnName("кп_br");

                entity.Property(e => e.КпRemont)
                    .HasPrecision(54)
                    .HasColumnName("кп_remont");

                entity.Property(e => e.КпRezerv)
                    .HasPrecision(54)
                    .HasColumnName("кп_rezerv");

                entity.Property(e => e.Мотопомпы)
                    .HasPrecision(56)
                    .HasColumnName("мотопомпы");

                entity.Property(e => e.Налицо).HasPrecision(54);

                entity.Property(e => e.Начкар)
                    .HasMaxLength(255)
                    .HasColumnName("начкар");

                entity.Property(e => e.Некомплект)
                    .HasPrecision(54)
                    .HasColumnName("некомплект");

                entity.Property(e => e.Нк)
                    .HasPrecision(54)
                    .HasColumnName("НК");

                entity.Property(e => e.Отпуск)
                    .HasPrecision(54)
                    .HasColumnName("отпуск");

                entity.Property(e => e.ПенаРасчёт)
                    .HasPrecision(54)
                    .HasColumnName("пена_расчёт");

                entity.Property(e => e.ПенаРезерв)
                    .HasPrecision(54)
                    .HasColumnName("пена_резерв");

                entity.Property(e => e.ПлавСредства)
                    .HasPrecision(56)
                    .HasColumnName("плав_средства");

                entity.Property(e => e.Пнк)
                    .HasPrecision(54)
                    .HasColumnName("ПНК");

                entity.Property(e => e.ПнсBr)
                    .HasPrecision(54)
                    .HasColumnName("пнс_br");

                entity.Property(e => e.ПнсRemont)
                    .HasPrecision(54)
                    .HasColumnName("пнс_remont");

                entity.Property(e => e.ПнсRezerv)
                    .HasPrecision(54)
                    .HasColumnName("пнс_rezerv");

                entity.Property(e => e.ПоБольничному)
                    .HasPrecision(54)
                    .HasColumnName("по_больничному");

                entity.Property(e => e.ПоСписку)
                    .HasPrecision(54)
                    .HasColumnName("по_списку");

                entity.Property(e => e.ПожПоездКорабльBr)
                    .HasPrecision(54)
                    .HasColumnName("пож_поезд_корабль_br");

                entity.Property(e => e.ПожПоездКорабльRemont)
                    .HasPrecision(54)
                    .HasColumnName("пож_поезд_корабль_remont");

                entity.Property(e => e.ПожПоездКорабльRezerv)
                    .HasPrecision(54)
                    .HasColumnName("пож_поезд_корабль_rezerv");

                entity.Property(e => e.Пожарный).HasPrecision(54);

                entity.Property(e => e.ПожарныйКорабльРемонт)
                    .HasPrecision(55)
                    .HasColumnName("пожарный_корабль_ремонт");

                entity.Property(e => e.ПорошокРасчёт)
                    .HasColumnType("int(1)")
                    .HasColumnName("порошок_расчёт");

                entity.Property(e => e.ПорошокРезерв)
                    .HasColumnType("int(1)")
                    .HasColumnName("порошок_резерв");

                entity.Property(e => e.Прочее)
                    .HasPrecision(55)
                    .HasColumnName("прочее");

                entity.Property(e => e.ПрочиеОтс)
                    .HasPrecision(54)
                    .HasColumnName("прочие_отс");

                entity.Property(e => e.Псг)
                    .HasMaxLength(127)
                    .HasColumnName("ПСГ");

                entity.Property(e => e.Пч)
                    .HasMaxLength(127)
                    .HasColumnName("ПЧ");

                entity.Property(e => e.Резерв)
                    .HasPrecision(54)
                    .HasColumnName("резерв");

                entity.Property(e => e.РемонтОсновной)
                    .HasPrecision(54)
                    .HasColumnName("ремонт_основной");

                entity.Property(e => e.РемонтСпециальной)
                    .HasPrecision(54)
                    .HasColumnName("ремонт_специальной");

                entity.Property(e => e.УксАбгBr)
                    .HasPrecision(54)
                    .HasColumnName("укс_абг_br");

                entity.Property(e => e.УксАбгRemont)
                    .HasPrecision(54)
                    .HasColumnName("укс_абг_remont");

                entity.Property(e => e.УксАбгRezerv)
                    .HasPrecision(54)
                    .HasColumnName("укс_абг_rezerv");
            });

            modelBuilder.Entity<FireGarn>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("fire_garns");

                entity.Property(e => e.Dataerr)
                    .HasColumnType("int(11)")
                    .HasColumnName("dataerr");

                entity.Property(e => e.Datafilled).HasColumnName("datafilled");

                entity.Property(e => e.ForControl)
                    .HasMaxLength(255)
                    .HasColumnName("forControl");

                entity.Property(e => e.ForRep2)
                    .HasMaxLength(255)
                    .HasColumnName("forRep2");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(511)
                    .HasColumnName("fullname")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.GarnTypeid)
                    .HasColumnType("int(11)")
                    .HasColumnName("garn_typeid")
                    .HasComment("подразделение, местный, территориальный");

                entity.Property(e => e.Garnizon)
                    .HasMaxLength(127)
                    .HasColumnName("garnizon");

                entity.Property(e => e.Garntype)
                    .HasMaxLength(31)
                    .HasColumnName("garntype");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Karaul)
                    .HasColumnType("int(11)")
                    .HasColumnName("karaul")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Mdate)
                    .HasColumnType("timestamp")
                    .HasColumnName("mdate")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.Name)
                    .HasMaxLength(127)
                    .HasColumnName("name");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder")
                    .HasDefaultValueSql("'100'");

                entity.Property(e => e.Old)
                    .IsRequired()
                    .HasColumnName("old")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Parent)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent");

                entity.Property(e => e.Rank)
                    .HasMaxLength(31)
                    .HasColumnName("rank");

                entity.Property(e => e.Visibility)
                    .IsRequired()
                    .HasColumnName("visibility")
                    .HasDefaultValueSql("'1'");
            });

            modelBuilder.Entity<FireLiquidationPlan>(entity =>
            {
                entity.ToTable("fire_liquidation_plan");

                entity.HasIndex(e => e.IdOrganization, "FK_fire_liquidation_plan_id_o2");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.IdOrganization)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_organization");

                entity.Property(e => e.IdИсполнителя)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_исполнителя");

                entity.Property(e => e.IdПсо)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_ПСО");

                entity.Property(e => e.АдресОбъекта)
                    .HasMaxLength(255)
                    .HasColumnName("Адрес_объекта");

                entity.Property(e => e.ВремяРедактирования)
                    .HasColumnType("timestamp")
                    .HasColumnName("Время_редактирования")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ДатаУтверждения)
                    .HasColumnType("datetime")
                    .HasColumnName("Дата_утверждения");

                entity.Property(e => e.Документ).HasMaxLength(255);

                entity.Property(e => e.Исполнен).HasDefaultValueSql("'0'");

                entity.Property(e => e.Исполнитель).HasMaxLength(255);

                entity.Property(e => e.КороткоеНазваниеОрганизации)
                    .HasMaxLength(255)
                    .HasColumnName("Короткое_название_организации");

                entity.Property(e => e.НаличиеЭлектронногоВарианта)
                    .HasMaxLength(255)
                    .HasColumnName("Наличие_электронного_варианта");

                entity.Property(e => e.НомерДокумента)
                    .HasColumnType("int(11)")
                    .HasColumnName("Номер_документа");

                entity.Property(e => e.Организация).HasMaxLength(255);

                entity.Property(e => e.ОтметкаОбИсполнении)
                    .HasMaxLength(255)
                    .HasColumnName("Отметка_об_исполнении");

                entity.Property(e => e.Псо)
                    .HasMaxLength(255)
                    .HasColumnName("ПСО");

                entity.Property(e => e.СоставлениеИлиКорректировка)
                    .HasMaxLength(255)
                    .HasColumnName("Составление_или_корректировка");

                entity.Property(e => e.СрокОтработки)
                    .HasColumnType("datetime")
                    .HasColumnName("Срок_отработки");

                entity.Property(e => e.ТипДокумента)
                    .HasMaxLength(255)
                    .HasColumnName("Тип_документа");

                entity.HasOne(d => d.IdOrganizationNavigation)
                    .WithMany(p => p.FireLiquidationPlans)
                    .HasForeignKey(d => d.IdOrganization)
                    .HasConstraintName("FK_fire_liquidation_plan_id_o2");
            });

            modelBuilder.Entity<FirePsgItog>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("fire_psg_itog");

                entity.Property(e => e.AcBr)
                    .HasPrecision(65)
                    .HasColumnName("ac_br");

                entity.Property(e => e.AcRemont)
                    .HasPrecision(65)
                    .HasColumnName("ac_remont");

                entity.Property(e => e.AcRezerv)
                    .HasPrecision(65)
                    .HasColumnName("ac_rezerv");

                entity.Property(e => e.AclBr)
                    .HasPrecision(65)
                    .HasColumnName("acl_br");

                entity.Property(e => e.AclRemont)
                    .HasPrecision(65)
                    .HasColumnName("acl_remont");

                entity.Property(e => e.AclRezerv)
                    .HasPrecision(65)
                    .HasColumnName("acl_rezerv");

                entity.Property(e => e.AlBr)
                    .HasPrecision(65)
                    .HasColumnName("al_br");

                entity.Property(e => e.AlRemont)
                    .HasPrecision(65)
                    .HasColumnName("al_remont");

                entity.Property(e => e.AlRezerv)
                    .HasPrecision(65)
                    .HasColumnName("al_rezerv");

                entity.Property(e => e.Category)
                    .HasMaxLength(0)
                    .HasColumnName("category")
                    .HasDefaultValueSql("''")
                    .IsFixedLength();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Isitog)
                    .HasColumnType("int(1)")
                    .HasColumnName("isitog");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(2)")
                    .HasColumnName("norder");

                entity.Property(e => e.Parent)
                    .HasColumnType("int(2)")
                    .HasColumnName("parent");

                entity.Property(e => e.SizodBr)
                    .HasPrecision(65)
                    .HasColumnName("sizod_br");

                entity.Property(e => e.SizodRezerv)
                    .HasPrecision(65)
                    .HasColumnName("sizod_rezerv");

                entity.Property(e => e.Tofirst)
                    .HasPrecision(65)
                    .HasColumnName("tofirst");

                entity.Property(e => e.Totow)
                    .HasPrecision(65)
                    .HasColumnName("totow");

                entity.Property(e => e.АвBr)
                    .HasPrecision(65)
                    .HasColumnName("ав_br");

                entity.Property(e => e.АвRemont)
                    .HasPrecision(65)
                    .HasColumnName("ав_remont");

                entity.Property(e => e.АвRezerv)
                    .HasPrecision(65)
                    .HasColumnName("ав_rezerv");

                entity.Property(e => e.АвсBr)
                    .HasPrecision(65)
                    .HasColumnName("АВС_br");

                entity.Property(e => e.АвсRezerv)
                    .HasPrecision(65)
                    .HasColumnName("АВС_rezerv");

                entity.Property(e => e.АнрBr)
                    .HasPrecision(65)
                    .HasColumnName("анр_br");

                entity.Property(e => e.АнрRemont)
                    .HasPrecision(65)
                    .HasColumnName("анр_remont");

                entity.Property(e => e.АнрRezerv)
                    .HasPrecision(65)
                    .HasColumnName("анр_rezerv");

                entity.Property(e => e.АрBr)
                    .HasPrecision(65)
                    .HasColumnName("ар_br");

                entity.Property(e => e.АрRemont)
                    .HasPrecision(65)
                    .HasColumnName("ар_remont");

                entity.Property(e => e.АрRezerv)
                    .HasPrecision(65)
                    .HasColumnName("ар_rezerv");

                entity.Property(e => e.АсаBr)
                    .HasPrecision(65)
                    .HasColumnName("аса_br");

                entity.Property(e => e.АсаRemont)
                    .HasPrecision(65)
                    .HasColumnName("аса_remont");

                entity.Property(e => e.АсаRezerv)
                    .HasPrecision(65)
                    .HasColumnName("аса_rezerv");

                entity.Property(e => e.АсаАппАсмBr)
                    .HasPrecision(65)
                    .HasColumnName("аса_апп_асм_br");

                entity.Property(e => e.АсаАппАсмRemont)
                    .HasPrecision(65)
                    .HasColumnName("аса_апп_асм_remont");

                entity.Property(e => e.АсаАппАсмRezerv)
                    .HasPrecision(65)
                    .HasColumnName("аса_апп_асм_rezerv");

                entity.Property(e => e.АсмпПсаBr)
                    .HasPrecision(65)
                    .HasColumnName("асмп_пса_br");

                entity.Property(e => e.АсмпПсаRemont)
                    .HasPrecision(65)
                    .HasColumnName("асмп_пса_remont");

                entity.Property(e => e.АсмпПсаRezerv)
                    .HasPrecision(65)
                    .HasColumnName("асмп_пса_rezerv");

                entity.Property(e => e.АсмрхBr)
                    .HasPrecision(65)
                    .HasColumnName("АСМРХ_br");

                entity.Property(e => e.АсмрхRezerv)
                    .HasPrecision(65)
                    .HasColumnName("АСМРХ_rezerv");

                entity.Property(e => e.АсоBr)
                    .HasPrecision(65)
                    .HasColumnName("асо_br");

                entity.Property(e => e.АсоRemont)
                    .HasPrecision(65)
                    .HasColumnName("асо_remont");

                entity.Property(e => e.АсоRezerv)
                    .HasPrecision(65)
                    .HasColumnName("асо_rezerv");

                entity.Property(e => e.АшBr)
                    .HasPrecision(65)
                    .HasColumnName("аш_br");

                entity.Property(e => e.АшRemont)
                    .HasPrecision(65)
                    .HasColumnName("аш_remont");

                entity.Property(e => e.АшRezerv)
                    .HasPrecision(65)
                    .HasColumnName("аш_rezerv");

                entity.Property(e => e.Бензин).HasPrecision(65);

                entity.Property(e => e.Болотоходы)
                    .HasPrecision(65)
                    .HasColumnName("болотоходы");

                entity.Property(e => e.Водитель).HasPrecision(65);

                entity.Property(e => e.Всего)
                    .HasPrecision(65)
                    .HasColumnName("всего");

                entity.Property(e => e.ВсегоОтс)
                    .HasPrecision(65)
                    .HasColumnName("всего_отс");

                entity.Property(e => e.ГасиРасчёт)
                    .HasPrecision(65)
                    .HasColumnName("ГАСИ_расчёт");

                entity.Property(e => e.ГасиРезерв)
                    .HasPrecision(65)
                    .HasColumnName("ГАСИ_резерв");

                entity.Property(e => e.Гдзс)
                    .HasPrecision(65)
                    .HasColumnName("ГДЗС");

                entity.Property(e => e.Диспетчер).HasPrecision(65);

                entity.Property(e => e.Дт)
                    .HasPrecision(65)
                    .HasColumnName("ДТ");

                entity.Property(e => e.Ко)
                    .HasPrecision(65)
                    .HasColumnName("КО");

                entity.Property(e => e.Командировка)
                    .HasPrecision(65)
                    .HasColumnName("командировка");

                entity.Property(e => e.КостюмыДругие)
                    .HasPrecision(65)
                    .HasColumnName("костюмы_другие");

                entity.Property(e => e.КостюмыЛ1Таск)
                    .HasPrecision(65)
                    .HasColumnName("костюмы_Л-1_ТАСК");

                entity.Property(e => e.КостюмыТок)
                    .HasPrecision(65)
                    .HasColumnName("костюмы_ТОК");

                entity.Property(e => e.КпBr)
                    .HasPrecision(65)
                    .HasColumnName("кп_br");

                entity.Property(e => e.КпRemont)
                    .HasPrecision(65)
                    .HasColumnName("кп_remont");

                entity.Property(e => e.КпRezerv)
                    .HasPrecision(65)
                    .HasColumnName("кп_rezerv");

                entity.Property(e => e.Мотопомпы)
                    .HasPrecision(65)
                    .HasColumnName("мотопомпы");

                entity.Property(e => e.Налицо).HasPrecision(65);

                entity.Property(e => e.Начкар).HasColumnName("начкар");

                entity.Property(e => e.Некомплект)
                    .HasPrecision(65)
                    .HasColumnName("некомплект");

                entity.Property(e => e.Нк)
                    .HasPrecision(65)
                    .HasColumnName("НК");

                entity.Property(e => e.Отпуск)
                    .HasPrecision(65)
                    .HasColumnName("отпуск");

                entity.Property(e => e.ПенаРасчёт)
                    .HasPrecision(65)
                    .HasColumnName("пена_расчёт");

                entity.Property(e => e.ПенаРезерв)
                    .HasPrecision(65)
                    .HasColumnName("пена_резерв");

                entity.Property(e => e.ПлавСредства)
                    .HasPrecision(65)
                    .HasColumnName("плав_средства");

                entity.Property(e => e.Пнк)
                    .HasPrecision(65)
                    .HasColumnName("ПНК");

                entity.Property(e => e.ПнсBr)
                    .HasPrecision(65)
                    .HasColumnName("пнс_br");

                entity.Property(e => e.ПнсRemont)
                    .HasPrecision(65)
                    .HasColumnName("пнс_remont");

                entity.Property(e => e.ПнсRezerv)
                    .HasPrecision(65)
                    .HasColumnName("пнс_rezerv");

                entity.Property(e => e.ПоБольничному)
                    .HasPrecision(65)
                    .HasColumnName("по_больничному");

                entity.Property(e => e.ПоСписку)
                    .HasPrecision(65)
                    .HasColumnName("по_списку");

                entity.Property(e => e.ПожПоездКорабльBr)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_корабль_br");

                entity.Property(e => e.ПожПоездКорабльRemont)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_корабль_remont");

                entity.Property(e => e.ПожПоездКорабльRezerv)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_корабль_rezerv");

                entity.Property(e => e.Пожарный).HasPrecision(65);

                entity.Property(e => e.ПожарныйКорабльРемонт)
                    .HasPrecision(65)
                    .HasColumnName("пожарный_корабль_ремонт");

                entity.Property(e => e.ПорошокРасчёт)
                    .HasPrecision(54)
                    .HasColumnName("порошок_расчёт");

                entity.Property(e => e.ПорошокРезерв)
                    .HasPrecision(54)
                    .HasColumnName("порошок_резерв");

                entity.Property(e => e.Прочее)
                    .HasPrecision(65)
                    .HasColumnName("прочее");

                entity.Property(e => e.ПрочиеОтс)
                    .HasPrecision(65)
                    .HasColumnName("прочие_отс");

                entity.Property(e => e.Псг)
                    .HasMaxLength(127)
                    .HasColumnName("ПСГ");

                entity.Property(e => e.Пч)
                    .HasMaxLength(127)
                    .HasColumnName("ПЧ");

                entity.Property(e => e.Резерв)
                    .HasPrecision(65)
                    .HasColumnName("резерв");

                entity.Property(e => e.РемонтОсновной)
                    .HasPrecision(65)
                    .HasColumnName("ремонт_основной");

                entity.Property(e => e.РемонтСпециальной)
                    .HasPrecision(65)
                    .HasColumnName("ремонт_специальной");

                entity.Property(e => e.УксАбгBr)
                    .HasPrecision(65)
                    .HasColumnName("укс_абг_br");

                entity.Property(e => e.УксАбгRemont)
                    .HasPrecision(65)
                    .HasColumnName("укс_абг_remont");

                entity.Property(e => e.УксАбгRezerv)
                    .HasPrecision(65)
                    .HasColumnName("укс_абг_rezerv");
            });

            modelBuilder.Entity<FirePsgStat>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("fire_psg_stat");

                entity.Property(e => e.AcBr)
                    .HasPrecision(65)
                    .HasColumnName("ac_br");

                entity.Property(e => e.AcRemont)
                    .HasPrecision(65)
                    .HasColumnName("ac_remont");

                entity.Property(e => e.AcRezerv)
                    .HasPrecision(65)
                    .HasColumnName("ac_rezerv");

                entity.Property(e => e.AclBr)
                    .HasPrecision(65)
                    .HasColumnName("acl_br");

                entity.Property(e => e.AclRemont)
                    .HasPrecision(65)
                    .HasColumnName("acl_remont");

                entity.Property(e => e.AclRezerv)
                    .HasPrecision(65)
                    .HasColumnName("acl_rezerv");

                entity.Property(e => e.AlBr)
                    .HasPrecision(65)
                    .HasColumnName("al_br");

                entity.Property(e => e.AlRemont)
                    .HasPrecision(65)
                    .HasColumnName("al_remont");

                entity.Property(e => e.AlRezerv)
                    .HasPrecision(65)
                    .HasColumnName("al_rezerv");

                entity.Property(e => e.Category)
                    .HasMaxLength(21)
                    .HasColumnName("category");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.Isitog)
                    .HasColumnType("int(11)")
                    .HasColumnName("isitog");

                entity.Property(e => e.Norder)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("norder");

                entity.Property(e => e.Parent)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("parent");

                entity.Property(e => e.SizodBr)
                    .HasPrecision(65)
                    .HasColumnName("sizod_br");

                entity.Property(e => e.SizodRezerv)
                    .HasPrecision(65)
                    .HasColumnName("sizod_rezerv");

                entity.Property(e => e.Tofirst)
                    .HasPrecision(65)
                    .HasColumnName("tofirst");

                entity.Property(e => e.Totow)
                    .HasPrecision(65)
                    .HasColumnName("totow");

                entity.Property(e => e.АвBr)
                    .HasPrecision(65)
                    .HasColumnName("ав_br");

                entity.Property(e => e.АвRemont)
                    .HasPrecision(65)
                    .HasColumnName("ав_remont");

                entity.Property(e => e.АвRezerv)
                    .HasPrecision(65)
                    .HasColumnName("ав_rezerv");

                entity.Property(e => e.АвсBr)
                    .HasPrecision(65)
                    .HasColumnName("АВС_br");

                entity.Property(e => e.АвсRezerv)
                    .HasPrecision(65)
                    .HasColumnName("АВС_rezerv");

                entity.Property(e => e.АнрBr)
                    .HasPrecision(65)
                    .HasColumnName("анр_br");

                entity.Property(e => e.АнрRemont)
                    .HasPrecision(65)
                    .HasColumnName("анр_remont");

                entity.Property(e => e.АнрRezerv)
                    .HasPrecision(65)
                    .HasColumnName("анр_rezerv");

                entity.Property(e => e.АрBr)
                    .HasPrecision(65)
                    .HasColumnName("ар_br");

                entity.Property(e => e.АрRemont)
                    .HasPrecision(65)
                    .HasColumnName("ар_remont");

                entity.Property(e => e.АрRezerv)
                    .HasPrecision(65)
                    .HasColumnName("ар_rezerv");

                entity.Property(e => e.АсаBr)
                    .HasPrecision(65)
                    .HasColumnName("аса_br");

                entity.Property(e => e.АсаRemont)
                    .HasPrecision(65)
                    .HasColumnName("аса_remont");

                entity.Property(e => e.АсаRezerv)
                    .HasPrecision(65)
                    .HasColumnName("аса_rezerv");

                entity.Property(e => e.АсаАппАсмBr)
                    .HasPrecision(65)
                    .HasColumnName("аса_апп_асм_br");

                entity.Property(e => e.АсаАппАсмRemont)
                    .HasPrecision(65)
                    .HasColumnName("аса_апп_асм_remont");

                entity.Property(e => e.АсаАппАсмRezerv)
                    .HasPrecision(65)
                    .HasColumnName("аса_апп_асм_rezerv");

                entity.Property(e => e.АсмпПсаBr)
                    .HasPrecision(65)
                    .HasColumnName("асмп_пса_br");

                entity.Property(e => e.АсмпПсаRemont)
                    .HasPrecision(65)
                    .HasColumnName("асмп_пса_remont");

                entity.Property(e => e.АсмпПсаRezerv)
                    .HasPrecision(65)
                    .HasColumnName("асмп_пса_rezerv");

                entity.Property(e => e.АсмрхBr)
                    .HasPrecision(65)
                    .HasColumnName("АСМРХ_br");

                entity.Property(e => e.АсмрхRezerv)
                    .HasPrecision(65)
                    .HasColumnName("АСМРХ_rezerv");

                entity.Property(e => e.АсоBr)
                    .HasPrecision(65)
                    .HasColumnName("асо_br");

                entity.Property(e => e.АсоRemont)
                    .HasPrecision(65)
                    .HasColumnName("асо_remont");

                entity.Property(e => e.АсоRezerv)
                    .HasPrecision(65)
                    .HasColumnName("асо_rezerv");

                entity.Property(e => e.АшBr)
                    .HasPrecision(65)
                    .HasColumnName("аш_br");

                entity.Property(e => e.АшRemont)
                    .HasPrecision(65)
                    .HasColumnName("аш_remont");

                entity.Property(e => e.АшRezerv)
                    .HasPrecision(65)
                    .HasColumnName("аш_rezerv");

                entity.Property(e => e.Бензин).HasPrecision(65);

                entity.Property(e => e.Болотоходы)
                    .HasPrecision(65)
                    .HasColumnName("болотоходы");

                entity.Property(e => e.Водитель).HasPrecision(65);

                entity.Property(e => e.Всего)
                    .HasPrecision(65)
                    .HasColumnName("всего");

                entity.Property(e => e.ВсегоОтс)
                    .HasPrecision(65)
                    .HasColumnName("всего_отс");

                entity.Property(e => e.ГасиРасчёт)
                    .HasPrecision(65)
                    .HasColumnName("ГАСИ_расчёт");

                entity.Property(e => e.ГасиРезерв)
                    .HasPrecision(65)
                    .HasColumnName("ГАСИ_резерв");

                entity.Property(e => e.Гдзс)
                    .HasPrecision(65)
                    .HasColumnName("ГДЗС");

                entity.Property(e => e.Диспетчер).HasPrecision(65);

                entity.Property(e => e.Дт)
                    .HasPrecision(65)
                    .HasColumnName("ДТ");

                entity.Property(e => e.Ко)
                    .HasPrecision(65)
                    .HasColumnName("КО");

                entity.Property(e => e.Командировка)
                    .HasPrecision(65)
                    .HasColumnName("командировка");

                entity.Property(e => e.КостюмыДругие)
                    .HasPrecision(65)
                    .HasColumnName("костюмы_другие");

                entity.Property(e => e.КостюмыЛ1Таск)
                    .HasPrecision(65)
                    .HasColumnName("костюмы_Л-1_ТАСК");

                entity.Property(e => e.КостюмыТок)
                    .HasPrecision(65)
                    .HasColumnName("костюмы_ТОК");

                entity.Property(e => e.КпBr)
                    .HasPrecision(65)
                    .HasColumnName("кп_br");

                entity.Property(e => e.КпRemont)
                    .HasPrecision(65)
                    .HasColumnName("кп_remont");

                entity.Property(e => e.КпRezerv)
                    .HasPrecision(65)
                    .HasColumnName("кп_rezerv");

                entity.Property(e => e.Мотопомпы)
                    .HasPrecision(65)
                    .HasColumnName("мотопомпы");

                entity.Property(e => e.Налицо).HasPrecision(65);

                entity.Property(e => e.Начкар)
                    .HasMaxLength(255)
                    .HasColumnName("начкар");

                entity.Property(e => e.Некомплект)
                    .HasPrecision(65)
                    .HasColumnName("некомплект");

                entity.Property(e => e.Нк)
                    .HasPrecision(65)
                    .HasColumnName("НК");

                entity.Property(e => e.Отпуск)
                    .HasPrecision(65)
                    .HasColumnName("отпуск");

                entity.Property(e => e.ПенаРасчёт)
                    .HasPrecision(65)
                    .HasColumnName("пена_расчёт");

                entity.Property(e => e.ПенаРезерв)
                    .HasPrecision(65)
                    .HasColumnName("пена_резерв");

                entity.Property(e => e.ПлавСредства)
                    .HasPrecision(65)
                    .HasColumnName("плав_средства");

                entity.Property(e => e.Пнк)
                    .HasPrecision(65)
                    .HasColumnName("ПНК");

                entity.Property(e => e.ПнсBr)
                    .HasPrecision(65)
                    .HasColumnName("пнс_br");

                entity.Property(e => e.ПнсRemont)
                    .HasPrecision(65)
                    .HasColumnName("пнс_remont");

                entity.Property(e => e.ПнсRezerv)
                    .HasPrecision(65)
                    .HasColumnName("пнс_rezerv");

                entity.Property(e => e.ПоБольничному)
                    .HasPrecision(65)
                    .HasColumnName("по_больничному");

                entity.Property(e => e.ПоСписку)
                    .HasPrecision(65)
                    .HasColumnName("по_списку");

                entity.Property(e => e.ПожПоездКорабльBr)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_корабль_br");

                entity.Property(e => e.ПожПоездКорабльRemont)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_корабль_remont");

                entity.Property(e => e.ПожПоездКорабльRezerv)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_корабль_rezerv");

                entity.Property(e => e.Пожарный).HasPrecision(65);

                entity.Property(e => e.ПожарныйКорабльРемонт)
                    .HasPrecision(65)
                    .HasColumnName("пожарный_корабль_ремонт");

                entity.Property(e => e.ПорошокРасчёт)
                    .HasPrecision(65)
                    .HasColumnName("порошок_расчёт");

                entity.Property(e => e.ПорошокРезерв)
                    .HasPrecision(65)
                    .HasColumnName("порошок_резерв");

                entity.Property(e => e.Прочее)
                    .HasPrecision(65)
                    .HasColumnName("прочее");

                entity.Property(e => e.ПрочиеОтс)
                    .HasPrecision(65)
                    .HasColumnName("прочие_отс");

                entity.Property(e => e.Псг)
                    .HasMaxLength(127)
                    .HasColumnName("ПСГ");

                entity.Property(e => e.Пч)
                    .HasMaxLength(143)
                    .HasColumnName("ПЧ");

                entity.Property(e => e.Резерв)
                    .HasPrecision(65)
                    .HasColumnName("резерв");

                entity.Property(e => e.РемонтОсновной)
                    .HasPrecision(65)
                    .HasColumnName("ремонт_основной");

                entity.Property(e => e.РемонтСпециальной)
                    .HasPrecision(65)
                    .HasColumnName("ремонт_специальной");

                entity.Property(e => e.УксАбгBr)
                    .HasPrecision(65)
                    .HasColumnName("укс_абг_br");

                entity.Property(e => e.УксАбгRemont)
                    .HasPrecision(65)
                    .HasColumnName("укс_абг_remont");

                entity.Property(e => e.УксАбгRezerv)
                    .HasPrecision(65)
                    .HasColumnName("укс_абг_rezerv");
            });

            modelBuilder.Entity<FirePsgStatByCategory>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("fire_psg_stat_by_category");

                entity.Property(e => e.AcBr)
                    .HasPrecision(65)
                    .HasColumnName("ac_br");

                entity.Property(e => e.AcRemont)
                    .HasPrecision(65)
                    .HasColumnName("ac_remont");

                entity.Property(e => e.AcRezerv)
                    .HasPrecision(65)
                    .HasColumnName("ac_rezerv");

                entity.Property(e => e.AclBr)
                    .HasPrecision(65)
                    .HasColumnName("acl_br");

                entity.Property(e => e.AclRemont)
                    .HasPrecision(65)
                    .HasColumnName("acl_remont");

                entity.Property(e => e.AclRezerv)
                    .HasPrecision(65)
                    .HasColumnName("acl_rezerv");

                entity.Property(e => e.AlBr)
                    .HasPrecision(65)
                    .HasColumnName("al_br");

                entity.Property(e => e.AlRemont)
                    .HasPrecision(65)
                    .HasColumnName("al_remont");

                entity.Property(e => e.AlRezerv)
                    .HasPrecision(65)
                    .HasColumnName("al_rezerv");

                entity.Property(e => e.Category)
                    .HasMaxLength(16)
                    .HasColumnName("category");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(15)")
                    .HasColumnName("id");

                entity.Property(e => e.Isitog)
                    .HasColumnType("int(1)")
                    .HasColumnName("isitog");

                entity.Property(e => e.Norder)
                    .HasColumnType("bigint(11)")
                    .HasColumnName("norder");

                entity.Property(e => e.Parent)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent");

                entity.Property(e => e.SizodBr)
                    .HasPrecision(65)
                    .HasColumnName("sizod_br");

                entity.Property(e => e.SizodRezerv)
                    .HasPrecision(65)
                    .HasColumnName("sizod_rezerv");

                entity.Property(e => e.Tofirst)
                    .HasPrecision(65)
                    .HasColumnName("tofirst");

                entity.Property(e => e.Totow)
                    .HasPrecision(65)
                    .HasColumnName("totow");

                entity.Property(e => e.АвBr)
                    .HasPrecision(65)
                    .HasColumnName("ав_br");

                entity.Property(e => e.АвRemont)
                    .HasPrecision(65)
                    .HasColumnName("ав_remont");

                entity.Property(e => e.АвRezerv)
                    .HasPrecision(65)
                    .HasColumnName("ав_rezerv");

                entity.Property(e => e.АвсBr)
                    .HasPrecision(65)
                    .HasColumnName("АВС_br");

                entity.Property(e => e.АвсRezerv)
                    .HasPrecision(65)
                    .HasColumnName("АВС_rezerv");

                entity.Property(e => e.АнрBr)
                    .HasPrecision(65)
                    .HasColumnName("анр_br");

                entity.Property(e => e.АнрRemont)
                    .HasPrecision(65)
                    .HasColumnName("анр_remont");

                entity.Property(e => e.АнрRezerv)
                    .HasPrecision(65)
                    .HasColumnName("анр_rezerv");

                entity.Property(e => e.АрBr)
                    .HasPrecision(65)
                    .HasColumnName("ар_br");

                entity.Property(e => e.АрRemont)
                    .HasPrecision(65)
                    .HasColumnName("ар_remont");

                entity.Property(e => e.АрRezerv)
                    .HasPrecision(65)
                    .HasColumnName("ар_rezerv");

                entity.Property(e => e.АсаBr)
                    .HasPrecision(65)
                    .HasColumnName("аса_br");

                entity.Property(e => e.АсаRemont)
                    .HasPrecision(65)
                    .HasColumnName("аса_remont");

                entity.Property(e => e.АсаRezerv)
                    .HasPrecision(65)
                    .HasColumnName("аса_rezerv");

                entity.Property(e => e.АсаАппАсмBr)
                    .HasPrecision(65)
                    .HasColumnName("аса_апп_асм_br");

                entity.Property(e => e.АсаАппАсмRemont)
                    .HasPrecision(65)
                    .HasColumnName("аса_апп_асм_remont");

                entity.Property(e => e.АсаАппАсмRezerv)
                    .HasPrecision(65)
                    .HasColumnName("аса_апп_асм_rezerv");

                entity.Property(e => e.АсмпПсаBr)
                    .HasPrecision(65)
                    .HasColumnName("асмп_пса_br");

                entity.Property(e => e.АсмпПсаRemont)
                    .HasPrecision(65)
                    .HasColumnName("асмп_пса_remont");

                entity.Property(e => e.АсмпПсаRezerv)
                    .HasPrecision(65)
                    .HasColumnName("асмп_пса_rezerv");

                entity.Property(e => e.АсмрхBr)
                    .HasPrecision(65)
                    .HasColumnName("АСМРХ_br");

                entity.Property(e => e.АсмрхRezerv)
                    .HasPrecision(65)
                    .HasColumnName("АСМРХ_rezerv");

                entity.Property(e => e.АсоBr)
                    .HasPrecision(65)
                    .HasColumnName("асо_br");

                entity.Property(e => e.АсоRemont)
                    .HasPrecision(65)
                    .HasColumnName("асо_remont");

                entity.Property(e => e.АсоRezerv)
                    .HasPrecision(65)
                    .HasColumnName("асо_rezerv");

                entity.Property(e => e.АшBr)
                    .HasPrecision(65)
                    .HasColumnName("аш_br");

                entity.Property(e => e.АшRemont)
                    .HasPrecision(65)
                    .HasColumnName("аш_remont");

                entity.Property(e => e.АшRezerv)
                    .HasPrecision(65)
                    .HasColumnName("аш_rezerv");

                entity.Property(e => e.Бензин).HasPrecision(65);

                entity.Property(e => e.Болотоходы)
                    .HasPrecision(65)
                    .HasColumnName("болотоходы");

                entity.Property(e => e.Водитель).HasPrecision(65);

                entity.Property(e => e.Всего)
                    .HasPrecision(65)
                    .HasColumnName("всего");

                entity.Property(e => e.ВсегоОтс)
                    .HasPrecision(65)
                    .HasColumnName("всего_отс");

                entity.Property(e => e.ГасиРасчёт)
                    .HasPrecision(65)
                    .HasColumnName("ГАСИ_расчёт");

                entity.Property(e => e.ГасиРезерв)
                    .HasPrecision(65)
                    .HasColumnName("ГАСИ_резерв");

                entity.Property(e => e.Гдзс)
                    .HasPrecision(65)
                    .HasColumnName("ГДЗС");

                entity.Property(e => e.Диспетчер).HasPrecision(65);

                entity.Property(e => e.Дт)
                    .HasPrecision(65)
                    .HasColumnName("ДТ");

                entity.Property(e => e.Ко)
                    .HasPrecision(65)
                    .HasColumnName("КО");

                entity.Property(e => e.Командировка)
                    .HasPrecision(65)
                    .HasColumnName("командировка");

                entity.Property(e => e.КостюмыДругие)
                    .HasPrecision(65)
                    .HasColumnName("костюмы_другие");

                entity.Property(e => e.КостюмыЛ1Таск)
                    .HasPrecision(65)
                    .HasColumnName("костюмы_Л-1_ТАСК");

                entity.Property(e => e.КостюмыТок)
                    .HasPrecision(65)
                    .HasColumnName("костюмы_ТОК");

                entity.Property(e => e.КпBr)
                    .HasPrecision(65)
                    .HasColumnName("кп_br");

                entity.Property(e => e.КпRemont)
                    .HasPrecision(65)
                    .HasColumnName("кп_remont");

                entity.Property(e => e.КпRezerv)
                    .HasPrecision(65)
                    .HasColumnName("кп_rezerv");

                entity.Property(e => e.Мотопомпы)
                    .HasPrecision(65)
                    .HasColumnName("мотопомпы");

                entity.Property(e => e.Налицо).HasPrecision(65);

                entity.Property(e => e.Начкар).HasColumnName("начкар");

                entity.Property(e => e.Некомплект)
                    .HasPrecision(65)
                    .HasColumnName("некомплект");

                entity.Property(e => e.Нк)
                    .HasPrecision(65)
                    .HasColumnName("НК");

                entity.Property(e => e.Отпуск)
                    .HasPrecision(65)
                    .HasColumnName("отпуск");

                entity.Property(e => e.ПенаРасчёт)
                    .HasPrecision(65)
                    .HasColumnName("пена_расчёт");

                entity.Property(e => e.ПенаРезерв)
                    .HasPrecision(65)
                    .HasColumnName("пена_резерв");

                entity.Property(e => e.ПлавСредства)
                    .HasPrecision(65)
                    .HasColumnName("плав_средства");

                entity.Property(e => e.Пнк)
                    .HasPrecision(65)
                    .HasColumnName("ПНК");

                entity.Property(e => e.ПнсBr)
                    .HasPrecision(65)
                    .HasColumnName("пнс_br");

                entity.Property(e => e.ПнсRemont)
                    .HasPrecision(65)
                    .HasColumnName("пнс_remont");

                entity.Property(e => e.ПнсRezerv)
                    .HasPrecision(65)
                    .HasColumnName("пнс_rezerv");

                entity.Property(e => e.ПоБольничному)
                    .HasPrecision(65)
                    .HasColumnName("по_больничному");

                entity.Property(e => e.ПоСписку)
                    .HasPrecision(65)
                    .HasColumnName("по_списку");

                entity.Property(e => e.ПожПоездКорабльBr)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_корабль_br");

                entity.Property(e => e.ПожПоездКорабльRemont)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_корабль_remont");

                entity.Property(e => e.ПожПоездКорабльRezerv)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_корабль_rezerv");

                entity.Property(e => e.Пожарный).HasPrecision(65);

                entity.Property(e => e.ПожарныйКорабльРемонт)
                    .HasPrecision(65)
                    .HasColumnName("пожарный_корабль_ремонт");

                entity.Property(e => e.ПорошокРасчёт)
                    .HasPrecision(32)
                    .HasColumnName("порошок_расчёт");

                entity.Property(e => e.ПорошокРезерв)
                    .HasPrecision(32)
                    .HasColumnName("порошок_резерв");

                entity.Property(e => e.Прочее)
                    .HasPrecision(65)
                    .HasColumnName("прочее");

                entity.Property(e => e.ПрочиеОтс)
                    .HasPrecision(65)
                    .HasColumnName("прочие_отс");

                entity.Property(e => e.Псг)
                    .HasMaxLength(127)
                    .HasColumnName("ПСГ");

                entity.Property(e => e.Пч)
                    .HasMaxLength(143)
                    .HasColumnName("ПЧ");

                entity.Property(e => e.Резерв)
                    .HasPrecision(65)
                    .HasColumnName("резерв");

                entity.Property(e => e.РемонтОсновной)
                    .HasPrecision(65)
                    .HasColumnName("ремонт_основной");

                entity.Property(e => e.РемонтСпециальной)
                    .HasPrecision(65)
                    .HasColumnName("ремонт_специальной");

                entity.Property(e => e.УксАбгBr)
                    .HasPrecision(65)
                    .HasColumnName("укс_абг_br");

                entity.Property(e => e.УксАбгRemont)
                    .HasPrecision(65)
                    .HasColumnName("укс_абг_remont");

                entity.Property(e => e.УксАбгRezerv)
                    .HasPrecision(65)
                    .HasColumnName("укс_абг_rezerv");
            });

            modelBuilder.Entity<FireTpsgStat>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("fire_tpsg_stat");

                entity.Property(e => e.AcBr)
                    .HasPrecision(65)
                    .HasColumnName("ac_br");

                entity.Property(e => e.AcRemont)
                    .HasPrecision(65)
                    .HasColumnName("ac_remont");

                entity.Property(e => e.AcRezerv)
                    .HasPrecision(65)
                    .HasColumnName("ac_rezerv");

                entity.Property(e => e.AclBr)
                    .HasPrecision(65)
                    .HasColumnName("acl_br");

                entity.Property(e => e.AclRemont)
                    .HasPrecision(65)
                    .HasColumnName("acl_remont");

                entity.Property(e => e.AclRezerv)
                    .HasPrecision(65)
                    .HasColumnName("acl_rezerv");

                entity.Property(e => e.AlBr)
                    .HasPrecision(65)
                    .HasColumnName("al_br");

                entity.Property(e => e.AlRemont)
                    .HasPrecision(65)
                    .HasColumnName("al_remont");

                entity.Property(e => e.AlRezerv)
                    .HasPrecision(65)
                    .HasColumnName("al_rezerv");

                entity.Property(e => e.Category)
                    .HasMaxLength(21)
                    .HasColumnName("category");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.Isitog)
                    .HasColumnType("int(11)")
                    .HasColumnName("isitog");

                entity.Property(e => e.Norder)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("norder");

                entity.Property(e => e.Parent)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("parent");

                entity.Property(e => e.SizodBr)
                    .HasPrecision(65)
                    .HasColumnName("sizod_br");

                entity.Property(e => e.SizodRezerv)
                    .HasPrecision(65)
                    .HasColumnName("sizod_rezerv");

                entity.Property(e => e.Tofirst)
                    .HasPrecision(65)
                    .HasColumnName("tofirst");

                entity.Property(e => e.Totow)
                    .HasPrecision(65)
                    .HasColumnName("totow");

                entity.Property(e => e.АвBr)
                    .HasPrecision(65)
                    .HasColumnName("ав_br");

                entity.Property(e => e.АвRemont)
                    .HasPrecision(65)
                    .HasColumnName("ав_remont");

                entity.Property(e => e.АвRezerv)
                    .HasPrecision(65)
                    .HasColumnName("ав_rezerv");

                entity.Property(e => e.АвсBr)
                    .HasPrecision(65)
                    .HasColumnName("АВС_br");

                entity.Property(e => e.АвсRezerv)
                    .HasPrecision(65)
                    .HasColumnName("АВС_rezerv");

                entity.Property(e => e.АнрBr)
                    .HasPrecision(65)
                    .HasColumnName("анр_br");

                entity.Property(e => e.АнрRemont)
                    .HasPrecision(65)
                    .HasColumnName("анр_remont");

                entity.Property(e => e.АнрRezerv)
                    .HasPrecision(65)
                    .HasColumnName("анр_rezerv");

                entity.Property(e => e.АрBr)
                    .HasPrecision(65)
                    .HasColumnName("ар_br");

                entity.Property(e => e.АрRemont)
                    .HasPrecision(65)
                    .HasColumnName("ар_remont");

                entity.Property(e => e.АрRezerv)
                    .HasPrecision(65)
                    .HasColumnName("ар_rezerv");

                entity.Property(e => e.АсаBr)
                    .HasPrecision(65)
                    .HasColumnName("аса_br");

                entity.Property(e => e.АсаRemont)
                    .HasPrecision(65)
                    .HasColumnName("аса_remont");

                entity.Property(e => e.АсаRezerv)
                    .HasPrecision(65)
                    .HasColumnName("аса_rezerv");

                entity.Property(e => e.АсаАппАсмBr)
                    .HasPrecision(65)
                    .HasColumnName("аса_апп_асм_br");

                entity.Property(e => e.АсаАппАсмRemont)
                    .HasPrecision(65)
                    .HasColumnName("аса_апп_асм_remont");

                entity.Property(e => e.АсаАппАсмRezerv)
                    .HasPrecision(65)
                    .HasColumnName("аса_апп_асм_rezerv");

                entity.Property(e => e.АсмпПсаBr)
                    .HasPrecision(65)
                    .HasColumnName("асмп_пса_br");

                entity.Property(e => e.АсмпПсаRemont)
                    .HasPrecision(65)
                    .HasColumnName("асмп_пса_remont");

                entity.Property(e => e.АсмпПсаRezerv)
                    .HasPrecision(65)
                    .HasColumnName("асмп_пса_rezerv");

                entity.Property(e => e.АсмрхBr)
                    .HasPrecision(65)
                    .HasColumnName("АСМРХ_br");

                entity.Property(e => e.АсмрхRezerv)
                    .HasPrecision(65)
                    .HasColumnName("АСМРХ_rezerv");

                entity.Property(e => e.АсоBr)
                    .HasPrecision(65)
                    .HasColumnName("асо_br");

                entity.Property(e => e.АсоRemont)
                    .HasPrecision(65)
                    .HasColumnName("асо_remont");

                entity.Property(e => e.АсоRezerv)
                    .HasPrecision(65)
                    .HasColumnName("асо_rezerv");

                entity.Property(e => e.АшBr)
                    .HasPrecision(65)
                    .HasColumnName("аш_br");

                entity.Property(e => e.АшRemont)
                    .HasPrecision(65)
                    .HasColumnName("аш_remont");

                entity.Property(e => e.АшRezerv)
                    .HasPrecision(65)
                    .HasColumnName("аш_rezerv");

                entity.Property(e => e.Бензин).HasPrecision(65);

                entity.Property(e => e.Болотоходы)
                    .HasPrecision(65)
                    .HasColumnName("болотоходы");

                entity.Property(e => e.Водитель).HasPrecision(65);

                entity.Property(e => e.Всего)
                    .HasPrecision(65)
                    .HasColumnName("всего");

                entity.Property(e => e.ВсегоОтс)
                    .HasPrecision(65)
                    .HasColumnName("всего_отс");

                entity.Property(e => e.ГасиРасчёт)
                    .HasPrecision(65)
                    .HasColumnName("ГАСИ_расчёт");

                entity.Property(e => e.ГасиРезерв)
                    .HasPrecision(65)
                    .HasColumnName("ГАСИ_резерв");

                entity.Property(e => e.Гдзс)
                    .HasPrecision(65)
                    .HasColumnName("ГДЗС");

                entity.Property(e => e.Диспетчер).HasPrecision(65);

                entity.Property(e => e.Дт)
                    .HasPrecision(65)
                    .HasColumnName("ДТ");

                entity.Property(e => e.Ко)
                    .HasPrecision(65)
                    .HasColumnName("КО");

                entity.Property(e => e.Командировка)
                    .HasPrecision(65)
                    .HasColumnName("командировка");

                entity.Property(e => e.КостюмыДругие)
                    .HasPrecision(65)
                    .HasColumnName("костюмы_другие");

                entity.Property(e => e.КостюмыЛ1Таск)
                    .HasPrecision(65)
                    .HasColumnName("костюмы_Л-1_ТАСК");

                entity.Property(e => e.КостюмыТок)
                    .HasPrecision(65)
                    .HasColumnName("костюмы_ТОК");

                entity.Property(e => e.КпBr)
                    .HasPrecision(65)
                    .HasColumnName("кп_br");

                entity.Property(e => e.КпRemont)
                    .HasPrecision(65)
                    .HasColumnName("кп_remont");

                entity.Property(e => e.КпRezerv)
                    .HasPrecision(65)
                    .HasColumnName("кп_rezerv");

                entity.Property(e => e.Мотопомпы)
                    .HasPrecision(65)
                    .HasColumnName("мотопомпы");

                entity.Property(e => e.Налицо).HasPrecision(65);

                entity.Property(e => e.Начкар).HasColumnName("начкар");

                entity.Property(e => e.Некомплект)
                    .HasPrecision(65)
                    .HasColumnName("некомплект");

                entity.Property(e => e.Нк)
                    .HasPrecision(65)
                    .HasColumnName("НК");

                entity.Property(e => e.Отпуск)
                    .HasPrecision(65)
                    .HasColumnName("отпуск");

                entity.Property(e => e.ПенаРасчёт)
                    .HasPrecision(65)
                    .HasColumnName("пена_расчёт");

                entity.Property(e => e.ПенаРезерв)
                    .HasPrecision(65)
                    .HasColumnName("пена_резерв");

                entity.Property(e => e.ПлавСредства)
                    .HasPrecision(65)
                    .HasColumnName("плав_средства");

                entity.Property(e => e.Пнк)
                    .HasPrecision(65)
                    .HasColumnName("ПНК");

                entity.Property(e => e.ПнсBr)
                    .HasPrecision(65)
                    .HasColumnName("пнс_br");

                entity.Property(e => e.ПнсRemont)
                    .HasPrecision(65)
                    .HasColumnName("пнс_remont");

                entity.Property(e => e.ПнсRezerv)
                    .HasPrecision(65)
                    .HasColumnName("пнс_rezerv");

                entity.Property(e => e.ПоБольничному)
                    .HasPrecision(65)
                    .HasColumnName("по_больничному");

                entity.Property(e => e.ПоСписку)
                    .HasPrecision(65)
                    .HasColumnName("по_списку");

                entity.Property(e => e.ПожПоездКорабльBr)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_корабль_br");

                entity.Property(e => e.ПожПоездКорабльRemont)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_корабль_remont");

                entity.Property(e => e.ПожПоездКорабльRezerv)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_корабль_rezerv");

                entity.Property(e => e.Пожарный).HasPrecision(65);

                entity.Property(e => e.ПожарныйКорабльРемонт)
                    .HasPrecision(65)
                    .HasColumnName("пожарный_корабль_ремонт");

                entity.Property(e => e.ПорошокРасчёт)
                    .HasPrecision(65)
                    .HasColumnName("порошок_расчёт");

                entity.Property(e => e.ПорошокРезерв)
                    .HasPrecision(65)
                    .HasColumnName("порошок_резерв");

                entity.Property(e => e.Прочее)
                    .HasPrecision(65)
                    .HasColumnName("прочее");

                entity.Property(e => e.ПрочиеОтс)
                    .HasPrecision(65)
                    .HasColumnName("прочие_отс");

                entity.Property(e => e.Псг)
                    .HasMaxLength(15)
                    .HasColumnName("ПСГ")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Пч)
                    .HasMaxLength(31)
                    .HasColumnName("ПЧ");

                entity.Property(e => e.Резерв)
                    .HasPrecision(65)
                    .HasColumnName("резерв");

                entity.Property(e => e.РемонтОсновной)
                    .HasPrecision(65)
                    .HasColumnName("ремонт_основной");

                entity.Property(e => e.РемонтСпециальной)
                    .HasPrecision(65)
                    .HasColumnName("ремонт_специальной");

                entity.Property(e => e.УксАбгBr)
                    .HasPrecision(65)
                    .HasColumnName("укс_абг_br");

                entity.Property(e => e.УксАбгRemont)
                    .HasPrecision(65)
                    .HasColumnName("укс_абг_remont");

                entity.Property(e => e.УксАбгRezerv)
                    .HasPrecision(65)
                    .HasColumnName("укс_абг_rezerv");
            });

            modelBuilder.Entity<Fireauto>(entity =>
            {
                entity.ToTable("fireauto");

                entity.HasIndex(e => e.Parent, "FK_firecars_parent");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Car)
                    .HasMaxLength(255)
                    .HasColumnName("car");

                entity.Property(e => e.IdГарнизон)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_гарнизон");

                entity.Property(e => e.Parent)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent");

                entity.Property(e => e.Psch)
                    .HasMaxLength(255)
                    .HasColumnName("psch");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .HasColumnName("type");

                entity.Property(e => e.Комментарий).HasMaxLength(255);

                entity.HasOne(d => d.ParentNavigation)
                    .WithMany(p => p.InverseParentNavigation)
                    .HasForeignKey(d => d.Parent)
                    .HasConstraintName("FK_firecars_parent");
            });

            modelBuilder.Entity<Firecall>(entity =>
            {
                entity.ToTable("firecalls");

                entity.HasComment("Выезда");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Additionalsr)
                    .HasMaxLength(255)
                    .HasColumnName("additionalsr");

                entity.Property(e => e.Adres)
                    .HasMaxLength(255)
                    .HasColumnName("adres");

                entity.Property(e => e.Calldata)
                    .HasColumnType("datetime")
                    .HasColumnName("calldata");

                entity.Property(e => e.Calltime)
                    .HasColumnType("datetime")
                    .HasColumnName("calltime");

                entity.Property(e => e.Comment)
                    .HasMaxLength(255)
                    .HasColumnName("comment");

                entity.Property(e => e.Fabula)
                    .HasMaxLength(511)
                    .HasColumnName("fabula");

                entity.Property(e => e.Firearea)
                    .HasMaxLength(255)
                    .HasColumnName("firearea");

                entity.Property(e => e.Gdzs)
                    .HasColumnType("int(11)")
                    .HasColumnName("gdzs");

                entity.Property(e => e.Gidrants)
                    .HasMaxLength(255)
                    .HasColumnName("gidrants");

                entity.Property(e => e.Locarea)
                    .HasMaxLength(255)
                    .HasColumnName("locarea");

                entity.Property(e => e.Mguid)
                    .HasMaxLength(36)
                    .HasColumnName("mguid");

                entity.Property(e => e.Obj)
                    .HasMaxLength(255)
                    .HasColumnName("obj");

                entity.Property(e => e.Objproperties)
                    .HasMaxLength(255)
                    .HasColumnName("objproperties");

                entity.Property(e => e.Objtype)
                    .HasMaxLength(255)
                    .HasColumnName("objtype");

                entity.Property(e => e.Peopleinfire)
                    .HasColumnType("int(11)")
                    .HasColumnName("peopleinfire");

                entity.Property(e => e.Rang)
                    .HasMaxLength(255)
                    .HasColumnName("rang");

                entity.Property(e => e.Result)
                    .HasMaxLength(255)
                    .HasColumnName("result");

                entity.Property(e => e.Returntime)
                    .HasColumnType("datetime")
                    .HasColumnName("returntime");

                entity.Property(e => e.Stepenognest)
                    .HasMaxLength(255)
                    .HasColumnName("stepenognest");

                entity.Property(e => e.Storeynum)
                    .HasColumnType("int(11)")
                    .HasColumnName("storeynum");

                entity.Property(e => e.Stvolnum)
                    .HasColumnType("int(11)")
                    .HasColumnName("stvolnum");

                entity.Property(e => e.Tchange)
                    .HasColumnType("timestamp")
                    .HasColumnName("tchange")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Timearrive)
                    .HasColumnType("datetime")
                    .HasColumnName("timearrive");

                entity.Property(e => e.Timedepart)
                    .HasColumnType("datetime")
                    .HasColumnName("timedepart");
            });

            modelBuilder.Entity<Firecar>(entity =>
            {
                entity.ToTable("firecars");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.IdГарнизон)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_гарнизон");

                entity.Property(e => e.Parent)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent");

                entity.Property(e => e.Автомобиль).HasMaxLength(255);

                entity.Property(e => e.Гарнизон).HasMaxLength(255);

                entity.Property(e => e.Комментарий).HasMaxLength(255);
            });

            modelBuilder.Entity<Firework>(entity =>
            {
                entity.ToTable("fireworks");

                entity.HasComment("Выезда");

                entity.HasIndex(e => e.IdГарнизона, "FK_fireworks_id_гарнизона");

                entity.HasIndex(e => e.IdАвтомобиля, "FK_fireworks_id_машины");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.IdАвтомобиля)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_автомобиля");

                entity.Property(e => e.IdГарнизона)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_гарнизона");

                entity.Property(e => e.N)
                    .HasColumnType("int(11)")
                    .HasColumnName("n");

                entity.Property(e => e.Nall)
                    .HasColumnType("int(11)")
                    .HasColumnName("nall");

                entity.Property(e => e.Nown)
                    .HasColumnType("int(11)")
                    .HasColumnName("nown");

                entity.Property(e => e.Автомобиль).HasMaxLength(255);

                entity.Property(e => e.Адрес).HasMaxLength(255);

                entity.Property(e => e.ВремяВозвращенияВДепо)
                    .HasColumnType("datetime")
                    .HasColumnName("Время_возвращения_в_депо");

                entity.Property(e => e.ВремяВыезда)
                    .HasColumnType("timestamp")
                    .HasColumnName("Время_выезда")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ВремяКМестуВызова)
                    .HasColumnType("datetime")
                    .HasColumnName("Время_к_месту_вызова");

                entity.Property(e => e.ВремяЛиквидации)
                    .HasColumnType("datetime")
                    .HasColumnName("Время_ликвидации");

                entity.Property(e => e.ВремяЛокализации)
                    .HasColumnType("datetime")
                    .HasColumnName("Время_локализации");

                entity.Property(e => e.ВремяСообщения)
                    .HasColumnType("datetime")
                    .HasColumnName("время_сообщения");

                entity.Property(e => e.Результат).HasMaxLength(255);

                entity.Property(e => e.Фабула).HasMaxLength(255);

                entity.HasOne(d => d.IdАвтомобиляNavigation)
                    .WithMany(p => p.Fireworks)
                    .HasForeignKey(d => d.IdАвтомобиля)
                    .HasConstraintName("FK_fireworks_id_автомобиля");

                entity.HasOne(d => d.IdГарнизонаNavigation)
                    .WithMany(p => p.Fireworks)
                    .HasForeignKey(d => d.IdГарнизона)
                    .HasConstraintName("FK_fireworks_id_гарнизона");
            });

            modelBuilder.Entity<Garndatum>(entity =>
            {
                entity.ToTable("garndata");

                entity.HasIndex(e => e.Parent, "FK_garndata_parent");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Dataerr)
                    .HasColumnType("int(11)")
                    .HasColumnName("dataerr");

                entity.Property(e => e.Datafilled).HasColumnName("datafilled");

                entity.Property(e => e.ForControl)
                    .HasMaxLength(255)
                    .HasColumnName("forControl");

                entity.Property(e => e.ForRep2)
                    .HasMaxLength(255)
                    .HasColumnName("forRep2");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(50)
                    .HasColumnName("fullname");

                entity.Property(e => e.GarnTypeid)
                    .HasColumnType("int(11)")
                    .HasColumnName("garn_typeid")
                    .HasComment("подразделение, местный, территориальный");

                entity.Property(e => e.Garnizon)
                    .HasMaxLength(127)
                    .HasColumnName("garnizon");

                entity.Property(e => e.Garntype)
                    .HasMaxLength(31)
                    .HasColumnName("garntype");

                entity.Property(e => e.Karaul)
                    .HasColumnType("int(11)")
                    .HasColumnName("karaul");

                entity.Property(e => e.Mdate)
                    .HasColumnType("timestamp")
                    .HasColumnName("mdate")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder");

                entity.Property(e => e.Parent)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent");

                entity.Property(e => e.Rank)
                    .HasMaxLength(31)
                    .HasColumnName("rank");

                entity.Property(e => e.SizodsBazaGdzs)
                    .HasColumnType("int(11)")
                    .HasColumnName("sizods_baza_gdzs");

                entity.Property(e => e.SizodsMname)
                    .HasMaxLength(255)
                    .HasColumnName("sizods_mname");

                entity.Property(e => e.SizodsPostGdzs)
                    .HasColumnType("int(11)")
                    .HasColumnName("sizods_post_gdzs");

                entity.Property(e => e.SizodsRaschet)
                    .HasColumnType("int(11)")
                    .HasColumnName("sizods_raschet");

                entity.Property(e => e.SizodsRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("sizods_rezerv");

                entity.Property(e => e.Visibility)
                    .IsRequired()
                    .HasColumnName("visibility")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Ав1Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ__1__br");

                entity.Property(e => e.Ав1Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ__1__remont");

                entity.Property(e => e.Ав1Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ__1__rezerv");

                entity.Property(e => e.АвBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ_br");

                entity.Property(e => e.АвRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ_remont");

                entity.Property(e => e.АвRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ_rezerv");

                entity.Property(e => e.АгдзсBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АГДЗС_br");

                entity.Property(e => e.АгдзсRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АГДЗС_remont");

                entity.Property(e => e.АгдзсRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АГДЗС_rezerv");

                entity.Property(e => e.АкпBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АКП_br");

                entity.Property(e => e.АкпRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АКП_remont");

                entity.Property(e => e.АкпRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АКП_rezerv");

                entity.Property(e => e.Ал30Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ_30_br");

                entity.Property(e => e.Ал30Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ_30_remont");

                entity.Property(e => e.Ал30Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ_30_rezerv");

                entity.Property(e => e.Ал50Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ_50_br");

                entity.Property(e => e.Ал50Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ_50_remont");

                entity.Property(e => e.Ал50Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ_50_rezerv");

                entity.Property(e => e.АмпBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АМП_br");

                entity.Property(e => e.АмпRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АМП_remont");

                entity.Property(e => e.АмпRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АМП_rezerv");

                entity.Property(e => e.АнрBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АНР_br");

                entity.Property(e => e.АнрRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АНР_remont");

                entity.Property(e => e.АнрRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АНР_rezerv");

                entity.Property(e => e.АппBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АПП_br");

                entity.Property(e => e.АппRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АПП_remont");

                entity.Property(e => e.АппRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АПП_rezerv");

                entity.Property(e => e.АрBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АР_br");

                entity.Property(e => e.АрRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АР_remont");

                entity.Property(e => e.АрRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АР_rezerv");

                entity.Property(e => e.Арс14Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("АРС_14_br");

                entity.Property(e => e.Арс14Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АРС_14_remont");

                entity.Property(e => e.Арс14Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АРС_14_rezerv");

                entity.Property(e => e.АсаBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСА_br");

                entity.Property(e => e.АсаRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСА_remont");

                entity.Property(e => e.АсаRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСА_rezerv");

                entity.Property(e => e.АсмBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСМ_br");

                entity.Property(e => e.АсмRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСМ_remont");

                entity.Property(e => e.АсмRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСМ_rezerv");

                entity.Property(e => e.АцBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦ_br");

                entity.Property(e => e.АцRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦ_remont");

                entity.Property(e => e.АцRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦ_rezerv");

                entity.Property(e => e.АцлBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦЛ_br");

                entity.Property(e => e.АцлRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦЛ_remont");

                entity.Property(e => e.АцлRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦЛ_rezerv");

                entity.Property(e => e.БензопилыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензопилы_br");

                entity.Property(e => e.БензопилыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензопилы_remont");

                entity.Property(e => e.БензопилыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензопилы_rezerv");

                entity.Property(e => e.БензорезыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензорезы_br");

                entity.Property(e => e.БензорезыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензорезы_remont");

                entity.Property(e => e.БензорезыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензорезы_rezerv");

                entity.Property(e => e.Бпла1Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА1_br");

                entity.Property(e => e.Бпла1Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА1_remont");

                entity.Property(e => e.Бпла1Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА1_rezerv");

                entity.Property(e => e.Бпла2Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА2_br");

                entity.Property(e => e.Бпла2Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА2_remont");

                entity.Property(e => e.Бпла2Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА2_rezerv");

                entity.Property(e => e.Водители2)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водители_2");

                entity.Property(e => e.Водители3)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водители_3");

                entity.Property(e => e.ВодолазноеСнаряжениеBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_br");

                entity.Property(e => e.ВодолазноеСнаряжениеRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_remont");

                entity.Property(e => e.ВодолазноеСнаряжениеRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_rezerv");

                entity.Property(e => e.ВодолазноеСнаряжениеКомплектBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_комплект_br");

                entity.Property(e => e.ВодолазноеСнаряжениеКомплектRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_комплект_remont");

                entity.Property(e => e.ВодолазноеСнаряжениеКомплектRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_комплект_rezerv");

                entity.Property(e => e.Водолазы2)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазы_2");

                entity.Property(e => e.Всего2)
                    .HasColumnType("int(11)")
                    .HasColumnName("Всего_2");

                entity.Property(e => e.Всего4)
                    .HasColumnType("int(11)")
                    .HasColumnName("Всего_4");

                entity.Property(e => e.ГасиМеханизированныйBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_механизированный_br");

                entity.Property(e => e.ГасиМеханизированныйRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_механизированный_remont");

                entity.Property(e => e.ГасиМеханизированныйRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_механизированный_rezerv");

                entity.Property(e => e.ГасиРучнойBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_ручной_br");

                entity.Property(e => e.ГасиРучнойRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_ручной_remont");

                entity.Property(e => e.ГасиРучнойRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_ручной_rezerv");

                entity.Property(e => e.Гимс2)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГИМС_2");

                entity.Property(e => e.ГрузовойАвтомобильBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Грузовой_автомобиль_br");

                entity.Property(e => e.ГрузовойАвтомобильRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Грузовой_автомобиль_remont");

                entity.Property(e => e.ГрузовойАвтомобильRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Грузовой_автомобиль_rezerv");

                entity.Property(e => e.ДежурныйОтГимс)
                    .HasMaxLength(255)
                    .HasColumnName("Дежурный_от_ГИМС");

                entity.Property(e => e.ДежурныйОтГпн)
                    .HasMaxLength(255)
                    .HasColumnName("Дежурный_от_ГПН");

                entity.Property(e => e.Диспетчер2)
                    .HasColumnType("int(11)")
                    .HasColumnName("Диспетчер_2");

                entity.Property(e => e.ДиспетчерПсг)
                    .HasMaxLength(255)
                    .HasColumnName("Диспетчер_ПСГ");

                entity.Property(e => e.ИглаBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Игла_br");

                entity.Property(e => e.ИглаRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Игла_remont");

                entity.Property(e => e.ИглаRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Игла_rezerv");

                entity.Property(e => e.КатераЛодкиBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Катера_лодки_br");

                entity.Property(e => e.КатераЛодкиRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Катера_лодки_remont");

                entity.Property(e => e.КатераЛодкиRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Катера_лодки_rezerv");

                entity.Property(e => e.КвадроциклыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Квадроциклы_br");

                entity.Property(e => e.КвадроциклыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Квадроциклы_remont");

                entity.Property(e => e.КвадроциклыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Квадроциклы_rezerv");

                entity.Property(e => e.Ко2)
                    .HasColumnType("int(11)")
                    .HasColumnName("КО_2");

                entity.Property(e => e.Ко3)
                    .HasColumnType("int(11)")
                    .HasColumnName("КО_3");

                entity.Property(e => e.Командировка4)
                    .HasColumnType("int(11)")
                    .HasColumnName("Командировка_4");

                entity.Property(e => e.Крпсс2)
                    .HasColumnType("int(11)")
                    .HasColumnName("КРПСС_2");

                entity.Property(e => e.ЛсВБр2)
                    .HasColumnType("int(11)")
                    .HasColumnName("ЛС_в_БР_2");

                entity.Property(e => e.МаркаБпла1)
                    .HasMaxLength(63)
                    .HasColumnName("Марка__БПЛА1");

                entity.Property(e => e.МаркаБпла2)
                    .HasColumnType("int(11)")
                    .HasColumnName("Марка__БПЛА2");

                entity.Property(e => e.МаркаГасиМех)
                    .HasColumnType("int(11)")
                    .HasColumnName("Марка__ГАСИ_мех");

                entity.Property(e => e.МаркаГасиРучной)
                    .HasMaxLength(255)
                    .HasColumnName("Марка__ГАСИ_ручной");

                entity.Property(e => e.МедКомплектBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мед_комплект_br");

                entity.Property(e => e.МедКомплектRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мед_комплект_remont");

                entity.Property(e => e.МедКомплектRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мед_комплект_rezerv");

                entity.Property(e => e.МотопомпыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мотопомпы_br");

                entity.Property(e => e.МотопомпыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мотопомпы_remont");

                entity.Property(e => e.МотопомпыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мотопомпы_rezerv");

                entity.Property(e => e.Налицо1)
                    .HasColumnType("int(11)")
                    .HasColumnName("Налицо_1");

                entity.Property(e => e.НачальникДежурнойСменыТпсг)
                    .HasMaxLength(255)
                    .HasColumnName("НачальникДежурнойСменыТПСГ")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.НачальникКараула)
                    .HasMaxLength(255)
                    .HasColumnName("Начальник_караула");

                entity.Property(e => e.НачальникТпсг)
                    .HasMaxLength(255)
                    .HasColumnName("НачальникТПСГ")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Нк2)
                    .HasColumnType("int(11)")
                    .HasColumnName("НК_2");

                entity.Property(e => e.Нк3)
                    .HasColumnType("int(11)")
                    .HasColumnName("НК_3");

                entity.Property(e => e.ОперативнаяГруппаBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Оперативная_группа_br");

                entity.Property(e => e.ОперативнаяГруппаRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Оперативная_группа_remont");

                entity.Property(e => e.ОперативнаяГруппаRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Оперативная_группа_rezerv");

                entity.Property(e => e.ОперативныйДежурныйПоГарнизону)
                    .HasMaxLength(255)
                    .HasColumnName("Оперативный_дежурный_по_гарнизону");

                entity.Property(e => e.ОтветственныйЗаСборДпо)
                    .HasMaxLength(255)
                    .HasColumnName("Ответственный_за_сбор_ДПО");

                entity.Property(e => e.Отпуск4)
                    .HasColumnType("int(11)")
                    .HasColumnName("Отпуск_4");

                entity.Property(e => e.ПвFault)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПВ_fault");

                entity.Property(e => e.ПвTotal)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПВ_total");

                entity.Property(e => e.ПгFault)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПГ_fault");

                entity.Property(e => e.ПгTotal)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПГ_total");

                entity.Property(e => e.ПенообразовательInrezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пенообразователь_inrezerv");

                entity.Property(e => e.ПенообразовательInwork)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пенообразователь_inwork");

                entity.Property(e => e.Пнк2)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПНК_2");

                entity.Property(e => e.Пнк3)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПНК_3");

                entity.Property(e => e.ПнсBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПНС_br");

                entity.Property(e => e.ПнсRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПНС_remont");

                entity.Property(e => e.ПнсRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПНС_rezerv");

                entity.Property(e => e.ПоБольничному4)
                    .HasColumnType("int(11)")
                    .HasColumnName("По_больничному_4");

                entity.Property(e => e.ПоСписку1)
                    .HasColumnType("int(11)")
                    .HasColumnName("По_списку_1");

                entity.Property(e => e.Пожарные2)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарные_2");

                entity.Property(e => e.Пожарные3)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарные_3");

                entity.Property(e => e.ПожарныйПоездBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарный_поезд_br");

                entity.Property(e => e.ПожарныйПоездRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарный_поезд_remont");

                entity.Property(e => e.ПожарныйПоездRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарный_поезд_rezerv");

                entity.Property(e => e.ПпFault)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПП_fault");

                entity.Property(e => e.ПпTotal)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПП_total");

                entity.Property(e => e.ПриспособленныеДляПеревозкиОвBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Приспособленные_для_перевозки_ОВ_br");

                entity.Property(e => e.ПриспособленныеДляПеревозкиОвRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Приспособленные_для_перевозки_ОВ_remont");

                entity.Property(e => e.ПриспособленныеДляПеревозкиОвRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Приспособленные_для_перевозки_ОВ_rezerv");

                entity.Property(e => e.Прочее4)
                    .HasColumnType("int(11)")
                    .HasColumnName("Прочее_4");

                entity.Property(e => e.ПсаBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПСА_br");

                entity.Property(e => e.ПсаRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПСА_remont");

                entity.Property(e => e.ПсаRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПСА_rezerv");

                entity.Property(e => e.РанцевыеОгнетушителиBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Ранцевые_огнетушители_br");

                entity.Property(e => e.РанцевыеОгнетушителиRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Ранцевые_огнетушители_remont");

                entity.Property(e => e.РанцевыеОгнетушителиRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Ранцевые_огнетушители_rezerv");

                entity.Property(e => e.РуководительСменыТпсг)
                    .HasMaxLength(255)
                    .HasColumnName("РуководительСменыТПСГ");

                entity.Property(e => e.СвпBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("СВП_br");

                entity.Property(e => e.СвпRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("СВП_remont");

                entity.Property(e => e.СвпRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("СВП_rezerv");

                entity.Property(e => e.СнегоходыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Снегоходы_br");

                entity.Property(e => e.СнегоходыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Снегоходы_remont");

                entity.Property(e => e.СнегоходыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Снегоходы_rezerv");

                entity.Property(e => e.СтаршийПомошникТпсг)
                    .HasMaxLength(255)
                    .HasColumnName("СтаршийПомошникТПСГ")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Таск)
                    .HasColumnType("int(11)")
                    .HasColumnName("ТАСК");

                entity.Property(e => e.Ток)
                    .HasColumnType("int(11)")
                    .HasColumnName("ТОК");

                entity.Property(e => e.УксBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("УКС_br");

                entity.Property(e => e.УксRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("УКС_remont");

                entity.Property(e => e.УксRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("УКС_rezerv");

                entity.HasOne(d => d.ParentNavigation)
                    .WithMany(p => p.InverseParentNavigation)
                    .HasForeignKey(d => d.Parent)
                    .HasConstraintName("FK_garndata_parent");
            });

            modelBuilder.Entity<Garnizon>(entity =>
            {
                entity.ToTable("garnizons");

                entity.HasIndex(e => e.Parent, "FK_garnizons_parent");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.DataChecked)
                    .HasColumnName("data_checked")
                    .HasComment("Данные проверены");

                entity.Property(e => e.DataFilled)
                    .HasColumnName("data_filled")
                    .HasComment("Данные заполнены");

                entity.Property(e => e.ExcelName)
                    .HasMaxLength(63)
                    .HasColumnName("excel_name");

                entity.Property(e => e.ForControl)
                    .HasMaxLength(511)
                    .HasColumnName("forControl")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ForRep2)
                    .HasMaxLength(255)
                    .HasColumnName("forRep2");

                entity.Property(e => e.Level)
                    .HasMaxLength(255)
                    .HasColumnName("level");

                entity.Property(e => e.Name)
                    .HasMaxLength(512)
                    .HasColumnName("name");

                entity.Property(e => e.NameShort)
                    .HasMaxLength(255)
                    .HasColumnName("name_short");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Order)
                    .HasColumnType("int(11)")
                    .HasColumnName("order");

                entity.Property(e => e.Parent)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent");

                entity.Property(e => e.Rank)
                    .HasMaxLength(63)
                    .HasColumnName("rank")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Type)
                    .HasMaxLength(31)
                    .HasColumnName("type")
                    .HasDefaultValueSql("'ФПС'");

                entity.Property(e => e.TypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("type_id")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.ParentNavigation)
                    .WithMany(p => p.InverseParentNavigation)
                    .HasForeignKey(d => d.Parent)
                    .HasConstraintName("FK_garnizons_parent");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("groups");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Comment)
                    .HasMaxLength(255)
                    .HasColumnName("comment");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Kostym>(entity =>
            {
                entity.ToTable("kostyms");

                entity.HasIndex(e => e.GarnizionId, "FK_kostyms_garnizion_id");

                entity.HasIndex(e => e.SubdivisionId, "FK_kostyms_subdivision_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.EditTime)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("edit_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Excel)
                    .HasMaxLength(255)
                    .HasColumnName("excel");

                entity.Property(e => e.GarnizionId)
                    .HasColumnType("int(11)")
                    .HasColumnName("garnizion_id");

                entity.Property(e => e.Mdate)
                    .HasColumnType("date")
                    .HasColumnName("mdate");

                entity.Property(e => e.Mname)
                    .HasMaxLength(250)
                    .HasColumnName("mname");

                entity.Property(e => e.N)
                    .HasColumnType("int(11)")
                    .HasColumnName("n");

                entity.Property(e => e.NameGarnizione)
                    .HasMaxLength(255)
                    .HasColumnName("name_garnizione");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder");

                entity.Property(e => e.Subdivision)
                    .HasMaxLength(255)
                    .HasColumnName("subdivision");

                entity.Property(e => e.SubdivisionId)
                    .HasColumnType("int(11)")
                    .HasColumnName("subdivision_id");

                entity.HasOne(d => d.SubdivisionNavigation)
                    .WithMany(p => p.Kostyms)
                    .HasForeignKey(d => d.SubdivisionId)
                    .HasConstraintName("FK_kostyms_subdivision_id");
            });

            modelBuilder.Entity<Mhelp>(entity =>
            {
                entity.ToTable("mhelp");

                entity.HasIndex(e => e.Parent, "FK_mhelp_parent");

                entity.HasIndex(e => e.Id, "id")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Author)
                    .HasColumnType("year(4)")
                    .HasColumnName("author");

                entity.Property(e => e.Catalog)
                    .HasMaxLength(255)
                    .HasColumnName("catalog");

                entity.Property(e => e.DateCreate)
                    .HasColumnType("datetime")
                    .HasColumnName("date_create");

                entity.Property(e => e.DateEdit)
                    .HasMaxLength(255)
                    .HasColumnName("date_edit");

                entity.Property(e => e.Descrexist)
                    .HasColumnType("int(11)")
                    .HasColumnName("descrexist")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.DocType)
                    .HasMaxLength(255)
                    .HasColumnName("docType");

                entity.Property(e => e.Enabled)
                    .HasColumnType("int(11)")
                    .HasColumnName("enabled")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Filename)
                    .HasMaxLength(255)
                    .HasColumnName("filename");

                entity.Property(e => e.Images)
                    .HasMaxLength(255)
                    .HasColumnName("images");

                entity.Property(e => e.Level)
                    .HasMaxLength(255)
                    .HasColumnName("level");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Params)
                    .HasMaxLength(255)
                    .HasColumnName("params");

                entity.Property(e => e.Parent)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent");

                entity.Property(e => e.Video)
                    .HasMaxLength(255)
                    .HasColumnName("video");

                entity.HasOne(d => d.ParentNavigation)
                    .WithMany(p => p.InverseParentNavigation)
                    .HasForeignKey(d => d.Parent)
                    .HasConstraintName("FK_mhelp_parent");
            });

            modelBuilder.Entity<Msg>(entity =>
            {
                entity.ToTable("msgs");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Cmd)
                    .HasMaxLength(255)
                    .HasColumnName("cmd");

                entity.Property(e => e.Comment)
                    .HasMaxLength(255)
                    .HasColumnName("comment");

                entity.Property(e => e.From)
                    .HasMaxLength(255)
                    .HasColumnName("from");

                entity.Property(e => e.IsNew)
                    .HasColumnName("isNew")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Lastedit)
                    .HasColumnType("timestamp")
                    .HasColumnName("lastedit")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Mdate)
                    .HasColumnType("timestamp")
                    .HasColumnName("mdate");

                entity.Property(e => e.Status)
                    .HasMaxLength(31)
                    .HasColumnName("status")
                    .HasDefaultValueSql("'новое'");

                entity.Property(e => e.StatusId)
                    .HasColumnType("int(11)")
                    .HasColumnName("status_id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Tema)
                    .HasMaxLength(255)
                    .HasColumnName("tema");

                entity.Property(e => e.Text)
                    .HasMaxLength(255)
                    .HasColumnName("text");

                entity.Property(e => e.To)
                    .HasMaxLength(255)
                    .HasColumnName("to");

                entity.Property(e => e.Visible)
                    .HasMaxLength(255)
                    .HasColumnName("visible");
            });

            modelBuilder.Entity<Mtable>(entity =>
            {
                entity.ToTable("mtable");

                entity.HasIndex(e => e.Parent, "FK_mtable_parent");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10)")
                    .HasColumnName("id");

                entity.Property(e => e.Filters)
                    .HasMaxLength(255)
                    .HasColumnName("filters")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Icon)
                    .HasColumnType("blob")
                    .HasColumnName("icon");

                entity.Property(e => e.MapParams)
                    .HasMaxLength(255)
                    .HasColumnName("map_params");

                entity.Property(e => e.MeChecked)
                    .HasColumnType("smallint(5)")
                    .HasColumnName("meChecked");

                entity.Property(e => e.N)
                    .HasColumnType("int(11)")
                    .HasColumnName("n");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Objectname)
                    .HasMaxLength(255)
                    .HasColumnName("objectname");

                entity.Property(e => e.PackageName)
                    .HasMaxLength(255)
                    .HasColumnName("packageName")
                    .HasDefaultValueSql("'cuks'");

                entity.Property(e => e.Parent)
                    .HasColumnType("int(10)")
                    .HasColumnName("parent");

                entity.Property(e => e.Repositoryname)
                    .HasMaxLength(50)
                    .HasColumnName("repositoryname");

                entity.Property(e => e.Setname)
                    .HasMaxLength(255)
                    .HasColumnName("setname");

                entity.Property(e => e.View)
                    .HasMaxLength(255)
                    .HasColumnName("view")
                    .HasComment("Имя view в котором будет отображаться таблица");

                entity.HasOne(d => d.ParentNavigation)
                    .WithMany(p => p.InverseParentNavigation)
                    .HasForeignKey(d => d.Parent)
                    .HasConstraintName("FK_mtable_parent");
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.ToTable("organization");

                entity.HasIndex(e => e.IdПсо, "FK_organization_id_ПСО");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.IdPsg)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_psg");

                entity.Property(e => e.IdPunct)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_punct");

                entity.Property(e => e.IdRegion)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_region");

                entity.Property(e => e.IdSettle)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_settle");

                entity.Property(e => e.IdГарнизона)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_гарнизона");

                entity.Property(e => e.IdДокумента)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_документа")
                    .HasComment("id последнего документа птп/ктп или null");

                entity.Property(e => e.IdПсо)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_ПСО");

                entity.Property(e => e.Psg)
                    .HasMaxLength(255)
                    .HasColumnName("psg")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Адрес).HasMaxLength(511);

                entity.Property(e => e.ВремяРедактирования)
                    .HasColumnType("timestamp")
                    .HasColumnName("Время_редактирования")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Гарнизон).HasMaxLength(255);

                entity.Property(e => e.ДатаКонтроля)
                    .HasMaxLength(255)
                    .HasColumnName("дата_контроля");

                entity.Property(e => e.Исполнен).HasColumnName("исполнен");

                entity.Property(e => e.Каталог)
                    .HasMaxLength(255)
                    .HasColumnName("каталог");

                entity.Property(e => e.Комментарии).HasMaxLength(255);

                entity.Property(e => e.МаршрутСледования)
                    .HasMaxLength(255)
                    .HasColumnName("Маршрут_следования");

                entity.Property(e => e.Наименование).HasMaxLength(511);

                entity.Property(e => e.НасПункт)
                    .HasMaxLength(255)
                    .HasColumnName("Нас_пункт");

                entity.Property(e => e.Номер).HasMaxLength(255);

                entity.Property(e => e.ПланТушения)
                    .HasMaxLength(255)
                    .HasColumnName("план_тушения");

                entity.Property(e => e.Поселение).HasMaxLength(255);

                entity.Property(e => e.Признак)
                    .HasColumnType("int(11)")
                    .HasColumnName("признак")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Псо)
                    .HasMaxLength(255)
                    .HasColumnName("ПСО");

                entity.Property(e => e.ПтпКтп)
                    .HasMaxLength(255)
                    .HasColumnName("птп_ктп");

                entity.Property(e => e.ПтпСтатус)
                    .HasMaxLength(255)
                    .HasColumnName("птп_статус");

                entity.Property(e => e.Район).HasMaxLength(255);

                entity.Property(e => e.РасстояниеДоПч).HasColumnName("Расстояние_до_ПЧ");

                entity.Property(e => e.Срок)
                    .HasColumnType("datetime")
                    .HasColumnName("срок");

                entity.Property(e => e.ТелефонДиспетчераЭнергослужбы)
                    .HasMaxLength(255)
                    .HasColumnName("Телефон_диспетчера_энергослужбы");

                entity.Property(e => e.ТелефонОтвЗаПб)
                    .HasMaxLength(255)
                    .HasColumnName("Телефон_отв_за_ПБ");

                entity.Property(e => e.ТелефонОхраны)
                    .HasMaxLength(255)
                    .HasColumnName("Телефон_охраны");

                entity.Property(e => e.ТелефонРуководителя)
                    .HasMaxLength(255)
                    .HasColumnName("Телефон_руководителя");

                entity.Property(e => e.ТипДокумента)
                    .HasMaxLength(255)
                    .HasColumnName("тип_документа");

                entity.Property(e => e.ЧислоЖизненноВажныхОбъектов)
                    .HasColumnType("int(11)")
                    .HasColumnName("Число_жизненно_важных_объектов");

                entity.Property(e => e.ЧислоЖилыхДомов)
                    .HasColumnType("int(11)")
                    .HasColumnName("Число_жилых_домов");

                entity.HasOne(d => d.IdПсоNavigation)
                    .WithMany(p => p.Organizations)
                    .HasForeignKey(d => d.IdПсо)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_organization_id_ПСО");
            });

            modelBuilder.Entity<Pena>(entity =>
            {
                entity.ToTable("penas");

                entity.HasIndex(e => e.SubdivisionId, "FK_penas_subdivision_id");

                entity.HasIndex(e => e.GarnizonId, "FK_penas_subdivision_id2");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.EditTime)
                    .HasColumnType("timestamp")
                    .HasColumnName("edit_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Excel)
                    .HasMaxLength(255)
                    .HasColumnName("excel");

                entity.Property(e => e.GarnizonId)
                    .HasColumnType("int(11)")
                    .HasColumnName("garnizon_id");

                entity.Property(e => e.Inrezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("inrezerv");

                entity.Property(e => e.Inwork)
                    .HasColumnType("int(11)")
                    .HasColumnName("inwork");

                entity.Property(e => e.Mdate)
                    .HasColumnType("date")
                    .HasColumnName("mdate");

                entity.Property(e => e.Mname)
                    .HasMaxLength(255)
                    .HasColumnName("mname");

                entity.Property(e => e.NameGarnizone)
                    .HasMaxLength(255)
                    .HasColumnName("name_garnizone");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder");

                entity.Property(e => e.Subdivision)
                    .HasMaxLength(255)
                    .HasColumnName("subdivision");

                entity.Property(e => e.SubdivisionId)
                    .HasColumnType("int(11)")
                    .HasColumnName("subdivision_id");

                entity.HasOne(d => d.SubdivisionNavigation)
                    .WithMany(p => p.Penas)
                    .HasForeignKey(d => d.SubdivisionId)
                    .HasConstraintName("FK_penas_subdivision_id");
            });

            modelBuilder.Entity<Personal>(entity =>
            {
                entity.ToTable("personal");

                entity.HasIndex(e => e.PostId, "FK_personal_post_id");

                entity.HasIndex(e => e.PsgId, "FK_personal_psg_id2");

                entity.HasIndex(e => e.SubdivisionId, "FK_personal_subdivision_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Comment)
                    .HasMaxLength(255)
                    .HasColumnName("comment");

                entity.Property(e => e.F).HasMaxLength(511);

                entity.Property(e => e.I).HasMaxLength(63);

                entity.Property(e => e.Inwork)
                    .HasColumnType("int(11)")
                    .HasColumnName("inwork")
                    .HasDefaultValueSql("'1'")
                    .HasComment("Работает 1 отгул 2 уволен 0");

                entity.Property(e => e.O).HasMaxLength(63);

                entity.Property(e => e.Order)
                    .HasColumnType("int(11)")
                    .HasColumnName("order");

                entity.Property(e => e.Otdel)
                    .HasMaxLength(255)
                    .HasColumnName("otdel");

                entity.Property(e => e.OtdelId)
                    .HasColumnType("int(11)")
                    .HasColumnName("otdel_id");

                entity.Property(e => e.OtpBeg)
                    .HasColumnType("datetime")
                    .HasColumnName("otp_beg");

                entity.Property(e => e.OtpEnd)
                    .HasColumnType("datetime")
                    .HasColumnName("otp_end");

                entity.Property(e => e.Post)
                    .HasMaxLength(255)
                    .HasColumnName("post");

                entity.Property(e => e.PostId)
                    .HasColumnType("int(11)")
                    .HasColumnName("post_id");

                entity.Property(e => e.Posyvnoy)
                    .HasMaxLength(255)
                    .HasColumnName("posyvnoy");

                entity.Property(e => e.PsgId)
                    .HasColumnType("int(11)")
                    .HasColumnName("psg_id");

                entity.Property(e => e.PsgName)
                    .HasMaxLength(255)
                    .HasColumnName("psg_name");

                entity.Property(e => e.Status)
                    .HasColumnType("int(11)")
                    .HasColumnName("status")
                    .HasDefaultValueSql("'0'")
                    .HasComment("Сегодня на смене (из графика)");

                entity.Property(e => e.Subdivision)
                    .HasMaxLength(255)
                    .HasColumnName("subdivision");

                entity.Property(e => e.SubdivisionId)
                    .HasColumnType("int(11)")
                    .HasColumnName("subdivision_id");

                entity.Property(e => e.TfDom)
                    .HasMaxLength(255)
                    .HasColumnName("tf_dom");

                entity.Property(e => e.TfMobil)
                    .HasMaxLength(255)
                    .HasColumnName("tf_mobil");

                entity.Property(e => e.TfWork)
                    .HasMaxLength(255)
                    .HasColumnName("tf_work");

                entity.Property(e => e.Zvanie)
                    .HasMaxLength(255)
                    .HasColumnName("zvanie");

                entity.HasOne(d => d.PostNavigation)
                    .WithMany(p => p.Personals)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_personal_post_id");

                entity.HasOne(d => d.SubdivisionNavigation)
                    .WithMany(p => p.Personals)
                    .HasForeignKey(d => d.SubdivisionId)
                    .HasConstraintName("FK_personal_subdivision_id");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("post");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Level)
                    .HasColumnType("int(11)")
                    .HasColumnName("level")
                    .HasComment("0 - территориальный 1 - ПСГ  2 - местный гарнизон");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder")
                    .HasDefaultValueSql("'20'");
            });

            modelBuilder.Entity<Psg>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("psg");

                entity.Property(e => e.Datafilled).HasColumnName("datafilled");

                entity.Property(e => e.Garnizon)
                    .HasMaxLength(127)
                    .HasColumnName("garnizon");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder")
                    .HasDefaultValueSql("'100'");

                entity.Property(e => e.Old)
                    .IsRequired()
                    .HasColumnName("old")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Parent)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent");
            });

            modelBuilder.Entity<Psgdatum>(entity =>
            {
                entity.ToTable("psgdata");

                entity.HasIndex(e => e.Parent, "FK_psgdata_parent");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Dataerr)
                    .HasColumnType("int(11)")
                    .HasColumnName("dataerr");

                entity.Property(e => e.Datafilled).HasColumnName("datafilled");

                entity.Property(e => e.Errcolumns)
                    .HasMaxLength(255)
                    .HasColumnName("errcolumns")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ForControl)
                    .HasMaxLength(255)
                    .HasColumnName("forControl");

                entity.Property(e => e.ForRep2)
                    .HasMaxLength(255)
                    .HasColumnName("forRep2");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(511)
                    .HasColumnName("fullname")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.GarnTypeid)
                    .HasColumnType("int(11)")
                    .HasColumnName("garn_typeid")
                    .HasComment("подразделение, местный, территориальный");

                entity.Property(e => e.Garnizon)
                    .HasMaxLength(127)
                    .HasColumnName("garnizon");

                entity.Property(e => e.Garntype)
                    .HasMaxLength(31)
                    .HasColumnName("garntype");

                entity.Property(e => e.Isgps)
                    .HasColumnType("int(11)")
                    .HasColumnName("isgps");

                entity.Property(e => e.Karaul)
                    .HasColumnType("int(11)")
                    .HasColumnName("karaul")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Mdate)
                    .HasColumnType("timestamp")
                    .HasColumnName("mdate")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder")
                    .HasDefaultValueSql("'100'");

                entity.Property(e => e.Old)
                    .IsRequired()
                    .HasColumnName("old")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Parent)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent");

                entity.Property(e => e.Rank)
                    .HasMaxLength(31)
                    .HasColumnName("rank");

                entity.Property(e => e.SizodsBazaGdzs)
                    .HasColumnType("int(11)")
                    .HasColumnName("sizods_baza_gdzs");

                entity.Property(e => e.SizodsMname)
                    .HasMaxLength(255)
                    .HasColumnName("sizods_mname");

                entity.Property(e => e.SizodsPostGdzs)
                    .HasColumnType("int(11)")
                    .HasColumnName("sizods_post_gdzs");

                entity.Property(e => e.SizodsRaschet)
                    .HasColumnType("int(11)")
                    .HasColumnName("sizods_raschet");

                entity.Property(e => e.SizodsRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("sizods_rezerv");

                entity.Property(e => e.Spisok)
                    .HasColumnType("int(11)")
                    .HasColumnName("spisok");

                entity.Property(e => e.Tofirst)
                    .HasColumnType("int(11)")
                    .HasColumnName("tofirst");

                entity.Property(e => e.Totwo)
                    .HasColumnType("int(11)")
                    .HasColumnName("totwo");

                entity.Property(e => e.Visibility)
                    .IsRequired()
                    .HasColumnName("visibility")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.АбгBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АБГ_br");

                entity.Property(e => e.АбгRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АБГ_remont");

                entity.Property(e => e.АбгRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АБГ_rezerv");

                entity.Property(e => e.Ав1Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ__1__br");

                entity.Property(e => e.Ав1Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ__1__remont");

                entity.Property(e => e.Ав1Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ__1__rezerv");

                entity.Property(e => e.АвBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ_br");

                entity.Property(e => e.АвRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ_remont");

                entity.Property(e => e.АвRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ_rezerv");

                entity.Property(e => e.АвиацияНаДежурстве)
                    .HasColumnType("int(11)")
                    .HasColumnName("авиация_на_дежурстве");

                entity.Property(e => e.АвиацияПоШтату)
                    .HasColumnType("int(11)")
                    .HasColumnName("авиация_по_штату");

                entity.Property(e => e.АвтомобилиНаДежурстве)
                    .HasColumnType("int(11)")
                    .HasColumnName("автомобили_на_дежурстве");

                entity.Property(e => e.АвтомобилиПоШтату)
                    .HasColumnType("int(11)")
                    .HasColumnName("автомобили_по_штату");

                entity.Property(e => e.АгдзсBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АГДЗС_br");

                entity.Property(e => e.АгдзсRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АГДЗС_remont");

                entity.Property(e => e.АгдзсRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АГДЗС_rezerv");

                entity.Property(e => e.АкпBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АКП_br");

                entity.Property(e => e.АкпRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АКП_remont");

                entity.Property(e => e.АкпRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АКП_rezerv");

                entity.Property(e => e.Ал30Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ_30_br");

                entity.Property(e => e.Ал30Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ_30_remont");

                entity.Property(e => e.Ал30Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ_30_rezerv");

                entity.Property(e => e.Ал50Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ_50_br");

                entity.Property(e => e.Ал50Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ_50_remont");

                entity.Property(e => e.Ал50Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ_50_rezerv");

                entity.Property(e => e.АмбулансBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("амбуланс_br");

                entity.Property(e => e.АмбулансRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("амбуланс_remont");

                entity.Property(e => e.АмбулансRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("амбуланс_rezerv");

                entity.Property(e => e.АмпBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АМП_br");

                entity.Property(e => e.АмпRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АМП_remont");

                entity.Property(e => e.АмпRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АМП_rezerv");

                entity.Property(e => e.АнрBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АНР_br");

                entity.Property(e => e.АнрRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АНР_remont");

                entity.Property(e => e.АнрRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АНР_rezerv");

                entity.Property(e => e.АппBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АПП_br");

                entity.Property(e => e.АппRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АПП_remont");

                entity.Property(e => e.АппRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АПП_rezerv");

                entity.Property(e => e.АрBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АР_br");

                entity.Property(e => e.АрRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АР_remont");

                entity.Property(e => e.АрRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АР_rezerv");

                entity.Property(e => e.Арс14Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("АРС_14_br");

                entity.Property(e => e.Арс14Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АРС_14_remont");

                entity.Property(e => e.Арс14Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АРС_14_rezerv");

                entity.Property(e => e.АсаBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСА_br");

                entity.Property(e => e.АсаRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСА_remont");

                entity.Property(e => e.АсаRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСА_rezerv");

                entity.Property(e => e.АсмBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСМ_br");

                entity.Property(e => e.АсмRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСМ_remont");

                entity.Property(e => e.АсмRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСМ_rezerv");

                entity.Property(e => e.АсмпхBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСМПХ_br");

                entity.Property(e => e.АсмпхRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСМПХ_remont");

                entity.Property(e => e.АсмпхRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСМПХ_rezerv");

                entity.Property(e => e.АсоBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСО_br");

                entity.Property(e => e.АсоRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСО_remont");

                entity.Property(e => e.АсоRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСО_rezerv");

                entity.Property(e => e.АцBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦ_br");

                entity.Property(e => e.АцRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦ_remont");

                entity.Property(e => e.АцRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦ_rezerv");

                entity.Property(e => e.АцлBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦЛ_br");

                entity.Property(e => e.АцлRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦЛ_remont");

                entity.Property(e => e.АцлRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦЛ_rezerv");

                entity.Property(e => e.АшBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АШ_br");

                entity.Property(e => e.АшRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АШ_remont");

                entity.Property(e => e.АшRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АШ_rezerv");

                entity.Property(e => e.БензопилыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензопилы_br");

                entity.Property(e => e.БензопилыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензопилы_remont");

                entity.Property(e => e.БензопилыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензопилы_rezerv");

                entity.Property(e => e.БензорезыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензорезы_br");

                entity.Property(e => e.БензорезыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензорезы_remont");

                entity.Property(e => e.БензорезыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензорезы_rezerv");

                entity.Property(e => e.БолотоходыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Болотоходы_br");

                entity.Property(e => e.БолотоходыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Болотоходы_remont");

                entity.Property(e => e.БолотоходыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Болотоходы_rezerv");

                entity.Property(e => e.Бпла1Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА1_br");

                entity.Property(e => e.Бпла1Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА1_remont");

                entity.Property(e => e.Бпла1Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА1_rezerv");

                entity.Property(e => e.Бпла2Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА2_br");

                entity.Property(e => e.Бпла2Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА2_remont");

                entity.Property(e => e.Бпла2Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА2_rezerv");

                entity.Property(e => e.Водители).HasColumnType("int(11)");

                entity.Property(e => e.ВодителиГдзс)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водители_гдзс");

                entity.Property(e => e.ВодолазноеСнаряжениеBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_br");

                entity.Property(e => e.ВодолазноеСнаряжениеRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_remont");

                entity.Property(e => e.ВодолазноеСнаряжениеRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_rezerv");

                entity.Property(e => e.ВодолазноеСнаряжениеКомплектBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_комплект_br");

                entity.Property(e => e.ВодолазноеСнаряжениеКомплектRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_комплект_remont");

                entity.Property(e => e.ВодолазноеСнаряжениеКомплектRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_комплект_rezerv");

                entity.Property(e => e.Водолазы).HasColumnType("int(11)");

                entity.Property(e => e.Всего).HasColumnType("int(11)");

                entity.Property(e => e.ВсегоОтсутствуют)
                    .HasColumnType("int(11)")
                    .HasColumnName("Всего_отсутствуют");

                entity.Property(e => e.ГасиМеханизированныйBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_механизированный_br");

                entity.Property(e => e.ГасиМеханизированныйRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_механизированный_remont");

                entity.Property(e => e.ГасиМеханизированныйRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_механизированный_rezerv");

                entity.Property(e => e.ГасиРучнойBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_ручной_br");

                entity.Property(e => e.ГасиРучнойRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_ручной_remont");

                entity.Property(e => e.ГасиРучнойRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_ручной_rezerv");

                entity.Property(e => e.Гимс)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГИМС");

                entity.Property(e => e.ГрузовойАвтомобильBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Грузовой_автомобиль_br");

                entity.Property(e => e.ГрузовойАвтомобильRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Грузовой_автомобиль_remont");

                entity.Property(e => e.ГрузовойАвтомобильRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Грузовой_автомобиль_rezerv");

                entity.Property(e => e.ДежурныйОтГимс)
                    .HasMaxLength(255)
                    .HasColumnName("Дежурный_от_ГИМС")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ДежурныйОтГпн)
                    .HasMaxLength(255)
                    .HasColumnName("Дежурный_от_ГПН")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Диспетчер).HasColumnType("int(11)");

                entity.Property(e => e.ДиспетчерПсг)
                    .HasMaxLength(255)
                    .HasColumnName("Диспетчер_ПСГ")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ИглаBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Игла_br");

                entity.Property(e => e.ИглаRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Игла_remont");

                entity.Property(e => e.ИглаRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Игла_rezerv");

                entity.Property(e => e.КатераЛодкиBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Катера_лодки_br");

                entity.Property(e => e.КатераЛодкиRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Катера_лодки_remont");

                entity.Property(e => e.КатераЛодкиRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Катера_лодки_rezerv");

                entity.Property(e => e.КвадроциклыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Квадроциклы_br");

                entity.Property(e => e.КвадроциклыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Квадроциклы_remont");

                entity.Property(e => e.КвадроциклыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Квадроциклы_rezerv");

                entity.Property(e => e.Ко)
                    .HasColumnType("int(11)")
                    .HasColumnName("КО");

                entity.Property(e => e.КоГдзс)
                    .HasColumnType("int(11)")
                    .HasColumnName("КО_гдзс");

                entity.Property(e => e.Командировка).HasColumnType("int(11)");

                entity.Property(e => e.КпBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("кп_br");

                entity.Property(e => e.КпRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("кп_remont");

                entity.Property(e => e.КпRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("кп_rezerv");

                entity.Property(e => e.Крпсс)
                    .HasColumnType("int(11)")
                    .HasColumnName("КРПСС");

                entity.Property(e => e.ЛсВБр)
                    .HasColumnType("int(11)")
                    .HasColumnName("ЛС_в_БР");

                entity.Property(e => e.ЛсНаДежурстве)
                    .HasColumnType("int(11)")
                    .HasColumnName("лс_на_дежурстве");

                entity.Property(e => e.ЛсПоСписку)
                    .HasColumnType("int(11)")
                    .HasColumnName("лс_по_списку");

                entity.Property(e => e.ЛсПоШтату)
                    .HasColumnType("int(11)")
                    .HasColumnName("лс_по_штату");

                entity.Property(e => e.МаркаБпла1)
                    .HasMaxLength(63)
                    .HasColumnName("Марка__БПЛА1");

                entity.Property(e => e.МаркаБпла2)
                    .HasColumnType("int(11)")
                    .HasColumnName("Марка__БПЛА2");

                entity.Property(e => e.МаркаГасиМех)
                    .HasColumnType("int(11)")
                    .HasColumnName("Марка__ГАСИ_мех");

                entity.Property(e => e.МаркаГасиРучной)
                    .HasMaxLength(255)
                    .HasColumnName("Марка__ГАСИ_ручной");

                entity.Property(e => e.МедКомплектBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мед_комплект_br");

                entity.Property(e => e.МедКомплектRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мед_комплект_remont");

                entity.Property(e => e.МедКомплектRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мед_комплект_rezerv");

                entity.Property(e => e.МотопомпыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мотопомпы_br");

                entity.Property(e => e.МотопомпыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мотопомпы_remont");

                entity.Property(e => e.МотопомпыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мотопомпы_rezerv");

                entity.Property(e => e.Налицо).HasColumnType("int(11)");

                entity.Property(e => e.НачальникДежурнойСменыТпсг)
                    .HasMaxLength(255)
                    .HasColumnName("НачальникДежурнойСменыТПСГ")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.НачальникКараула)
                    .HasMaxLength(255)
                    .HasColumnName("Начальник_караула")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.НачальникТпсг)
                    .HasMaxLength(255)
                    .HasColumnName("НачальникТПСГ")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Нк)
                    .HasColumnType("int(11)")
                    .HasColumnName("НК");

                entity.Property(e => e.НкГдзс)
                    .HasColumnType("int(11)")
                    .HasColumnName("НК_гдзс");

                entity.Property(e => e.НожницыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Ножницы_br");

                entity.Property(e => e.НожницыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Ножницы_remont");

                entity.Property(e => e.НожницыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Ножницы_rezerv");

                entity.Property(e => e.ОперативнаяГруппаBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Оперативная_группа_br");

                entity.Property(e => e.ОперативнаяГруппаRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Оперативная_группа_remont");

                entity.Property(e => e.ОперативнаяГруппаRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Оперативная_группа_rezerv");

                entity.Property(e => e.ОперативныйДежурныйПоГарнизону)
                    .HasMaxLength(255)
                    .HasColumnName("Оперативный_дежурный_по_гарнизону")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ОтветственныйЗаСборДпо)
                    .HasMaxLength(255)
                    .HasColumnName("Ответственный_за_сбор_ДПО")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Отпуск).HasColumnType("int(11)");

                entity.Property(e => e.ПвFault)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПВ_fault");

                entity.Property(e => e.ПвTotal)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПВ_total");

                entity.Property(e => e.ПгFault)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПГ_fault");

                entity.Property(e => e.ПгTotal)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПГ_total");

                entity.Property(e => e.ПенообразовательInrezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пенообразователь_inrezerv");

                entity.Property(e => e.ПенообразовательInwork)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пенообразователь_inwork");

                entity.Property(e => e.ПлавсрBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("плавср_br");

                entity.Property(e => e.ПлавсрRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("плавср_remont");

                entity.Property(e => e.ПлавсрRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("плавср_rezerv");

                entity.Property(e => e.ПлавсредстваНаДежурстве)
                    .HasColumnType("int(11)")
                    .HasColumnName("плавсредства_на_дежурстве");

                entity.Property(e => e.ПлавсредстваПоШтату)
                    .HasColumnType("int(11)")
                    .HasColumnName("плавсредства_по_штату");

                entity.Property(e => e.ПлавсредствоBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("плавсредство_br");

                entity.Property(e => e.ПлавсредствоRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("плавсредство_remont");

                entity.Property(e => e.ПлавсредствоRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("плавсредство_rezerv");

                entity.Property(e => e.Пнк)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПНК");

                entity.Property(e => e.ПнкГдзс)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПНК_гдзс");

                entity.Property(e => e.ПнсBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПНС_br");

                entity.Property(e => e.ПнсRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПНС_remont");

                entity.Property(e => e.ПнсRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПНС_rezerv");

                entity.Property(e => e.ПоБольничному)
                    .HasColumnType("int(11)")
                    .HasColumnName("По_больничному");

                entity.Property(e => e.ПоСписку)
                    .HasColumnType("int(11)")
                    .HasColumnName("По_списку");

                entity.Property(e => e.ПовышАш)
                    .HasColumnType("int(11)")
                    .HasColumnName("повышАШ");

                entity.Property(e => e.ПовышВод)
                    .HasColumnType("int(11)")
                    .HasColumnName("повышВод");

                entity.Property(e => e.ПовышГдзс)
                    .HasColumnType("int(11)")
                    .HasColumnName("повышГДЗС");

                entity.Property(e => e.ПовышИтогоЛс)
                    .HasColumnType("int(11)")
                    .HasColumnName("повышИтогоЛС");

                entity.Property(e => e.ПовышКо)
                    .HasColumnType("int(11)")
                    .HasColumnName("повышКО");

                entity.Property(e => e.ПовышПож)
                    .HasColumnType("int(11)")
                    .HasColumnName("повышПож");

                entity.Property(e => e.Пожарные).HasColumnType("int(11)");

                entity.Property(e => e.ПожарныеГдзс)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарные_гдзс");

                entity.Property(e => e.ПожарныйКорабльBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарный_корабль_br");

                entity.Property(e => e.ПожарныйКорабльRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарный_корабль_remont");

                entity.Property(e => e.ПожарныйКорабльRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарный_корабль_rezerv");

                entity.Property(e => e.ПожарныйПоездBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарный_поезд_br");

                entity.Property(e => e.ПожарныйПоездRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарный_поезд_remont");

                entity.Property(e => e.ПожарныйПоездRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарный_поезд_rezerv");

                entity.Property(e => e.Посписку1)
                    .HasColumnType("int(11)")
                    .HasColumnName("посписку1");

                entity.Property(e => e.Посписку2)
                    .HasColumnType("int(11)")
                    .HasColumnName("посписку2");

                entity.Property(e => e.Посписку3)
                    .HasColumnType("int(11)")
                    .HasColumnName("посписку3");

                entity.Property(e => e.Посписку4)
                    .HasColumnType("int(11)")
                    .HasColumnName("посписку4");

                entity.Property(e => e.ПпFault)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПП_fault");

                entity.Property(e => e.ПпTotal)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПП_total");

                entity.Property(e => e.ПриспособленныеДляПеревозкиОвBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Приспособленные_для_перевозки_ОВ_br");

                entity.Property(e => e.ПриспособленныеДляПеревозкиОвRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Приспособленные_для_перевозки_ОВ_remont");

                entity.Property(e => e.ПриспособленныеДляПеревозкиОвRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Приспособленные_для_перевозки_ОВ_rezerv");

                entity.Property(e => e.Прочее).HasColumnType("int(11)");

                entity.Property(e => e.ПсаBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПСА_br");

                entity.Property(e => e.ПсаRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПСА_remont");

                entity.Property(e => e.ПсаRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПСА_rezerv");

                entity.Property(e => e.РазжимBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Разжим_br");

                entity.Property(e => e.РазжимRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Разжим_remont");

                entity.Property(e => e.РазжимRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Разжим_rezerv");

                entity.Property(e => e.РанцевыеОгнетушителиBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Ранцевые_огнетушители_br");

                entity.Property(e => e.РанцевыеОгнетушителиRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Ранцевые_огнетушители_remont");

                entity.Property(e => e.РанцевыеОгнетушителиRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Ранцевые_огнетушители_rezerv");

                entity.Property(e => e.РуководительСменыТпсг)
                    .HasMaxLength(255)
                    .HasColumnName("РуководительСменыТПСГ")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.СвпBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("СВП_br");

                entity.Property(e => e.СвпRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("СВП_remont");

                entity.Property(e => e.СвпRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("СВП_rezerv");

                entity.Property(e => e.СнегоходыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Снегоходы_br");

                entity.Property(e => e.СнегоходыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Снегоходы_remont");

                entity.Property(e => e.СнегоходыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Снегоходы_rezerv");

                entity.Property(e => e.СпецтехникаНаДежурстве)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("спецтехника_на_дежурстве");

                entity.Property(e => e.СпецтехникаПоШтату)
                    .HasColumnType("int(11)")
                    .HasColumnName("спецтехника_по_штату");

                entity.Property(e => e.СтаршийПомошникТпсг)
                    .HasMaxLength(255)
                    .HasColumnName("СтаршийПомошникТПСГ")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Таск)
                    .HasColumnType("int(11)")
                    .HasColumnName("ТАСК");

                entity.Property(e => e.ТехникаВсегоНаДежурстве)
                    .HasColumnType("int(11)")
                    .HasColumnName("техника_всего_на_дежурстве");

                entity.Property(e => e.ТехникаВсегоПоШтату)
                    .HasColumnType("int(11)")
                    .HasColumnName("техника_всего_по_штату");

                entity.Property(e => e.Ток)
                    .HasColumnType("int(11)")
                    .HasColumnName("ТОК");

                entity.Property(e => e.УксBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("УКС_br");

                entity.Property(e => e.УксRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("УКС_remont");

                entity.Property(e => e.УксRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("УКС_rezerv");

                entity.HasOne(d => d.ParentNavigation)
                    .WithMany(p => p.InverseParentNavigation)
                    .HasForeignKey(d => d.Parent)
                    .HasConstraintName("FK_psgdata_parent");
            });

            modelBuilder.Entity<Psgparam>(entity =>
            {
                entity.ToTable("psgparams");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.SubdivisionId)
                    .HasColumnType("int(11)")
                    .HasColumnName("subdivision_id");

                entity.Property(e => e.ПовышАш)
                    .HasColumnType("int(11)")
                    .HasColumnName("повышАШ");

                entity.Property(e => e.ПовышВод)
                    .HasColumnType("int(11)")
                    .HasColumnName("повышВод");

                entity.Property(e => e.ПовышГдзс)
                    .HasColumnType("int(11)")
                    .HasColumnName("повышГДЗС");

                entity.Property(e => e.ПовышКо)
                    .HasColumnType("int(11)")
                    .HasColumnName("повышКО");

                entity.Property(e => e.ПовышПож)
                    .HasColumnType("int(11)")
                    .HasColumnName("повышПож");

                entity.Property(e => e.Посписку)
                    .HasColumnType("int(11)")
                    .HasColumnName("посписку");
            });

            modelBuilder.Entity<Psostatistic>(entity =>
            {
                entity.ToTable("psostatistics");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.IdПсг)
                    .HasColumnType("int(11)")
                    .HasColumnName("idПСГ");

                entity.Property(e => e.IdПсо)
                    .HasColumnType("int(11)")
                    .HasColumnName("idПСО");

                entity.Property(e => e.ВремяИзменения)
                    .HasColumnType("timestamp")
                    .HasColumnName("времяИзменения")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ВсегоОрганизаций)
                    .HasColumnType("int(11)")
                    .HasColumnName("всегоОрганизаций");

                entity.Property(e => e.ЗаполненоСрокЗаДень)
                    .HasColumnType("int(11)")
                    .HasColumnName("заполненоСрокЗаДень");

                entity.Property(e => e.ЗаполненоСрокЗаМесяц)
                    .HasColumnType("int(11)")
                    .HasColumnName("заполненоСрокЗаМесяц");

                entity.Property(e => e.ЗаполненоСрокЗаНеделю)
                    .HasColumnType("int(11)")
                    .HasColumnName("заполненоСрокЗаНеделю");

                entity.Property(e => e.КаталоговЗаДень)
                    .HasColumnType("int(11)")
                    .HasColumnName("каталоговЗаДень");

                entity.Property(e => e.КаталоговЗаМесяц)
                    .HasColumnType("int(11)")
                    .HasColumnName("каталоговЗаМесяц");

                entity.Property(e => e.КаталоговЗаНеделю)
                    .HasColumnType("int(11)")
                    .HasColumnName("каталоговЗаНеделю");

                entity.Property(e => e.Комментарий)
                    .HasMaxLength(255)
                    .HasDefaultValueSql("''");

                entity.Property(e => e.НаДату)
                    .HasColumnType("date")
                    .HasColumnName("наДату");

                entity.Property(e => e.ОсталосьЗаГод).HasColumnType("int(11)");

                entity.Property(e => e.ОсталосьЗаМесяц).HasColumnType("int(11)");

                entity.Property(e => e.Просрочено)
                    .HasColumnType("int(11)")
                    .HasColumnName("просрочено");

                entity.Property(e => e.Псг)
                    .HasMaxLength(255)
                    .HasColumnName("ПСГ");

                entity.Property(e => e.Псо)
                    .HasMaxLength(255)
                    .HasColumnName("ПСО");

                entity.Property(e => e.СКаталогом)
                    .HasColumnType("int(11)")
                    .HasColumnName("сКаталогом");

                entity.Property(e => e.СделаноЗаМесяц)
                    .HasColumnType("int(11)")
                    .HasColumnName("сделаноЗаМесяц");

                entity.Property(e => e.СделаноЗаНеделю)
                    .HasColumnType("int(11)")
                    .HasColumnName("сделаноЗаНеделю");

                entity.Property(e => e.СделаноСначалаГода)
                    .HasColumnType("int(11)")
                    .HasColumnName("сделаноСначалаГода");

                entity.Property(e => e.СоСроком)
                    .HasColumnType("int(11)")
                    .HasColumnName("соСроком");
            });

            modelBuilder.Entity<Report3gu>(entity =>
            {
                entity.ToTable("report3gu");

                entity.HasIndex(e => e.GarnizoneId, "FK_report3gu_garnizone_id");

                entity.HasIndex(e => e.PsgId, "FK_report3gu_psg_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Dataerr)
                    .HasColumnType("int(11)")
                    .HasColumnName("dataerr");

                entity.Property(e => e.Garnizon)
                    .HasMaxLength(127)
                    .HasColumnName("garnizon");

                entity.Property(e => e.GarnizoneId)
                    .HasColumnType("int(11)")
                    .HasColumnName("garnizone_id");

                entity.Property(e => e.Karaul)
                    .HasColumnType("int(11)")
                    .HasColumnName("karaul");

                entity.Property(e => e.Mdate)
                    .HasColumnType("timestamp")
                    .HasColumnName("mdate")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Mtype)
                    .HasColumnType("int(11)")
                    .HasColumnName("mtype")
                    .HasComment("подразделение, местный, территориальный");

                entity.Property(e => e.PsgId)
                    .HasColumnType("int(11)")
                    .HasColumnName("psg_id");

                entity.Property(e => e.PsgName)
                    .HasMaxLength(63)
                    .HasColumnName("psg_name");

                entity.Property(e => e.SizodsBazaGdzs)
                    .HasColumnType("int(11)")
                    .HasColumnName("sizods_baza_gdzs");

                entity.Property(e => e.SizodsMname)
                    .HasMaxLength(255)
                    .HasColumnName("sizods_mname");

                entity.Property(e => e.SizodsPostGdzs)
                    .HasColumnType("int(11)")
                    .HasColumnName("sizods_post_gdzs");

                entity.Property(e => e.SizodsRaschet)
                    .HasColumnType("int(11)")
                    .HasColumnName("sizods_raschet");

                entity.Property(e => e.SizodsRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("sizods_rezerv");

                entity.Property(e => e.Ав1Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ__1__br");

                entity.Property(e => e.Ав1Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ__1__remont");

                entity.Property(e => e.Ав1Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ__1__rezerv");

                entity.Property(e => e.АвBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ_br");

                entity.Property(e => e.АвRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ_remont");

                entity.Property(e => e.АвRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ_rezerv");

                entity.Property(e => e.АгдзсBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АГДЗС_br");

                entity.Property(e => e.АгдзсRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АГДЗС_remont");

                entity.Property(e => e.АгдзсRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АГДЗС_rezerv");

                entity.Property(e => e.АкпBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АКП_br");

                entity.Property(e => e.АкпRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АКП_remont");

                entity.Property(e => e.АкпRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АКП_rezerv");

                entity.Property(e => e.Ал30Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ-30_br");

                entity.Property(e => e.Ал30Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ-30_remont");

                entity.Property(e => e.Ал30Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ-30_rezerv");

                entity.Property(e => e.Ал50Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ-50_br");

                entity.Property(e => e.Ал50Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ-50_remont");

                entity.Property(e => e.Ал50Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ-50_rezerv");

                entity.Property(e => e.АмпBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АМП_br");

                entity.Property(e => e.АмпRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АМП_remont");

                entity.Property(e => e.АмпRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АМП_rezerv");

                entity.Property(e => e.АнрBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АНР_br");

                entity.Property(e => e.АнрRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АНР_remont");

                entity.Property(e => e.АнрRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АНР_rezerv");

                entity.Property(e => e.АппBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АПП_br");

                entity.Property(e => e.АппRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АПП_remont");

                entity.Property(e => e.АппRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АПП_rezerv");

                entity.Property(e => e.АрBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АР_br");

                entity.Property(e => e.АрRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АР_remont");

                entity.Property(e => e.АрRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АР_rezerv");

                entity.Property(e => e.Арс14Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("АРС-14_br");

                entity.Property(e => e.Арс14Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АРС-14_remont");

                entity.Property(e => e.Арс14Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АРС-14_rezerv");

                entity.Property(e => e.АсаBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСА_br");

                entity.Property(e => e.АсаRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСА_remont");

                entity.Property(e => e.АсаRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСА_rezerv");

                entity.Property(e => e.АсмBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСМ_br");

                entity.Property(e => e.АсмRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСМ_remont");

                entity.Property(e => e.АсмRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСМ_rezerv");

                entity.Property(e => e.АцBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦ_br");

                entity.Property(e => e.АцRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦ_remont");

                entity.Property(e => e.АцRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦ_rezerv");

                entity.Property(e => e.АцлBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦЛ_br");

                entity.Property(e => e.АцлRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦЛ_remont");

                entity.Property(e => e.АцлRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦЛ_rezerv");

                entity.Property(e => e.БензопилыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензопилы_br");

                entity.Property(e => e.БензопилыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензопилы_remont");

                entity.Property(e => e.БензопилыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензопилы_rezerv");

                entity.Property(e => e.БензорезыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензорезы_br");

                entity.Property(e => e.БензорезыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензорезы_remont");

                entity.Property(e => e.БензорезыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензорезы_rezerv");

                entity.Property(e => e.Бпла1Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА1_br");

                entity.Property(e => e.Бпла1Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА1_remont");

                entity.Property(e => e.Бпла1Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА1_rezerv");

                entity.Property(e => e.Бпла2Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА2_br");

                entity.Property(e => e.Бпла2Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА2_remont");

                entity.Property(e => e.Бпла2Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА2_rezerv");

                entity.Property(e => e.Водители2)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водители_2");

                entity.Property(e => e.Водители3)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водители_3");

                entity.Property(e => e.ВодолазноеСнаряжениеBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_br");

                entity.Property(e => e.ВодолазноеСнаряжениеRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_remont");

                entity.Property(e => e.ВодолазноеСнаряжениеRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_rezerv");

                entity.Property(e => e.ВодолазноеСнаряжениеКомплектBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_комплект_br");

                entity.Property(e => e.ВодолазноеСнаряжениеКомплектRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_комплект_remont");

                entity.Property(e => e.ВодолазноеСнаряжениеКомплектRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_комплект_rezerv");

                entity.Property(e => e.Водолазы2)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазы_2");

                entity.Property(e => e.Всего2)
                    .HasColumnType("int(11)")
                    .HasColumnName("Всего_2");

                entity.Property(e => e.Всего4)
                    .HasColumnType("int(11)")
                    .HasColumnName("Всего_4");

                entity.Property(e => e.ГасиМеханизированныйBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_механизированный_br");

                entity.Property(e => e.ГасиМеханизированныйRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_механизированный_remont");

                entity.Property(e => e.ГасиМеханизированныйRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_механизированный_rezerv");

                entity.Property(e => e.ГасиРучнойBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_ручной_br");

                entity.Property(e => e.ГасиРучнойRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_ручной_remont");

                entity.Property(e => e.ГасиРучнойRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_ручной_rezerv");

                entity.Property(e => e.Гимс2)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГИМС_2");

                entity.Property(e => e.ГрузовойАвтомобильBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Грузовой_автомобиль_br");

                entity.Property(e => e.ГрузовойАвтомобильRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Грузовой_автомобиль_remont");

                entity.Property(e => e.ГрузовойАвтомобильRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Грузовой_автомобиль_rezerv");

                entity.Property(e => e.ДежурныйОтГимс)
                    .HasMaxLength(255)
                    .HasColumnName("Дежурный_от_ГИМС");

                entity.Property(e => e.ДежурныйОтГпн)
                    .HasMaxLength(255)
                    .HasColumnName("Дежурный_от_ГПН");

                entity.Property(e => e.Диспетчер2)
                    .HasColumnType("int(11)")
                    .HasColumnName("Диспетчер_2");

                entity.Property(e => e.ДиспетчерЕсс01)
                    .HasMaxLength(255)
                    .HasColumnName("Диспетчер_ЕСС_01");

                entity.Property(e => e.ИглаBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Игла_br");

                entity.Property(e => e.ИглаRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Игла_remont");

                entity.Property(e => e.ИглаRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Игла_rezerv");

                entity.Property(e => e.КатераЛодкиBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Катера_лодки_br");

                entity.Property(e => e.КатераЛодкиRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Катера_лодки_remont");

                entity.Property(e => e.КатераЛодкиRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Катера_лодки_rezerv");

                entity.Property(e => e.КвадроциклыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Квадроциклы_br");

                entity.Property(e => e.КвадроциклыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Квадроциклы_remont");

                entity.Property(e => e.КвадроциклыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Квадроциклы_rezerv");

                entity.Property(e => e.Ко2)
                    .HasColumnType("int(11)")
                    .HasColumnName("КО_2");

                entity.Property(e => e.Ко3)
                    .HasColumnType("int(11)")
                    .HasColumnName("КО_3");

                entity.Property(e => e.Командировка4)
                    .HasColumnType("int(11)")
                    .HasColumnName("Командировка_4");

                entity.Property(e => e.Крпсс2)
                    .HasColumnType("int(11)")
                    .HasColumnName("КРПСС_2");

                entity.Property(e => e.ЛсВБр2)
                    .HasColumnType("int(11)")
                    .HasColumnName("ЛС_в_БР_2");

                entity.Property(e => e.МаркаБпла1)
                    .HasMaxLength(63)
                    .HasColumnName("Марка__БПЛА1");

                entity.Property(e => e.МаркаБпла2)
                    .HasColumnType("int(11)")
                    .HasColumnName("Марка__БПЛА2");

                entity.Property(e => e.МаркаГасиМех)
                    .HasColumnType("int(11)")
                    .HasColumnName("Марка__ГАСИ_мех");

                entity.Property(e => e.МаркаГасиРучной)
                    .HasMaxLength(255)
                    .HasColumnName("Марка__ГАСИ_ручной");

                entity.Property(e => e.МедКомплектBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мед_комплект_br");

                entity.Property(e => e.МедКомплектRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мед_комплект_remont");

                entity.Property(e => e.МедКомплектRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мед_комплект_rezerv");

                entity.Property(e => e.МотопомпыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мотопомпы_br");

                entity.Property(e => e.МотопомпыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мотопомпы_remont");

                entity.Property(e => e.МотопомпыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мотопомпы_rezerv");

                entity.Property(e => e.Налицо1)
                    .HasColumnType("int(11)")
                    .HasColumnName("Налицо_1");

                entity.Property(e => e.НачальникДежурнойСменыТпсг)
                    .HasMaxLength(255)
                    .HasColumnName("НачальникДежурнойСменыТПСГ")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.НачальникКараула)
                    .HasMaxLength(255)
                    .HasColumnName("Начальник_караула");

                entity.Property(e => e.НачальникТпсг)
                    .HasMaxLength(255)
                    .HasColumnName("НачальникТПСГ")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Нк2)
                    .HasColumnType("int(11)")
                    .HasColumnName("НК_2");

                entity.Property(e => e.Нк3)
                    .HasColumnType("int(11)")
                    .HasColumnName("НК_3");

                entity.Property(e => e.ОперативнаяГруппаBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Оперативная_группа_br");

                entity.Property(e => e.ОперативнаяГруппаRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Оперативная_группа_remont");

                entity.Property(e => e.ОперативнаяГруппаRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Оперативная_группа_rezerv");

                entity.Property(e => e.ОперативныйДежурныйПоГарнизону)
                    .HasMaxLength(255)
                    .HasColumnName("Оперативный_дежурный_по_гарнизону");

                entity.Property(e => e.ОтветственныйЗаСборДпо)
                    .HasMaxLength(255)
                    .HasColumnName("Ответственный_за_сбор_ДПО");

                entity.Property(e => e.Отпуск4)
                    .HasColumnType("int(11)")
                    .HasColumnName("Отпуск_4");

                entity.Property(e => e.ПвFault)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПВ_fault");

                entity.Property(e => e.ПвTotal)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПВ_total");

                entity.Property(e => e.ПгFault)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПГ_fault");

                entity.Property(e => e.ПгTotal)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПГ_total");

                entity.Property(e => e.ПенообразовательInrezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пенообразователь_inrezerv");

                entity.Property(e => e.ПенообразовательInwork)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пенообразователь_inwork");

                entity.Property(e => e.Пнк2)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПНК_2");

                entity.Property(e => e.Пнк3)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПНК_3");

                entity.Property(e => e.ПнсBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПНС_br");

                entity.Property(e => e.ПнсRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПНС_remont");

                entity.Property(e => e.ПнсRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПНС_rezerv");

                entity.Property(e => e.ПоБольничному4)
                    .HasColumnType("int(11)")
                    .HasColumnName("По_больничному_4");

                entity.Property(e => e.ПоСписку1)
                    .HasColumnType("int(11)")
                    .HasColumnName("По_списку_1");

                entity.Property(e => e.Пожарные2)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарные_2");

                entity.Property(e => e.Пожарные3)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарные_3");

                entity.Property(e => e.ПожарныйПоездBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарный_поезд_br");

                entity.Property(e => e.ПожарныйПоездRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарный_поезд_remont");

                entity.Property(e => e.ПожарныйПоездRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарный_поезд_rezerv");

                entity.Property(e => e.ПпFault)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПП_fault");

                entity.Property(e => e.ПпTotal)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПП_total");

                entity.Property(e => e.ПриспособленныеДляПеревозкиОвBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Приспособленные_для_перевозки_ОВ_br");

                entity.Property(e => e.ПриспособленныеДляПеревозкиОвRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Приспособленные_для_перевозки_ОВ_remont");

                entity.Property(e => e.ПриспособленныеДляПеревозкиОвRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Приспособленные_для_перевозки_ОВ_rezerv");

                entity.Property(e => e.Прочее4)
                    .HasColumnType("int(11)")
                    .HasColumnName("Прочее_4");

                entity.Property(e => e.ПсаBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПСА_br");

                entity.Property(e => e.ПсаRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПСА_remont");

                entity.Property(e => e.ПсаRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПСА_rezerv");

                entity.Property(e => e.РанцевыеОгнетушителиBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Ранцевые_огнетушители_br");

                entity.Property(e => e.РанцевыеОгнетушителиRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Ранцевые_огнетушители_remont");

                entity.Property(e => e.РанцевыеОгнетушителиRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Ранцевые_огнетушители_rezerv");

                entity.Property(e => e.РуководительСменыТпсг)
                    .HasMaxLength(255)
                    .HasColumnName("РуководительСменыТПСГ");

                entity.Property(e => e.СвпBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("СВП_br");

                entity.Property(e => e.СвпRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("СВП_remont");

                entity.Property(e => e.СвпRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("СВП_rezerv");

                entity.Property(e => e.СнегоходыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Снегоходы_br");

                entity.Property(e => e.СнегоходыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Снегоходы_remont");

                entity.Property(e => e.СнегоходыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Снегоходы_rezerv");

                entity.Property(e => e.СтаршийПомошникТпсг)
                    .HasMaxLength(255)
                    .HasColumnName("СтаршийПомошникТПСГ")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Таск)
                    .HasColumnType("int(11)")
                    .HasColumnName("ТАСК");

                entity.Property(e => e.Ток)
                    .HasColumnType("int(11)")
                    .HasColumnName("ТОК");

                entity.Property(e => e.УксBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("УКС_br");

                entity.Property(e => e.УксRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("УКС_remont");

                entity.Property(e => e.УксRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("УКС_rezerv");

                entity.HasOne(d => d.Garnizone)
                    .WithMany(p => p.Report3guGarnizones)
                    .HasForeignKey(d => d.GarnizoneId)
                    .HasConstraintName("FK_report3gu_garnizone_id");

                entity.HasOne(d => d.Psg)
                    .WithMany(p => p.Report3guPsgs)
                    .HasForeignKey(d => d.PsgId)
                    .HasConstraintName("FK_report3gu_psg_id");
            });

            modelBuilder.Entity<Reportstroevka>(entity =>
            {
                entity.ToTable("reportstroevka");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.ForControl)
                    .HasMaxLength(511)
                    .HasColumnName("forControl")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Garnizon)
                    .HasMaxLength(127)
                    .HasColumnName("garnizon");

                entity.Property(e => e.GarnizoneId)
                    .HasColumnType("int(11)")
                    .HasColumnName("garnizone_id");

                entity.Property(e => e.Karaul)
                    .HasColumnType("int(11)")
                    .HasColumnName("karaul");

                entity.Property(e => e.Mdate)
                    .HasColumnType("timestamp")
                    .HasColumnName("mdate")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Mtype)
                    .HasColumnType("int(11)")
                    .HasColumnName("mtype")
                    .HasComment("подразделение, местный, территориальный");

                entity.Property(e => e.PsgId)
                    .HasColumnType("int(11)")
                    .HasColumnName("psg_id");

                entity.Property(e => e.PsgName)
                    .HasMaxLength(63)
                    .HasColumnName("psg_name");

                entity.Property(e => e.SizodsBazaGdzs)
                    .HasColumnType("int(11)")
                    .HasColumnName("sizods_baza_gdzs");

                entity.Property(e => e.SizodsMname)
                    .HasMaxLength(255)
                    .HasColumnName("sizods_mname");

                entity.Property(e => e.SizodsPostGdzs)
                    .HasColumnType("int(11)")
                    .HasColumnName("sizods_post_gdzs");

                entity.Property(e => e.SizodsRaschet)
                    .HasColumnType("int(11)")
                    .HasColumnName("sizods_raschet");

                entity.Property(e => e.SizodsRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("sizods_rezerv");

                entity.Property(e => e.Ав1Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ__1__br");

                entity.Property(e => e.Ав1Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ__1__remont");

                entity.Property(e => e.Ав1Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ__1__rezerv");

                entity.Property(e => e.АвBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ_br");

                entity.Property(e => e.АвRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ_remont");

                entity.Property(e => e.АвRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АВ_rezerv");

                entity.Property(e => e.АгдзсBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АГДЗС_br");

                entity.Property(e => e.АгдзсRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АГДЗС_remont");

                entity.Property(e => e.АгдзсRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АГДЗС_rezerv");

                entity.Property(e => e.АкпBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АКП_br");

                entity.Property(e => e.АкпRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АКП_remont");

                entity.Property(e => e.АкпRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АКП_rezerv");

                entity.Property(e => e.Ал30Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ-30_br");

                entity.Property(e => e.Ал30Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ-30_remont");

                entity.Property(e => e.Ал30Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ-30_rezerv");

                entity.Property(e => e.Ал50Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ-50_br");

                entity.Property(e => e.Ал50Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ-50_remont");

                entity.Property(e => e.Ал50Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЛ-50_rezerv");

                entity.Property(e => e.АмпBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АМП_br");

                entity.Property(e => e.АмпRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АМП_remont");

                entity.Property(e => e.АмпRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АМП_rezerv");

                entity.Property(e => e.АнрBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АНР_br");

                entity.Property(e => e.АнрRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АНР_remont");

                entity.Property(e => e.АнрRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АНР_rezerv");

                entity.Property(e => e.АппBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АПП_br");

                entity.Property(e => e.АппRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АПП_remont");

                entity.Property(e => e.АппRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АПП_rezerv");

                entity.Property(e => e.АрBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АР_br");

                entity.Property(e => e.АрRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АР_remont");

                entity.Property(e => e.АрRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АР_rezerv");

                entity.Property(e => e.Арс14Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("АРС-14_br");

                entity.Property(e => e.Арс14Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АРС-14_remont");

                entity.Property(e => e.Арс14Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АРС-14_rezerv");

                entity.Property(e => e.АсаBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСА_br");

                entity.Property(e => e.АсаRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСА_remont");

                entity.Property(e => e.АсаRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСА_rezerv");

                entity.Property(e => e.АсмBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСМ_br");

                entity.Property(e => e.АсмRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСМ_remont");

                entity.Property(e => e.АсмRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АСМ_rezerv");

                entity.Property(e => e.АцBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦ_br");

                entity.Property(e => e.АцRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦ_remont");

                entity.Property(e => e.АцRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦ_rezerv");

                entity.Property(e => e.АцлBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦЛ_br");

                entity.Property(e => e.АцлRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦЛ_remont");

                entity.Property(e => e.АцлRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("АЦЛ_rezerv");

                entity.Property(e => e.БензопилыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензопилы_br");

                entity.Property(e => e.БензопилыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензопилы_remont");

                entity.Property(e => e.БензопилыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензопилы_rezerv");

                entity.Property(e => e.БензорезыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензорезы_br");

                entity.Property(e => e.БензорезыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензорезы_remont");

                entity.Property(e => e.БензорезыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Бензорезы_rezerv");

                entity.Property(e => e.Бпла1Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА1_br");

                entity.Property(e => e.Бпла1Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА1_remont");

                entity.Property(e => e.Бпла1Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА1_rezerv");

                entity.Property(e => e.Бпла2Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА2_br");

                entity.Property(e => e.Бпла2Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА2_remont");

                entity.Property(e => e.Бпла2Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("БПЛА2_rezerv");

                entity.Property(e => e.Водители2)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водители_2");

                entity.Property(e => e.Водители3)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водители_3");

                entity.Property(e => e.ВодолазноеСнаряжениеBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_br");

                entity.Property(e => e.ВодолазноеСнаряжениеRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_remont");

                entity.Property(e => e.ВодолазноеСнаряжениеRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_rezerv");

                entity.Property(e => e.ВодолазноеСнаряжениеКомплектBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_комплект_br");

                entity.Property(e => e.ВодолазноеСнаряжениеКомплектRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_комплект_remont");

                entity.Property(e => e.ВодолазноеСнаряжениеКомплектRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазное_снаряжение_комплект_rezerv");

                entity.Property(e => e.Водолазы2)
                    .HasColumnType("int(11)")
                    .HasColumnName("Водолазы_2");

                entity.Property(e => e.Всего2)
                    .HasColumnType("int(11)")
                    .HasColumnName("Всего_2");

                entity.Property(e => e.Всего4)
                    .HasColumnType("int(11)")
                    .HasColumnName("Всего_4");

                entity.Property(e => e.ГасиМеханизированныйBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_механизированный_br");

                entity.Property(e => e.ГасиМеханизированныйRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_механизированный_remont");

                entity.Property(e => e.ГасиМеханизированныйRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_механизированный_rezerv");

                entity.Property(e => e.ГасиРучнойBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_ручной_br");

                entity.Property(e => e.ГасиРучнойRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_ручной_remont");

                entity.Property(e => e.ГасиРучнойRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГАСИ_ручной_rezerv");

                entity.Property(e => e.Гимс2)
                    .HasColumnType("int(11)")
                    .HasColumnName("ГИМС_2");

                entity.Property(e => e.ГрузовойАвтомобильBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Грузовой_автомобиль_br");

                entity.Property(e => e.ГрузовойАвтомобильRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Грузовой_автомобиль_remont");

                entity.Property(e => e.ГрузовойАвтомобильRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Грузовой_автомобиль_rezerv");

                entity.Property(e => e.ДежурныйОтГимс)
                    .HasMaxLength(255)
                    .HasColumnName("Дежурный_от_ГИМС");

                entity.Property(e => e.ДежурныйОтГпн)
                    .HasMaxLength(255)
                    .HasColumnName("Дежурный_от_ГПН");

                entity.Property(e => e.Диспетчер2)
                    .HasColumnType("int(11)")
                    .HasColumnName("Диспетчер_2");

                entity.Property(e => e.ДиспетчерЕсс01)
                    .HasMaxLength(255)
                    .HasColumnName("Диспетчер_ЕСС_01");

                entity.Property(e => e.ИглаBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Игла_br");

                entity.Property(e => e.ИглаRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Игла_remont");

                entity.Property(e => e.ИглаRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Игла_rezerv");

                entity.Property(e => e.КатераЛодкиBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Катера_лодки_br");

                entity.Property(e => e.КатераЛодкиRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Катера_лодки_remont");

                entity.Property(e => e.КатераЛодкиRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Катера_лодки_rezerv");

                entity.Property(e => e.КвадроциклыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Квадроциклы_br");

                entity.Property(e => e.КвадроциклыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Квадроциклы_remont");

                entity.Property(e => e.КвадроциклыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Квадроциклы_rezerv");

                entity.Property(e => e.Ко2)
                    .HasColumnType("int(11)")
                    .HasColumnName("КО_2");

                entity.Property(e => e.Ко3)
                    .HasColumnType("int(11)")
                    .HasColumnName("КО_3");

                entity.Property(e => e.Командировка4)
                    .HasColumnType("int(11)")
                    .HasColumnName("Командировка_4");

                entity.Property(e => e.Крпсс2)
                    .HasColumnType("int(11)")
                    .HasColumnName("КРПСС_2");

                entity.Property(e => e.ЛсВБр2)
                    .HasColumnType("int(11)")
                    .HasColumnName("ЛС_в_БР_2");

                entity.Property(e => e.МаркаБпла1)
                    .HasMaxLength(63)
                    .HasColumnName("Марка__БПЛА1");

                entity.Property(e => e.МаркаБпла2)
                    .HasColumnType("int(11)")
                    .HasColumnName("Марка__БПЛА2");

                entity.Property(e => e.МаркаГасиМех)
                    .HasColumnType("int(11)")
                    .HasColumnName("Марка__ГАСИ_мех");

                entity.Property(e => e.МаркаГасиРучной)
                    .HasMaxLength(255)
                    .HasColumnName("Марка__ГАСИ_ручной");

                entity.Property(e => e.МедКомплектBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мед_комплект_br");

                entity.Property(e => e.МедКомплектRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мед_комплект_remont");

                entity.Property(e => e.МедКомплектRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мед_комплект_rezerv");

                entity.Property(e => e.МотопомпыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мотопомпы_br");

                entity.Property(e => e.МотопомпыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мотопомпы_remont");

                entity.Property(e => e.МотопомпыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Мотопомпы_rezerv");

                entity.Property(e => e.Налицо1)
                    .HasColumnType("int(11)")
                    .HasColumnName("Налицо_1");

                entity.Property(e => e.НачальникКараула)
                    .HasMaxLength(255)
                    .HasColumnName("Начальник_караула");

                entity.Property(e => e.Нк2)
                    .HasColumnType("int(11)")
                    .HasColumnName("НК_2");

                entity.Property(e => e.Нк3)
                    .HasColumnType("int(11)")
                    .HasColumnName("НК_3");

                entity.Property(e => e.ОперативнаяГруппаBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Оперативная_группа_br");

                entity.Property(e => e.ОперативнаяГруппаRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Оперативная_группа_remont");

                entity.Property(e => e.ОперативнаяГруппаRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Оперативная_группа_rezerv");

                entity.Property(e => e.ОперативныйДежурныйПоГарнизону)
                    .HasMaxLength(255)
                    .HasColumnName("Оперативный_дежурный_по_гарнизону");

                entity.Property(e => e.ОтветственныйЗаСборДпо)
                    .HasMaxLength(255)
                    .HasColumnName("Ответственный_за_сбор_ДПО");

                entity.Property(e => e.Отпуск4)
                    .HasColumnType("int(11)")
                    .HasColumnName("Отпуск_4");

                entity.Property(e => e.ПвFault)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПВ_fault");

                entity.Property(e => e.ПвTotal)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПВ_total");

                entity.Property(e => e.ПгFault)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПГ_fault");

                entity.Property(e => e.ПгTotal)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПГ_total");

                entity.Property(e => e.ПенообразовательInrezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пенообразователь_inrezerv");

                entity.Property(e => e.ПенообразовательInwork)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пенообразователь_inwork");

                entity.Property(e => e.Пнк2)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПНК_2");

                entity.Property(e => e.Пнк3)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПНК_3");

                entity.Property(e => e.ПнсBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПНС_br");

                entity.Property(e => e.ПнсRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПНС_remont");

                entity.Property(e => e.ПнсRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПНС_rezerv");

                entity.Property(e => e.ПоБольничному4)
                    .HasColumnType("int(11)")
                    .HasColumnName("По_больничному_4");

                entity.Property(e => e.ПоСписку1)
                    .HasColumnType("int(11)")
                    .HasColumnName("По_списку_1");

                entity.Property(e => e.Пожарные2)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарные_2");

                entity.Property(e => e.Пожарные3)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарные_3");

                entity.Property(e => e.ПожарныйПоездBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарный_поезд_br");

                entity.Property(e => e.ПожарныйПоездRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарный_поезд_remont");

                entity.Property(e => e.ПожарныйПоездRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Пожарный_поезд_rezerv");

                entity.Property(e => e.ПпFault)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПП_fault");

                entity.Property(e => e.ПпTotal)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПП_total");

                entity.Property(e => e.ПриспособленныеДляПеревозкиОвBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Приспособленные_для_перевозки_ОВ_br");

                entity.Property(e => e.ПриспособленныеДляПеревозкиОвRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Приспособленные_для_перевозки_ОВ_remont");

                entity.Property(e => e.ПриспособленныеДляПеревозкиОвRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Приспособленные_для_перевозки_ОВ_rezerv");

                entity.Property(e => e.Прочее4)
                    .HasColumnType("int(11)")
                    .HasColumnName("Прочее_4");

                entity.Property(e => e.ПсаBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПСА_br");

                entity.Property(e => e.ПсаRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПСА_remont");

                entity.Property(e => e.ПсаRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("ПСА_rezerv");

                entity.Property(e => e.РанцевыеОгнетушителиBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Ранцевые_огнетушители_br");

                entity.Property(e => e.РанцевыеОгнетушителиRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Ранцевые_огнетушители_remont");

                entity.Property(e => e.РанцевыеОгнетушителиRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Ранцевые_огнетушители_rezerv");

                entity.Property(e => e.СвпBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("СВП_br");

                entity.Property(e => e.СвпRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("СВП_remont");

                entity.Property(e => e.СвпRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("СВП_rezerv");

                entity.Property(e => e.СнегоходыBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("Снегоходы_br");

                entity.Property(e => e.СнегоходыRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("Снегоходы_remont");

                entity.Property(e => e.СнегоходыRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("Снегоходы_rezerv");

                entity.Property(e => e.Таск)
                    .HasColumnType("int(11)")
                    .HasColumnName("ТАСК");

                entity.Property(e => e.Ток)
                    .HasColumnType("int(11)")
                    .HasColumnName("ТОК");

                entity.Property(e => e.УксBr)
                    .HasColumnType("int(11)")
                    .HasColumnName("УКС_br");

                entity.Property(e => e.УксRemont)
                    .HasColumnType("int(11)")
                    .HasColumnName("УКС_remont");

                entity.Property(e => e.УксRezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("УКС_rezerv");
            });

            modelBuilder.Entity<Sizod>(entity =>
            {
                entity.ToTable("sizod");

                entity.HasIndex(e => e.GarnizonId, "FK_sizod_garnizon_id");

                entity.HasIndex(e => e.SubdivisionId, "FK_sizod_subdivision_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.BazaGdzs)
                    .HasColumnType("int(11)")
                    .HasColumnName("baza_gdzs");

                entity.Property(e => e.EditTime)
                    .HasColumnType("timestamp")
                    .HasColumnName("edit_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Excel)
                    .HasMaxLength(255)
                    .HasColumnName("excel");

                entity.Property(e => e.GarnizonId)
                    .HasColumnType("int(11)")
                    .HasColumnName("garnizon_id");

                entity.Property(e => e.Mdate)
                    .HasColumnType("date")
                    .HasColumnName("mdate");

                entity.Property(e => e.Mname)
                    .HasMaxLength(50)
                    .HasColumnName("mname");

                entity.Property(e => e.NameGarnizone)
                    .HasMaxLength(255)
                    .HasColumnName("name_garnizone");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder");

                entity.Property(e => e.PostGdzs)
                    .HasColumnType("int(11)")
                    .HasColumnName("post_gdzs");

                entity.Property(e => e.Raschet)
                    .HasColumnType("int(11)")
                    .HasColumnName("raschet");

                entity.Property(e => e.Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("rezerv");

                entity.Property(e => e.Subdivision)
                    .HasMaxLength(255)
                    .HasColumnName("subdivision");

                entity.Property(e => e.SubdivisionId)
                    .HasColumnType("int(11)")
                    .HasColumnName("subdivision_id");

                entity.HasOne(d => d.SubdivisionNavigation)
                    .WithMany(p => p.Sizods)
                    .HasForeignKey(d => d.SubdivisionId)
                    .HasConstraintName("FK_sizod_subdivision_id");
            });

            modelBuilder.Entity<Sostav>(entity =>
            {
                entity.ToTable("sostav");

                entity.HasIndex(e => e.GarnizoneId, "FK_sostav_garnizone_id");

                entity.HasIndex(e => e.SubdivisionId, "FK_sostav_subdivision_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Count)
                    .HasColumnType("int(11)")
                    .HasColumnName("count")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.EditTime)
                    .HasColumnType("timestamp")
                    .HasColumnName("edit_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Excel)
                    .HasMaxLength(255)
                    .HasColumnName("excel");

                entity.Property(e => e.GarnizoneId)
                    .HasColumnType("int(11)")
                    .HasColumnName("garnizone_id");

                entity.Property(e => e.Mdate)
                    .HasColumnType("date")
                    .HasColumnName("mdate");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.NameFull)
                    .HasMaxLength(255)
                    .HasColumnName("name_full");

                entity.Property(e => e.NameGarnizone)
                    .HasMaxLength(255)
                    .HasColumnName("name_garnizone");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder");

                entity.Property(e => e.Parent)
                    .HasMaxLength(255)
                    .HasColumnName("parent");

                entity.Property(e => e.SostavVid)
                    .HasMaxLength(255)
                    .HasColumnName("sostav_vid");

                entity.Property(e => e.SostavVidId)
                    .HasColumnType("int(11)")
                    .HasColumnName("sostav_vid_id");

                entity.Property(e => e.Subdivision)
                    .HasMaxLength(255)
                    .HasColumnName("subdivision");

                entity.Property(e => e.SubdivisionId)
                    .HasColumnType("int(11)")
                    .HasColumnName("subdivision_id");

                entity.HasOne(d => d.SubdivisionNavigation)
                    .WithMany(p => p.Sostavs)
                    .HasForeignKey(d => d.SubdivisionId)
                    .HasConstraintName("FK_sostav_subdivision_id");
            });

            modelBuilder.Entity<SostavVid>(entity =>
            {
                entity.ToTable("sostav_vid");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Comment)
                    .HasMaxLength(255)
                    .HasColumnName("comment");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(255)
                    .HasColumnName("fullname");

                entity.Property(e => e.Group)
                    .HasMaxLength(255)
                    .HasColumnName("group");

                entity.Property(e => e.GroupId)
                    .HasColumnType("int(11)")
                    .HasColumnName("group_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Sredstva>(entity =>
            {
                entity.ToTable("sredstva");

                entity.HasIndex(e => e.GarnizonId, "FK_sredstva_garnizon_id2");

                entity.HasIndex(e => e.SubdivisionId, "FK_sredstva_subdivision_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("br")
                    .HasComment("Боевой расчет");

                entity.Property(e => e.EditTime)
                    .HasColumnType("timestamp")
                    .HasColumnName("edit_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("Время последнего изменения");

                entity.Property(e => e.Excel)
                    .HasMaxLength(255)
                    .HasColumnName("excel");

                entity.Property(e => e.GarnizonId)
                    .HasColumnType("int(11)")
                    .HasColumnName("garnizon_id");

                entity.Property(e => e.Mdate)
                    .HasColumnType("date")
                    .HasColumnName("mdate")
                    .HasComment("На какую дату");

                entity.Property(e => e.NameGarnizone)
                    .HasMaxLength(255)
                    .HasColumnName("name_garnizone");

                entity.Property(e => e.NameSredstvo)
                    .HasMaxLength(255)
                    .HasColumnName("name_sredstvo")
                    .HasComment("Наименование средства");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder");

                entity.Property(e => e.Parent)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent");

                entity.Property(e => e.Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("remont")
                    .HasComment("В ремонте");

                entity.Property(e => e.Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("rezerv")
                    .HasComment("В резерве");

                entity.Property(e => e.SredstvoId)
                    .HasColumnType("int(11)")
                    .HasColumnName("sredstvo_id");

                entity.Property(e => e.SredstvoVid)
                    .HasMaxLength(255)
                    .HasColumnName("sredstvo_vid");

                entity.Property(e => e.SredstvoVidId)
                    .HasColumnType("int(11)")
                    .HasColumnName("sredstvo_vid_id");

                entity.Property(e => e.Subdivision)
                    .HasMaxLength(255)
                    .HasColumnName("subdivision");

                entity.Property(e => e.SubdivisionId)
                    .HasColumnType("int(11)")
                    .HasColumnName("subdivision_id");

                entity.Property(e => e.Tofirst)
                    .HasColumnType("int(11)")
                    .HasColumnName("tofirst");

                entity.Property(e => e.Totow)
                    .HasColumnType("int(11)")
                    .HasColumnName("totow");

                entity.HasOne(d => d.SubdivisionNavigation)
                    .WithMany(p => p.Sredstvas)
                    .HasForeignKey(d => d.SubdivisionId)
                    .HasConstraintName("FK_sredstva_subdivision_id");
            });

            modelBuilder.Entity<SredstvaVid>(entity =>
            {
                entity.ToTable("sredstva_vid");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Comment)
                    .HasMaxLength(255)
                    .HasColumnName("comment");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(255)
                    .HasColumnName("fullname");

                entity.Property(e => e.Group)
                    .HasMaxLength(255)
                    .HasColumnName("group");

                entity.Property(e => e.GroupId)
                    .HasColumnType("int(11)")
                    .HasColumnName("group_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<SredstvaДоУдал>(entity =>
            {
                entity.ToTable("sredstva_до удал");

                entity.HasIndex(e => e.GarnizonId, "FK_sredstva_garnizon_id2");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Br)
                    .HasColumnType("int(11)")
                    .HasColumnName("br")
                    .HasComment("Боевой расчет");

                entity.Property(e => e.EditTime)
                    .HasColumnType("timestamp")
                    .HasColumnName("edit_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("Время последнего изменения");

                entity.Property(e => e.Excel)
                    .HasMaxLength(255)
                    .HasColumnName("excel");

                entity.Property(e => e.GarnizonId)
                    .HasColumnType("int(11)")
                    .HasColumnName("garnizon_id");

                entity.Property(e => e.Mdate)
                    .HasColumnType("date")
                    .HasColumnName("mdate")
                    .HasComment("На какую дату");

                entity.Property(e => e.NameGarnizone)
                    .HasMaxLength(255)
                    .HasColumnName("name_garnizone");

                entity.Property(e => e.NameSredstvo)
                    .HasMaxLength(255)
                    .HasColumnName("name_sredstvo")
                    .HasComment("Наименование средства");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder");

                entity.Property(e => e.Parent)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent");

                entity.Property(e => e.Remont)
                    .HasColumnType("int(11)")
                    .HasColumnName("remont")
                    .HasComment("В ремонте");

                entity.Property(e => e.Rezerv)
                    .HasColumnType("int(11)")
                    .HasColumnName("rezerv")
                    .HasComment("В резерве");

                entity.Property(e => e.SredstvoId)
                    .HasColumnType("int(11)")
                    .HasColumnName("sredstvo_id");

                entity.Property(e => e.SredstvoVid)
                    .HasMaxLength(255)
                    .HasColumnName("sredstvo_vid");

                entity.Property(e => e.SredstvoVidId)
                    .HasColumnType("int(11)")
                    .HasColumnName("sredstvo_vid_id");

                entity.Property(e => e.Subdivision)
                    .HasMaxLength(255)
                    .HasColumnName("subdivision");

                entity.Property(e => e.SubdivisionId)
                    .HasColumnType("int(11)")
                    .HasColumnName("subdivision_id");
            });

            modelBuilder.Entity<Stroevkaparam>(entity =>
            {
                entity.ToTable("stroevkaparams");

                entity.HasIndex(e => e.Parent, "FK_stroevkaparams_parent");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Comment)
                    .HasMaxLength(255)
                    .HasColumnName("comment");

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(255)
                    .HasColumnName("display_name");

                entity.Property(e => e.ExcelCol)
                    .HasMaxLength(255)
                    .HasColumnName("excel_col");

                entity.Property(e => e.ExcelRow)
                    .HasMaxLength(255)
                    .HasColumnName("excel_row");

                entity.Property(e => e.InitValue)
                    .HasColumnType("int(11)")
                    .HasColumnName("initValue");

                entity.Property(e => e.Level)
                    .HasColumnType("int(11)")
                    .HasColumnName("level");

                entity.Property(e => e.Mtable)
                    .HasMaxLength(255)
                    .HasColumnName("mtable");

                entity.Property(e => e.Mtype)
                    .HasMaxLength(255)
                    .HasColumnName("mtype");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder")
                    .HasDefaultValueSql("'0'")
                    .HasComment("Порядок");

                entity.Property(e => e.Operation)
                    .HasColumnType("int(11)")
                    .HasColumnName("operation");

                entity.Property(e => e.Parent)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent");

                entity.Property(e => e.RemontExcelCol)
                    .HasMaxLength(255)
                    .HasColumnName("remont_excel_col");

                entity.Property(e => e.RemontVal)
                    .HasMaxLength(255)
                    .HasColumnName("remont_val");

                entity.Property(e => e.ReservVal)
                    .HasMaxLength(255)
                    .HasColumnName("reserv_val");

                entity.Property(e => e.RezervExcelCol)
                    .HasMaxLength(255)
                    .HasColumnName("rezerv_excel_col");

                entity.HasOne(d => d.ParentNavigation)
                    .WithMany(p => p.InverseParentNavigation)
                    .HasForeignKey(d => d.Parent)
                    .HasConstraintName("FK_stroevkaparams_parent");
            });

            modelBuilder.Entity<Vid>(entity =>
            {
                entity.ToTable("vid");

                entity.HasIndex(e => e.GarnizonId, "FK_vid_garnizon_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.GarnizonId)
                    .HasColumnType("int(11)")
                    .HasColumnName("garnizon_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.HasOne(d => d.Garnizon)
                    .WithMany(p => p.Vids)
                    .HasForeignKey(d => d.GarnizonId)
                    .HasConstraintName("FK_vid_garnizon_id");
            });

            modelBuilder.Entity<Waters>(entity =>
            {
                entity.ToTable("waters");

                entity.HasIndex(e => e.GarnizonId, "FK_waters_garnizon_id");

                entity.HasIndex(e => e.SubdivisionId, "FK_waters_subdivision_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.EditTime)
                    .HasColumnType("timestamp")
                    .HasColumnName("edit_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Excel)
                    .HasMaxLength(255)
                    .HasColumnName("excel");

                entity.Property(e => e.Fault)
                    .HasColumnType("int(11)")
                    .HasColumnName("fault");

                entity.Property(e => e.GarnizonId)
                    .HasColumnType("int(11)")
                    .HasColumnName("garnizon_id");

                entity.Property(e => e.Mdate)
                    .HasColumnType("date")
                    .HasColumnName("mdate");

                entity.Property(e => e.Mname)
                    .HasMaxLength(255)
                    .HasColumnName("mname");

                entity.Property(e => e.NameGarnizone)
                    .HasMaxLength(255)
                    .HasColumnName("name_garnizone");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder");

                entity.Property(e => e.Subdivision)
                    .HasMaxLength(255)
                    .HasColumnName("subdivision");

                entity.Property(e => e.SubdivisionId)
                    .HasColumnType("int(11)")
                    .HasColumnName("subdivision_id");

                entity.Property(e => e.Total)
                    .HasColumnType("int(11)")
                    .HasColumnName("total");

                entity.HasOne(d => d.SubdivisionNavigation)
                    .WithMany(p => p.Waters)
                    .HasForeignKey(d => d.SubdivisionId)
                    .HasConstraintName("FK_waters_subdivision_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
