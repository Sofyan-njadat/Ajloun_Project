using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ajloun_Project.Models;

namespace Ajloun_Project.Controllers
{
    public class ArtWorkshopsController : Controller
    {
        private readonly MyDbContext _context;

        public ArtWorkshopsController(MyDbContext context)
        {
            _context = context;
        }

        // ✅ عرض الورشات للإدارة
        public async Task<IActionResult> Index()
        {
            var workshops = await _context.ArtWorkshops
                .Where(w => !w.IsHidden)
                .OrderByDescending(w => w.StartDate)
                .ToListAsync();

            return View(workshops);
        }

        // ✅ إنشاء ورشة جديدة (من المودال)
        [HttpPost]
        public async Task<IActionResult> Create(ArtWorkshop workshop, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            // ✅ رفع الصورة
            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "workshops");
                Directory.CreateDirectory(uploadsFolder);
                string uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                workshop.ImageUrl = "/images/workshops/" + uniqueName;
            }

            _context.ArtWorkshops.Add(workshop);
            await _context.SaveChangesAsync();
            TempData["AlertType"] = "success";
            TempData["AlertMessage"] = "تمت إضافة الورشة بنجاح!";
            return RedirectToAction("Index");
        }

        // ✅ تعديل ورشة (من المودال)
        [HttpPost]
        public async Task<IActionResult> Edit(ArtWorkshop workshop)
        {
            var existing = await _context.ArtWorkshops.FindAsync(workshop.Id);
            if (existing == null) return NotFound();

            existing.Title = workshop.Title;
            existing.Description = workshop.Description;
            existing.StartDate = workshop.StartDate;
            existing.EndDate = workshop.EndDate;
            existing.MinAge = workshop.MinAge;
            existing.Category = workshop.Category;
            existing.ImageUrl = workshop.ImageUrl;

            _context.Update(existing);
            await _context.SaveChangesAsync();

            TempData["AlertType"] = "success";
            TempData["AlertMessage"] = "تم التعديل بنجاح";
            return RedirectToAction("Index");
        }

        // ✅ إخفاء ورشة
        [HttpPost]
        public async Task<IActionResult> Hide(int id)
        {
            var workshop = await _context.ArtWorkshops.FindAsync(id);
            if (workshop == null) return NotFound();

            workshop.IsHidden = true;
            await _context.SaveChangesAsync();

            TempData["AlertType"] = "info";
            TempData["AlertMessage"] = "تم إخفاء الورشة";
            return RedirectToAction("Index");
        }

        // ✅ عرض الحجوزات (صفحة مستقلة)
        public async Task<IActionResult> ManageRegistrations()
        {
            var regs = await _context.WorkshopRegistrations
                .Include(r => r.Workshop)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return View(regs);
        }

        // ✅ قبول حجز
        [HttpPost]
        public async Task<IActionResult> AcceptRegistration(int id)
        {
            var reg = await _context.WorkshopRegistrations.FindAsync(id);
            if (reg == null) return NotFound();

            reg.Status = "Accepted";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageRegistrations));
        }

        // ✅ رفض حجز
        [HttpPost]
        public async Task<IActionResult> RejectRegistration(int id, string reason)
        {
            var reg = await _context.WorkshopRegistrations.FindAsync(id);
            if (reg == null) return NotFound();

            reg.Status = "Rejected";
            reg.RejectionReason = reason;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageRegistrations));
        }

        public async Task<IActionResult> Explore(string? category)
        {
            var workshopsQuery = _context.ArtWorkshops.Where(w => !w.IsHidden);

            if (!string.IsNullOrEmpty(category))
                workshopsQuery = workshopsQuery.Where(w => w.Category == category);

            var workshops = await workshopsQuery.OrderByDescending(w => w.StartDate).ToListAsync();

            ViewBag.SelectedCategory = category;
            ViewBag.AllCategories = await _context.ArtWorkshops
                .Where(w => !string.IsNullOrEmpty(w.Category))
                .Select(w => w.Category)
                .Distinct()
                .ToListAsync();

            return View(workshops);
        }
        [HttpPost]
        public async Task<IActionResult> RegisterToWorkshop(int workshopId, IFormFile birthCertificateImage)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
                return RedirectToAction("signIn", "User");

            string? filePath = null;
            if (birthCertificateImage != null && birthCertificateImage.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(birthCertificateImage.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ImageWorkshop");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var savePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await birthCertificateImage.CopyToAsync(stream);
                }

                filePath = "/ImageWorkshop/" + fileName;
            }

            var registration = new WorkshopRegistration
            {
                UserId = userId.Value,
                WorkshopId = workshopId,
                Status = "Pending",
                BirthCertificateImage = filePath,
                CreatedAt = DateTime.Now
            };

            _context.WorkshopRegistrations.Add(registration);
            await _context.SaveChangesAsync();

            TempData["AlertType"] = "success";
            TempData["AlertMessage"] = "تم إرسال طلب التسجيل بنجاح، بانتظار الموافقة.";

            return RedirectToAction("Explore");
        }

        public async Task<IActionResult> MyRegistrations()
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
                return RedirectToAction("Login");

            var registrations = await _context.WorkshopRegistrations
                .Include(r => r.Workshop)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return View(registrations);
        }


    }
}
