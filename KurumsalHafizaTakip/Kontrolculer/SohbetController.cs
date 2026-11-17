using KurumsalHafizaTakip.Hubs;
using KurumsalHafizaTakip.Modeller;
using KurumsalHafizaTakip.VeriTabaniBaglanti;
using KurumsalHafizaTakip.VeriTabaniTabs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Security.Claims;

namespace KurumsalHafizaTakip.Kontrolculer
{
    [Authorize]
    public class SohbetController : Controller
    {
        private readonly UygulamaDbBaglanti _baglanti;
        private readonly IHubContext<BildirimHub> _hubContext;
        private readonly IWebHostEnvironment _env;

        public SohbetController(UygulamaDbBaglanti baglanti, IHubContext<BildirimHub> hubContext, IWebHostEnvironment env)
        {
            _baglanti = baglanti;
            _hubContext = hubContext;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            ViewBag.Kullanicilar = await _baglanti.Kullanicilar.Where(k => k.Id != userId && k.Durum == "Aktif").ToListAsync();
            ViewBag.BenimId = userId;
            return View("~/Gorunumler/Sohbet/Index.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> GecmisGetir(int karsiId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            // ÇÖZÜM 1: Yeni mesajların üstte görünmesi için OrderByDescending kullanıyoruz
            var gecmis = await _baglanti.AnlikMesajlar
                .Where(m => (m.GonderenId == userId && m.AliciId == karsiId) || (m.GonderenId == karsiId && m.AliciId == userId))
                .OrderByDescending(m => m.Tarih)
                .Select(m => new {
                    gonderenId = m.GonderenId,
                    icerik = m.Icerik,
                    dosyaYolu = m.DosyaYolu,
                    saat = m.Tarih.ToString("HH:mm")
                }).ToListAsync();

            return Json(gecmis);
        }

        [HttpPost]
        public async Task<IActionResult> Gonder([FromForm] int aliciId, [FromForm] string? icerik, IFormFile? dosya)
        {
            if (string.IsNullOrWhiteSpace(icerik) && dosya == null) return BadRequest();

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            var yeniMesaj = new AnlikMesaj { GonderenId = userId, AliciId = aliciId, Icerik = icerik ?? "" };

            if (dosya != null && dosya.Length > 0)
            {
                var klasor = Path.Combine(_env.WebRootPath, "dosyalar", "mesajlar");
                if (!Directory.Exists(klasor)) Directory.CreateDirectory(klasor);

                var dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(dosya.FileName);
                using (var stream = new FileStream(Path.Combine(klasor, dosyaAdi), FileMode.Create))
                {
                    await dosya.CopyToAsync(stream);
                }
                yeniMesaj.DosyaYolu = "/dosyalar/mesajlar/" + dosyaAdi;
            }

            // 1. Mesajı veritabanına kaydet
            _baglanti.AnlikMesajlar.Add(yeniMesaj);
            await _baglanti.SaveChangesAsync();

            var gonderen = await _baglanti.Kullanicilar.FindAsync(userId);
            // Senin yazdığın gibi AdSoyad kullanıyoruz
            string gonderenAdi = gonderen != null ? gonderen.AdSoyad : "Biri";

            // -------------------------------------------------------------------------
            // İŞTE SENİN KODUNDA EKSİK OLAN VE ÇAN İKONUNU ÇALIŞTIRACAK KISIM:
            // -------------------------------------------------------------------------
            var kaliciBildirim = new Bildirim
            {
                KullaniciId = aliciId,
                Baslik = "Yeni Mesaj 💬",
                Mesaj = $"{gonderenAdi} size yeni bir mesaj gönderdi.",
                Tur = "Mesaj",
                IlgiliKayitId = userId
            };

            _baglanti.Bildirimler.Add(kaliciBildirim);
            await _baglanti.SaveChangesAsync();
            // -------------------------------------------------------------------------

            // 2. İşlem: Sohbet ekranı açıksa mesajı oraya düşür
            await _hubContext.Clients.User(aliciId.ToString())
                .SendAsync("CanliMesajGeldi", userId, yeniMesaj.Icerik, yeniMesaj.Tarih.ToString("HH:mm"), yeniMesaj.DosyaYolu);

            // 3. İşlem: Sağ üstteki mavi kutuyu tetikleyen kod
            await _hubContext.Clients.User(aliciId.ToString())
                .SendAsync("BildirimAl", $"💬 {gonderenAdi} size yeni bir mesaj gönderdi!");

            return Json(new { basarili = true, saat = yeniMesaj.Tarih.ToString("HH:mm"), dosyaYolu = yeniMesaj.DosyaYolu });
        }
    }
}