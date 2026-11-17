using Microsoft.AspNetCore.Mvc;
using System.Linq;
using KurumsalHafizaTakip.VeriTabaniBaglanti;
using KurumsalHafizaTakip.Modeller;

namespace KurumsalHafizaTakip.Kontrolculer
{
    public class DepartmanController : Controller
    {
        // Veritabanı bağlantımızı tutacağımız değişken
        private readonly UygulamaDbBaglanti _context;

        // Constructor (Yapıcı metot) ile veritabanı bağlantımızı başlatıyoruz
        public DepartmanController(UygulamaDbBaglanti context)
        {
            _context = context;
        }

        // 1. ANA SAYFA: Tüm departmanları listeler
        public IActionResult Index()
        {
            // Veritabanından tüm departmanları çekiyoruz
            var departmanlar = _context.Departmanlar.ToList();

            // HATA ÇÖZÜMÜ: Sisteme özel "Gorunumler" klasörümüzün yolunu tam olarak veriyoruz
            return View("~/Gorunumler/Departman/Index.cshtml", departmanlar);
        }

        // 2. EKLE SAYFASI: Sadece boş formu ekrana getirir
        [HttpGet]
        public IActionResult Ekle()
        {
            // HATA ÇÖZÜMÜ: Sisteme özel yol ile boş formu açıyoruz
            return View("~/Gorunumler/Departman/Ekle.cshtml");
        }

        // 3. KAYDETME İŞLEMİ: Formdan gelen veriyi veritabanına yazar
        [HttpPost]
        public IActionResult Ekle(Departman yeniDepartman)
        {
            // Formdan gelen veriler kurallara uygun mu kontrolü
            if (ModelState.IsValid)
            {
                // Yeni departmanı tabloya ekle
                _context.Departmanlar.Add(yeniDepartman);
                // Değişiklikleri kaydet
                _context.SaveChanges();
                // İşlem bitince listeleme (Index) sayfasına yönlendir
                return RedirectToAction("Index");
            }

            // Eğer bir hata varsa sayfayı girilen verilerle ve özel yolumuzla tekrar göster
            return View("~/Gorunumler/Departman/Ekle.cshtml", yeniDepartman);
        }

        // 4. SİLME İŞLEMİ: ID'si verilen departmanı bulup siler
        public IActionResult Sil(int id)
        {
            // Silinecek departmanı bul
            var departman = _context.Departmanlar.Find(id);
            if (departman != null)
            {
                // Departmanı tablodan çıkar
                _context.Departmanlar.Remove(departman);
                // Değişiklikleri kaydet
                _context.SaveChanges();
            }

            // İşlem bitince listeleme sayfasına yönlendir
            return RedirectToAction("Index");
        }
    }
}