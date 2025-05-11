using AjlounProjecttt.Models;
using AjlounProjecttt.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AjlounProjecttt.Controllers
{
    public class CraftOrdersController : Controller
    {
        private readonly MyDbContext _context;

        public CraftOrdersController(MyDbContext context)
        {
            _context = context;
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

    }
}
