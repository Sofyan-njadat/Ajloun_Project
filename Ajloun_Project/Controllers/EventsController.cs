using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ajloun_Project.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Ajloun_Project.Controllers
{
    public class EventsController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EventsController(MyDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Events/AdminEvents
        public async Task<IActionResult> AdminEvents()
        {
            var events = await _context.CulturalEvents
                .OrderByDescending(e => e.Date)
                .ToListAsync();

            return View(events);
        }

        // GET: Events/Get/5
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

        // POST: Events/Create
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

        // DELETE: Events/Delete/5
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


        //User
        public IActionResult Events()
        {
            var associations = _context.CulturalAssociations
                .Select(a => new { a.AssociationId, a.Name })
                .ToList<dynamic>();

            var culturalTypes = _context.CulturalEvents
                .Select(e => e.EventType)
                .Distinct()
                .Where(e => !string.IsNullOrEmpty(e))
                .ToList();

            var associationTypes = _context.AssociationEvents
                .Select(e => e.EventType)
                .Distinct()
                .Where(e => !string.IsNullOrEmpty(e))
                .ToList();

            var allTypes = culturalTypes
                .Union(associationTypes)
                .Distinct()
                .ToList();

            var model = new EventsViewModel
            {
                CulturalEvents = _context.CulturalEvents.ToList(),
                AssociationEvents = _context.AssociationEvents
                    .Include(a => a.Association)
                    .ToList(),
                Associations = associations,
                EventTypes = allTypes // أضف هذه الخاصية إلى ViewModel
            };

            return View(model);
        }

        public async Task<IActionResult> EventDetails(int id, string type)
        {
            if (type == "cultural")
            {
                var culturalEvent = await _context.CulturalEvents
                    .FirstOrDefaultAsync(e => e.EventId == id);

                if (culturalEvent == null)
                {
                    return NotFound();
                }

                return View(culturalEvent);
            }
            else if (type == "association")
            {
                var associationEvent = await _context.AssociationEvents
                    .Include(e => e.Association)
                    .FirstOrDefaultAsync(e => e.EventId == id);

                if (associationEvent == null)
                {
                    return NotFound();
                }

                return View(associationEvent);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAttendance(int eventId, string type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            var userId = HttpContext.Session.GetInt32("userId");

            string eventType = type?.ToLower();

            if (eventType == "cultural")
            {
                var culturalEvent = await _context.CulturalEvents.FindAsync(eventId);
                if (culturalEvent == null)
                    return NotFound("الفعالية الثقافية غير موجودة.");

                var existingReservation = await _context.CulturalEventReservations
                    .FirstOrDefaultAsync(r => r.EventId == eventId && r.UserId == userId);

                if (existingReservation != null)
                    TempData["BadRequest"] = "لقد قمت بالتسجيل مسبقًا لهذه الفعالية.";

                var reservation = new CulturalEventReservation
                {
                    EventId = eventId,
                    UserId = userId.Value,
                    Status = "Pending",
                    ReservationDate = DateTime.Now
                };

                _context.CulturalEventReservations.Add(reservation);
                TempData["SuccessMessage"] = "تم تسجيل حضورك بنجاح في الفعالية!";
            }
            else if (eventType == "association")
            {
                var assocEvent = await _context.AssociationEvents.FindAsync(eventId);
                if (assocEvent == null)
                    return NotFound("فعالية الجمعية غير موجودة.");

                var existingRegistration = await _context.AssocEventRegistrations
                    .FirstOrDefaultAsync(r => r.AssocEventId == eventId && r.UserId == userId);

                if (existingRegistration != null)
                    TempData["BadRequest"] = "لقد قمت بالتسجيل مسبقًا لهذه الفعالية.";

                var registration = new AssocEventRegistration
                {
                    AssocEventId = eventId,
                    UserId = userId.Value,
                    RegisteredAt = DateTime.Now
                };

                _context.AssocEventRegistrations.Add(registration);
                TempData["SuccessMessage"] = "تم تسجيل حضورك بنجاح في الفعالية!";
            }
            else
            {
                return BadRequest("نوع الفعالية غير معروف.");
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("EventDetails", new { id = eventId, type = eventType });
        }
    }
} 