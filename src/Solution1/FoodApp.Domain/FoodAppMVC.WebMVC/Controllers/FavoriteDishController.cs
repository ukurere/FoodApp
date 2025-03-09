using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodApp.Infrastructure;
using FoodApp.Domain.Entities;

namespace FoodAppMVC.WebMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoriteDishController : ControllerBase
    {
        private readonly FoodAppContext _context;

        public FavoriteDishController(FoodAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFavoriteDishes()
        {
            var favoriteDishes = await _context.FavoriteDishes.ToListAsync();
            return Ok(favoriteDishes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFavoriteDishById(int id)
        {
            var favoriteDish = await _context.FavoriteDishes.FindAsync(id);
            if (favoriteDish == null) return NotFound();

            return Ok(favoriteDish);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFavoriteDish([FromBody] FavoriteDish favoriteDish)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.FavoriteDishes.Add(favoriteDish);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFavoriteDishById), new { id = favoriteDish.Id }, favoriteDish);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFavoriteDish(int id, [FromBody] FavoriteDish favoriteDish)
        {
            if (id != favoriteDish.Id) return BadRequest();

            _context.Entry(favoriteDish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavoriteDishExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFavoriteDish(int id)
        {
            var favoriteDish = await _context.FavoriteDishes.FindAsync(id);
            if (favoriteDish == null) return NotFound();

            _context.FavoriteDishes.Remove(favoriteDish);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FavoriteDishExists(int id)
        {
            return _context.FavoriteDishes.Any(e => e.Id == id);
        }
    }
}
