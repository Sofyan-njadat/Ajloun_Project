using Ajloun_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Project.Controllers
{
    public class HallBookingController : Controller
    {
        private readonly MyDbContext _context;

        public HallBookingController(MyDbContext context)
        {
            _context = context;
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
        public IActionResult Create(HallBooking model)
        {
            // التأكد من تمرير قائمة الهيئات حتى لو صار خطأ
            ViewBag.Associations = _context.CulturalAssociations
                .Where(a => !string.IsNullOrEmpty(a.Name))
                .Select(a => a.Name)
                .ToList();

            // التحقق البسيط يدوي (بدون استخدام ModelState)
            if (model.EventDate < DateOnly.FromDateTime(DateTime.Today))
            {
                TempData["Error"] = "⚠️ لا يمكن اختيار تاريخ قديم.";
                return View(model);
            }

            if (model.StartTime.HasValue && model.EndTime.HasValue && model.EndTime <= model.StartTime)
            {
                TempData["Error"] = "⚠️ وقت الانتهاء يجب أن يكون بعد وقت البدء.";
                return View(model);
            }

            var isDuplicate = _context.HallBookings.Any(b =>
                b.RequestingParty == model.RequestingParty &&
                b.EventDate == model.EventDate &&
                b.StartTime == model.StartTime &&
                b.EndTime == model.EndTime &&
                b.EventTitle == model.EventTitle);

            if (isDuplicate)
            {
                TempData["Error"] = "⚠️ نفس الطلب مكرر من قبل.";
                return View(model);
            }

            var hasConflict = _context.HallBookings.Any(b =>
                b.EventDate == model.EventDate &&
                b.StartTime < model.EndTime &&
                b.EndTime > model.StartTime);

            if (hasConflict)
            {
                TempData["Error"] = "⚠️ يوجد فعالية أخرى في نفس التوقيت.";
                return View(model);
            }

            // الحجز سليم – حفظه
            model.Status = "Pending";
            model.RequestDate = DateOnly.FromDateTime(DateTime.Now);

            _context.HallBookings.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "✅ تم إرسال طلب الحجز بنجاح.";
            return RedirectToAction("Create");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
