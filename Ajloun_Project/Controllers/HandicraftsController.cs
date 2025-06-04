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
    }
}

