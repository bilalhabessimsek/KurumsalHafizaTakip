using KurumsalHafizaTakip.Modeller;
using KurumsalHafizaTakip.VeriTabaniTabs;
using Microsoft.EntityFrameworkCore;

namespace KurumsalHafizaTakip.VeriTabaniBaglanti
{
    public class UygulamaDbBaglanti : DbContext
    {
        // 1. KRİTİK CONSTRUCTOR: Program.cs'den gelen konfigürasyonu ana DbContext'e iletir.
        public UygulamaDbBaglanti(DbContextOptions<UygulamaDbBaglanti> options) : base(options)
        {
        }

        // 2. TABLO TANIMLAMALARI (DbSets): C# sınıflarını SQL tablolarına eşler.
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Gorev> Gorevler { get; set; }
        public DbSet<Proje> Projeler { get; set; }
        public DbSet<ProjeEkip> ProjeEkipleri { get; set; }
        public DbSet<Bildirim> Bildirimler { get; set; }
        public DbSet<AnlikMesaj> Mesajlar { get; set; }
        public DbSet<ArsivDokuman> Arsiv { get; set; }
        public DbSet<RevizeVeNotlar> Notlar { get; set; }
        public DbSet<Departman> Departmanlar { get; set; }
        public DbSet<ToplantiTutanagi> ToplantiTutanaklari { get; set; }
        public DbSet<AnlikMesaj> AnlikMesajlar { get; set; }
        // 3. YEDEK YAPILANDIRMA: Migration (Add-Migration) komutları sırasında 
        // bağlantı dizesini bulamazsa bu blok devreye girer.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=KHATS_DB;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // İleride tablolar arası karmaşık ilişkiler (Cascade silme vb.) 
            // gerekirse buraya Fluent API kodlarını yazacağız.
        }
    }
}