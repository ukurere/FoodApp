using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodApp.Infrastructure;

namespace FoodAppMVC.WebMVC.Controllers
{
    public class FavoriteDishesController : Controller
    {
        private readonly FoodAppContext _context;

        public FavoriteDishesController(FoodAppContext context)
        {
            _context = context;
        }

        // GET: FavoriteDishes
        public async Task<IActionResult> Index()
        {
            var foodAppContext = _context.FavoriteDishes.Include(f => f.Dish).Include(f => f.User);
            return View(await foodAppContext.ToListAsync());
        }

        // GET: FavoriteDishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favoriteDish = await _context.FavoriteDishes
                .Include(f => f.Dish)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favoriteDish == null)
            {
                return NotFound();
            }

            return View(favoriteDish);
        }

        // GET: FavoriteDishes/Create
        public IActionResult Create()
        {
            ViewData["DishID"] = new SelectList(_context.Dishes, "DishId", "Name");
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        // POST: FavoriteDishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,DishID,DateAdded,Id")] FavoriteDish favoriteDish)
        {
            if (ModelState.IsValid)
            {
                _context.Add(favoriteDish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DishID"] = new SelectList(_context.Dishes, "DishId", "Name", favoriteDish.DishID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "Email", favoriteDish.UserID);
            return View(favoriteDish);
        }

        // GET: FavoriteDishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favoriteDish = await _context.FavoriteDishes.FindAsync(id);
            if (favoriteDish == null)
            {
                return NotFound();
            }
            ViewData["DishID"] = new SelectList(_context.Dishes, "DishId", "Name", favoriteDish.DishID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "Email", favoriteDish.UserID);
            return View(favoriteDish);
        }

        // POST: FavoriteDishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,DishID,DateAdded,Id")] FavoriteDish favoriteDish)
        {
            if (id != favoriteDish.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favoriteDish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavoriteDishExists(favoriteDish.Id))
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
            ViewData["DishID"] = new SelectList(_context.Dishes, "DishId", "Name", favoriteDish.DishID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "Email", favoriteDish.UserID);
            return View(favoriteDish);
        }

        // GET: FavoriteDishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favoriteDish = await _context.FavoriteDishes
                .Include(f => f.Dish)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favoriteDish == null)
            {
                return NotFound();
            }

            return View(favoriteDish);
        }

        // POST: FavoriteDishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var favoriteDish = await _context.FavoriteDishes.FindAsync(id);
            if (favoriteDish != null)
            {
                _context.FavoriteDishes.Remove(favoriteDish);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavoriteDishExists(int id)
        {
            return _context.FavoriteDishes.Any(e => e.Id == id);
        }
    }
}
