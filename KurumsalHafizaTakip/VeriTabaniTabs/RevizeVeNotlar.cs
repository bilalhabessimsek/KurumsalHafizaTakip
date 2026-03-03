namespace KurumsalHafizaTakip.VeriTabaniTabs
{
    public class RevizeVeNotlar
    {
        public int Id { get; set; }
        public int GorevId { get; set; }
        public string Mesaj { get; set; }
        public virtual Gorev Gorev { get; set; }
    }
}