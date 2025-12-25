namespace KampusKodu.Models
{
    public class HomeViewModel
    {
        // Sol menüdeki başlıklar için
        public List<Topic> Gundem { get; set; }

        // Orta alandaki canlı akış için
        public List<Entry> Akis { get; set; }
    }
}