using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurumsalHafizaTakip.VeriTabaniBaglanti;
using KurumsalHafizaTakip.VeriTabaniTabs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace KurumsalHafizaTakip.Kontrolculer
{
    [Authorize] // Sadece giriş yapanlar görebilir
    public class ToplantiController : Controller
    {
        private readonly UygulamaDbBaglanti _baglanti;
        private readonly IWebHostEnvironment _env; // Dosya kaydetmek için ortam değişkeni

        public ToplantiController(UygulamaDbBaglanti baglanti, IWebHostEnvironment env)
        {
            _baglanti = baglanti;
            _env = env;
        }

        // Tutanakların Listelendiği Sayfa
        public async Task<IActionResult> Index()
        {
            var tutanaklar = await _baglanti.ToplantiTutanaklari
                .Include(t => t.Ekleyen)
                .OrderByDescending(t => t.Tarih)
                .ToListAsync();

            return View("~/Gorunumler/Toplanti/Index.cshtml", tutanaklar);
        }

        // Yeni Tutanak Formunu Açan Metot
        [HttpGet]
        [Authorize(Roles = "Admin, Görev Sorumlusu")]
        public IActionResult Ekle()
        {
            return View("~/Gorunumler/Toplanti/Ekle.cshtml");
        }

        // Formu ve Dosyayı Kaydeden Metot
        [HttpPost]
        [Authorize(Roles = "Admin, Görev Sorumlusu")]
        public async Task<IActionResult> Ekle(ToplantiTutanagi t, IFormFile? dosya)
        {
            t.EkleyenId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            // Eğer PDF veya Resim yüklendiyse sunucuya kaydet
            if (dosya != null && dosya.Length > 0)
            {
                var klasor = Path.Combine(_env.WebRootPath, "dosyalar", "tutanaklar");
                if (!Directory.Exists(klasor)) Directory.CreateDirectory(klasor);

                var dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(dosya.FileName);
                var tamYol = Path.Combine(klasor, dosyaAdi);

                using (var stream = new FileStream(tamYol, FileMode.Create))
                {
                    await dosya.CopyToAsync(stream);
                }
                t.DosyaYolu = "/dosyalar/tutanaklar/" + dosyaAdi;
            }

            _baglanti.ToplantiTutanaklari.Add(t);
            await _baglanti.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}