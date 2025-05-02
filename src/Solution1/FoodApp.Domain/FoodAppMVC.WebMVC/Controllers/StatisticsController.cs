using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodApp.Infrastructure;
using FoodAppMVC.WebMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodAppMVC.WebMVC.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly FoodAppContext _context;

        public StatisticsController(FoodAppContext context)
        {
            _context = context;
        }

        // GET: /Statistics
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Statistics/PopularDishes
        [HttpGet]
        public async Task<IActionResult> PopularDishes(DateTime? startDate, DateTime? endDate)
        {
            var favorites = _context.FavoriteDishes.Include(f => f.Dish).AsQueryable();
            var comments = _context.Comments.Include(c => c.Dish).AsQueryable();
            var ratings = _context.Ratings.Include(r => r.Dish).AsQueryable();

            if (startDate.HasValue)
            {
                var from = startDate.Value;
                favorites = favorites.Where(f => f.DateAdded >= from);
                comments = comments.Where(c => c.DatePosted >= from);
                ratings = ratings.Where(r => r.DateRated >= from);
            }

            if (endDate.HasValue)
            {
                var to = endDate.Value;
                favorites = favorites.Where(f => f.DateAdded <= to);
                comments = comments.Where(c => c.DatePosted <= to);
                ratings = ratings.Where(r => r.DateRated <= to);
            }

            var favoriteCounts = await favorites
                .GroupBy(f => new { f.DishID, f.Dish.Name })
                .Select(g => new { g.Key.Name, Likes = g.Count() })
                .ToListAsync();

            var commentCounts = await comments
                .GroupBy(c => new { c.DishID, c.Dish.Name })
                .Select(g => new { g.Key.Name, Comments = g.Count() })
                .ToListAsync();

            var ratingAverages = await ratings
                .GroupBy(r => new { r.DishID, r.Dish.Name })
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
                    AverageRating = Math.Round(ratingAverages.FirstOrDefault(r => r.Name == name)?.AvgRating ?? 0, 2)
                })
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
        public async Task<IActionResult> DishStatsData(int dishId, DateTime? startDate, DateTime? endDate)
        {
            var likes = _context.FavoriteDishes.Where(f => f.DishID == dishId);
            var comments = _context.Comments.Where(c => c.DishID == dishId);
            var ratings = _context.Ratings.Where(r => r.DishID == dishId);

            if (startDate.HasValue)
            {
                var from = startDate.Value;
                likes = likes.Where(f => f.DateAdded >= from);
                comments = comments.Where(c => c.DatePosted >= from);
                ratings = ratings.Where(r => r.DateRated >= from);
            }

            if (endDate.HasValue)
            {
                var to = endDate.Value;
                likes = likes.Where(f => f.DateAdded <= to);
                comments = comments.Where(c => c.DatePosted <= to);
                ratings = ratings.Where(r => r.DateRated <= to);
            }

            var likeCount = await likes.CountAsync();
            var commentCount = await comments.CountAsync();
            var avgRating = await ratings.AnyAsync()
                ? Math.Round(await ratings.AverageAsync(r => r.RatingValue), 2)
                : 0;

            return Json(new
            {
                Likes = likeCount,
                Comments = commentCount,
                AverageRating = avgRating
            });
        }

    }
}
