using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ajloun_Project.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Ajloun_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventsController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EventsController(MyDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/Events
        public async Task<IActionResult> AdminEvents()
        {
            var events = await _context.CulturalEvents
                .OrderByDescending(e => e.Date)
                .ToListAsync();

            return View(events);
        }

        // GET: Admin/Events/Get/5
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var culturalEvent = await _context.CulturalEvents.FindAsync(id);
            if (culturalEvent == null)
            {
                return NotFound();
            }

            return Json(new
            {
                eventId = culturalEvent.EventId,
                title = culturalEvent.Title,
                description = culturalEvent.Description,
                eventDate = culturalEvent.Date?.ToString("yyyy-MM-dd") ?? null,
                location = culturalEvent.Location,
                imageUrl = culturalEvent.PosterUrl
            });
        }

        // POST: Admin/Events/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CulturalEvent culturalEvent, IFormFile image)
        {
            try
            {
                if (culturalEvent.EventId > 0)
                {
                    // تحديث فعالية موجودة
                    var existingEvent = await _context.CulturalEvents.FindAsync(culturalEvent.EventId);
                    if (existingEvent == null)
                    {
                        return Json(new { success = false, message = "الفعالية غير موجودة" });
                    }

                    // تحديث البيانات الأساسية
                    existingEvent.Title = culturalEvent.Title;
                    existingEvent.Description = culturalEvent.Description;
                    existingEvent.Date = culturalEvent.Date;
                    existingEvent.Location = culturalEvent.Location;

                    // معالجة الصورة إذا تم تحميل صورة جديدة
                    if (image != null && image.Length > 0)
                    {
                        // حذف الصورة القديمة
                        if (!string.IsNullOrEmpty(existingEvent.PosterUrl))
                        {
                            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingEvent.PosterUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // حفظ الصورة الجديدة
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "EventsIMGs");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }

                        existingEvent.PosterUrl = "/EventsIMGs/" + uniqueFileName;
                    }

                    _context.Update(existingEvent);
                }
                else
                {
                    // إضافة فعالية جديدة
                    culturalEvent.CreatedAt = DateTime.Now;

                    if (image != null && image.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "EventsIMGs");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }

                        culturalEvent.PosterUrl = "/EventsIMGs/" + uniqueFileName;
                    }

                    _context.Add(culturalEvent);
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // DELETE: Admin/Events/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var culturalEvent = await _context.CulturalEvents.FindAsync(id);
            if (culturalEvent == null)
            {
                return NotFound();
            }

            // حذف الصورة إذا كانت موجودة
            if (!string.IsNullOrEmpty(culturalEvent.PosterUrl))
            {
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, culturalEvent.PosterUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.CulturalEvents.Remove(culturalEvent);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        private bool EventExists(int id)
        {
            return _context.CulturalEvents.Any(e => e.EventId == id);
        }
    }
} 