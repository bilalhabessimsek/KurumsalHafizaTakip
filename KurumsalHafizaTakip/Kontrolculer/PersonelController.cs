using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurumsalHafizaTakip.VeriTabaniBaglanti;
using KurumsalHafizaTakip.VeriTabaniTabs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR; // Yeni eklendi
using KurumsalHafizaTakip.Hubs;      // Yeni eklendi

namespace KurumsalHafizaTakip.Kontrolculer
{
    [Authorize(Roles = "Admin, Personel Görevlisi")]
    public class PersonelController : Controller
    {
        private readonly UygulamaDbBaglanti _baglanti;
        private readonly IHubContext<BildirimHub> _hubContext; // Yeni: SignalR bağlantısı

        // Constructor'da SignalR Hub'ı içeri alıyoruz
        public PersonelController(UygulamaDbBaglanti baglanti, IHubContext<BildirimHub> hubContext)
        {
            _baglanti = baglanti;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            var personeller = await _baglanti.Kullanicilar
                .Where(u => u.Durum == "Aktif")
                .ToListAsync();
            return View("~/Gorunumler/Personel/Index.cshtml", personeller);
        }

        public IActionResult Ekle() => View("~/Gorunumler/Personel/Ekle.cshtml");

        [HttpPost]
        public async Task<IActionResult> Ekle(Kullanici yeniPersonel)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(yeniPersonel.Email))
                    yeniPersonel.Email = "yeni@khats.com";

                yeniPersonel.Durum = "Aktif";

                _baglanti.Kullanicilar.Add(yeniPersonel);
                await _baglanti.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("~/Gorunumler/Personel/Ekle.cshtml", yeniPersonel);
        }

        // KRİTİK GÜNCELLEME: Anlık Kovma Sinyali Eklenen Metot
        public async Task<IActionResult> Sil(int id)
        {
            var personel = await _baglanti.Kullanicilar.FindAsync(id);

            if (personel != null && personel.Rol != "Admin")
            {
                personel.Durum = "Pasif";
                _baglanti.Kullanicilar.Update(personel);
                await _baglanti.SaveChangesAsync();

                // KRİTİK ADIM: SignalR üzerinden sadece bu kullanıcıya mesaj gönderiyoruz
                // 'OturumuKapat' sinyali layout'taki JavaScript tarafından yakalanacak
                await _hubContext.Clients.User(personel.Id.ToString()).SendAsync("OturumuKapat");
            }
            return RedirectToAction(nameof(Index));


        }
    }
}