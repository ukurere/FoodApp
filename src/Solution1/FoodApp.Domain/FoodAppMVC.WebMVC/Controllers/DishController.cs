using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodApp.Infrastructure;
using FoodApp.Domain.Entities;

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
        public async Task<IActionResult> GetAllDishes()
        {
            var dishes = await _context.Dishes.ToListAsync();
            return Ok(dishes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDishById(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return NotFound();

            return Ok(dish);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDish([FromBody] Dish dish)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDishById), new { id = dish.DishId }, dish);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDish(int id, [FromBody] Dish dish)
        {
            if (id != dish.DishId) return BadRequest();

            _context.Entry(dish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishExists(id)) return NotFound();
                throw;
            }

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

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.DishId == id);
        }
    }
}
