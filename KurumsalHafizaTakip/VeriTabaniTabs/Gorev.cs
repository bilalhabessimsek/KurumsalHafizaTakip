namespace KurumsalHafizaTakip.VeriTabaniTabs
{
    public class Gorev
    {
        public int Id { get; set; }
        public string Baslik { get; set; }
        public string Durum { get; set; } // Onay Bekliyor, Bitti
        public int PersonelId { get; set; }
        public virtual Kullanici Personel { get; set; }
    }
}