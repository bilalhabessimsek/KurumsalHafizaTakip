using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurumsalHafizaTakip.VeriTabaniBaglanti;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;

namespace KurumsalHafizaTakip.Kontrolculer
{
    [Authorize] // Sadece giriş yapanlar bildirimlerini görebilir
    public class BildirimController : Controller
    {
        private readonly UygulamaDbBaglanti _baglanti;

        public BildirimController(UygulamaDbBaglanti baglanti)
        {
            _baglanti = baglanti;
        }

        // Sağ üstteki çan ikonuna basıldığında okunmamış bildirimleri getirecek olan metod
        [HttpGet]
        public async Task<IActionResult> OkunmamislariGetir()
        {
            // Sisteme giriş yapmış kullanıcının ID'sini alıyoruz
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            // Bu kullanıcıya ait, henüz okunmamış olan bildirimleri tarihe göre en yeni üstte olacak şekilde getiriyoruz
            var bildirimler = await _baglanti.Bildirimler
                .Where(b => b.KullaniciId == userId && !b.OkunduMu)
                .OrderByDescending(b => b.Tarih)
                .Select(b => new {
                    id = b.Id,
                    baslik = b.Baslik,
                    mesaj = b.Mesaj,
                    tur = b.Tur,
                    ilgiliKayitId = b.IlgiliKayitId,
                    saat = b.Tarih.ToString("dd MMM HH:mm")
                })
                .ToListAsync();

            return Json(bildirimler);
        }

        // Kullanıcı bildirime tıkladığında onu "Okundu" olarak işaretleyen metod
        [HttpPost]
        public async Task<IActionResult> OkunduIsaretle(int id)
        {
            var bildirim = await _baglanti.Bildirimler.FindAsync(id);
            if (bildirim != null)
            {
                bildirim.OkunduMu = true; // Okundu yap
                await _baglanti.SaveChangesAsync();
                return Json(new { basarili = true });
            }
            return Json(new { basarili = false });
        }
    }
}