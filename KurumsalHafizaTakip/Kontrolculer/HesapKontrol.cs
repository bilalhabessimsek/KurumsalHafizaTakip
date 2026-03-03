using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using KurumsalHafizaTakip.VeriTabaniBaglanti;

namespace KurumsalHafizaTakip.Kontrolculer
{
    public class HesapKontrol : Controller
    {
        private readonly UygulamaDbBaglanti _baglanti;
        public HesapKontrol(UygulamaDbBaglanti baglanti) { _baglanti = baglanti; }

        public IActionResult Giris() => View();

        [HttpPost]
        public async Task<IActionResult> Giris(string email, string sifre)
        {
            var kullanici = _baglanti.Kullanicilar.FirstOrDefault(k => k.Email == email && k.Sifre == sifre);
            if (kullanici != null)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, kullanici.AdSoyad) };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                return RedirectToAction("Index", "Gorev");
            }
            return View();
        }

        public async Task<IActionResult> Cikis() { await HttpContext.SignOutAsync(); return RedirectToAction("Giris"); }
    }
}