using Microsoft.AspNetCore.Mvc;
using Ajloun_Project.Models;
using Ajloun_Project.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Ajloun_Project.Controllers
{
    public static class DbContextExtensions
    {
        // طريقة امتداد لفصل الكائن عن التتبع
        public static void Detach<T>(this MyDbContext context, T entity) where T : class
        {
            if (entity == null) return;
            var entry = context.Entry(entity);
            if (entry != null)
            {
                entry.State = EntityState.Detached;
            }
        }
    }

    public class CourseController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CourseController(MyDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // عرض قائمة الدورات للإدارة
        public async Task<IActionResult> ManageCourses()
        {
            var courses = await _context.Courses.ToListAsync();
            return View(courses);
        }

        // عرض صفحة إنشاء دورة جديدة
        public IActionResult CreateCourse()
        {
            return View();
        }

        // إضافة دورة جديدة
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(Course course, IFormFile courseImage)
        {
            if (ModelState.IsValid)
            {
                if (courseImage != null && courseImage.Length > 0)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + courseImage.FileName;
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "courses");
                    
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await courseImage.CopyToAsync(fileStream);
                    }
                    
                    course.Courseimg = "/images/courses/" + uniqueFileName;
                }

                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "تمت إضافة الدورة بنجاح";
                return RedirectToAction(nameof(ManageCourses));
            }
            return View(course);
        }

        // عرض صفحة تعديل الدورة
        public async Task<IActionResult> EditCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // تحديث الدورة
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourse(Course course, IFormFile courseImage)
        {
            try
            {
                var existingCourse = await _context.Courses.FindAsync(course.CourseId);
                if (existingCourse == null)
                {
                    return NotFound();
                }

                // تحديث البيانات الأساسية فقط
                existingCourse.Name = course.Name;
                existingCourse.Description = course.Description;
                existingCourse.AgeRange = course.AgeRange;

                // تحديث الصورة فقط إذا تم تحميل واحدة جديدة
                if (courseImage != null && courseImage.Length > 0)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + courseImage.FileName;
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "courses");
                    
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await courseImage.CopyToAsync(fileStream);
                    }
                    
                    // تحديث مسار الصورة فقط إذا تم تحميل صورة جديدة
                    existingCourse.Courseimg = "/images/courses/" + uniqueFileName;
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "تم تحديث الدورة بنجاح";
                return RedirectToAction(nameof(ManageCourses));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث الدورة: " + ex.Message;
                return View(course);
            }
        }

        // التحقق من وجود الدورة
        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }

        // حذف الدورة
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            // التحقق من عدم وجود طلبات تسجيل مرتبطة بالدورة
            var hasApplications = await _context.CourseApplications.AnyAsync(a => a.CourseId == id);
            if (hasApplications)
            {
                TempData["ErrorMessage"] = "لا يمكن حذف الدورة لأنها تحتوي على طلبات تسجيل";
                return RedirectToAction(nameof(ManageCourses));
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "تم حذف الدورة بنجاح";
            return RedirectToAction(nameof(ManageCourses));
        }

        // عرض طلبات التسجيل في الدورات
        public async Task<IActionResult> CourseApplications()
        {
            var applications = await _context.CourseApplications
                .Include(a => a.Course)
                .Include(a => a.User)
                .OrderByDescending(a => a.SubmittedAt)
                .ToListAsync();
            return View(applications);
        }

        // تحديث حالة طلب التسجيل
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateApplicationStatus(int id, string status)
        {
            var application = await _context.CourseApplications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            application.Status = status;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "تم تحديث حالة الطلب بنجاح";
            return RedirectToAction(nameof(CourseApplications));
        }
    }
} 