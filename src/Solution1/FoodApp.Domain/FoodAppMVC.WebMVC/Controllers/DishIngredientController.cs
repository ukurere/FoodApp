using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodApp.Infrastructure;
using FoodApp.Domain.Entities;

namespace FoodAppMVC.WebMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishIngredientController : ControllerBase
    {
        private readonly FoodAppContext _context;

        public DishIngredientController(FoodAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDishIngredients()
        {
            var dishIngredients = await _context.DishIngredients.ToListAsync();
            return Ok(dishIngredients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDishIngredientById(int id)
        {
            var dishIngredient = await _context.DishIngredients.FindAsync(id);
            if (dishIngredient == null) return NotFound();

            return Ok(dishIngredient);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDishIngredient([FromBody] DishIngredient dishIngredient)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.DishIngredients.Add(dishIngredient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDishIngredientById), new { id = dishIngredient.Id }, dishIngredient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDishIngredient(int id, [FromBody] DishIngredient dishIngredient)
        {
            if (id != dishIngredient.Id) return BadRequest();

            _context.Entry(dishIngredient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishIngredientExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDishIngredient(int id)
        {
            var dishIngredient = await _context.DishIngredients.FindAsync(id);
            if (dishIngredient == null) return NotFound();

            _context.DishIngredients.Remove(dishIngredient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DishIngredientExists(int id)
        {
            return _context.DishIngredients.Any(e => e.Id == id);
        }
    }
}
