using Ajloun_Project.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Project.Controllers
{
    public class AdminController : Controller
    {
        private readonly MyDbContext _context;

        public AdminController(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        //////////////////////////////////////////////////////////////////////////////////////
        public IActionResult Handicrafts()
        {
            var handicrafts = _context.Handicrafts.ToList();
            return View(handicrafts);
        }
        //////////////////////////////////////////////////////////////////////////////////////

        // عرض نموذج إضافة منتج
        public IActionResult CreateHandicrafts()
        {
            return View();
        }
        //////////////////////////////////////////////////////////////////////////////////////

        // إضافة منتج جديد
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateHandicrafts(Handicraft handicraft)
        {
            if (ModelState.IsValid)
            {
                handicraft.CreatedAt = DateTime.Now;
                _context.Handicrafts.Add(handicraft);
                _context.SaveChanges();
                return RedirectToAction(nameof(Handicrafts));
            }
            return View(handicraft);
        }
        //////////////////////////////////////////////////////////////////////////////////////

        // عرض تفاصيل منتج معين
        public IActionResult Details(int id)
        {
            var handicraft = _context.Handicrafts.FirstOrDefault(h => h.CraftId == id);
            if (handicraft == null)
            {
                return NotFound();
            }
            return View(handicraft);
        }
        //////////////////////////////////////////////////////////////////////////////////////

        // حذف منتج
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var handicraft = _context.Handicrafts.FirstOrDefault(h => h.CraftId == id);
            if (handicraft != null)
            {
                _context.Handicrafts.Remove(handicraft);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Handicrafts));
        }








        // عرض جميع الهيئات الثقافية
        public IActionResult CulturalAssociations()
        {
            var associations = _context.CulturalAssociations.ToList();
            return View(associations);
        }
        [HttpPost]
        public IActionResult UpdateStatus(int id, string status)
        {
            var assoc = _context.CulturalAssociations.FirstOrDefault(a => a.AssociationId == id);
            if (assoc != null && (status == "Approved" || status == "Rejected"))
            {
                assoc.Status = status;
                _context.SaveChanges();
            }
            return RedirectToAction("CulturalAssociations");
        }





        // عرض طلبات الانضمام للهيئات الثقافية
        public IActionResult AssociationJoinRequests()
        {
            var requests = _context.AssociationJoinRequests
                .Include(r => r.User)
                .Include(r => r.Association)
                .OrderBy(r => r.Status == "Pending" ? 0 : 1) 
                .ToList();

            return View(requests);
        }



        // الموافقة على طلب الانضمام
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApproveRequest(int requestId)
        {
            var request = _context.AssociationJoinRequests.FirstOrDefault(r => r.RequestId == requestId);
            if (request != null)
            {
                request.Status = "Approved";
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(AssociationJoinRequests));
        }

        // رفض طلب الانضمام
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RejectRequest(int requestId)
        {
            var request = _context.AssociationJoinRequests.FirstOrDefault(r => r.RequestId == requestId);
            if (request != null)
            {
                request.Status = "Rejected";
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(AssociationJoinRequests));
        }

        // حذف طلب الانضمام
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRequest(int requestId)
        {
            var request = _context.AssociationJoinRequests.FirstOrDefault(r => r.RequestId == requestId);
            if (request != null)
            {
                _context.AssociationJoinRequests.Remove(request);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(AssociationJoinRequests));
        }



        public IActionResult HallBookingsRequests()
        {
            var bookings = _context.HallBookings
                .OrderBy(b => b.Status == "Pending" ? 0 : 1) // ترتيب البندنغ بالأول
                .ToList();

            return View(bookings);
        }
        [HttpPost]
        public IActionResult UpdateHallBookingStatus(int id, string status)
        {
            var booking = _context.HallBookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }

            booking.Status = status;
            _context.SaveChanges();

            return RedirectToAction("HallBookingsRequests");
        }


    }
}
