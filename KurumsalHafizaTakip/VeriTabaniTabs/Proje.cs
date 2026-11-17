namespace KurumsalHafizaTakip.VeriTabaniTabs
{
    public class Proje
    {
        public int Id { get; set; }
        public string ProjeAdi { get; set; } = "";
        public string Durum { get; set; } = "Aktif";
        public string? Aciklama { get; set; }
        public int? YoneticiId { get; set; }
        public Kullanici? Yonetici { get; set; }

        public List<ProjeEkip>? Ekip { get; set; }
    }
}