using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ajloun_Project.Models;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Ajloun_Project.Areas.Admin.Controllers
{
    public class StatisticsController(MyDbContext context) : Controller
    {
        private readonly MyDbContext _context = context;

        public async Task<IActionResult> Statistics()
        {
            // الحصول على آخر 7 أيام
            var last7Days = Enumerable.Range(0, 7)
                .Select(i => DateTime.Now.Date.AddDays(-i))
                .Reverse()
                .ToList();

            var statistics = new
            {
                // إحصائيات الدورات
                TotalCourses = await _context.Courses.CountAsync(),

                // إحصائيات المهرجانات
                TotalFestivals = await _context.Festivals.CountAsync(),

                // إحصائيات الفعاليات الثقافية
                TotalCulturalEvents = await _context.CulturalEvents.CountAsync(),

                // إحصائيات الهيئات الثقافية
                TotalCulturalAssociations = await _context.CulturalAssociations.CountAsync(),

                // إحصائيات حجوزات المسارح
                TotalHallBookings = await _context.HallBookings.CountAsync(),

                // إحصائيات الكتب
                TotalBooks = await _context.Books.CountAsync(),

                // إحصائيات حجوزات الكتب
                TotalBookReservations = await _context.BookReservations.CountAsync(),

                // إحصائيات الأعمال الفنية
                TotalArtworks = await _context.Artworks.CountAsync(),

                // إحصائيات الحرف اليدوية
                TotalHandicrafts = await _context.Handicrafts.CountAsync(),

                // إحصائيات المستخدمين
                TotalUsers = await _context.Users.CountAsync(),

                // بيانات الرسم البياني
                ChartData = new
                {
                    Labels = last7Days.Select(d => d.ToString("yyyy-MM-dd")).ToList(),
                    FestivalsData = last7Days.Select(date => 
                        _context.Festivals.Count(f => f.StartDate.HasValue && f.StartDate.Value.Date == date.Date)).ToList(),
                    BooksData = last7Days.Select(date => 
                        _context.Books.Count(b => b.CreatedAt.HasValue && b.CreatedAt.Value.Date == date.Date)).ToList()
                },

                // الأخبار الأخيرة
                RecentNews = await _context.News
                    .OrderByDescending(n => n.PublishDate)
                    .Take(5)
                    .Select(n => new
                    {
                        n.NewsId,
                        n.Title,
                        n.Content,
                        n.PublishDate,
                        n.ImageUrl
                    })
                    .ToListAsync()
            };

            return View(statistics);
        }
    }
} 