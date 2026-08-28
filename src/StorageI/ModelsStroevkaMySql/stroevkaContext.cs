using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace StorageI.ModelsStroevkaMySql
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

        public virtual DbSet<ApivotMat> ApivotMats { get; set; } = null!;
        public virtual DbSet<CacheNachkar> CacheNachkars { get; set; } = null!;
        public virtual DbSet<Contact> Contacts { get; set; } = null!;
        public virtual DbSet<FirePsgStat> FirePsgStats { get; set; } = null!;
        public virtual DbSet<Kostym> Kostyms { get; set; } = null!;
        public virtual DbSet<Pch> Pchs { get; set; } = null!;
        public virtual DbSet<Pena> Penas { get; set; } = null!;
        public virtual DbSet<Personal> Personals { get; set; } = null!;
        public virtual DbSet<Personalpost> Personalposts { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<Psg> Psgs { get; set; } = null!;
        public virtual DbSet<PsgTotalRow> PsgTotalRows { get; set; } = null!;
        public virtual DbSet<Psgdatum> Psgdata { get; set; } = null!;
        public virtual DbSet<Psgstat> Psgstats { get; set; } = null!;
        public virtual DbSet<Sizod> Sizods { get; set; } = null!;
        public virtual DbSet<Sostav> Sostavs { get; set; } = null!;
        public virtual DbSet<Sredstva> Sredstvas { get; set; } = null!;
        public virtual DbSet<Titog> Titogs { get; set; } = null!;
        public virtual DbSet<Water> Waters { get; set; } = null!;

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
            modelBuilder.Entity<ApivotMat>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("apivot_mat");

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

                entity.Property(e => e.Datafilled).HasColumnName("datafilled");

                entity.Property(e => e.Isitog)
                    .HasColumnType("int(1)")
                    .HasColumnName("isitog");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder");

                entity.Property(e => e.Parent)
                    .HasColumnType("bigint(11)")
                    .HasColumnName("parent");

                entity.Property(e => e.PchId)
                    .HasColumnType("int(11)")
                    .HasColumnName("pch_id");

                entity.Property(e => e.RowId)
                    .HasMaxLength(16)
                    .HasColumnName("row_id");

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

                entity.Property(e => e.Водитель).HasColumnType("bigint(20)");

                entity.Property(e => e.Всего)
                    .HasPrecision(32)
                    .HasColumnName("всего");

                entity.Property(e => e.ВсегоОтс)
                    .HasPrecision(35)
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

                entity.Property(e => e.Диспетчер).HasColumnType("bigint(20)");

                entity.Property(e => e.Дт)
                    .HasPrecision(33)
                    .HasColumnName("ДТ");

                entity.Property(e => e.Ко)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("КО");

                entity.Property(e => e.Командировка)
                    .HasColumnType("bigint(20)")
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

                entity.Property(e => e.Налицо).HasPrecision(34);

                entity.Property(e => e.Начкар)
                    .HasMaxLength(255)
                    .HasColumnName("начкар");

                entity.Property(e => e.Некомплект)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("некомплект");

                entity.Property(e => e.Нк)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("НК");

                entity.Property(e => e.Отпуск)
                    .HasColumnType("bigint(20)")
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
                    .HasColumnType("bigint(20)")
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
                    .HasColumnType("bigint(20)")
                    .HasColumnName("по_больничному");

                entity.Property(e => e.ПоСписку)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("по_списку");

                entity.Property(e => e.ПожКорабльКатерBr)
                    .HasPrecision(32)
                    .HasColumnName("пож_корабль_катер_br");

                entity.Property(e => e.ПожКорабльКатерRemont)
                    .HasPrecision(32)
                    .HasColumnName("пож_корабль_катер_remont");

                entity.Property(e => e.ПожКорабльКатерRezerv)
                    .HasPrecision(32)
                    .HasColumnName("пож_корабль_катер_rezerv");

                entity.Property(e => e.ПожПоездBr)
                    .HasPrecision(32)
                    .HasColumnName("пож_поезд_br");

                entity.Property(e => e.ПожПоездRemont)
                    .HasPrecision(32)
                    .HasColumnName("пож_поезд_remont");

                entity.Property(e => e.ПожПоездRezerv)
                    .HasPrecision(32)
                    .HasColumnName("пож_поезд_rezerv");

                entity.Property(e => e.ПожПоездКорабльBr)
                    .HasPrecision(32)
                    .HasColumnName("пож_поезд_корабль_br");

                entity.Property(e => e.ПожПоездКорабльRemont)
                    .HasPrecision(32)
                    .HasColumnName("пож_поезд_корабль_remont");

                entity.Property(e => e.ПожПоездКорабльRezerv)
                    .HasPrecision(32)
                    .HasColumnName("пож_поезд_корабль_rezerv");

                entity.Property(e => e.Пожарный).HasColumnType("bigint(20)");

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
                    .HasPrecision(38)
                    .HasColumnName("прочие_отс");

                entity.Property(e => e.Псг)
                    .HasMaxLength(127)
                    .HasColumnName("ПСГ");

                entity.Property(e => e.Пч)
                    .HasMaxLength(127)
                    .HasColumnName("ПЧ");

                entity.Property(e => e.Резерв)
                    .HasColumnType("bigint(20)")
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
                entity.ToTable("cache_nachkar");

                entity.HasIndex(e => e.SubdivisionId, "idx_subdivision");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Karaul)
                    .HasColumnType("int(11)")
                    .HasColumnName("karaul")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.LastUpdated)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Nachkar)
                    .HasMaxLength(255)
                    .HasColumnName("nachkar");

                entity.Property(e => e.PsgId)
                    .HasColumnType("int(11)")
                    .HasColumnName("psgId");

                entity.Property(e => e.SubdivisionId)
                    .HasColumnType("int(11)")
                    .HasColumnName("subdivision_id");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("contacts");

                entity.HasIndex(e => e.GarnizonId, "FK_contacts_garnizon_id");

                entity.HasIndex(e => e.SubdivisionId, "FK_contacts_subdivision_id2");

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

                entity.HasOne(d => d.Subdivision1)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.SubdivisionId)
                    .HasConstraintName("FK_contacts_subdivision_id2");
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
                    .HasMaxLength(100)
                    .HasColumnName("category");

                entity.Property(e => e.Datafilled)
                    .HasMaxLength(4)
                    .HasColumnName("datafilled")
                    .IsFixedLength();

                entity.Property(e => e.Isitog)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("isitog");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder");

                entity.Property(e => e.Parent)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("parent");

                entity.Property(e => e.PchId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("pch_id");

                entity.Property(e => e.RowId1)
                    .HasMaxLength(16)
                    .HasColumnName("row_id1");

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

                entity.Property(e => e.Водитель).HasPrecision(63);

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

                entity.Property(e => e.Диспетчер).HasPrecision(63);

                entity.Property(e => e.Дт)
                    .HasPrecision(65)
                    .HasColumnName("ДТ");

                entity.Property(e => e.Ко)
                    .HasPrecision(63)
                    .HasColumnName("КО");

                entity.Property(e => e.Командировка)
                    .HasPrecision(63)
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
                    .HasPrecision(63)
                    .HasColumnName("некомплект");

                entity.Property(e => e.Нк)
                    .HasPrecision(63)
                    .HasColumnName("НК");

                entity.Property(e => e.Отпуск)
                    .HasPrecision(63)
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
                    .HasPrecision(63)
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
                    .HasPrecision(63)
                    .HasColumnName("по_больничному");

                entity.Property(e => e.ПоСписку)
                    .HasPrecision(63)
                    .HasColumnName("по_списку");

                entity.Property(e => e.ПожКорабльКатерBr)
                    .HasPrecision(65)
                    .HasColumnName("пож_корабль_катер_br");

                entity.Property(e => e.ПожКорабльКатерRemont)
                    .HasPrecision(65)
                    .HasColumnName("пож_корабль_катер_remont");

                entity.Property(e => e.ПожКорабльКатерRezerv)
                    .HasPrecision(65)
                    .HasColumnName("пож_корабль_катер_rezerv");

                entity.Property(e => e.ПожПоездBr)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_br");

                entity.Property(e => e.ПожПоездRemont)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_remont");

                entity.Property(e => e.ПожПоездRezerv)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_rezerv");

                entity.Property(e => e.ПожПоездКорабльBr)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_корабль_br");

                entity.Property(e => e.ПожПоездКорабльRemont)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_корабль_remont");

                entity.Property(e => e.ПожПоездКорабльRezerv)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_корабль_rezerv");

                entity.Property(e => e.Пожарный).HasPrecision(63);

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
                    .HasPrecision(63)
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

            modelBuilder.Entity<Kostym>(entity =>
            {
                entity.ToTable("kostyms");

                entity.HasIndex(e => e.GarnizionId, "FK_kostyms_garnizion_id");

                entity.HasIndex(e => e.SubdivisionId, "FK_kostyms_subdivision_id2");

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

                entity.HasOne(d => d.Subdivision1)
                    .WithMany(p => p.Kostyms)
                    .HasForeignKey(d => d.SubdivisionId)
                    .HasConstraintName("FK_kostyms_subdivision_id2");
            });

            modelBuilder.Entity<Pch>(entity =>
            {
                entity.ToTable("pchs");

                entity.HasIndex(e => e.Garntype, "idx_garntype");

                entity.HasIndex(e => e.Name, "idx_name");

                entity.HasIndex(e => e.Parent, "idx_parent");

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

                entity.Property(e => e.RowId)
                    .HasMaxLength(16)
                    .HasColumnName("row_id");

                entity.Property(e => e.Visibility)
                    .IsRequired()
                    .HasColumnName("visibility")
                    .HasDefaultValueSql("'1'");
            });

            modelBuilder.Entity<Pena>(entity =>
            {
                entity.ToTable("penas");

                entity.HasIndex(e => e.GarnizonId, "FK_penas_subdivision_id2");

                entity.HasIndex(e => e.SubdivisionId, "FK_penas_subdivision_id3");

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

                entity.HasOne(d => d.Subdivision1)
                    .WithMany(p => p.Penas)
                    .HasForeignKey(d => d.SubdivisionId)
                    .HasConstraintName("FK_penas_subdivision_id3");
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

            modelBuilder.Entity<Personalpost>(entity =>
            {
                entity.ToTable("personalpost");

                entity.HasIndex(e => e.PersonalId, "FK_personalpost_PersonalId");

                entity.HasIndex(e => e.PostId, "FK_personalpost_PostId");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11) unsigned")
                    .HasColumnName("id");

                entity.Property(e => e.Fio)
                    .HasMaxLength(255)
                    .HasColumnName("FIO");

                entity.Property(e => e.PersonalId).HasColumnType("int(11)");

                entity.Property(e => e.Post).HasMaxLength(255);

                entity.Property(e => e.PostId).HasColumnType("int(11)");

                entity.HasOne(d => d.Personal)
                    .WithMany(p => p.Personalposts)
                    .HasForeignKey(d => d.PersonalId)
                    .HasConstraintName("FK_personalpost_PersonalId");

                entity.HasOne(d => d.PostNavigation)
                    .WithMany(p => p.Personalposts)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_personalpost_PostId");
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
                entity.ToTable("psg");

                entity.HasIndex(e => e.Garnizon, "idx_garnizon");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Datafilled).HasColumnName("datafilled");

                entity.Property(e => e.Garnizon)
                    .HasMaxLength(127)
                    .HasColumnName("garnizon");

                entity.Property(e => e.MainPchId)
                    .HasColumnType("int(11)")
                    .HasColumnName("mainPchId")
                    .HasDefaultValueSql("'54'");

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

                entity.Property(e => e.RowId)
                    .HasMaxLength(16)
                    .HasColumnName("row_id");
            });

            modelBuilder.Entity<PsgTotalRow>(entity =>
            {
                entity.ToTable("psg_total_rows");

                entity.HasComment("Итоговые строки для ПСГ");

                entity.HasIndex(e => e.PsgId, "IDX_psg_id");

                entity.HasIndex(e => e.RowId, "IDX_row_id")
                    .IsUnique();

                entity.HasIndex(e => e.TotalFlag, "IDX_total_flag");

                entity.HasIndex(e => e.Id, "UK__row_Id")
                    .IsUnique();

                entity.HasIndex(e => new { e.PsgId, e.CategoryType }, "UK_psg_total_rows")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "UK_psg_total_rows_Id")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "UK_row_Id")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CategoryDisplay)
                    .HasMaxLength(100)
                    .HasColumnName("category_display");

                entity.Property(e => e.CategoryType)
                    .HasMaxLength(20)
                    .HasColumnName("category_type")
                    .HasComment("Тип категории: main, gps, fps, other, vpo, chpo, asf");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(100)
                    .HasColumnName("display_name")
                    .HasComment("Отображаемое наименование (с отступами)");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name")
                    .HasComment("Наименование итоговой строки в представлении");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder")
                    .HasComment("Порядок сортировки (отрицательные числа для группировки)");

                entity.Property(e => e.PsgId)
                    .HasColumnType("int(11)")
                    .HasColumnName("psg_id")
                    .HasComment("ID ПСГ (из таблицы psg или 11 для территориального)");

                entity.Property(e => e.RowId)
                    .HasMaxLength(14)
                    .HasColumnName("row_id")
                    .HasComment(" Уникальный ID строки (14 символов): признак(2)+кодПСГ(4)+0000(4)+кодТерриториального(4)");

                entity.Property(e => e.TotalFlag)
                    .HasMaxLength(2)
                    .HasColumnName("total_flag")
                    .HasComment("Признак итога: 01-итог по ПСГ, 02-ГПС, 03-ФПС, 04-другие, 05-ВПО, 06-ЧПО, 07-АСФ");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp")
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");
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

            modelBuilder.Entity<Psgstat>(entity =>
            {
                entity.ToTable("psgstat");

                entity.HasIndex(e => e.Parent, "FK_psgstat_parent");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Datafilled)
                    .HasColumnType("int(11)")
                    .HasColumnName("datafilled");

                entity.Property(e => e.Displayname)
                    .HasColumnType("text")
                    .HasColumnName("displayname");

                entity.Property(e => e.Garntype)
                    .HasColumnType("text")
                    .HasColumnName("garntype");

                entity.Property(e => e.Inreport)
                    .HasColumnType("int(11)")
                    .HasColumnName("inreport")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Isitog)
                    .HasColumnType("int(11)")
                    .HasColumnName("isitog")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Karaul)
                    .HasColumnType("int(11)")
                    .HasColumnName("karaul");

                entity.Property(e => e.Mdate)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("mdate")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Name)
                    .HasColumnType("text")
                    .HasColumnName("name");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder");

                entity.Property(e => e.Parent)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent");

                entity.Property(e => e.Rank)
                    .HasColumnType("int(11)")
                    .HasColumnName("rank");

                entity.Property(e => e.Used)
                    .HasColumnType("int(11)")
                    .HasColumnName("used");

                entity.HasOne(d => d.ParentNavigation)
                    .WithMany(p => p.InverseParentNavigation)
                    .HasForeignKey(d => d.Parent)
                    .HasConstraintName("FK_psgstat_parent");
            });

            modelBuilder.Entity<Sizod>(entity =>
            {
                entity.ToTable("sizod");

                entity.HasIndex(e => e.GarnizonId, "FK_sizod_garnizon_id");

                entity.HasIndex(e => e.SubdivisionId, "FK_sizod_subdivision_id2");

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

                entity.HasOne(d => d.Subdivision1)
                    .WithMany(p => p.Sizods)
                    .HasForeignKey(d => d.SubdivisionId)
                    .HasConstraintName("FK_sizod_subdivision_id2");
            });

            modelBuilder.Entity<Sostav>(entity =>
            {
                entity.ToTable("sostav");

                entity.HasIndex(e => e.GarnizoneId, "FK_sostav_garnizone_id");

                entity.HasIndex(e => e.SubdivisionId, "FK_sostav_subdivision_id2");

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

                entity.HasOne(d => d.Subdivision1)
                    .WithMany(p => p.Sostavs)
                    .HasForeignKey(d => d.SubdivisionId)
                    .HasConstraintName("FK_sostav_subdivision_id2");
            });

            modelBuilder.Entity<Sredstva>(entity =>
            {
                entity.ToTable("sredstva");

                entity.HasIndex(e => e.GarnizonId, "FK_sredstva_garnizon_id2");

                entity.HasIndex(e => e.SubdivisionId, "FK_sredstva_subdivision_id2");

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

                entity.HasOne(d => d.Subdivision1)
                    .WithMany(p => p.Sredstvas)
                    .HasForeignKey(d => d.SubdivisionId)
                    .HasConstraintName("FK_sredstva_subdivision_id2");
            });

            modelBuilder.Entity<Titog>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("titogs");

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
                    .HasMaxLength(100)
                    .HasColumnName("category");

                entity.Property(e => e.CategoryType)
                    .HasMaxLength(5)
                    .HasColumnName("category_type")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Datafilled)
                    .HasMaxLength(0)
                    .HasColumnName("datafilled")
                    .IsFixedLength();

                entity.Property(e => e.Isitog)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("isitog");

                entity.Property(e => e.Norder)
                    .HasColumnType("int(11)")
                    .HasColumnName("norder");

                entity.Property(e => e.Parent)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("parent");

                entity.Property(e => e.PchId)
                    .HasColumnType("bigint(25)")
                    .HasColumnName("pch_id");

                entity.Property(e => e.RowId)
                    .HasMaxLength(14)
                    .HasColumnName("row_id");

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

                entity.Property(e => e.Водитель).HasPrecision(63);

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

                entity.Property(e => e.Диспетчер).HasPrecision(63);

                entity.Property(e => e.Дт)
                    .HasPrecision(65)
                    .HasColumnName("ДТ");

                entity.Property(e => e.Ко)
                    .HasPrecision(63)
                    .HasColumnName("КО");

                entity.Property(e => e.Командировка)
                    .HasPrecision(63)
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
                    .HasPrecision(63)
                    .HasColumnName("некомплект");

                entity.Property(e => e.Нк)
                    .HasPrecision(63)
                    .HasColumnName("НК");

                entity.Property(e => e.Отпуск)
                    .HasPrecision(63)
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
                    .HasPrecision(63)
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
                    .HasPrecision(63)
                    .HasColumnName("по_больничному");

                entity.Property(e => e.ПоСписку)
                    .HasPrecision(63)
                    .HasColumnName("по_списку");

                entity.Property(e => e.ПожКорабльКатерBr)
                    .HasPrecision(65)
                    .HasColumnName("пож_корабль_катер_br");

                entity.Property(e => e.ПожКорабльКатерRemont)
                    .HasPrecision(65)
                    .HasColumnName("пож_корабль_катер_remont");

                entity.Property(e => e.ПожКорабльКатерRezerv)
                    .HasPrecision(65)
                    .HasColumnName("пож_корабль_катер_rezerv");

                entity.Property(e => e.ПожПоездBr)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_br");

                entity.Property(e => e.ПожПоездRemont)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_remont");

                entity.Property(e => e.ПожПоездRezerv)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_rezerv");

                entity.Property(e => e.ПожПоездКорабльBr)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_корабль_br");

                entity.Property(e => e.ПожПоездКорабльRemont)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_корабль_remont");

                entity.Property(e => e.ПожПоездКорабльRezerv)
                    .HasPrecision(65)
                    .HasColumnName("пож_поезд_корабль_rezerv");

                entity.Property(e => e.Пожарный).HasPrecision(63);

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
                    .HasPrecision(63)
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

            modelBuilder.Entity<Water>(entity =>
            {
                entity.ToTable("waters");

                entity.HasIndex(e => e.GarnizonId, "FK_waters_garnizon_id");

                entity.HasIndex(e => e.SubdivisionId, "FK_waters_subdivision_id2");

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

                //entity.HasOne(d => d.Subdivision1)
                //    .WithMany(p => p.Waters)
                //    .HasForeignKey(d => d.SubdivisionId)
                //    .HasConstraintName("FK_waters_subdivision_id2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
