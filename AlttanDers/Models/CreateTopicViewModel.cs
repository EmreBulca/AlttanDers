using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace KampusKodu.Models
{
    public class CreateTopicViewModel
    {
        [Required(ErrorMessage = "Başlık girmelisin.")]
        [StringLength(100, ErrorMessage = "Başlık çok uzun.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "İçerik boş olamaz.")]
        public string Content { get; set; }

        public string? Author { get; set; }

        public string? Faculty { get; set; }
        public string? Department { get; set; }
        public IFormFile? File { get; set; }
        
        public TopicType Type { get; set; } // Ders Notu mu Genel mi?
    }
}