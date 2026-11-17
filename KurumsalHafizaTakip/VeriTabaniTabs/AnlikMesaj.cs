namespace KurumsalHafizaTakip.VeriTabaniTabs
{
    public class AnlikMesaj
    {
        public int Id { get; set; }
        public int GonderenId { get; set; }
        public int AliciId { get; set; }
        public string? Icerik { get; set; } // Sadece dosya atılabilir diye null olabilir
        public string? DosyaYolu { get; set; } // Yüklenen belge/resim yolu
        public DateTime Tarih { get; set; } = DateTime.Now;
    }
}