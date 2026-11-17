using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurumsalHafizaTakip.VeriTabaniBaglanti;
using KurumsalHafizaTakip.VeriTabaniTabs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR; // SignalR için gerekli
using KurumsalHafizaTakip.Hubs;      // Hub klasörün için gerekli

namespace KurumsalHafizaTakip.Kontrolculer
{
    [Authorize(Roles = "Admin, Görev Sorumlusu")]
    public class ProjeController : Controller
    {
        private readonly UygulamaDbBaglanti _baglanti;
        private readonly IHubContext<BildirimHub> _hubContext; // Bildirim kanalı

        // Constructor'da Hub'ı içeri alıyoruz
        public ProjeController(UygulamaDbBaglanti baglanti, IHubContext<BildirimHub> hubContext)
        {
            _baglanti = baglanti;
            _hubContext = hubContext;
        }

        // ProjeController.cs içindeki Index metodu
        public async Task<IActionResult> Index()
        {
            // KRİTİK: .Where(p => p.Durum == "Aktif") ekleyerek iptal olanları eliyoruz
            var projeler = await _baglanti.Projeler
                .Include(p => p.Yonetici)
                .Where(p => p.Durum == "Aktif")
                .ToListAsync();

            return View("~/Gorunumler/Proje/Index.cshtml", projeler);
        }

        public IActionResult Ekle()
        {
            ViewBag.Yoneticiler = _baglanti.Kullanicilar.Where(k => k.Rol == "Görev Sorumlusu").ToList();
            return View("~/Gorunumler/Proje/Ekle.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Ekle(Proje p)
        {
            _baglanti.Projeler.Add(p);
            await _baglanti.SaveChangesAsync();

            // DEĞİŞİKLİK BURADA: Sadece atanan yöneticiye bildirim gidiyor
            await _hubContext.Clients.User(p.YoneticiId.ToString())
                .SendAsync("BildirimAl", $"📁 YENİ PROJE: '{p.ProjeAdi}' projesine yönetici olarak atandın!");

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, Görev Sorumlusu")]
        public async Task<IActionResult> IptalEt(int id)
        {
            // Projeyi buluyoruz
            var proje = await _baglanti.Projeler.FindAsync(id);

            if (proje != null)
            {
                // KRİTİK DEĞİŞİKLİK: Remove(proje) satırını sildik!
                // Projeyi silmek yerine durumunu güncelliyoruz ki arşivde görebilelim.
                proje.Durum = "İptal Edildi";

                _baglanti.Projeler.Update(proje);
                await _baglanti.SaveChangesAsync();

                // SignalR Bildirimi: "Silindi" yerine "İptal edilip arşive taşındı" mesajı
                await _hubContext.Clients.All.SendAsync("BildirimAl",
                    $"📜 ARŞİVLEME: '{proje.ProjeAdi}' projesi iptal edildi ve kurumsal hafızaya (arşive) kaldırıldı.");
            }

            return RedirectToAction("Index");
        }
    }
}