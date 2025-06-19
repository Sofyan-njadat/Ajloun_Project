using Ajloun_Project.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Project.Controllers
{
    public class HallBookingController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HallBookingController(MyDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Create()
        {
            // جلب قائمة الهيئات فقط بالأسماء
            ViewBag.Associations = _context.CulturalAssociations
                .Where(a => !string.IsNullOrEmpty(a.Name))
                .Select(a => a.Name)
                .ToList();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HallBooking model, IFormFile UploadedFile)
        {
            ViewBag.Associations = _context.CulturalAssociations
                .Where(a => !string.IsNullOrEmpty(a.Name))
                .Select(a => a.Name)
                .ToList();

            // التحقق من التاريخ
            if (model.EventDate < DateOnly.FromDateTime(DateTime.Today))
            {
                TempData["Error"] = "⚠️ لا يمكن اختيار تاريخ قديم.";
                return View(model);
            }

            // التحقق من التوقيت
            if (model.StartTime.HasValue && model.EndTime.HasValue && model.EndTime <= model.StartTime)
            {
                TempData["Error"] = "⚠️ وقت الانتهاء يجب أن يكون بعد وقت البدء.";
                return View(model);
            }

            // التحقق من التكرار (فقط في حالة الإضافة)
            if (model.BookingId == 0)
            {
                var isDuplicate = _context.HallBookings.Any(b =>
                    b.EventDate == model.EventDate &&
                    b.StartTime == model.StartTime &&
                    b.EndTime == model.EndTime &&
                    b.EventTitle == model.EventTitle &&
                    (
                        (model.UserId.HasValue && b.UserId == model.UserId) ||
                        (model.CultureOrgId.HasValue && b.CultureOrgId == model.CultureOrgId)
                    )
                );

                if (isDuplicate)
                {
                    TempData["Error"] = "⚠️ نفس الطلب مكرر من قبل.";
                    return View(model);
                }
            }

            // التحقق من تضارب المواعيد
            var hasConflict = _context.HallBookings.Any(b =>
                b.BookingId != model.BookingId && // ← استثني السجل نفسه في حالة التعديل
                b.EventDate == model.EventDate &&
                b.StartTime < model.EndTime &&
                b.EndTime > model.StartTime
            );

            if (hasConflict)
            {
                TempData["Error"] = "⚠️ يوجد فعالية أخرى في نفس التوقيت.";
                return View(model);
            }

            // رفع الملف
            if (UploadedFile != null && UploadedFile.Length > 0)
            {
                var uploadsPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(UploadedFile.FileName);
                var fullPath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    UploadedFile.CopyTo(stream);
                }

                model.AttachmentPath = "/uploads/" + fileName;
            }

            // حالة جديدة = إضافة
            if (model.BookingId == 0)
            {
                model.Status = "Pending";
                model.RequestDate = DateOnly.FromDateTime(DateTime.Now);

                _context.HallBookings.Add(model);
                TempData["Success"] = "✅ تم إرسال طلب الحجز بنجاح.";
            }
            else
            {
                // تعديل
                var existing = _context.HallBookings.FirstOrDefault(b => b.BookingId == model.BookingId);
                if (existing == null)
                {
                    TempData["Error"] = "⚠️ لم يتم العثور على الحجز للتعديل.";
                    return RedirectToAction("Create");
                }

                // تحديث البيانات
                _context.Entry(existing).CurrentValues.SetValues(model);
                TempData["Success"] = "✅ تم تعديل بيانات الحجز بنجاح.";
            }

            _context.SaveChanges();
            return RedirectToAction("Create");
        }

    }
}
