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
            if (model.EventDate < DateOnly.FromDateTime(DateTime.Today))
            {
                ModelState.AddModelError("EventDate", "لا يمكن اختيار تاريخ قديم.");
            }

            if (model.StartTime.HasValue && model.EndTime.HasValue && model.EndTime <= model.StartTime)
            {
                ModelState.AddModelError("EndTime", "وقت الانتهاء يجب أن يكون بعد وقت البدء.");
            }

            // جمع الحجوزات المتضاربة
            var conflictingBookings = _context.HallBookings
                .Where(b =>
                    b.EventDate == model.EventDate &&
                    b.StartTime < model.EndTime &&
                    b.EndTime > model.StartTime)
                .ToList();

            if (conflictingBookings.Any())
            {
                ModelState.AddModelError(string.Empty, "⚠️ يوجد فعالية أخرى محجوزة في نفس الوقت.");
                ViewBag.Conflicts = conflictingBookings;
            }

            bool isExactDuplicate = _context.HallBookings.Any(b =>
                b.RequestingParty == model.RequestingParty &&
                b.EventDate == model.EventDate &&
                b.StartTime == model.StartTime &&
                b.EndTime == model.EndTime &&
                b.EventTitle == model.EventTitle
            );
            if (isExactDuplicate)
            {
                ModelState.AddModelError(string.Empty, "⚠️ نفس الطلب مكرر من قبل.");
            }

            var latestEnd = _context.HallBookings
                .Where(b => b.EventDate == model.EventDate)
                .OrderByDescending(b => b.EndTime)
                .Select(b => b.EndTime)
                .FirstOrDefault();

            if (latestEnd.HasValue && model.StartTime <= latestEnd)
            {
                ModelState.AddModelError("StartTime", "⚠️ لا يمكن الحجز قبل انتهاء آخر فعالية في نفس اليوم.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Status = "Pending";
            model.RequestDate = DateOnly.FromDateTime(DateTime.Now);

            _context.HallBookings.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "تم إرسال طلب الحجز بنجاح.";
            return RedirectToAction("Create");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
