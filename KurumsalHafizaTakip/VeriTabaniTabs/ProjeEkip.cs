namespace KurumsalHafizaTakip.VeriTabaniTabs
{
    public class ProjeEkip
    {
        public int Id { get; set; }
        public int ProjeId { get; set; }
        public virtual Proje Proje { get; set; } // Proje ile ilişki
        public int PersonelId { get; set; }
        public virtual Kullanici Personel { get; set; } // Personel ile ilişki
    }
}