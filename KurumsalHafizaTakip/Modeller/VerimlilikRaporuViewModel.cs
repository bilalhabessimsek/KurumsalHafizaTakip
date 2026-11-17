namespace KurumsalHafizaTakip.Modeller
{
    public class VerimlilikRaporuViewModel
    {
        public string PersonelAdi { get; set; } = "";
        public int ToplamPuan { get; set; } // Tamamlanan işlerin toplam puanı
        public double VerimlilikYuzdesi { get; set; }
        public bool FazlaMesaiYapti { get; set; } // 1000 puan sınırı
        public string PerformansDurumu { get; set; } = "";
    }
}