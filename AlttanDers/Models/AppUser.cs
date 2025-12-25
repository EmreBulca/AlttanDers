using Microsoft.AspNetCore.Identity;

namespace KampusKodu.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public int Score { get; set; } = 0; // Puan Sistemi
    }
}
