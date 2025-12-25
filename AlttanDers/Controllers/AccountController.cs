using KampusKodu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KampusKodu.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly KampusKodu.Data.ApplicationDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, KampusKodu.Data.ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // 5. PROFİL SAYFASI
        public async Task<IActionResult> Profile(string id)
        {
            var userId = id ?? _userManager.GetUserId(User);
            if (userId == null) return RedirectToAction("Login");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            // Ekstra: Kullanıcının son aktivitelerini çekelim
            ViewBag.TopicCount = _context.Topics.Count(t => t.UserId == userId);
            ViewBag.EntryCount = _context.Entries.Count(e => e.UserId == userId);
            
            // Son 5 Başlığı
            ViewBag.RecentTopics = _context.Topics.Where(t => t.UserId == userId).OrderByDescending(t => t.CreatedDate).Take(5).ToList();

            return View(user);
        }

        // 6. SIRALAMA (LEADERBOARD)
        public IActionResult Leaderboard()
        {
            // En yüksek puanlı 20 kullanıcı
            var users = _userManager.Users.OrderByDescending(u => u.Score).Take(20).ToList();
            return View(users);
        }

        // GİRİŞ YAP (GET)
        public IActionResult Login()
        {
            return View();
        }

        // GİRİŞ YAP (POST)
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "Hatalı e-posta veya şifre.";
            return View();
        }

        // KAYIT OL (GET)
        public IActionResult Register()
        {
            return View();
        }

        // KAYIT OL (POST)
        [HttpPost]
        public async Task<IActionResult> Register(string fullName, string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ViewBag.Error = "Şifreler eşleşmiyor.";
                return View();
            }

            var user = new AppUser
            {
                UserName = email, // Username olarak da maili kullanıyoruz
                Email = email,
                FullName = fullName
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ViewBag.Error += error.Description + " ";
            }
            return View();
        }

        // ÇIKIŞ YAP
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
