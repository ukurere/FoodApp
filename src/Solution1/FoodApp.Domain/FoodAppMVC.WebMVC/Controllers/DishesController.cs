using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodApp.Infrastructure;

namespace FoodAppMVC.WebMVC.Controllers;

public class DishesController : Controller
{
    private readonly FoodAppContext _context;

    public DishesController(FoodAppContext context)
    {
        _context = context;
    }

    // GET: Dishes
    public async Task<IActionResult> Index()
    {
        return View(await _context.Dishes.ToListAsync());
    }

    // GET: Dishes/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var dish = await _context.Dishes
            .FirstOrDefaultAsync(m => m.DishId == id);
        if (dish == null)
        {
            return NotFound();
        }

        return View(dish);
    }

    // GET: Dishes/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Dishes/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("DishId,Name,PreparationTimeMinutes,Type,Recipe")] Dish dish)
    {
        if (ModelState.IsValid)
        {
            _context.Add(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(dish);
    }

    // GET: Dishes/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var dish = await _context.Dishes.FindAsync(id);
        if (dish == null)
        {
            return NotFound();
        }
        return View(dish);
    }

    // POST: Dishes/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("DishId,Name,PreparationTimeMinutes,Type,Recipe")] Dish dish)
    {
        if (id != dish.DishId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(dish);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishExists(dish.DishId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(dish);
    }

    // GET: Dishes/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var dish = await _context.Dishes
            .FirstOrDefaultAsync(m => m.DishId == id);
        if (dish == null)
        {
            return NotFound();
        }

        return View(dish);
    }

    // POST: Dishes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var dish = await _context.Dishes.FindAsync(id);
        if (dish != null)
        {
            _context.Dishes.Remove(dish);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DishExists(int id)
    {
        return _context.Dishes.Any(e => e.DishId == id);
    }
}
