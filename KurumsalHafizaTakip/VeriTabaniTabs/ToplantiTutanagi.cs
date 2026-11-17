namespace KurumsalHafizaTakip.VeriTabaniTabs
{
    public class ToplantiTutanagi
    {
        public int Id { get; set; }
        public string Konu { get; set; } = "";
        public DateTime Tarih { get; set; } = DateTime.Now;
        public string Katilimcilar { get; set; } = "";
        public string AlinanKararlar { get; set; } = "";
        public string? DosyaYolu { get; set; } // İmzalı PDF veya görsel için

        public int EkleyenId { get; set; }
        public Kullanici? Ekleyen { get; set; }
    }
}