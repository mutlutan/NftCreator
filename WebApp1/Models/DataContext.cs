using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebApp1.Models
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<NftProje> NftProje { get; set; }
        public virtual DbSet<TemAbonelikDonem> TemAbonelikDonem { get; set; }
        public virtual DbSet<TemAbonelikDurum> TemAbonelikDurum { get; set; }
        public virtual DbSet<TemAbonelikUrun> TemAbonelikUrun { get; set; }
        public virtual DbSet<TemAbonelikUrunPlan> TemAbonelikUrunPlan { get; set; }
        public virtual DbSet<TemAdres> TemAdres { get; set; }
        public virtual DbSet<TemAuditLog> TemAuditLog { get; set; }
        public virtual DbSet<TemCinsiyet> TemCinsiyet { get; set; }
        public virtual DbSet<TemDepartman> TemDepartman { get; set; }
        public virtual DbSet<TemDovizKurArsiv> TemDovizKurArsiv { get; set; }
        public virtual DbSet<TemEgitimDurum> TemEgitimDurum { get; set; }
        public virtual DbSet<TemGorev> TemGorev { get; set; }
        public virtual DbSet<TemIlce> TemIlce { get; set; }
        public virtual DbSet<TemKullanici> TemKullanici { get; set; }
        public virtual DbSet<TemKullaniciAbonelik> TemKullaniciAbonelik { get; set; }
        public virtual DbSet<TemKullaniciAbonelikOdeme> TemKullaniciAbonelikOdeme { get; set; }
        public virtual DbSet<TemKullaniciLisans> TemKullaniciLisans { get; set; }
        public virtual DbSet<TemKullaniciSifre> TemKullaniciSifre { get; set; }
        public virtual DbSet<TemMailAntet> TemMailAntet { get; set; }
        public virtual DbSet<TemMailHareket> TemMailHareket { get; set; }
        public virtual DbSet<TemMailHareketDurum> TemMailHareketDurum { get; set; }
        public virtual DbSet<TemMailSablon> TemMailSablon { get; set; }
        public virtual DbSet<TemMesaj> TemMesaj { get; set; }
        public virtual DbSet<TemOturumLog> TemOturumLog { get; set; }
        public virtual DbSet<TemParaBirim> TemParaBirim { get; set; }
        public virtual DbSet<TemParametre> TemParametre { get; set; }
        public virtual DbSet<TemRequestLog> TemRequestLog { get; set; }
        public virtual DbSet<TemRol> TemRol { get; set; }
        public virtual DbSet<TemSehir> TemSehir { get; set; }
        public virtual DbSet<TemUlke> TemUlke { get; set; }
        public virtual DbSet<TemVersiyon> TemVersiyon { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Turkish_CI_AS");

            modelBuilder.Entity<NftProje>(entity =>
            {
                entity.HasIndex(e => e.Durum, "IX_NftProje_Durum");

                entity.HasIndex(e => e.Gid, "IX_NftProje_GId");

                entity.HasIndex(e => e.TarihSaat, "IX_NftProje_TarihSaat");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(250);

                entity.Property(e => e.Gid).HasColumnName("GId");

                entity.Property(e => e.TarihSaat).HasColumnType("datetime");

                entity.HasOne(d => d.Kullanici)
                    .WithMany(p => p.NftProje)
                    .HasForeignKey(d => d.KullaniciId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NftProje_KullaniciId");
            });

            modelBuilder.Entity<TemAbonelikDonem>(entity =>
            {
                entity.HasIndex(e => e.Durum, "IX_TemAbonelikDonem_Durum");

                entity.HasIndex(e => e.Ad, "UX_TemAbonelikDonem_Ad")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TemAbonelikDurum>(entity =>
            {
                entity.HasIndex(e => e.Durum, "IX_TemAbonelikDurum_Durum");

                entity.HasIndex(e => e.Ad, "UX_TemAbonelikDurum_Ad")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Aciklama).HasMaxLength(250);

                entity.Property(e => e.Ad)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Kod)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<TemAbonelikUrun>(entity =>
            {
                entity.HasIndex(e => e.Durum, "IX_TemAbonelikUrun_Durum");

                entity.HasIndex(e => e.Ad, "UX_TemAbonelikUrun_Ad")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(50);

                entity.Property(e => e.Kod).HasMaxLength(50);
            });

            modelBuilder.Entity<TemAbonelikUrunPlan>(entity =>
            {
                entity.HasIndex(e => e.AbonelikUrunId, "IX_TemAbonelikUrunPlan_AbonelikUrunId");

                entity.HasIndex(e => e.Durum, "IX_TemAbonelikUrunPlan_Durum");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(50);

                entity.Property(e => e.Kod).HasMaxLength(50);

                entity.Property(e => e.Ucret).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.AbonelikDonem)
                    .WithMany(p => p.TemAbonelikUrunPlan)
                    .HasForeignKey(d => d.AbonelikDonemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemAbonelikUrun_AbonelikDonemId");

                entity.HasOne(d => d.AbonelikUrun)
                    .WithMany(p => p.TemAbonelikUrunPlan)
                    .HasForeignKey(d => d.AbonelikUrunId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemAbonelikUrun_AbonelikUrunId");

                entity.HasOne(d => d.ParaBirim)
                    .WithMany(p => p.TemAbonelikUrunPlan)
                    .HasForeignKey(d => d.ParaBirimId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemAbonelikUrun_ParaBirimId");
            });

            modelBuilder.Entity<TemAdres>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(100);

                entity.Property(e => e.Kod).HasMaxLength(20);

                entity.HasOne(d => d.Ust)
                    .WithMany(p => p.InverseUst)
                    .HasForeignKey(d => d.UstId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemAdres_UstId");
            });

            modelBuilder.Entity<TemAuditLog>(entity =>
            {
                entity.HasIndex(e => e.OperationDate, "IX_TemAuditLog_OperationDate");

                entity.HasIndex(e => e.OperationType, "IX_TemAuditLog_OperationType");

                entity.HasIndex(e => e.PrimaryKeyField, "IX_TemAuditLog_PrimaryKeyField");

                entity.HasIndex(e => e.PrimaryKeyValue, "IX_TemAuditLog_PrimaryKeyValue");

                entity.HasIndex(e => e.TableName, "IX_TemAuditLog_TableName");

                entity.HasIndex(e => e.UserId, "IX_TemAuditLog_UserId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.IpAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OperationDate).HasColumnType("datetime");

                entity.Property(e => e.OperationType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.PrimaryKeyField)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrimaryKeyValue).HasMaxLength(50);

                entity.Property(e => e.TableName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TemCinsiyet>(entity =>
            {
                entity.HasIndex(e => e.Durum, "IX_TemCinsiyet_Durum");

                entity.HasIndex(e => e.Ad, "UX_TemCinsiyet_Ad")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<TemDepartman>(entity =>
            {
                entity.HasIndex(e => e.Durum, "IX_TemDepartman_Durum");

                entity.HasIndex(e => e.Ad, "UX_TemDepartman_Ad")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Sira).HasColumnType("decimal(18, 4)");

                entity.HasOne(d => d.Ust)
                    .WithMany(p => p.InverseUst)
                    .HasForeignKey(d => d.UstId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemDepartman_UstId");
            });

            modelBuilder.Entity<TemDovizKurArsiv>(entity =>
            {
                entity.HasIndex(e => e.ParaBirimId, "IX_TemDovizKurArsiv_ParaBirimId");

                entity.HasIndex(e => e.Tarih, "IX_TemDovizKurArsiv_Tarih");

                entity.HasIndex(e => e.TarihSaat, "IX_TemDovizKurArsiv_TarihSaat");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DovizAlis).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.DovizSatis).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.EfektifAlis).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.EfektifSatis).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Tarih).HasColumnType("date");

                entity.Property(e => e.TarihSaat).HasColumnType("date");

                entity.HasOne(d => d.ParaBirim)
                    .WithMany(p => p.TemDovizKurArsiv)
                    .HasForeignKey(d => d.ParaBirimId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemDovizKurArsiv_ParaBirimId");
            });

            modelBuilder.Entity<TemEgitimDurum>(entity =>
            {
                entity.HasIndex(e => e.Ad, "UX_TemEgitimDurum_Ad")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<TemGorev>(entity =>
            {
                entity.HasIndex(e => e.Durum, "IX_TemGorev_Durum");

                entity.HasIndex(e => e.Ad, "UX_TemGorev_Ad")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TemIlce>(entity =>
            {
                entity.HasIndex(e => e.Ad, "IX_TemIlce_Ad");

                entity.HasIndex(e => e.SehirId, "IX_TemIlce_SehirId");

                entity.HasIndex(e => e.Sira, "IX_TemIlce_Sira");

                entity.HasIndex(e => new { e.SehirId, e.Ad }, "UX_TemIlce_SehirId_Ad")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Kod).HasMaxLength(10);

                entity.HasOne(d => d.Sehir)
                    .WithMany(p => p.TemIlce)
                    .HasForeignKey(d => d.SehirId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemIlce_SehirId");
            });

            modelBuilder.Entity<TemKullanici>(entity =>
            {
                entity.HasIndex(e => e.Ad, "UX_TemKullanici_Ad")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(50);

                entity.Property(e => e.KayitZaman).HasColumnType("datetime");

                entity.Property(e => e.Sifre).HasMaxLength(100);
            });

            modelBuilder.Entity<TemKullaniciAbonelik>(entity =>
            {
                entity.HasIndex(e => e.AbonelikDurumId, "IX_TemKullaniciAbonelik_AbonelikDurumId");

                entity.HasIndex(e => e.AbonelikUrunPlanId, "IX_TemKullaniciAbonelik_AbonelikUrunPlanId");

                entity.HasIndex(e => e.KullaniciId, "IX_TemKullaniciAbonelik_KullaniciId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.InsertDateTime).HasColumnType("datetime");

                entity.Property(e => e.Jeton).HasMaxLength(50);

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.AbonelikDurum)
                    .WithMany(p => p.TemKullaniciAbonelik)
                    .HasForeignKey(d => d.AbonelikDurumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemKullaniciAbonelik_AbonelikDurumId");

                entity.HasOne(d => d.AbonelikUrunPlan)
                    .WithMany(p => p.TemKullaniciAbonelik)
                    .HasForeignKey(d => d.AbonelikUrunPlanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemKullaniciAbonelik_AbonelikUrunPlanId");

                entity.HasOne(d => d.Kullanici)
                    .WithMany(p => p.TemKullaniciAbonelik)
                    .HasForeignKey(d => d.KullaniciId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemKullaniciAbonelik_KullaniciId");
            });

            modelBuilder.Entity<TemKullaniciAbonelikOdeme>(entity =>
            {
                entity.HasIndex(e => e.KullaniciAbonelikId, "IX_TemKullaniciAbonelikOdeme_KullaniciAbonelikId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BaslamaTarihi).HasColumnType("date");

                entity.Property(e => e.BitisTarihi).HasColumnType("date");

                entity.Property(e => e.InsertDateTime).HasColumnType("datetime");

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.KullaniciAbonelik)
                    .WithMany(p => p.TemKullaniciAbonelikOdeme)
                    .HasForeignKey(d => d.KullaniciAbonelikId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemKullaniciAbonelikOdeme_KullaniciAbonelikId");
            });

            modelBuilder.Entity<TemKullaniciLisans>(entity =>
            {
                entity.HasIndex(e => e.KullaniciId, "IX_TemKullaniciLisans_KullaniciId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BaslamaTarihi).HasColumnType("date");

                entity.Property(e => e.BitisTarihi).HasColumnType("date");

                entity.HasOne(d => d.Kullanici)
                    .WithMany(p => p.TemKullaniciLisans)
                    .HasForeignKey(d => d.KullaniciId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemKullaniciLisans_KullaniciId");
            });

            modelBuilder.Entity<TemKullaniciSifre>(entity =>
            {
                entity.HasIndex(e => e.KullaniciId, "IX_TemKullaniciSifre_KullaniciId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.KayitZaman).HasColumnType("datetime");

                entity.Property(e => e.Sifre).HasMaxLength(100);

                entity.HasOne(d => d.Kullanici)
                    .WithMany(p => p.TemKullaniciSifre)
                    .HasForeignKey(d => d.KullaniciId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemKullaniciSifre_KullaniciId");
            });

            modelBuilder.Entity<TemMailAntet>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(50);
            });

            modelBuilder.Entity<TemMailHareket>(entity =>
            {
                entity.HasIndex(e => e.DurumId, "IX_TemMailHareket_DurumId");

                entity.HasIndex(e => e.SablonId, "IX_TemMailHareket_SablonId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Adres).HasMaxLength(500);

                entity.Property(e => e.Gizli).HasMaxLength(500);

                entity.Property(e => e.KayitZaman).HasColumnType("datetime");

                entity.Property(e => e.Konu).HasMaxLength(250);

                entity.Property(e => e.Kopya).HasMaxLength(500);

                entity.Property(e => e.SonDenemeZaman).HasColumnType("datetime");

                entity.HasOne(d => d.Durum)
                    .WithMany(p => p.TemMailHareket)
                    .HasForeignKey(d => d.DurumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemMailHareket_DurumId");

                entity.HasOne(d => d.Sablon)
                    .WithMany(p => p.TemMailHareket)
                    .HasForeignKey(d => d.SablonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemMailHareket_SablonId");
            });

            modelBuilder.Entity<TemMailHareketDurum>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad).HasMaxLength(50);
            });

            modelBuilder.Entity<TemMailSablon>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Gizli).HasMaxLength(500);

                entity.Property(e => e.Konu).HasMaxLength(250);

                entity.Property(e => e.Kopya).HasMaxLength(500);

                entity.HasOne(d => d.Antet)
                    .WithMany(p => p.TemMailSablon)
                    .HasForeignKey(d => d.AntetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemMailSablon_AntetId");
            });

            modelBuilder.Entity<TemMesaj>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Baslik).HasMaxLength(50);

                entity.HasOne(d => d.Alici)
                    .WithMany(p => p.TemMesajAlici)
                    .HasForeignKey(d => d.AliciId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemMesaj_AliciId");

                entity.HasOne(d => d.Gonderici)
                    .WithMany(p => p.TemMesajGonderici)
                    .HasForeignKey(d => d.GondericiId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemMesaj_GondericiId");

                entity.HasOne(d => d.Ust)
                    .WithMany(p => p.InverseUst)
                    .HasForeignKey(d => d.UstId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemMesaj_UstId");
            });

            modelBuilder.Entity<TemOturumLog>(entity =>
            {
                entity.HasIndex(e => e.KullaniciId, "IX_TemOturumLog_KullaniciId");

                entity.HasIndex(e => e.OturumGuid, "IX_TemOturumLog_OturumGuid");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CikisZaman).HasColumnType("datetime");

                entity.Property(e => e.GirisZaman).HasColumnType("datetime");

                entity.Property(e => e.InternetProtokolAdres).HasMaxLength(50);

                entity.Property(e => e.OturumGuid).HasMaxLength(50);

                entity.Property(e => e.Tarayici).HasMaxLength(250);

                entity.HasOne(d => d.Kullanici)
                    .WithMany(p => p.TemOturumLog)
                    .HasForeignKey(d => d.KullaniciId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemOturumLog_KullaniciId");
            });

            modelBuilder.Entity<TemParaBirim>(entity =>
            {
                entity.HasIndex(e => e.Ad, "UX_TemParaBirim_Ad")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.AltBirim).HasMaxLength(20);

                entity.Property(e => e.Kod).HasMaxLength(10);

                entity.Property(e => e.Simge).HasMaxLength(10);

                entity.Property(e => e.UpdateCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TemParametre>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DataBackupGun).HasMaxLength(15);

                entity.Property(e => e.FileBackupGun).HasMaxLength(15);

                entity.Property(e => e.HostAddress)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.IyzicoApiKey).HasMaxLength(50);

                entity.Property(e => e.IyzicoBaseUrl).HasMaxLength(50);

                entity.Property(e => e.IyzicoSecretKey).HasMaxLength(50);

                entity.Property(e => e.KurumAd).HasMaxLength(100);

                entity.Property(e => e.KurumAdes).HasMaxLength(250);

                entity.Property(e => e.KurumBoylam).HasMaxLength(15);

                entity.Property(e => e.KurumCep1).HasMaxLength(50);

                entity.Property(e => e.KurumCep2).HasMaxLength(50);

                entity.Property(e => e.KurumEnlem).HasMaxLength(15);

                entity.Property(e => e.KurumFax1).HasMaxLength(50);

                entity.Property(e => e.KurumFax2).HasMaxLength(50);

                entity.Property(e => e.KurumMail).HasMaxLength(50);

                entity.Property(e => e.KurumSemtSehir).HasMaxLength(100);

                entity.Property(e => e.KurumTelefon1).HasMaxLength(50);

                entity.Property(e => e.KurumTelefon2).HasMaxLength(50);

                entity.Property(e => e.KurumUnvan).HasMaxLength(250);

                entity.Property(e => e.MailHost).HasMaxLength(100);

                entity.Property(e => e.MailPassword).HasMaxLength(100);

                entity.Property(e => e.MailUserName).HasMaxLength(100);
            });

            modelBuilder.Entity<TemRequestLog>(entity =>
            {
                entity.HasIndex(e => e.OperationDate, "IX_TemRequestLog_OperationDate");

                entity.HasIndex(e => e.OperationKod, "IX_TemRequestLog_OperationKod");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.IpAddress).HasMaxLength(50);

                entity.Property(e => e.OperationDate).HasColumnType("datetime");

                entity.Property(e => e.OperationKod).HasMaxLength(50);

                entity.Property(e => e.SessionGuid).HasMaxLength(50);
            });

            modelBuilder.Entity<TemRol>(entity =>
            {
                entity.HasIndex(e => e.Ad, "UX_TemRol_Ad")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<TemSehir>(entity =>
            {
                entity.HasIndex(e => e.Ad, "IX_TemSehir_Ad");

                entity.HasIndex(e => e.UlkeId, "IX_TemSehir_UlkeId");

                entity.HasIndex(e => new { e.UlkeId, e.Ad }, "UX_TemSehir_UlkeId_Ad")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.AlanKod).HasMaxLength(10);

                entity.Property(e => e.Kod).HasMaxLength(10);

                entity.HasOne(d => d.Ulke)
                    .WithMany(p => p.TemSehir)
                    .HasForeignKey(d => d.UlkeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemSehir_UlkeId");
            });

            modelBuilder.Entity<TemUlke>(entity =>
            {
                entity.HasIndex(e => e.Ad, "UX_TemUlke_Ad")
                    .IsUnique();

                entity.HasIndex(e => e.Kod, "UX_TemUlke_Kod")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.AlanKod).HasMaxLength(10);

                entity.Property(e => e.Kod)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<TemVersiyon>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Tarih).HasColumnType("datetime");
            });

            modelBuilder.HasSequence<int>("sqNftProje");

            modelBuilder.HasSequence<int>("sqTemAbonelikUrun").StartsAt(101);

            modelBuilder.HasSequence<int>("sqTemAbonelikUrunPlan");

            modelBuilder.HasSequence<int>("sqTemAdres");

            modelBuilder.HasSequence<int>("sqTemAuditLog");

            modelBuilder.HasSequence<int>("sqTemDepartman");

            modelBuilder.HasSequence<int>("sqTemDovizKurArsiv");

            modelBuilder.HasSequence<int>("sqTemGorev");

            modelBuilder.HasSequence<int>("sqTemIlce").StartsAt(1001);

            modelBuilder.HasSequence<int>("sqTemKullanici");

            modelBuilder.HasSequence<int>("sqTemKullaniciAbonelik");

            modelBuilder.HasSequence<int>("sqTemKullaniciAbonelikOdeme");

            modelBuilder.HasSequence<int>("sqTemKullaniciLisans");

            modelBuilder.HasSequence<int>("sqTemKullaniciSifre");

            modelBuilder.HasSequence<int>("sqTemMailAntet");

            modelBuilder.HasSequence<int>("sqTemMailHareket");

            modelBuilder.HasSequence<int>("sqTemMesaj");

            modelBuilder.HasSequence<int>("sqTemOturumLog");

            modelBuilder.HasSequence<int>("sqTemParaBirim");

            modelBuilder.HasSequence<int>("sqTemRequestLog");

            modelBuilder.HasSequence<int>("sqTemRol").StartsAt(1101);

            modelBuilder.HasSequence<int>("sqTemSehir");

            modelBuilder.HasSequence<int>("sqTemUlke");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
