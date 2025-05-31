using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using Ajloun_Project.Models;
using System.Linq;

namespace Ajloun_Project.Controllers.Ali
{
    public class UserPostsController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _env;

        public UserPostsController(MyDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
                return RedirectToAction("signIn", "User");

            var categories = _context.UserPosts
                .Where(p => !string.IsNullOrEmpty(p.Category))
                .Select(p => p.Category)
                .Distinct()
                .ToList();

            ViewBag.Categories = categories;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserPost post, string? CustomCategory, IFormFile? imageFile)
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
                return RedirectToAction("signIn", "User");

            post.UserId = userId.Value;
            post.Status = "قيد المراجعة";
            post.CreatedAt = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(CustomCategory))
                post.Category = CustomCategory;

            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsDir = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsDir))
                    Directory.CreateDirectory(uploadsDir);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var fullPath = Path.Combine(uploadsDir, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                post.FilePath = "/uploads/" + fileName;
            }
            else
            {
                post.FilePath = "";
            }

            _context.UserPosts.Add(post);
            await _context.SaveChangesAsync();

            TempData["Success"] = "✅ تم إرسال المشاركة بنجاح.";
            return RedirectToAction("Create");
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _context.UserPosts
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(posts);
        }

        public async Task<IActionResult> PublicLibrary()
        {
            var approvedPosts = await _context.UserPosts
                .Include(p => p.User)
                .Where(p => p.Status == "موافق عليه" && !string.IsNullOrEmpty(p.Category))
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            var grouped = approvedPosts
                .GroupBy(p => p.Category)
                .ToDictionary(group => group.Key, group => group.ToList());

            ViewBag.GroupedPosts = grouped;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var post = await _context.UserPosts.FindAsync(id);
            if (post == null)
                return NotFound();

            post.Status = "موافق عليه";
            await _context.SaveChangesAsync();

            TempData["Success"] = "✅ تمّت الموافقة على المشاركة.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var post = await _context.UserPosts.FindAsync(id);
            if (post == null)
                return NotFound();

            post.Status = "مرفوض";
            await _context.SaveChangesAsync();

            TempData["Success"] = "❌ تم رفض المشاركة.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int id, string content, string category)
        {
            var post = await _context.UserPosts.FindAsync(id);
            if (post == null)
                return NotFound();

            post.Content = content;
            post.Category = category;
            await _context.SaveChangesAsync();

            TempData["Success"] = "✏️ تم تعديل المشاركة بنجاح.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.UserPosts.FindAsync(id);
            if (post == null)
                return NotFound();

            _context.UserPosts.Remove(post);
            await _context.SaveChangesAsync();

            TempData["Success"] = "🗑️ تم حذف المشاركة.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetPostDetails(int id)
        {
            var post = _context.UserPosts
                .Include(p => p.User) // إن كنت تحتاج بيانات المستخدم
                .FirstOrDefault(p => p.PostId == id);

            if (post == null)
                return NotFound();

            return Json(new
            {
                content = post.Content,
                category = post.Category,
                user = new
                {
                    fullName = post.User?.FullName,
                    email = post.User?.Email,
                    phone = post.User?.Phone
                },
                filePath = post.FilePath,
                status = post.Status
            });
        }

    }
}
