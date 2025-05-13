using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ajloun_Project.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Ajloun_Project.Areas.Admin.Controllers
{
    public class FestivalsController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FestivalsController(MyDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/Festivals
        public async Task<IActionResult> AdminFestivals()
        {
            var festivals = await _context.Festivals
                .OrderByDescending(f => f.StartDate)
                .ToListAsync();

            return View(festivals);
        }

        // GET: Admin/Festivals/Get/5
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var festival = await _context.Festivals.FindAsync(id);
            if (festival == null)
            {
                return NotFound();
            }

            return Json(new
            {
                festivalId = festival.FestivalId,
                title = festival.Title,
                description = festival.Description,
                festivalType = festival.FestivalType,
                startDate = festival.StartDate?.ToString("yyyy-MM-dd") ?? null,
                endDate = festival.EndDate?.ToString("yyyy-MM-dd") ?? null,
                location = festival.Location,
                bannerImageUrl = festival.BannerImageUrl
            });
        }

        // POST: Admin/Festivals/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Festival festival, IFormFile bannerImage)
        {
            try
            {
                if (festival.FestivalId > 0)
                {
                    // تحديث مهرجان موجود
                    var existingFestival = await _context.Festivals.FindAsync(festival.FestivalId);
                    if (existingFestival == null)
                    {
                        return Json(new { success = false, message = "المهرجان غير موجود" });
                    }

                    // تحديث البيانات الأساسية
                    existingFestival.Title = festival.Title;
                    existingFestival.Description = festival.Description;
                    existingFestival.FestivalType = festival.FestivalType;
                    existingFestival.StartDate = festival.StartDate;
                    existingFestival.EndDate = festival.EndDate;
                    existingFestival.Location = festival.Location;

                    // معالجة الصورة إذا تم تحميل صورة جديدة
                    if (bannerImage != null && bannerImage.Length > 0)
                    {
                        // حذف الصورة القديمة
                        if (!string.IsNullOrEmpty(existingFestival.BannerImageUrl))
                        {
                            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingFestival.BannerImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // حفظ الصورة الجديدة
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "FestivalsIMGs");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + bannerImage.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await bannerImage.CopyToAsync(fileStream);
                        }

                        existingFestival.BannerImageUrl = "/FestivalsIMGs/" + uniqueFileName;
                    }

                    _context.Update(existingFestival);
                }
                else
                {
                    // إضافة مهرجان جديد
                    festival.CreatedAt = DateTime.Now;

                    if (bannerImage != null && bannerImage.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "FestivalsIMGs");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + bannerImage.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await bannerImage.CopyToAsync(fileStream);
                        }

                        festival.BannerImageUrl = "/FestivalsIMGs/" + uniqueFileName;
                    }

                    _context.Add(festival);
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // DELETE: Admin/Festivals/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var festival = await _context.Festivals.FindAsync(id);
            if (festival == null)
            {
                return NotFound();
            }

            // حذف الصورة إذا كانت موجودة
            if (!string.IsNullOrEmpty(festival.BannerImageUrl))
            {
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, festival.BannerImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Festivals.Remove(festival);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        private bool FestivalExists(int id)
        {
            return _context.Festivals.Any(e => e.FestivalId == id);
        }


        //User
        public IActionResult Festivals(string? festivalType, string? dateFilter, string? festivalLocation)
        {
            //HttpContext.Session.SetInt32("userId", 1);

            var query = _context.Festivals.AsQueryable();

            // تطبيق فلتر النوع
            if (!string.IsNullOrEmpty(festivalType))
            {
                query = query.Where(f => f.FestivalType == festivalType);
            }

            // تطبيق فلتر التاريخ
            if (!string.IsNullOrEmpty(dateFilter))
            {
                if (dateFilter == "upcoming")
                {
                    query = query.Where(f => f.EndDate >= DateTime.Today);
                }
                else if (dateFilter == "past")
                {
                    query = query.Where(f => f.EndDate < DateTime.Today);
                }
            }

            if (!string.IsNullOrEmpty(festivalLocation))
            {
                query = query.Where(f => f.Location == festivalLocation);
            }

            var festivals = query.ToList();

            var types = _context.Festivals
                .Select(f => f.FestivalType)
                .Distinct()
                .ToList();

            var locations = _context.Festivals
                .Select(f => f.Location)
                .Distinct()
                .ToList();

            ViewBag.Types = types;
            ViewBag.Locations = locations;
            ViewBag.SelectedType = festivalType;
            ViewBag.SelectedDate = dateFilter;
            ViewBag.SelectedLocation = festivalLocation;

            return View(festivals);
        }

        [HttpPost]
        public async Task<IActionResult> createFestivalBooking(FestivalReservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            var userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            reservation.UserId = userId.Value;

            _context.FestivalReservations.Add(reservation);
            await _context.SaveChangesAsync();

            ViewBag.UserId = HttpContext.Session.GetInt32("userId");

            TempData["SuccessMessage"] = "تم تسجيل مشاركتك بنجاح في المهرجان!";

            return RedirectToAction("Festivals");
        }
    }
} 