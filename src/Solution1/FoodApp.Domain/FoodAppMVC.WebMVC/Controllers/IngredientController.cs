using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodApp.Domain.Entities;
using FoodApp.Infrastructure;

namespace FoodAppMVC.WebMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientController : ControllerBase
    {
        private readonly FoodAppContext _context;

        public IngredientController(FoodAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingredient>>> GetIngredients()
        {
            return await _context.Ingredients.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ingredient>> GetIngredient(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null) return NotFound();
            return ingredient;
        }

        [HttpPost]
        public async Task<ActionResult<Ingredient>> CreateIngredient(Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetIngredient), new { id = ingredient.IngredientId }, ingredient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIngredient(int id, Ingredient ingredient)
        {
            if (id != ingredient.IngredientId) return BadRequest();
            _context.Entry(ingredient).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null) return NotFound();
            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
