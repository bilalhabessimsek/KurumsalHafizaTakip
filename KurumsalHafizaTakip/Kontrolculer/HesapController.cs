using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using KurumsalHafizaTakip.VeriTabaniBaglanti;
using Microsoft.EntityFrameworkCore;

namespace KurumsalHafizaTakip.Kontrolculer
{
    public class HesapController : Controller
    {
        private readonly UygulamaDbBaglanti _baglanti;

        public HesapController(UygulamaDbBaglanti baglanti)
        {
            _baglanti = baglanti;
        }

        // GET: /Hesap/Giris
        [HttpGet]
        public IActionResult Giris()
        {
            // GitHub yapındaki özel klasör yolunu mühürledik
            return View("~/Gorunumler/Hesap/LoginPage.cshtml");
        }

        // POST: /Hesap/Giris
        [HttpPost]
        public async Task<IActionResult> Giris(string kullaniciAdi, string sifre)
        {
            // 1. Veritabanında kullanıcıyı bul
            var kullanici = await _baglanti.Kullanicilar
                .FirstOrDefaultAsync(u => u.KullaniciAdi == kullaniciAdi && u.Sifre == sifre);

            if (kullanici == null)
            {
                ViewBag.Hata = "Kullanıcı adı veya şifre hatalı!";
                return View("~/Gorunumler/Hesap/LoginPage.cshtml");
            }

            // 2. Aktiflik durumunu kontrol et
            if (kullanici.Durum != "Aktif")
            {
                ViewBag.Hata = "Hesabınız pasif durumdadır!";
                return View("~/Gorunumler/Hesap/LoginPage.cshtml");
            }

            // 3. Güvenlik Haklarını (Claims) Tanımla
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, kullanici.AdSoyad),
                new Claim(ClaimTypes.NameIdentifier, kullanici.Id.ToString()),
                new Claim(ClaimTypes.Role, kullanici.Rol)
            };

            // 4. Kimlik Nesnesini Oluştur (identity ismi burada tanımlandı)
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // 5. Oturumu Başlat (Buradaki identity ile yukarıdaki isim artık eşleşiyor!)
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return RedirectToAction("Dashboard", "Gorev");
        }

        public async Task<IActionResult> Cikis()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Giris");
        }
    }
}