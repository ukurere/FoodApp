using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodApp.Infrastructure;
using FoodAppMVC.WebMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FoodAppMVC.WebMVC.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly FoodAppContext _context;

        public StatisticsController(FoodAppContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PopularDishes(DateTime? endDate)
        {
            var favoritesQuery = _context.FavoriteDishes.Include(f => f.Dish).AsQueryable();
            var commentsQuery = _context.Comments.Include(c => c.Dish).AsQueryable();
            var ratingsQuery = _context.Ratings.Include(r => r.Dish).AsQueryable();

            if (endDate.HasValue)
            {
                var to = endDate.Value.Date.AddDays(1).AddTicks(-1);
                favoritesQuery = favoritesQuery.Where(f => f.DateAdded <= to);
                commentsQuery = commentsQuery.Where(c => c.DatePosted <= to);
                ratingsQuery = ratingsQuery.Where(r => r.DateRated <= to);
            }

            var favoriteCounts = await favoritesQuery
                .GroupBy(f => new { f.DishID, f.Dish.Name })
                .Select(g => new { g.Key.Name, Likes = g.Count() })
                .ToListAsync();

            var commentCounts = await commentsQuery
                .GroupBy(c => new { c.DishID, c.Dish.Name })
                .Select(g => new { g.Key.Name, Comments = g.Count() })
                .ToListAsync();

            var ratingAverages = await ratingsQuery
                .GroupBy(r => new { r.DishID, r.Dish.Name })
                .Where(g => g.Any())
                .Select(g => new { g.Key.Name, AvgRating = g.Average(r => r.RatingValue) })
                .ToListAsync();

            var allDishNames = favoriteCounts.Select(x => x.Name)
                .Union(commentCounts.Select(x => x.Name))
                .Union(ratingAverages.Select(x => x.Name))
                .Distinct();

            var result = allDishNames
                .Select(name => new DishStatsDto
                {
                    DishName = name,
                    Likes = favoriteCounts.FirstOrDefault(f => f.Name == name)?.Likes ?? 0,
                    Comments = commentCounts.FirstOrDefault(c => c.Name == name)?.Comments ?? 0,
                    AverageRating = ratingAverages.FirstOrDefault(r => r.Name == name)?.AvgRating ?? 0
                })
                .Where(x => x.Likes > 0 || x.Comments > 0 || x.AverageRating > 0)
                .OrderByDescending(x => x.Likes + x.Comments + x.AverageRating)
                .Take(5)
                .ToList();

            return Json(result);
        }

        [HttpGet]
        public IActionResult DishStats()
        {
            var dishList = _context.Dishes
                .Select(d => new { d.DishId, d.Name })
                .ToList();

            ViewBag.Dishes = new SelectList(dishList, "DishId", "Name");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DishStatsTimeline(int dishId, DateTime? startDate, DateTime? endDate)
        {
            var allDates = await _context.Comments
                .Where(c => c.DishID == dishId)
                .Select(c => c.DatePosted.Date)
                .Union(_context.FavoriteDishes.Where(f => f.DishID == dishId).Select(f => f.DateAdded.Date))
                .Union(_context.Ratings.Where(r => r.DishID == dishId).Select(r => r.DateRated.Date))
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();

            if (allDates.Count == 0)
                return Json(new object[0]);

            var firstDate = startDate ?? allDates.First();
            var lastDate = endDate?.Date ?? DateTime.UtcNow.Date;

            var timeline = new List<object>();

            for (var date = firstDate.Date; date <= lastDate; date = date.AddDays(1))
            {
                var likeCount = await _context.FavoriteDishes.CountAsync(f => f.DishID == dishId && f.DateAdded.Date <= date);
                var commentCount = await _context.Comments.CountAsync(c => c.DishID == dishId && c.DatePosted.Date <= date);
                var relevantRatings = await _context.Ratings
                    .Where(r => r.DishID == dishId && r.DateRated.Date <= date)
                    .ToListAsync();
                var avgRating = relevantRatings.Count > 0 ? Math.Round(relevantRatings.Average(r => r.RatingValue), 2) : 0;

                timeline.Add(new
                {
                    date = date.ToString("yyyy-MM-dd"),
                    likes = likeCount,
                    comments = commentCount,
                    averageRating = avgRating
                });
            }

            return Json(timeline);
        }
    }
}
