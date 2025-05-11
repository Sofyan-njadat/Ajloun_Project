using AjlounProjecttt.Models;
using AjlounProjecttt.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AjlounProjecttt.Controllers
{
    public class ArtworksController : Controller
    {
        private readonly MyDbContext _context;

        public ArtworksController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult PendingArtworks()
        {
            var artworks = _context.Artworks
                .Where(a => a.Status == "Pending")
                .Select(a => new PendingArtworkViewModel
                {
                    ArtworkId = a.ArtworkId,
                    Title = a.Title,
                    ArtistName = a.ArtistName,
                    Type = a.Type,
                    Description = a.Description,
                    ImageUrl = a.ImageUrl
                })
                .ToList();

            return View(artworks);
        }

        public IActionResult Approve(int id)
        {
            var artwork = _context.Artworks.Find(id);
            if (artwork != null)
            {
                artwork.Status = "Approved";
                _context.SaveChanges();
            }

            return RedirectToAction("PendingArtworks");
        }

        public IActionResult Reject(int id)
        {
            var artwork = _context.Artworks.Find(id);
            if (artwork != null)
            {
                artwork.Status = "Rejected";
                _context.SaveChanges();
            }

            return RedirectToAction("PendingArtworks");
        }
    }
}
