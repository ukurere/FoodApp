using Microsoft.AspNetCore.Mvc;
using FoodApp.Infrastructure;
using FoodAppMVC.WebMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FoodAppMVC.WebMVC.Controllers
{
    public class DishesController : Controller
    {
        private readonly FoodAppContext _context;

        public DishesController(FoodAppContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string? selectedTime, string? selectedType, List<string>? selectedIngredients)
        {
            var userId = User.Identity != null && User.Identity.IsAuthenticated
                ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
                : -1;

            var favoriteDishIds = userId != -1
                ? await _context.FavoriteDishes
                    .Where(f => f.UserID == userId)
                    .Select(f => f.DishID)
                    .ToListAsync()
                : new List<int>();

            var allIngredients = await _context.Ingredients.Select(i => i.Name).Distinct().ToListAsync();
            var allTimes = new List<string> { "5", "10", "20", "30", "40", "60" };
            var allTypes = new List<string> { "Breakfast", "Dessert", "Side Dish", "Salad" };

            var query = _context.Dishes
                .Include(d => d.DishIngredients)
                    .ThenInclude(di => di.Ingredient)
                .AsQueryable();

            if (!string.IsNullOrEmpty(selectedType))
                query = query.Where(d => d.Type == selectedType);

            if (!string.IsNullOrEmpty(selectedTime) && int.TryParse(selectedTime, out int maxTime))
                query = query.Where(d => d.PreparationTimeMinutes <= maxTime);

            if (selectedIngredients != null && selectedIngredients.Any())
                query = query.Where(d => d.DishIngredients.Any(di => selectedIngredients.Contains(di.Ingredient.Name)));

            var filteredDishes = await query.Select(d => new DishViewModel
            {
                Id = d.DishId,
                Name = d.Name,
                Description = d.Recipe,
                CookingTime = d.PreparationTimeMinutes,
                Type = d.Type,
                ImageUrl = $"/images/{d.Name.ToLower().Replace(" ", "")}.jpg",
                IsFavorite = favoriteDishIds.Contains(d.DishId)
            }).ToListAsync();

            return View(new DishesFilterViewModel
            {
                AllIngredients = allIngredients,
                AllTimes = allTimes,
                AllTypes = allTypes,
                SelectedTime = selectedTime,
                SelectedType = selectedType,
                SelectedIngredients = selectedIngredients ?? new(),
                FilteredDishes = filteredDishes
            });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var dish = await _context.Dishes
                .Include(d => d.DishIngredients)
                .ThenInclude(di => di.Ingredient)
                .FirstOrDefaultAsync(d => d.DishId == id);

            if (dish == null)
                return NotFound();

            bool isFavorite = false;

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                isFavorite = await _context.FavoriteDishes
                    .AnyAsync(f => f.UserID == userId && f.DishID == id);
            }

            ViewBag.IsFavorite = isFavorite;
            return View(dish);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Comments(int id)
        {
            var dish = await _context.Dishes
                .Include(d => d.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(d => d.DishId == id);

            if (dish == null)
                return NotFound();

            return View(dish);
        }

        [Authorize]
        public async Task<IActionResult> Favorites()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var favoriteDishes = await _context.FavoriteDishes
                .Where(f => f.UserID == userId)
                .Include(f => f.Dish)
                .Select(f => f.Dish)
                .ToListAsync();

            var model = favoriteDishes.Select(d => new DishViewModel
            {
                Id = d.DishId,
                Name = d.Name,
                Description = d.Recipe,
                CookingTime = d.PreparationTimeMinutes,
                Type = d.Type,
                ImageUrl = $"/images/{d.Name.ToLower().Replace(" ", "")}.jpg",
                IsFavorite = true
            }).ToList();

            return View(model);
        }

    }
}
