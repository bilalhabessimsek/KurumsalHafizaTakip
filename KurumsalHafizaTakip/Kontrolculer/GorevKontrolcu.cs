using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurumsalHafizaTakip.VeriTabaniBaglanti;

namespace KurumsalHafizaTakip.Kontrolculer
{
    public class GorevKontrolcu : Controller
    {
        private readonly UygulamaDbBaglanti _baglanti;
        public GorevKontrolcu(UygulamaDbBaglanti baglanti) { _baglanti = baglanti; }

        public IActionResult Index()
        {
            var gorevler = _baglanti.Gorevler.Include(g => g.Personel).ToList();
            return View(gorevler);
        }
    }
}