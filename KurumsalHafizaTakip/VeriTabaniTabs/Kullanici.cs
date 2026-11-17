namespace KurumsalHafizaTakip.VeriTabaniTabs
{
    public class Kullanici
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; } = string.Empty;
        public string KullaniciAdi { get; set; } = string.Empty;
        public string Sifre { get; set; } = string.Empty;

        // Bu alanın burada olması şart!
        public string Email { get; set; } = string.Empty;

        public string Rol { get; set; } = "Personel";
        public string Durum { get; set; } = "Aktif";
    }
}