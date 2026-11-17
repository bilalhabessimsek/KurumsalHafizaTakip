namespace KurumsalHafizaTakip.VeriTabaniTabs
{
    public class RevizeVeNotlar
    {
        public int Id { get; set; }
        public int GorevId { get; set; }

        // CS8618 Uyarısını çözen kritik atama: string.Empty
        public string Mesaj { get; set; } = string.Empty;

        // Navigasyon özelliği: null! ile derleyiciye "bu dolu gelecek" diyoruz
        public virtual Gorev Gorev { get; set; } = null!;
    }
}