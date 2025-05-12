using System.Diagnostics;
using Ajloun_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Ajloun_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _context; // ? ??? DbContext

        public HomeController(ILogger<HomeController> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Articles(string? search, string? category)
        {
            var articles = _context.Articles.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                articles = articles.Where(a => a.Title.Contains(search));
            }

            if (!string.IsNullOrEmpty(category))
            {
                articles = articles.Where(a => a.Category == category);
            }

            return View(articles.OrderByDescending(a => a.PublishDate).ToList());
        }


        public IActionResult ArticleDetails(int id)
        {
            using (var context = new MyDbContext())
            {
                var article = context.Articles.FirstOrDefault(a => a.ArticleId == id);
                if (article == null)
                {
                    return NotFound();
                }

                return View(article);
            }
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
