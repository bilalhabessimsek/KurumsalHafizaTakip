namespace KurumsalHafizaTakip.VeriTabaniTabs
{
    public class Kullanici
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; }
        public string Email { get; set; }
        public string Sifre { get; set; }
        public string Rol { get; set; } // Admin, IK, Yonetici, Calisan
    }
}