using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FoodApp.Infrastructure;
using FoodApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodAppMVC.WebMVC.Controllers
{
    [Authorize]
    public class FavoriteDishesController : Controller
    {
        private readonly FoodAppContext _context;

        public FavoriteDishesController(FoodAppContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int dishId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var favorite = await _context.FavoriteDishes
                .FirstOrDefaultAsync(f => f.UserID == userId && f.DishID == dishId);

            if (favorite != null)
            {
                _context.FavoriteDishes.Remove(favorite);
            }
            else
            {
                _context.FavoriteDishes.Add(new FavoriteDish { DishID = dishId, UserID = userId });
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
