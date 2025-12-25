using Microsoft.AspNetCore.Mvc;
using KampusKodu.Data;
using KampusKodu.Models;
using Microsoft.EntityFrameworkCore;

namespace KampusKodu.Controllers
{
    public class TopicsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<AppUser> _userManager;

        public TopicsController(ApplicationDbContext context, Microsoft.AspNetCore.Identity.UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ... (Details, Create, CreateEntry metodları değişmedi) ...
        // Not: Create ve CreateEntry metodlarında "User.Identity.Name" kullanarak Author'u otomatik alabilirsin
        // ama şimdilik "CreateEntry" POST metodunu güncelleyelim

        // 6. YORUM SİLME İŞLEMİ
        [HttpPost]
        public async Task<IActionResult> DeleteEntry(int id)
        {
            var entry = await _context.Entries.FindAsync(id);
            if (entry == null) return RedirectToAction("Index", "Home");

            var user = await _userManager.GetUserAsync(User);
            var isAdmin = User.IsInRole("Admin");

            // Kural: Admin silebilir YA DA Yorum Sahibi silebilir
            // (Not: entry.UserId ile kontrol ediyoruz)
            
            bool isOwner = user != null && entry.UserId == user.Id;

            if (isAdmin || isOwner)
            {
                int topicId = entry.TopicId;
                _context.Entries.Remove(entry);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = topicId });
            }

            // Yetkisiz
            return RedirectToAction("Details", new { id = entry.TopicId });
        }

        // 7. KONU SİLME İŞLEMİ (YENİ)
        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            var topic = await _context.Topics.Include(t => t.Entries).FirstOrDefaultAsync(t => t.Id == id);
            if (topic == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var isAdmin = User.IsInRole("Admin");
            bool isOwner = user != null && topic.UserId == user.Id;

            if (isAdmin || isOwner)
            {
                // İlişkili entry'ler cascade ile silinebilir ama garanti olsun diye önce onları temizleyelim
                if (topic.Entries != null)
                {
                    _context.Entries.RemoveRange(topic.Entries);
                }

                _context.Topics.Remove(topic);
                await _context.SaveChangesAsync();
                
                return RedirectToAction("Index", "Home");
            }

            return Unauthorized(); // Veya hata sayfası
        }
        
        // DİĞER METODLARI AYNI BIRAKIYORUZ (Şimdilik)
        
        public IActionResult Details(int id)
        {
             var topic = _context.Topics.Include(t => t.Entries).FirstOrDefault(t => t.Id == id);
             if(topic == null) return NotFound();
             return View(topic);
        }

        public IActionResult CreateEntry(int id)
        {
            var topic = _context.Topics.Find(id);
            if(topic == null) return NotFound();

            return View(new CreateEntryViewModel { TopicId = id, TopicTitle = topic.Title });
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateEntry(CreateEntryViewModel model)
        {
            if(ModelState.IsValid)
            {
                 var user = await _userManager.GetUserAsync(User);

                 string? filePath = null;
                 // DOSYA YÜKLEME (Yorumlar için)
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

                 // PUAN SİSTEMİ (Yorum 1 Puan)
                 if (user != null)
                 {
                     user.Score += 1;
                     await _userManager.UpdateAsync(user);
                 }
                 
                 var entry = new Entry
                 {
                     Content = model.Content,
                     CreatedDate = DateTime.Now,
                     TopicId = model.TopicId,
                     UserId = user?.Id, 
                     Author = user != null ? user.FullName : (string.IsNullOrEmpty(model.Author) ? "Anonim" : model.Author),
                     FilePath = filePath // Dosya yolu
                 };
                 _context.Entries.Add(entry);
                 await _context.SaveChangesAsync();
                 return RedirectToAction("Details", new { id = model.TopicId });
            }
            return View(model);
        }
    }
}