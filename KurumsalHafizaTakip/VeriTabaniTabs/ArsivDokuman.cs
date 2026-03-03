namespace KurumsalHafizaTakip.VeriTabaniTabs
{
    public class ArsivDokuman
    {
        public int Id { get; set; }
        public int GorevId { get; set; }
        public string DosyaYolu { get; set; }
        public virtual Gorev Gorev { get; set; }
    }
}