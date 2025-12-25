using KampusKodu.Data;
using KampusKodu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

// BURAYA DİKKAT: "KampusKodu" yerine kendi proje adın yazmalı
namespace KampusKodu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<AppUser> _userManager;

        public HomeController(ApplicationDbContext context, Microsoft.AspNetCore.Identity.UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 1. ANA SAYFA
        public IActionResult Index(string q)
        {
            var viewModel = new HomeViewModel();

            var topicQuery = _context.Topics.Include(t => t.Entries).AsQueryable();

            if (!string.IsNullOrEmpty(q))
            {
                topicQuery = topicQuery.Where(t => t.Title.Contains(q));
                ViewData["ArananKelime"] = q;
            }

            viewModel.Gundem = topicQuery.OrderByDescending(t => t.CreatedDate).ToList();

            viewModel.Akis = _context.Entries
                                    .Include(e => e.Topic)
                                    .OrderByDescending(e => e.CreatedDate)
                                    .Take(20)
                                    .ToList();

            return View(viewModel);
        }

        // 3. YENİ KONU KAYDETME (POST)
        // 3. YENİ KONU KAYDETME (POST)
        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> CreateTopic(CreateTopicViewModel model)
        {
            // Validasyon: Eğer Ders Notu ise Fakülte/Bölüm Zorunlu
            if (model.Type == TopicType.LectureNote)
            {
                if (string.IsNullOrEmpty(model.Faculty)) ModelState.AddModelError("Faculty", "Fakülte seçmelisiniz.");
                if (string.IsNullOrEmpty(model.Department)) ModelState.AddModelError("Department", "Bölüm seçmelisiniz.");
            }
            else
            {
                // Genel başlık ise fakülte/bölüm zorunlu değil
                ModelState.Remove("Faculty");
                ModelState.Remove("Department");
                model.Faculty = null;
                model.Department = null;
            }

            if (ModelState.IsValid)
            {
                string? filePath = null;

                // DOSYA YÜKLEME İŞLEMİ
                if (model.File != null && model.File.Length > 0)
                {
                    var extension = Path.GetExtension(model.File.FileName);
                    var fileName = Guid.NewGuid().ToString() + extension;
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);
                    var path = Path.Combine(uploadDir, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.File.CopyToAsync(stream);
                    }
                    filePath = "/uploads/" + fileName;
                }

                var user = await _userManager.GetUserAsync(User);

                // PUAN SİSTEMİ
                if (user != null)
                {
                    user.Score += (model.Type == TopicType.LectureNote) ? 10 : 5;
                    await _userManager.UpdateAsync(user);
                }

                var topic = new Topic
                {
                    Title = model.Title,
                    CreatedDate = DateTime.Now,
                    Faculty = model.Faculty,
                    Department = model.Department,
                    FilePath = filePath,
                    UserId = user?.Id, // Kullanıcı ID'si
                    Type = model.Type // Türü Kaydet
                };

                _context.Topics.Add(topic);
                await _context.SaveChangesAsync();
                
                 // İlk entry (açıklama)
                var entry = new Entry
                {
                    Content = model.Content,
                    // Eğer giriş yapmışsa ismini, yapmamışsa formdan geleni, o da yoksa "Anonim"
                    Author = user != null ? user.FullName : (string.IsNullOrEmpty(model.Author) ? "Anonim Öğrenci" : model.Author),
                    CreatedDate = DateTime.Now,
                    TopicId = topic.Id,
                    UserId = user?.Id // İlk yorumun sahibi de konuyu açan kişi
                };
                _context.Entries.Add(entry);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
             return RedirectToAction("Index", "Home");
        }

        // 2. İLETİŞİM SAYFASI (Adını tekrar Contact yaptım, hata düzelir)
        public IActionResult Contact()
        {
            return View();
        }

        // 3. KONU DETAY (Bunu kullanmıyorsan silebilirsin ama dursun zararı yok)
        public IActionResult KonuDetay()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
} 