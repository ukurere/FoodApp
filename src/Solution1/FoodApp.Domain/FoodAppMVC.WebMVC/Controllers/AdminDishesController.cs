using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodApp.Infrastructure;
using FoodApp.Domain.Entities;
using FoodAppMVC.WebMVC.Models;

namespace FoodAppMVC.WebMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDishesController : Controller
    {
        private readonly FoodAppContext _context;
        private readonly IWebHostEnvironment _environment;

        public AdminDishesController(FoodAppContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: /AdminDishes
        public async Task<IActionResult> Index()
        {
            var dishes = await _context.Dishes.ToListAsync();
            return View(dishes);
        }

        // GET: /AdminDishes/Create
        [HttpGet]
        public IActionResult Create()
        {
            var ingredients = _context.Ingredients.ToList();
            ViewBag.Ingredients = new MultiSelectList(ingredients, "IngredientId", "Name");

            return View(new AddDishViewModel());
        }


        // POST: /AdminDishes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddDishViewModel model)
        {
            ViewBag.Ingredients = new MultiSelectList(_context.Ingredients.ToList(), "Id", "Name");

            if (!ModelState.IsValid)
                return View(model);

            var dish = new Dish
            {
                Name = model.Name,
                Type = model.Type,
                PreparationTimeMinutes = model.PreparationTimeMinutes,
                Recipe = model.Recipe,
                Calories = model.Calories,
                Proteins = model.Proteins,
                Fats = model.Fats,
                Carbohydrates = model.Carbohydrates
            };

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(model.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                await model.ImageFile.CopyToAsync(fileStream);
                dish.ImagePath = "/images/" + uniqueFileName;
            }

            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();

            if (model.SelectedIngredientIds != null)
            {
                foreach (var id in model.SelectedIngredientIds)
                {
                    _context.DishIngredients.Add(new DishIngredient { DishID = dish.DishId, IngredientID = id });
                }
                await _context.SaveChangesAsync();
            }

            // Переходимо на сторінку всіх користувачів
            return RedirectToAction("Index", "AdminDishes");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dish = await _context.Dishes
                .Include(d => d.DishIngredients)
                .FirstOrDefaultAsync(d => d.DishId == id);

            if (dish == null) return NotFound();

            var model = new EditDishViewModel
            {
                DishId = dish.DishId,
                Name = dish.Name,
                Type = dish.Type,
                PreparationTimeMinutes = dish.PreparationTimeMinutes,
                Recipe = dish.Recipe,
                Calories = dish.Calories,
                Proteins = dish.Proteins,
                Fats = dish.Fats,
                Carbohydrates = dish.Carbohydrates,
                SelectedIngredientIds = dish.DishIngredients.Select(di => di.IngredientID).ToList(),
                ExistingImagePath = dish.ImagePath
            };

            ViewBag.AllIngredients = _context.Ingredients.Select(i => new SelectListItem
            {
                Value = i.IngredientId.ToString(),
                Text = i.Name,
                Selected = model.SelectedIngredientIds.Contains(i.IngredientId)
            }).ToList();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditDishViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AllIngredients = _context.Ingredients.Select(i => new SelectListItem
                {
                    Value = i.IngredientId.ToString(),
                    Text = i.Name,
                    Selected = model.SelectedIngredientIds.Contains(i.IngredientId)
                }).ToList();

                return View(model);
            }

            var dish = await _context.Dishes
                .Include(d => d.DishIngredients)
                .FirstOrDefaultAsync(d => d.DishId == id);

            if (dish == null) return NotFound();

            dish.Name = model.Name;
            dish.Type = model.Type;
            dish.PreparationTimeMinutes = model.PreparationTimeMinutes;
            dish.Recipe = model.Recipe;
            dish.Calories = model.Calories;
            dish.Proteins = model.Proteins;
            dish.Fats = model.Fats;
            dish.Carbohydrates = model.Carbohydrates;

            // Обробка зображення
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(model.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await model.ImageFile.CopyToAsync(stream);
                dish.ImagePath = "/images/" + uniqueFileName;
            }

            // Заміна інгредієнтів
            _context.DishIngredients.RemoveRange(dish.DishIngredients);

            if (model.SelectedIngredientIds != null)
            {
                foreach (var idIngr in model.SelectedIngredientIds)
                {
                    _context.DishIngredients.Add(new DishIngredient
                    {
                        DishID = dish.DishId,
                        IngredientID = idIngr
                    });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dish = await _context.Dishes
                .Include(d => d.DishIngredients)
                .FirstOrDefaultAsync(d => d.DishId == id);

            if (dish == null) return NotFound();

            _context.DishIngredients.RemoveRange(dish.DishIngredients);
            _context.Dishes.Remove(dish);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
