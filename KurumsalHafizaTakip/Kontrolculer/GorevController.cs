using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurumsalHafizaTakip.VeriTabaniBaglanti;
using KurumsalHafizaTakip.VeriTabaniTabs;
using KurumsalHafizaTakip.Modeller;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using KurumsalHafizaTakip.Hubs;

namespace KurumsalHafizaTakip.Kontrolculer
{
    [Authorize]
    public class GorevController : Controller
    {
        private readonly UygulamaDbBaglanti _baglanti;
        private readonly IHubContext<BildirimHub> _hubContext;

        public GorevController(UygulamaDbBaglanti baglanti, IHubContext<BildirimHub> hubContext)
        {
            _baglanti = baglanti;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var sorgu = _baglanti.Gorevler.AsQueryable();

            if (userRole != "Admin" && userRole != "Görev Sorumlusu")
                sorgu = sorgu.Where(g => g.PersonelId == userId);

            ViewBag.AktifGorevler = await sorgu.CountAsync(g => g.Durum != "Onaylandı" && g.Durum != "İptal Edildi");
            ViewBag.OnayBekleyen = await sorgu.CountAsync(g => g.Durum == "Onay Bekliyor");
            ViewBag.Tamamlanan = await sorgu.CountAsync(g => g.Durum == "Onaylandı");
            ViewBag.ToplamGorev = await sorgu.CountAsync();

            return View("~/Gorunumler/Gorev/Dashboard.cshtml");
        }

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var sorgu = _baglanti.Gorevler.Include(g => g.Personel).Include(g => g.Proje)
                .Where(g => g.Durum != "İptal Edildi" && g.Durum != "Onaylandı").AsQueryable();

            if (userRole != "Admin" && userRole != "Görev Sorumlusu")
                sorgu = sorgu.Where(g => g.PersonelId == userId);

            return View("~/Gorunumler/Gorev/AnlikTakipTable.cshtml", await sorgu.ToListAsync());
        }

        public async Task<IActionResult> Arsiv()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            var gorevSorgu = _baglanti.Gorevler.Include(g => g.Personel).Include(g => g.Proje)
                .Where(g => g.Durum == "İptal Edildi" || g.Durum == "Onaylandı").AsQueryable();

            var projeSorgu = _baglanti.Projeler.Include(p => p.Yonetici)
                .Where(p => p.Durum == "İptal Edildi" || p.Durum == "Tamamlandı").AsQueryable();

            if (userRole != "Admin" && userRole != "Görev Sorumlusu")
            {
                gorevSorgu = gorevSorgu.Where(g => g.PersonelId == userId);
                projeSorgu = projeSorgu.Where(p => p.YoneticiId == userId);
            }

            var paketModel = new ArsivViewModel
            {
                Gorevler = await gorevSorgu.ToListAsync(),
                Projeler = await projeSorgu.ToListAsync()
            };

