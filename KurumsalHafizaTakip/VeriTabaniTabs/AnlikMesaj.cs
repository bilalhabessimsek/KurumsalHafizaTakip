namespace KurumsalHafizaTakip.VeriTabaniTabs
{
    public class AnlikMesaj
    {
        public int Id { get; set; }
        public string Icerik { get; set; }
        public int GonderenId { get; set; }
        public virtual Kullanici Gonderen { get; set; }
    }
}