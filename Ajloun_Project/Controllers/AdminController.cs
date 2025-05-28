using Ajloun_Project.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

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

        public IActionResult PendingArtworks()
        {
            var artworks = _context.Artworks
                .Select(a => new AjlounProject.ViewModels.PendingArtworkViewModel
                {
                    ArtworkId = a.ArtworkId,
                    Title = a.Title,
                    ArtistName = a.ArtistName,
                    Type = a.Type,
                    Description = a.Description,
                    ImageUrl = a.ImageUrl,
                    Status = a.Status
                })
                .OrderByDescending(a => a.ArtworkId)
                .ToList();

            return View(artworks);
        }

        [HttpGet]
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

        [HttpGet]
        public IActionResult Reject(int id)
        {
            var artwork = _context.Artworks.Find(id);
            if (artwork != null)
            {
                artwork.Status = "Rejected";
                _context.SaveChanges();
            }
            return RedirectToAction("PendingArtworks");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteArtwork(int id)
        {
            var artwork = await _context.Artworks.FindAsync(id);
            if (artwork != null)
            {
                // حذف الصورة من المجلد
                if (!string.IsNullOrEmpty(artwork.ImageUrl))
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, artwork.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Artworks.Remove(artwork);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم حذف العمل الفني بنجاح";
            }
            return RedirectToAction(nameof(PendingArtworks));
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
            var associations = _context.CulturalAssociations
                .Include(a => a.Category)
                .ToList();
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
        public async Task<IActionResult> Edit(int id, Admin admin, string NewPassword)
        {
            // للتأكد من صحة رقم المشرف
            if (id != admin.AdminId)
            {
                return NotFound();
            }

            // دائمًا نزيل التحقق من كلمة المرور
            ModelState.Remove("PasswordHash");

            // الحصول على المشرف الحالي من قاعدة البيانات
            var currentAdmin = await _context.Admins.FindAsync(id);
            if (currentAdmin == null)
            {
                return NotFound();
            }

            // فحص التكرار من خارج النموذج لتحسين الرسائل
            var usernameExists = await _context.Admins.AnyAsync(a => a.Username == admin.Username && a.AdminId != id);
            var emailExists = await _context.Admins.AnyAsync(a => a.Email == admin.Email && a.AdminId != id);

            if (usernameExists)
            {
                ModelState.AddModelError("Username", "اسم المستخدم مستخدم بالفعل");
            }

            if (emailExists)
            {
                ModelState.AddModelError("Email", "البريد الإلكتروني مستخدم بالفعل");
            }

            // إضافة كلمة المرور القديمة للنموذج الذي سيتم حفظه
            admin.PasswordHash = currentAdmin.PasswordHash;

            // إذا كان هناك أخطاء في التحقق، نعيد عرض النموذج مع الأخطاء
            if (usernameExists || emailExists)
            {
                ViewData["ErrorMessage"] = "حدثت أخطاء في النموذج";
                return View(admin);
            }

            try
            {
                // تحديث قيم المشرف
                currentAdmin.Username = admin.Username;
                currentAdmin.Email = admin.Email;
                currentAdmin.FullName = admin.FullName;
                currentAdmin.Role = admin.Role;

                // تحديث كلمة المرور فقط إذا تم إدخال واحدة جديدة
                if (!string.IsNullOrWhiteSpace(NewPassword))
                {
                    currentAdmin.PasswordHash = HashPassword(NewPassword);
                }

                // حفظ التغييرات
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "تم تحديث بيانات المشرف بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // التقاط أي خطأ قد يحدث أثناء التحديث وعرضه للمستخدم
                ViewData["ErrorMessage"] = $"حدث خطأ أثناء التحديث: {ex.Message}";
                return View(admin);
            }
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

        // عرض نموذج تعديل الهيئة الثقافية
        public IActionResult EditAssociation(int id)
        {
            var association = _context.CulturalAssociations
                .Include(a => a.Category)
                .FirstOrDefault(a => a.AssociationId == id);
            
            if (association == null)
            {
                return NotFound();
            }
            
            // نحضر كل التصنيفات لعرضها في القائمة المنسدلة
            ViewBag.Categories = _context.AssociationCategories.ToList();
            
            return View(association);
        }

        // تحديث بيانات الهيئة الثقافية
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAssociation(int id, CulturalAssociation association)
        {
            if (id != association.AssociationId)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(association);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(CulturalAssociations));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.CulturalAssociations.Any(a => a.AssociationId == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            
            // إذا حدث خطأ، نعيد تحميل التصنيفات
            ViewBag.Categories = _context.AssociationCategories.ToList();
            return View(association);
        }

        // عرض جميع المناطق
        public IActionResult ManageCategories()
        {
            var categories = _context.AssociationCategories
                .Include(c => c.CulturalAssociations)
                .ToList();
            return View(categories);
        }

        // عرض جميع المستخدمين
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // عرض تفاصيل مستخدم
        public async Task<IActionResult> UserDetails(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // إضافة فئة جديدة
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["Error"] = "يرجى إدخال اسم المنطقة";
                return RedirectToAction(nameof(ManageCategories));
            }

            try
            {
                var category = new AssociationCategory { Name = name };
                _context.AssociationCategories.Add(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تمت إضافة المنطقة بنجاح";
            }
            catch (Exception)
            {
                TempData["Error"] = "حدث خطأ أثناء إضافة المنطقة";
            }

            return RedirectToAction(nameof(ManageCategories));
        }

        // تعديل فئة
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["Error"] = "يرجى إدخال اسم المنطقة";
                return RedirectToAction(nameof(ManageCategories));
            }

            try
            {
                var category = await _context.AssociationCategories.FindAsync(id);
                if (category == null)
                {
                    TempData["Error"] = "المنطقة غير موجودة";
                    return RedirectToAction(nameof(ManageCategories));
                }

                category.Name = name;
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم تحديث المنطقة بنجاح";
            }
            catch (Exception)
            {
                TempData["Error"] = "حدث خطأ أثناء تحديث المنطقة";
            }

            return RedirectToAction(nameof(ManageCategories));
        }

        // حذف فئة
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _context.AssociationCategories
                    .Include(c => c.CulturalAssociations)
                    .FirstOrDefaultAsync(c => c.CategoryId == id);

                if (category == null)
                {
                    TempData["Error"] = "المنطقة غير موجودة";
                    return RedirectToAction(nameof(ManageCategories));
                }

                if (category.CulturalAssociations.Any())
                {
                    TempData["Error"] = "لا يمكن حذف المنطقة لأنها تحتوي على هيئات ثقافية";
                    return RedirectToAction(nameof(ManageCategories));
                }

                _context.AssociationCategories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم حذف المنطقة بنجاح";
            }
            catch (Exception)
            {
                TempData["Error"] = "حدث خطأ أثناء حذف المنطقة";
            }

            return RedirectToAction(nameof(ManageCategories));
        }
    }
}
