using Ajloun_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Project.Controllers.Ali
{
    public class NewsController : Controller
    {
        
            private readonly MyDbContext _context;

            public NewsController(MyDbContext context)
            {
                _context = context;
            }

        // عرض الأخبار
        public IActionResult Index(string? category, string? search)
        {
            // استرجاع الأخبار التي حالتها Published فقط
            var news = _context.News
                .Where(n => n.Status == "Published");

            // فلترة حسب الفئة (category)
            if (!string.IsNullOrEmpty(category))
            {
                if (category.ToLower() == "latest")
                {
                    news = news.OrderByDescending(n => n.PublishDate);
                }
                else
                {
                    news = news.Where(n => n.Category == category);
                }
            }

            // فلترة حسب البحث في العنوان أو المحتوى
            if (!string.IsNullOrEmpty(search))
            {
                news = news.Where(n =>
                    n.Title.Contains(search) ||
                    n.Content.Contains(search));
            }

            // استخراج الفئات الموجودة فعلياً في الأخبار وتمريرها للفيو
            var categories = _context.News
                .Where(n => !string.IsNullOrEmpty(n.Category))
                .Select(n => n.Category)
                .Distinct()
                .ToList();

            ViewBag.Categories = categories;

            // إرجاع البيانات إلى الفيو بعد ترتيبها تنازلياً حسب التاريخ
            return View(news.OrderByDescending(n => n.PublishDate).ToList());
        }


        // تفاصيل خبر
        public IActionResult Details(int id)
            {
                var news = _context.News.FirstOrDefault(n => n.NewsId == id);
                if (news == null) return NotFound();
                return View(news);
            }

            // GET: Create
            [HttpGet]
            public IActionResult Create()
            {
                return View();
            }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(News news, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                news.PublishDate = DateTime.Now;
                news.Status = "Published";

                // ✅ رفع صورة جديدة فقط
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fileStream);
                    }

                    news.ImageUrl = "/images/" + uniqueFileName;
                }
                else
                {
                    // ❌ في حال لم تُرفع صورة
                    news.ImageUrl = null;
                }

                _context.News.Add(news);
                _context.SaveChanges();

                return RedirectToAction("AdminList");
            }

            return View(news);
        }


        // عرض كل الأخبار للإدارة
        public IActionResult AdminList()
            {
                var newsList = _context.News
                    .OrderByDescending(n => n.PublishDate)
                    .ToList();

                return View(newsList);
            }

            // حذف خبر
            [HttpPost]
            public IActionResult AdminDelete(int id)
            {
                var news = _context.News.Find(id);
                if (news != null)
                {
                    _context.News.Remove(news);
                    _context.SaveChanges();
                }
                return RedirectToAction("AdminList");
            }
            // GET: تعديل خبر
            [HttpGet]
            public IActionResult AdminEdit(int id)
            {
                var news = _context.News.Find(id);
                if (news == null) return NotFound();

                // استخراج الصور من wwwroot/images
                var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                var images = Directory.Exists(imageFolder)
                    ? Directory.GetFiles(imageFolder).Select(f => Path.GetFileName(f)).ToList()
                    : new List<string>();

                ViewBag.ImageList = images;

                return View(news);
            }


        // POST: تعديل خبر
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminEdit(News news, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                var existingNews = _context.News.FirstOrDefault(n => n.NewsId == news.NewsId);
                if (existingNews == null) return NotFound();

                existingNews.Title = news.Title;
                existingNews.Content = news.Content;
                existingNews.Category = news.Category;
                existingNews.Status = news.Status;

                // ✅ رفع صورة جديدة فقط
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fileStream);
                    }

                    existingNews.ImageUrl = "/images/" + uniqueFileName;
                }

                _context.News.Update(existingNews);
                _context.SaveChanges();

                return RedirectToAction("AdminList");
            }

            return View("Admin_Edit_News", news); // ← يفضل تحديد اسم الفيو إذا كان مختلفًا عن اسم الأكشن
        }

    }
}

    
