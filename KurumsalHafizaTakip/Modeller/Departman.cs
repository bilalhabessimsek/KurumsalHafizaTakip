namespace KurumsalHafizaTakip.Modeller
{
    // Departman tablomuzu temsil eden model sınıfımız
    public class Departman
    {
        // Her departmanın benzersiz kimliği (Primary Key)
        public int Id { get; set; }

        // Departmanın adı (Örn: İnsan Kaynakları)
        public string Ad { get; set; }

        // Departmanın ne iş yaptığına dair kısa bilgi
        public string Aciklama { get; set; }
    }
}