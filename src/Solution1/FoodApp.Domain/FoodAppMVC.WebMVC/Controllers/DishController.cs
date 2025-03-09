using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodApp.Domain.Entities;
using FoodApp.Infrastructure;

namespace FoodAppMVC.WebMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly FoodAppContext _context;

        public DishController(FoodAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dish>>> GetDishes()
        {
            return await _context.Dishes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> GetDish(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return NotFound();
            return dish;
        }

        [HttpPost]
        public async Task<ActionResult<Dish>> CreateDish(Dish dish)
        {
            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDish), new { id = dish.DishId }, dish);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDish(int id, Dish dish)
        {
            if (id != dish.DishId) return BadRequest();
            _context.Entry(dish).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return NotFound();
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
