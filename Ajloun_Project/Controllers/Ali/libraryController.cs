namespace Ajloun_Project.Controllers.Ali
{
    using Microsoft.AspNetCore.Mvc;
    using Ajloun_Project.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class LibraryController : Controller
    {
        private readonly MyDbContext _context;

        public LibraryController(MyDbContext context)
        {
            _context = context;
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
            // تحقق إذا المستخدم مسجل دخول، إذا مش مسجل خليه مؤقتًا 1
            int userId = HttpContext.Session.GetInt32("userId") ?? 1;

            // تأكيد وجود الكتاب
            var book = await _context.Books.FindAsync(reservation.BookId);
            if (book == null || book.AvailableCopies <= 0)
                return NotFound();

            // تعبئة بيانات الحجز
            reservation.UserId = userId;
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

        [HttpPost]
        public async Task<IActionResult> AddBook(Book book)
        {
            book.IsAvailable = book.AvailableCopies > 0;
            book.CreatedAt = DateTime.Now;
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageBooks");
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
        public async Task<IActionResult> EditBook(Book updatedBook)
        {
            var book = await _context.Books.FindAsync(updatedBook.BookId);
            if (book == null)
                return NotFound();


            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.Description = updatedBook.Description;
            book.CoverImageUrl = updatedBook.CoverImageUrl;
            book.PublishedYear = updatedBook.PublishedYear;
            book.CategoryId = updatedBook.CategoryId;
            book.AvailableCopies = updatedBook.AvailableCopies;
            book.IsAvailable = updatedBook.AvailableCopies > 0;

            await _context.SaveChangesAsync();
            return RedirectToAction("ManageBooks");
        }

    }
}
