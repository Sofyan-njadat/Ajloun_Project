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

            // إحصائيات المهرجانات
            var festivalsData = await _context.Festivals
                .Where(f => f.StartDate.HasValue && 
                           f.StartDate.Value.Date >= last7Days.First() && 
                           f.StartDate.Value.Date <= last7Days.Last())
                .GroupBy(f => f.StartDate.Value.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            // إحصائيات الكتب (متاحة ومستعارة)
            var availableBooks = await _context.Books.Where(b => b.IsAvailable).CountAsync();
            var borrowedBooks = await _context.Books.Where(b => !b.IsAvailable).CountAsync();

            var statistics = new
            {
                TotalFestivals = await _context.Festivals.CountAsync(),
                TotalNews = await _context.News.CountAsync(),
                RecentNews = await _context.News
                    .OrderByDescending(n => n.PublishDate)
                    .Take(5)
                    .Select(n => new
                    {
                        n.NewsId,
                        n.Title,
                        n.PublishDate
                    })
                    .ToListAsync(),
                LibraryStats = new
                {
                    TotalBooks = await _context.Books.CountAsync(),
                    AvailableBooks = availableBooks,
                    BorrowedBooks = borrowedBooks,
                    TotalCategories = await _context.BookCategories.CountAsync()
                },
                // بيانات الرسم البياني
                ChartData = new
                {
                    Labels = last7Days.Select(d => d.ToString("yyyy-MM-dd")).ToList(),
                    FestivalsData = last7Days.Select(date => 
                        festivalsData.FirstOrDefault(f => f.Date.Date == date.Date)?.Count ?? 0).ToList(),
                    BooksData = last7Days.Select(_ => availableBooks).ToList() // عرض عدد الكتب المتاحة لكل يوم
                }
            };

            return View(statistics);
        }
    }
} 