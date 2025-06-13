using Microsoft.AspNetCore.Mvc;
using Ajloun_Project.Models;
using System.IO;
using System.Linq;

namespace Ajloun_Project.Controllers
{
    public class GalleryController : Controller
    {
        private readonly MyDbContext _context;

        public GalleryController(MyDbContext context)
        {
            _context = context;
        }

        // عرض الصور للزوار
        public IActionResult Gallery()
        {
            var images = _context.GalleryImages.ToList();
            return View(images);
        }

        // عرض الصور للادمن مع خيارات الحذف
        public IActionResult AdminGallery()
        {
            var images = _context.GalleryImages.ToList();
            return View(images);
        }



        [HttpPost]
        public IActionResult Add(string Title, string Category, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Gallery");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                var model = new GalleryImage
                {
                    Title = Title,
                    Category = Category,
                    ImageUrl = "/Images/Gallery/" + fileName
                };

                _context.GalleryImages.Add(model);
                _context.SaveChanges();
            }

            return RedirectToAction("AdminGallery");
        }

        // حذف صورة
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var image = _context.GalleryImages.Find(id);
            if (image != null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                _context.GalleryImages.Remove(image);
                _context.SaveChanges();
            }

            return RedirectToAction("AdminGallery");
        }
    }
}