            return View("~/Gorunumler/Gorev/Arsiv.cshtml", paketModel);
        }

        public async Task<IActionResult> Detay(int id)
        {
            var gorev = await _baglanti.Gorevler.Include(g => g.Personel).Include(g => g.Notlar)
                .Include(g => g.Proje).ThenInclude(p => p!.Ekip)!.ThenInclude(e => e.Personel)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (gorev == null) return NotFound();

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (userRole != "Admin" && userRole != "Görev Sorumlusu" && gorev.PersonelId != userId)
                return RedirectToAction("Index");

            var ekiptekiIdler = gorev.Proje?.Ekip?.Select(e => e.PersonelId).ToList() ?? new List<int>();
            ViewBag.TumPersoneller = await _baglanti.Kullanicilar
                .Where(k => k.Rol == "Personel" && !ekiptekiIdler.Contains(k.Id)).ToListAsync();

            return View("~/Gorunumler/Gorev/Detay.cshtml", gorev);
        }

        // --- İŞTE O EKSİK OLAN VE SAYFAYI AÇAN GET METODU (GÜNCELLENDİ) ---
        [HttpGet]
        [Authorize(Roles = "Admin, Görev Sorumlusu")]
        public async Task<IActionResult> Ekle()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            // Görev atanacak personelleri getir (Hepsi aktif personel olmalı)
            ViewBag.Personeller = await _baglanti.Kullanicilar.Where(k => k.Rol == "Personel" && k.Durum == "Aktif").ToListAsync();

            // Proje sorgusunu başlat
            var projeSorgu = _baglanti.Projeler.Where(p => p.Durum == "Aktif").AsQueryable();

            // KRİTİK GÜVENLİK DUVARI:
            // Eğer giren kişi Admin DEĞİLSE (yani Görev Sorumlusu ise), SADECE KENDİ yönettiği projeleri görsün.
            // Admin ise bu if bloğuna girmez, şirketteki tüm projeleri görür.
            if (userRole != "Admin")
            {
                projeSorgu = projeSorgu.Where(p => p.YoneticiId == userId);
            }

            ViewBag.Projeler = await projeSorgu.ToListAsync();

            return View("~/Gorunumler/Gorev/Ekle.cshtml");
        }

        // --- FORMU VERİTABANINA YAZAN POST METODU ---
        [HttpPost]
        [Authorize(Roles = "Admin, Görev Sorumlusu")]
        public async Task<IActionResult> Ekle(Gorev yeniGorev)
        {
            yeniGorev.Durum = "Başlamadı";
            _baglanti.Gorevler.Add(yeniGorev);
            await _baglanti.SaveChangesAsync();

            // Sadece görevin atandığı personele bildirim gidiyor
            await _hubContext.Clients.User(yeniGorev.PersonelId.ToString())
                .SendAsync("BildirimAl", $"🚀 YENİ İŞ: Sana '{yeniGorev.Baslik}' görevi atandı!");

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, Görev Sorumlusu")]
        public async Task<IActionResult> IptalEt(int id)
        {
            var gorev = await _baglanti.Gorevler.FindAsync(id);
            if (gorev != null)
            {
                gorev.Durum = "İptal Edildi";
                await _baglanti.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("BildirimAl", $"⚠️ GÖREV İPTAL: {gorev.Baslik} iptal edildi.");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> NotEkle(int gorevId, string mesaj)
        {
            if (!string.IsNullOrEmpty(mesaj))
            {
                _baglanti.Notlar.Add(new RevizeVeNotlar { GorevId = gorevId, Mesaj = mesaj });
                await _baglanti.SaveChangesAsync();

                // İstersen bunu da sadece ilgili görev sahibine gidecek şekilde güncelleyebilirsin ileride
                await _hubContext.Clients.All.SendAsync("BildirimAl", "Yeni bir kurumsal hafıza notu eklendi! 📜");
            }
            return RedirectToAction("Detay", new { id = gorevId });
        }

        [HttpPost]
        public async Task<IActionResult> DurumGuncelle(int id, string yeniDurum)
        {
            var gorev = await _baglanti.Gorevler.FindAsync(id);
            if (gorev != null)
            {
                gorev.Durum = yeniDurum;
                if (yeniDurum == "Onaylandı") gorev.OnayTarihi = DateTime.Now;
                await _baglanti.SaveChangesAsync();

                await _hubContext.Clients.All.SendAsync("BildirimAl", $"Görev durumu '{yeniDurum}' olarak güncellendi! ✅");
            }
            return RedirectToAction("Detay", new { id = id });
        }

        // GorevController.cs içerisine eklenecekler
        [HttpPost]
        [Authorize(Roles = "Admin, Görev Sorumlusu")]
        public async Task<IActionResult> EkipEkle(int gorevId, int projeId, int personelId)
        {
            // Personel zaten bu projede var mı kontrol edelim ki çift kayıt atmasın
            var projedeVarMi = await _baglanti.ProjeEkipleri
                .AnyAsync(e => e.ProjeId == projeId && e.PersonelId == personelId);

            if (!projedeVarMi)
            {
                _baglanti.ProjeEkipleri.Add(new ProjeEkip { ProjeId = projeId, PersonelId = personelId });
                await _baglanti.SaveChangesAsync();
            }

            // İşlem bitince aynı görevin detay sayfasına geri dön
            return RedirectToAction("Detay", new { id = gorevId });
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Görev Sorumlusu")]
        public async Task<IActionResult> PersonelCikar(int gorevId, int ekipId)
        {
            var ekipUyesi = await _baglanti.ProjeEkipleri.FindAsync(ekipId);
            if (ekipUyesi != null)
            {
                _baglanti.ProjeEkipleri.Remove(ekipUyesi);
                await _baglanti.SaveChangesAsync();
            }
            return RedirectToAction("Detay", new { id = gorevId });
        }

    }
}