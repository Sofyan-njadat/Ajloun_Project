using Ajloun_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Project.Controllers
{
    public class HandicraftsController : Controller
    {
        private readonly MyDbContext _context;

        public HandicraftsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Handicrafts
        public async Task<IActionResult> Index(string category)
        {
            var handicrafts = _context.Handicrafts.AsQueryable();

            // Если указана категория, фильтруем по ней
            if (!string.IsNullOrEmpty(category))
            {
                handicrafts = handicrafts.Where(h => h.Category == category);
            }

            // Получаем список всех категорий для фильтра
            var categories = await _context.Handicrafts
                .Select(h => h.Category)
                .Distinct()
                .Where(c => c != null)
                .ToListAsync();

            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = category;

            return View(await handicrafts.ToListAsync());
        }

        // GET: Handicrafts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var handicraft = await _context.Handicrafts
                .FirstOrDefaultAsync(m => m.CraftId == id);

            if (handicraft == null)
            {
                return NotFound();
            }

            return View(handicraft);
        }

        // POST: Handicrafts/Order
        [HttpPost]
        public async Task<IActionResult> Order(int craftId, int quantity)
        {
            // Проверяем, авторизован ли пользователь
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {
                // Если пользователь не авторизован, перенаправляем его на страницу входа
                // с указанием страницы возврата
                TempData["ErrorMessage"] = "يجب عليك تسجيل الدخول أولاً لتقديم طلب شراء";
                return RedirectToAction("SignIn", "User", new { returnUrl = Url.Action("Details", "Handicrafts", new { id = craftId }) });
            }

            // Проверяем, существует ли выбранное изделие
            var handicraft = await _context.Handicrafts.FindAsync(craftId);
            if (handicraft == null)
            {
                return NotFound();
            }

            // Проверяем, достаточно ли товара на складе
            if (handicraft.Stock < quantity)
            {
                TempData["ErrorMessage"] = "الكمية المطلوبة غير متوفرة في المخزون";
                return RedirectToAction("Details", new { id = craftId });
            }

            // Создаем новый заказ
            var order = new CraftOrder
            {
                CraftId = craftId,
                UserId = userId,
                Quantity = quantity,
                OrderDate = DateTime.Now,
                Status = "Pending" // Статус "В ожидании"
            };

            // Сохраняем заказ в базу данных
            _context.CraftOrders.Add(order);
            await _context.SaveChangesAsync();

            // Уменьшаем количество товара на складе
            handicraft.Stock -= quantity;
            _context.Update(handicraft);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "تم تقديم طلب الشراء بنجاح وسيتم التواصل معك قريباً";
            return RedirectToAction("Index");
        }

        // GET: Handicrafts/MyOrders
        public async Task<IActionResult> MyOrders()
        {
            // Проверяем, авторизован ли пользователь
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {
                return RedirectToAction("SignIn", "User", new { returnUrl = Url.Action("MyOrders", "Handicrafts") });
            }

            // Получаем заказы пользователя с информацией о изделиях
            var orders = await _context.CraftOrders
                .Where(o => o.UserId == userId)
                .Include(o => o.Craft)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }
    }
}

