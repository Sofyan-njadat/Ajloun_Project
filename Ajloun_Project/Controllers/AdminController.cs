using Ajloun_Project.Models;
using AjlounProject.ViewModels;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Ajloun_Project.Controllers
{
    public class AdminController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(MyDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
        public async Task<IActionResult> CreateHandicrafts(Handicraft handicraft, IFormFile productImage)
        {
            if (ModelState.IsValid)
            {
                if (productImage != null && productImage.Length > 0)
                {
                    // إنشاء اسم فريد للملف
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + productImage.FileName;
                    
                    // تحديد مسار حفظ الصورة
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "handicrafts");
                    
                    // التأكد من وجود المجلد
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    
                    // حفظ الصورة
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await productImage.CopyToAsync(fileStream);
                    }
                    
                    // حفظ مسار الصورة في قاعدة البيانات
                    handicraft.ImageUrl = "/images/handicrafts/" + uniqueFileName;
                }

                handicraft.CreatedAt = DateTime.Now;
                _context.Handicrafts.Add(handicraft);
                await _context.SaveChangesAsync();
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteHandicraft(int id)
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

        // عرض قائمة المشرفين
        public async Task<IActionResult> Index()
        {
            var admins = await _context.Admins.ToListAsync();
            return View(admins);
        }

        // عرض نموذج إضافة مشرف جديد
        public IActionResult Create()
        {
            return View();
        }

        // إضافة مشرف جديد
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Admin admin)
        {
            if (ModelState.IsValid)
            {
                // التحقق من عدم تكرار اسم المستخدم والبريد الإلكتروني
                if (await _context.Admins.AnyAsync(a => a.Username == admin.Username))
                {
                    ModelState.AddModelError("Username", "اسم المستخدم مستخدم بالفعل");
                    return View(admin);
                }

                if (await _context.Admins.AnyAsync(a => a.Email == admin.Email))
                {
                    ModelState.AddModelError("Email", "البريد الإلكتروني مستخدم بالفعل");
                    return View(admin);
                }

                // تشفير كلمة المرور
                admin.PasswordHash = HashPassword(admin.PasswordHash);

                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(admin);
        }

        // عرض نموذج تعديل المشرف
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // تحديث بيانات المشرف
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Admin admin)
        {
            if (id != admin.AdminId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // التحقق من عدم تكرار اسم المستخدم والبريد الإلكتروني
                    var existingAdmin = await _context.Admins
                        .FirstOrDefaultAsync(a => (a.Username == admin.Username || a.Email == admin.Email) && a.AdminId != id);

                    if (existingAdmin != null)
                    {
                        if (existingAdmin.Username == admin.Username)
                            ModelState.AddModelError("Username", "اسم المستخدم مستخدم بالفعل");
                        if (existingAdmin.Email == admin.Email)
                            ModelState.AddModelError("Email", "البريد الإلكتروني مستخدم بالفعل");
                        return View(admin);
                    }

                    // إذا تم تغيير كلمة المرور، قم بتشفيرها
                    if (!string.IsNullOrEmpty(admin.PasswordHash))
                    {
                        admin.PasswordHash = HashPassword(admin.PasswordHash);
                    }
                    else
                    {
                        // إذا لم يتم تغيير كلمة المرور، احتفظ بالقيمة القديمة
                        var oldAdmin = await _context.Admins.AsNoTracking().FirstOrDefaultAsync(a => a.AdminId == id);
                        admin.PasswordHash = oldAdmin.PasswordHash;
                    }







        //AHmad


        public IActionResult PendingArtworks()
        {
            var artworks = _context.Artworks
                .Where(a => a.Status == "Pending")
                .Select(a => new PendingArtworkViewModel
                {
                    ArtworkId = a.ArtworkId,
                    Title = a.Title,
                    ArtistName = a.ArtistName,
                    Type = a.Type,
                    Description = a.Description,
                    ImageUrl = a.ImageUrl
                })
                .ToList();

            return View(artworks);
        }

        public IActionResult Approve(int id)
        {
            var artwork = _context.Artworks.Find(id);
            if (artwork != null)
            {
                artwork.Status = "Approved";
                _context.SaveChanges();
            }

            return RedirectToAction("PendingArtworks");
        }

        public IActionResult Reject(int id)
        {
            var artwork = _context.Artworks.Find(id);
            if (artwork != null)
            {
                artwork.Status = "Rejected";
                _context.SaveChanges();
            }

                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(admin.AdminId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(admin);
        }
            return RedirectToAction("PendingArtworks");
        }








        public IActionResult HandicraftPurchaseOrders()
        {
            var orders = _context.CraftOrders
                                 .Include(o => o.Craft)
                                 .Include(o => o.User)
                                 .OrderByDescending(o => o.OrderDate)
                                 .Select(o => new CraftOrderViewModel
                                 {
                                     OrderId = o.OrderId,
                                     CraftTitle = o.Craft.Title,
                                     UserName = o.User.FullName,
                                     Quantity = o.Quantity,
                                     OrderDate = o.OrderDate,
                                     Status = o.Status
                                 }).ToList();

            return View(orders);
        }














        // حذف مشرف
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin != null)
            {
                _context.Admins.Remove(admin);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(int id)
        {
            return _context.Admins.Any(e => e.AdminId == id);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // عرض نموذج تعديل منتج
        public IActionResult EditHandicraft(int id)
        {
            var handicraft = _context.Handicrafts.FirstOrDefault(h => h.CraftId == id);
            if (handicraft == null)
            {
                return NotFound();
            }
            return View(handicraft);
        }

        // تحديث بيانات المنتج
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditHandicraft(int id, Handicraft handicraft, IFormFile productImage)
        {
            if (id != handicraft.CraftId)
            {
                return NotFound();
            }

            try
            {
                var existingHandicraft = await _context.Handicrafts.FindAsync(id);
                if (existingHandicraft == null)
                {
                    return NotFound();
                }

                // تحديث البيانات الأساسية
                existingHandicraft.Title = handicraft.Title;
                existingHandicraft.Description = handicraft.Description;
                existingHandicraft.Price = handicraft.Price;
                existingHandicraft.Stock = handicraft.Stock;
                existingHandicraft.Category = handicraft.Category;

                // تحديث الصورة فقط إذا تم تحميل صورة جديدة
                if (productImage != null && productImage.Length > 0)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + productImage.FileName;
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "handicrafts");
                    
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await productImage.CopyToAsync(fileStream);
                    }
                    
                    existingHandicraft.ImageUrl = "/images/handicrafts/" + uniqueFileName;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Handicrafts));
            }
            catch (Exception)
            {
                return View(handicraft);
            }
        }

        public IActionResult CraftOrders()
        {
            var orders = _context.CraftOrders
                .Include(o => o.Craft)
                .Include(o => o.User)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
            return View(orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderStatus(int id, string status)
        {
            var order = _context.CraftOrders.Find(id);
            if (order != null)
            {
                order.Status = status;
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(CraftOrders));
        }
    }
}
