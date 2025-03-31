using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodApp.Infrastructure;
using FoodAppMVC.WebMVC.Models;


namespace FoodAppMVC.WebMVC.Controllers;

public class StatsController : Controller
{
    private readonly FoodAppContext _context;

    public StatsController(FoodAppContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> IngredientFrequency()
    {
        var data = await _context.DishIngredients
            .Include(di => di.Ingredient)
            .GroupBy(di => di.Ingredient.Name)
            .Select(g => new IngredientFrequencyViewModel
            {
                IngredientName = g.Key,
                UsageCount = g.Count()
            })
            .OrderByDescending(x => x.UsageCount)
            .ToListAsync();

        return View(data);
    }
}
