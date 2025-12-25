using KampusKodu.Data;
using KampusKodu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace KampusKodu.Services
{
    public static class DbSeeder
    {
        public static async Task SeedAdminUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // 1. Admin Rolü Var mı? Yoksa oluştur.
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // 2. Admin Kullanıcısı Var mı?
            var adminEmail = "emreblca7555@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Sistem Yöneticisi",
                    EmailConfirmed = true
                };

                // Şifre: Kolay123456
                var result = await userManager.CreateAsync(newAdmin, "Kolay123456");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}
