using KampusKodu.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KampusKodu.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Veritabanındaki tablolarımız
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Entry> Entries { get; set; }
    }
}