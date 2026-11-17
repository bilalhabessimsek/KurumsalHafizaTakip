using System;

namespace KurumsalHafizaTakip.Modeller
{
    public class Bildirim
    {
        public int Id { get; set; }

        // Bildirimin kime gittiğini tutar
        public int KullaniciId { get; set; }

        // Bildirimin Başlığı (Örn: "Yeni Görev Ataması")
        public string Baslik { get; set; }

        // Bildirimin İçeriği (Örn: "Yıllık Finans Raporu hazırlama işine atandınız.")
        public string Mesaj { get; set; }

        // Bildirimin Türü (Örn: "Görev", "Duyuru", "Sistem", "Toplantı")
        // Bu sayede işe atanma ile normal duyuruyu birbirinden ayırabilirsin
        public string Tur { get; set; }

        // Hangi göreve/projeye atandıysa onun ID'si (Tıklayınca o sayfanın açılması için)
        public int? IlgiliKayitId { get; set; }

        // Kullanıcı bu bildirimi gördü mü? (Sağ üstteki kırmızı bildirim sayısını (1,2,3) silmek için)
        public bool OkunduMu { get; set; } = false;

        // Olayın gerçekleştiği tarih
        public DateTime Tarih { get; set; } = DateTime.Now;
    }
}