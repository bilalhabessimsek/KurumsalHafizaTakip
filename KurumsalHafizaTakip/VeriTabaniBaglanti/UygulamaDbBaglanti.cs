using KurumsalHafizaTakip.VeriTabaniTabs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace KurumsalHafizaTakip.VeriTabaniBaglanti
{
    public class UygulamaDbBaglanti : DbContext
    {
        public UygulamaDbBaglanti(DbContextOptions<UygulamaDbBaglanti> options) : base(options) { }

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Gorev> Gorevler { get; set; }
        public DbSet<AnlikMesaj> Mesajlar { get; set; }
        public DbSet<ArsivDokuman> Dokumanlar { get; set; }
        public DbSet<RevizeVeNotlar> Notlar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kullanici>().ToTable("Kullanicilar");
            modelBuilder.Entity<Gorev>().ToTable("Gorevler");
            base.OnModelCreating(modelBuilder);
        }
    }
}