using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KampusKodu.Models
{
    public class Entry
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "İçerik girmelisiniz.")]
        public string Content { get; set; }

        public string? Author { get; set; } = "Anonim Öğrenci"; // Giriş yapmayanlar için varsayılan

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Hangi başlığa ait? (Foreign Key)
        // Hangi başlığa ait? (Foreign Key)
        public int TopicId { get; set; }

        [ForeignKey("TopicId")]
        public Topic? Topic { get; set; }

        public string? FilePath { get; set; } // Yorumlarda dosya desteği

        // İlişki: Yorumu yapan kişi
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
    }
}