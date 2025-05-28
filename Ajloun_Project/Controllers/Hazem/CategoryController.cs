using Microsoft.AspNetCore.Mvc;
using Ajloun_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Ajloun_Project.Controllers.Hazem
{
    public class CategoryController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CategoryController(MyDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: /Category
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses.ToListAsync();
            return View(courses);
        }

        // GET: /Category/ManageCategories
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageCategories()
        {
            var bookCategories = await _context.BookCategories
                .Include(c => c.Books)
                .ToListAsync();
            return View("Category", bookCategories);
        }
        
        // POST: /Category/AddCategory
        [HttpPost]
        // [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(BookCategory category)
        {
            if (ModelState.IsValid)
            {
                _context.BookCategories.Add(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم إضافة التصنيف بنجاح";
                return RedirectToAction(nameof(ManageCategories));
            }
            TempData["Error"] = "حدث خطأ أثناء إضافة التصنيف";
            return RedirectToAction(nameof(ManageCategories));
        }
        
        // GET: /Category/EditCategory/5
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _context.BookCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            
            return View(category);
        }
        
        // POST: /Category/EditCategory
        [HttpPost]
        // [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id, BookCategory category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "تم تحديث التصنيف بنجاح";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.BookCategories.AnyAsync(c => c.CategoryId == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ManageCategories));
            }
            TempData["Error"] = "حدث خطأ أثناء تحديث التصنيف";
            return View(category);
        }
        
        // POST: /Category/DeleteCategory
        [HttpPost]
        // [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.BookCategories
                .Include(c => c.Books)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
                
            if (category == null)
            {
                return NotFound();
            }
            
            if (category.Books != null && category.Books.Any())
            {
                TempData["Error"] = "لا يمكن حذف التصنيف لأنه مرتبط بكتب";
                return RedirectToAction(nameof(ManageCategories));
            }
            
            _context.BookCategories.Remove(category);
            await _context.SaveChangesAsync();
            TempData["Success"] = "تم حذف التصنيف بنجاح";
            
            return RedirectToAction(nameof(ManageCategories));
        }

        // GET: /Category/Apply/1
       
        public async Task<IActionResult> Apply(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewBag.Course = course;
            return View();
        }

        // POST: /Category/Apply
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(int courseId, IFormFile birthCertificateImage, bool agreement)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }

            // Get the authenticated user's ID from session
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Apply", new { id = courseId }) });
            }

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null || user.BirthDate == null)
            {
                ModelState.AddModelError("", "معلومات المستخدم أو تاريخ الميلاد غير متوفرة.");
                ViewBag.Course = course;
                return View();
            }

            // Calculate user's age
            var birthDate = user.BirthDate.Value.ToDateTime(TimeOnly.MinValue);
            var age = DateTime.Today.Year - birthDate.Year;
            if (birthDate > DateTime.Today.AddYears(-age)) age--;

            // Validate age against course's AgeRange
            bool isAgeValid = false;
            var ageRanges = course.AgeRange.Split(", ", StringSplitOptions.RemoveEmptyEntries);
            foreach (var range in ageRanges)
            {
                var bounds = range.Split('-').Select(int.Parse).ToArray();
                if (bounds.Length == 2 && age >= bounds[0] && age <= bounds[1])
                {
                    isAgeValid = true;
                    break;
                }
            }

            if (!isAgeValid)
            {
                ModelState.AddModelError("", $"عمرك ({age} سنة) غير مناسب لهذا البرنامج. العمر المطلوب: {course.AgeRange} سنة.");
                ViewBag.Course = course;
                return View();
            }

            if (!agreement)
            {
                ModelState.AddModelError("", "يجب الموافقة على الشروط والأحكام.");
                ViewBag.Course = course;
                return View();
            }

            // Handle file upload
            string? filePath = null;
            if (birthCertificateImage != null && birthCertificateImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "Uploads");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(birthCertificateImage.FileName);
                filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await birthCertificateImage.CopyToAsync(stream);
                }
                filePath = $"/Uploads/{fileName}";
            }
            else
            {
                ModelState.AddModelError("", "يرجى رفع صورة شهادة الميلاد.");
                ViewBag.Course = course;
                return View();
            }

            // Create CourseApplication
            var application = new CourseApplication
            {
                CourseId = courseId,
                UserId = userId.Value,
                BirthCertificateImage = filePath,
                Agreement = agreement,
                Status = "Pending",
                SubmittedAt = DateTime.Now
            };

            _context.CourseApplications.Add(application);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم تقديم طلبك بنجاح! سيتم مراجعته قريبًا.";
            return RedirectToAction("Index");
        }
    }
}