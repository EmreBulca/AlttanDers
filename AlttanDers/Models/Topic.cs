using System.ComponentModel.DataAnnotations;

namespace KampusKodu.Models
{
    public class Topic
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık boş bırakılamaz.")]
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir.")]
        public string Title { get; set; }

        public string? Faculty { get; set; }
        public string? Department { get; set; }
        public string? FilePath { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        // İlişki: Konuyu açan kişi
        public string? UserId { get; set; }
        public AppUser? User { get; set; }

        // Bir başlığın birden çok entry'si olabilir
        public ICollection<Entry>? Entries { get; set; }

        public TopicType Type { get; set; } = TopicType.LectureNote; // Varsayılan: Ders Notu
    }

    public enum TopicType
    {
        LectureNote = 0, // Ders Notu / Soru (10 Puan)
        General = 1      // Genel Başlık (5 Puan)
    }
}