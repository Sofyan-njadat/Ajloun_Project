using Ajloun_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Project.Controllers
{
    public class ArtWorksController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ArtWorksController(MyDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult ArtWorks()
        {
            var artworks = _context.Artworks
                      .Where(a => a.Status == "Approved").ToList();
            
            return View(artworks);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitArtwork(Artwork model, IFormFile ImageFile)
        {
            if (!ModelState.IsValid || ImageFile == null)
            {
                TempData["Error"] = "Please fill all fields and upload an image.";
                return RedirectToAction("Artworks");
            }
            // Save image to wwwroot/Images/Artworks
            var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
            var path = Path.Combine(_env.WebRootPath, "Images", "Artworks");
            Directory.CreateDirectory(path); // Ensure folder exists
            var fullPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            model.ImageUrl = "/Images/Artworks/" + fileName;
            model.Status = "Pending";

            _context.Artworks.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم إرسال العمل الفني بنجاح!";
            return RedirectToAction("Artworks");
        }
    }
}
