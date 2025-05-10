using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Project.Controllers
{
    public class GalleryController : Controller
    {
        public IActionResult Gallery()
        {
            return View();
        }
    }
}
