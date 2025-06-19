using System.Diagnostics;
using Ajloun_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using Ajloun_Project.Services;
using Ajloun_Project.Services;

namespace Ajloun_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _context; // ? ??? DbContext
        private readonly IContactEmailService _emailService;

        public HomeController(ILogger<HomeController> logger, MyDbContext context, IContactEmailService emailService)
        {
            _logger = logger;
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            var culturalEvents = _context.CulturalEvents
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => new
                {
                    e.Title,
                    e.Description,
                    e.PosterUrl
                });

            var assocEvents = _context.AssociationEvents
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => new
                {
                    e.Title,
                    e.Description,
                    e.PosterUrl
                });

            ViewBag.UpcomingEvents = culturalEvents.Concat(assocEvents).Take(6).ToList();

            //        var artists = _context.Artworks
            //.GroupBy(a => new { a.ArtistName, a.ArtistEmail, a.ImageUrl })
            //.Select(g => new
            //{
            //    ArtistName = g.Key.ArtistName,
            //    ArtistEmail = g.Key.ArtistEmail,
            //    ImageUrl = g.Key.ImageUrl
            //}).ToList();

            var artists = _context.Artworks
                  .Where(a => a.Status == "Approved").ToList();
            ViewBag.Artists = artists;


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

        // إضافة إجراء جديد لصفحة About Us
        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var emailBody = $@"
        <h3>رسالة جديدة من نموذج الاتصال</h3>
        <p><strong>الاسم:</strong> {model.Name}</p>
        <p><strong>البريد الإلكتروني:</strong> {model.Email}</p>
        <p><strong>رقم الهاتف:</strong> {model.Phone}</p>
        <p><strong>الموضوع:</strong> {model.Subject}</p>
        <p><strong>الرسالة:</strong></p>
        <p>{model.Message}</p>";

            var success = await _emailService.SendContactEmailAsync(
                $"رسالة جديدة: {model.Subject}",
                emailBody,
                model.Email,
                model.Name
            );

            if (success)
            {
                TempData["SuccessMessage"] = "تم إرسال رسالتك بنجاح. سنتواصل معك قريباً.";
                return RedirectToAction(nameof(Contact));
            }
            else
            {
                ModelState.AddModelError("", "حدث خطأ أثناء إرسال الرسالة. الرجاء المحاولة مرة أخرى.");
                return View(model);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
