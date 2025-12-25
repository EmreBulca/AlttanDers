using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; // Dosya için

namespace KampusKodu.Models
{
    public class CreateEntryViewModel
    {
        public int TopicId { get; set; } // Hangi başlığa yazıyoruz?

        public string? TopicTitle { get; set; } // Başlığın adını ekranda göstermek için

        [Required(ErrorMessage = "Bir şeyler yazmalısın.")]
        public string Content { get; set; }

        public string? Author { get; set; }

        public IFormFile? File { get; set; } // Yorum görselleri için
    }
}