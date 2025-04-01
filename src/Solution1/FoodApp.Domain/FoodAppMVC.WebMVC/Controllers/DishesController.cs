using Microsoft.AspNetCore.Mvc;
using FoodApp.Infrastructure;
using FoodAppMVC.WebMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodAppMVC.WebMVC.Controllers
{
    public class DishesController : Controller
    {
        private readonly FoodAppContext _context;

        public DishesController(FoodAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? selectedTime, string? selectedType, List<string>? selectedIngredients)
        {
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
                ImageUrl = $"/images/{d.Name.ToLower().Replace(" ", "")}.jpg"
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

        public async Task<IActionResult> Details(int id)
        {
            var dish = await _context.Dishes
                .Include(d => d.DishIngredients)
                .ThenInclude(di => di.Ingredient)
                .FirstOrDefaultAsync(d => d.DishId == id);

            if (dish == null)
                return NotFound();

            return View(dish);
        }


    }
}
