using System;
using System.Collections.Generic;

namespace KurumsalHafizaTakip.VeriTabaniTabs
{
    public class Gorev
    {
        public int Id { get; set; }
        public string Baslik { get; set; } = "";
        public string Durum { get; set; } = "Başlamadı";

        // HAFTA 7: İşin Zorluk Puanı
        public int AgirlikPuani { get; set; } = 100;

        // KRİTİK EKSİK: Raporlama için onay tarihi
        public DateTime? OnayTarihi { get; set; }

        // İlişkiler
        public int PersonelId { get; set; }
        public Kullanici? Personel { get; set; }

        public int ProjeId { get; set; }
        public Proje? Proje { get; set; }

        public List<RevizeVeNotlar>? Notlar { get; set; }
    }
}