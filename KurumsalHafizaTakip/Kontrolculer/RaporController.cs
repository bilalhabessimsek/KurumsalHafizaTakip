using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurumsalHafizaTakip.VeriTabaniBaglanti;
using KurumsalHafizaTakip.Modeller;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace KurumsalHafizaTakip.Kontrolculer
{
    // Sadece Admin ve Görev Sorumluları erişebilir
    [Authorize(Roles = "Admin, Görev Sorumlusu")]
    public class RaporController : Controller
    {
        private readonly UygulamaDbBaglanti _baglanti;

        // Dependency Injection ile veritabanı bağlantısını alıyoruz
        public RaporController(UygulamaDbBaglanti baglanti)
        {
            _baglanti = baglanti;
        }

        // 1. DİNAMİK VERİMLİLİK RAPORU (Haftalık, Aylık ve Personel Filtreli)
        public async Task<IActionResult> Verimlilik(int? personelId, string donem = "hepsi")
        {
            // Zaman filtresi hesaplaması
            DateTime? baslangicTarihi = donem switch
            {
                "haftalik" => DateTime.Now.AddDays(-7),
                "aylik" => DateTime.Now.AddMonths(-1),
                _ => null
            };

            // Filtre dropdown listesi için personelleri çekiyoruz
            ViewBag.Personeller = await _baglanti.Kullanicilar
                .Where(k => k.Rol == "Personel")
                .ToListAsync();

            // Personel filtresi sorgusu oluşturma
            var sorgu = _baglanti.Kullanicilar.Where(k => k.Rol == "Personel").AsQueryable();
            if (personelId.HasValue)
            {
                sorgu = sorgu.Where(k => k.Id == personelId.Value);
            }

            var personeller = await sorgu.ToListAsync();
            var raporListesi = new List<VerimlilikRaporuViewModel>();

            foreach (var p in personeller)
            {
                // Onaylanmış görevleri filtreliyoruz
                var gorevSorgu = _baglanti.Gorevler.Where(g => g.PersonelId == p.Id && g.Durum == "Onaylandı");

                // Eğer tarih filtresi seçilmişse uygula
                if (baslangicTarihi.HasValue)
                {
                    gorevSorgu = gorevSorgu.Where(g => g.OnayTarihi >= baslangicTarihi.Value);
                }

                var bitenIsler = await gorevSorgu.ToListAsync();
                int toplamPuan = bitenIsler.Sum(g => g.AgirlikPuani);

                raporListesi.Add(new VerimlilikRaporuViewModel
                {
                    PersonelAdi = p.AdSoyad,
                    ToplamPuan = toplamPuan,
                    VerimlilikYuzdesi = (double)toplamPuan / 10, // 1000 Puan %100 kabul edilir
                    FazlaMesaiYapti = toplamPuan >= 1000,
                    PerformansDurumu = toplamPuan >= 1000 ? "Efsane" : (toplamPuan >= 500 ? "İyi" : "Normal")
                });
            }

            // Sayfa seçimlerini ViewBag ile geri gönderiyoruz
            ViewBag.SeciliPersonel = personelId;
            ViewBag.SeciliDonem = donem;

            return View("~/Gorunumler/Rapor/Verimlilik.cshtml", raporListesi);
        }

        // 2. EXCEL (CSV) ÇIKTISI ÜRETME METODU
        public async Task<IActionResult> VerimlilikCsvIndir(int? personelId, string donem = "hepsi")
        {
            DateTime? baslangicTarihi = donem switch
            {
                "haftalik" => DateTime.Now.AddDays(-7),
                "aylik" => DateTime.Now.AddMonths(-1),
                _ => null
            };

            var sorgu = _baglanti.Kullanicilar.Where(k => k.Rol == "Personel").AsQueryable();
            if (personelId.HasValue) sorgu = sorgu.Where(k => k.Id == personelId.Value);

            var personeller = await sorgu.ToListAsync();

            // CSV başlıklarını oluştur
            var csv = new StringBuilder();
            csv.AppendLine("Personel Adi;Toplam Puan;Verimlilik Yuzdesi;Durum");

            foreach (var p in personeller)
            {
                var gorevSorgu = _baglanti.Gorevler.Where(g => g.PersonelId == p.Id && g.Durum == "Onaylandı");
                if (baslangicTarihi.HasValue)
                {
                    gorevSorgu = gorevSorgu.Where(g => g.OnayTarihi >= baslangicTarihi.Value);
                }

                var bitenIsler = await gorevSorgu.ToListAsync();
                int puan = bitenIsler.Sum(g => g.AgirlikPuani);
                double yuzde = (double)puan / 10;
                string durum = puan >= 1000 ? "Efsane" : (puan >= 500 ? "Iyi" : "Normal");

                csv.AppendLine($"{p.AdSoyad};{puan};%{yuzde};{durum}");
            }

            // UTF-8 formatında dosyayı döndür
            var dosyaVerisi = Encoding.UTF8.GetBytes(csv.ToString());
            return File(dosyaVerisi, "text/csv", $"KHATS_Performans_{DateTime.Now:yyyyMMdd}.csv");

            //ehhe
        }
    }
}