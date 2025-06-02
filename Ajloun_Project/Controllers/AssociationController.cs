using Ajloun_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Project.Controllers
{
    public class AssociationController : Controller
    {

        private readonly MyDbContext _context;

        public AssociationController(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AllAssociation(int? categoryId)
        {
            var categories = await _context.AssociationCategories.ToListAsync();
            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = categoryId;

            var query = _context.CulturalAssociations
                .Include(a => a.Category)
                .Where(a => a.Status == "Approved");

            if (categoryId.HasValue)
            {
                query = query.Where(a => a.CategoryId == categoryId);
            }

            var associations = await query.ToListAsync();
            return View(associations);
        }


        public async Task<IActionResult> CreateAssociation()
        {
            ViewBag.Categories = await _context.AssociationCategories.ToListAsync();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateAssociation(CulturalAssociation association)
        {
            if (ModelState.IsValid)
            {
                association.Status = "Pending";

                _context.CulturalAssociations.Add(association);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم إضافة الهيئة الثقافية بنجاح وهي قيد المراجعة";
                return RedirectToAction("AllAssociation");
            }

            ViewBag.Categories = await _context.AssociationCategories.ToListAsync();
            return View(association);
        }



    }
}
