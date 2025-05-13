using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ajloun_Project.Models;

namespace Ajloun_Project.Controllers.Ali
{
    public class UserProfileController : Controller
    {
        private readonly MyDbContext _context;

        public UserProfileController(MyDbContext context)
        {
            _context = context;
        }

        private int? GetUserId()
        {
            return HttpContext.Session.GetInt32("userId");
        }

        public async Task<IActionResult> UserInfo()
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("signIn", "User");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                return NotFound();

            return View(user);
        }

        public async Task<IActionResult> UserCourseApplications()
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("signIn", "User");

            var applications = await _context.CourseApplications
                .Include(c => c.Course)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return View(applications);
        }

        public async Task<IActionResult> UserAssociationJoinRequests()
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("signIn", "User");

            var requests = await _context.AssociationJoinRequests
                .Include(r => r.Association)
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return View(requests);
        }

        public async Task<IActionResult> UserEventRegistrations()
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("signIn", "User");

            var registrations = await _context.AssocEventRegistrations
                .Include(r => r.AssocEvent)
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return View(registrations);
        }

        public async Task<IActionResult> UserBookReservations()
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("signIn", "User");

            var reservations = await _context.BookReservations
                .Include(r => r.Book)
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return View(reservations);
        }

        public async Task<IActionResult> UserCraftOrders()
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("signIn", "User");

            var orders = await _context.CraftOrders
                .Include(o => o.Craft)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> UserFestivalReservations()
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("signIn", "User");

            var reservations = await _context.FestivalReservations
                .Include(r => r.Festival)
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return View(reservations);
        }

        public async Task<IActionResult> UserCulturalEventReservations()
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("signIn", "User");

            var reservations = await _context.CulturalEventReservations
                .Include(r => r.Event)
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return View(reservations);
        }

        [HttpGet]
        public IActionResult ChangeUserPassword()
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("signIn", "User");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserPassword(string OldPassword, string NewPassword, string ConfirmPassword)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("signIn", "User");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound();

            if (user.PasswordHash != OldPassword)
            {
                TempData["ErrorMessage"] = "كلمة المرور الحالية غير صحيحة.";
                return RedirectToAction("ChangeUserPassword");
            }

            if (NewPassword != ConfirmPassword)
            {
                TempData["ErrorMessage"] = "كلمتا المرور غير متطابقتين.";
                return RedirectToAction("ChangeUserPassword");
            }

            user.PasswordHash = NewPassword;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "تم تغيير كلمة المرور بنجاح.";
            return RedirectToAction("ChangeUserPassword");
        }

        [HttpGet]
        public async Task<IActionResult> EditUserInfo()
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("signIn", "User");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserInfo(User updatedUser, IFormFile? ProfileImageFile)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("signIn", "User");

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            user.FullName = updatedUser.FullName;
            user.Phone = updatedUser.Phone;
            user.Gender = updatedUser.Gender;
            user.Address = updatedUser.Address;

            if (ProfileImageFile != null && ProfileImageFile.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(ProfileImageFile.FileName)}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/users", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await ProfileImageFile.CopyToAsync(stream);
                }

                user.ProfileImage = fileName;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "تم تحديث معلومات الحساب بنجاح.";
            return RedirectToAction("UserInfo");
        }
        [HttpPost]
        public IActionResult Logout()
        {
            // إزالة البيانات المخزنة في الجلسة
            HttpContext.Session.Clear();

            // إعادة التوجيه إلى صفحة تسجيل الدخول أو الرئيسية
            return RedirectToAction("SignIn", "User");
        }

    }
}
