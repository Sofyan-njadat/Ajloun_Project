namespace Ajloun_Project.Controllers.Ali
{
    using Microsoft.AspNetCore.Mvc;
    using Ajloun_Project.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.IO;

    public class LibraryController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LibraryController(MyDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // 1. عرض جميع الكتب مع الفلترة
        public async Task<IActionResult> Index(int? categoryId, string? search)
        {
            var books = _context.Books
                .Include(b => b.Category)
                .Where(b => b.IsAvailable);

            // فلترة حسب التصنيف
            if (categoryId.HasValue)
                books = books.Where(b => b.CategoryId == categoryId);

            // البحث حسب العنوان
            if (!string.IsNullOrWhiteSpace(search))
                books = books.Where(b => b.Title.Contains(search));

            // تمرير التصنيفات والبحث المختار إلى ViewBag
            var categories = await _context.BookCategories.ToListAsync();
            ViewBag.Categories = categories; // إذا كنت تستخدمها في أماكن ثانية
            ViewBag.CategoryList = new SelectList(categories, "CategoryId", "Name", categoryId); // لربط TagHelper
            ViewBag.SelectedCategory = categoryId;
            ViewBag.SearchTerm = search;

            return View(await books.ToListAsync());
        }

        // 2. عرض تفاصيل كتاب
        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null) return NotFound();
            return View(book);
        }

        // 3. نموذج حجز كتاب
        public async Task<IActionResult> Reserve(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null || book.AvailableCopies == 0)
                return NotFound();

            var reservation = new BookReservation
            {
                BookId = id
            };

            return View(reservation);
        }


        // 4. تنفيذ الحجز
        [HttpPost]
        public async Task<IActionResult> Reserve(BookReservation reservation)
        {
            var userId = HttpContext.Session.GetInt32("userId");

            // إذا المستخدم غير مسجل دخول، رجعه على تسجيل الدخول مع رسالة
            if (!userId.HasValue)
            {
                TempData["LoginRequired"] = "يرجى تسجيل الدخول أولاً لإتمام الحجز.";
                return RedirectToAction("signIn", "User", new { returnUrl = Url.Action("Reserve", new { id = reservation.BookId }) });
            }

            // تأكيد وجود الكتاب
            var book = await _context.Books.FindAsync(reservation.BookId);
            if (book == null || book.AvailableCopies <= 0)
                return NotFound();

            // تعبئة بيانات الحجز
            reservation.UserId = userId.Value;
            reservation.Agreement = true;
            reservation.ReservationDate = DateTime.Now;
            reservation.Status = "Pending";

            _context.BookReservations.Add(reservation);

            // تقليل عدد النسخ المتاحة
            book.AvailableCopies--;
            if (book.AvailableCopies == 0)
                book.IsAvailable = false;

            await _context.SaveChangesAsync();

            return RedirectToAction("ReservationSuccess");
        }

        public IActionResult ReservationSuccess()
        {
            return View();
        }

        // ---------------- ADMIN ZONE ---------------- //

        // 5. عرض جميع الحجوزات (للمشرف)
        public async Task<IActionResult> ManageReservations()
        {
            var reservations = await _context.BookReservations
                .Include(r => r.Book)
                .Include(r => r.User)
                .ToListAsync();

            return View(reservations);
        }

        // 6. تعديل حالة الحجز
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var res = await _context.BookReservations
                .Include(r => r.Book)
                .FirstOrDefaultAsync(r => r.ReservationId == id);

            if (res == null)
                return NotFound();

            // معالجة الإرجاع
            if (res.Status != "Returned" && status == "Returned")
            {
                res.Book.AvailableCopies++;
                res.Book.IsAvailable = true;
            }

            res.Status = status;
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageReservations");
        }

        // 7. إدارة التصنيفات
        public async Task<IActionResult> Categories()
        {
            var categories = await _context.BookCategories.ToListAsync();
            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                _context.BookCategories.Add(new BookCategory { Name = name });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Categories");
        }

        // 8. إدارة الكتب
        public async Task<IActionResult> ManageBooks()
        {
            var books = await _context.Books.Include(b => b.Category).ToListAsync();
            ViewBag.Categories = await _context.BookCategories.ToListAsync();
            return View(books);
        }

        // GET: AddBook
        public async Task<IActionResult> AddBook()
        {
            ViewBag.Categories = await _context.BookCategories.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook(Book book, IFormFile coverImage)
        {
            if (ModelState.IsValid)
            {
                if (coverImage != null && coverImage.Length > 0)
                {
                    // إنشاء اسم فريد للملف
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + coverImage.FileName;
                    
                    // تحديد مسار حفظ الصورة
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "books");
                    
                    // التأكد من وجود المجلد
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    
                    // حفظ الصورة
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await coverImage.CopyToAsync(fileStream);
                    }
                    
                    // حفظ مسار الصورة في قاعدة البيانات
                    book.CoverImageUrl = "/images/books/" + uniqueFileName;
                }

                book.IsAvailable = true;
            book.CreatedAt = DateTime.Now;
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageBooks));
            }
            ViewBag.Categories = await _context.BookCategories.ToListAsync();
            return View(book);
        }

        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ManageBooks");
        }

        public async Task<IActionResult> EditBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound();

            ViewBag.Categories = await _context.BookCategories.ToListAsync();
            return View(book);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBook(Book updatedBook, IFormFile coverImage)
        {
            var book = await _context.Books.FindAsync(updatedBook.BookId);
            if (book == null)
                return NotFound();

            // تحديث الصورة إذا تم تحميل صورة جديدة
            if (coverImage != null && coverImage.Length > 0)
            {
                // حذف الصورة القديمة إذا كانت موجودة
                if (!string.IsNullOrEmpty(book.CoverImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, book.CoverImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // إنشاء اسم فريد للملف
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + coverImage.FileName;
                
                // تحديد مسار حفظ الصورة
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "books");
                
                // التأكد من وجود المجلد
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                
                // حفظ الصورة
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await coverImage.CopyToAsync(fileStream);
                }
                
                // حفظ مسار الصورة في قاعدة البيانات
                book.CoverImageUrl = "/images/books/" + uniqueFileName;
            }

            // تحديث باقي البيانات
            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.Description = updatedBook.Description;
            book.PublishedYear = updatedBook.PublishedYear;
            book.CategoryId = updatedBook.CategoryId;
            book.AvailableCopies = updatedBook.AvailableCopies;
            book.IsAvailable = updatedBook.AvailableCopies > 0;

            try
            {
            await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageBooks));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "حدث خطأ أثناء حفظ التغييرات: " + ex.Message);
                ViewBag.Categories = await _context.BookCategories.ToListAsync();
                return View(updatedBook);
            }
        }

    }
}
