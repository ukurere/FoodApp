using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using FoodAppMVC.WebMVC.Models;
using FoodApp.Domain.Entities;
using FoodApp.Infrastructure;

namespace FoodAppMVC.WebMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly FoodAppContext _context;

        public UsersController(FoodAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "A user with this email already exists.");
                return View(model);
            }

            var isAdmin = model.Email.ToLower() == "yevgieniia.riabichenko@gmail.com";

            var user = new User
            {
                Username = isAdmin ? "admin" : model.Username,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                RegistrationDate = DateTime.Now,
                Role = isAdmin ? "Admin" : "Visitor"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            await SignInUser(user);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError("Email", "Invalid email or password.");
                return View(model);
            }

            await SignInUser(user);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _context.Users.FindAsync(userId);

            return user == null ? NotFound() : View(user);
        }

        [Authorize]
        public async Task<IActionResult> Favorites()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var favoriteDishes = await _context.FavoriteDishes
                .Where(f => f.UserID == userId)
                .Include(f => f.Dish)
                .Select(f => f.Dish)
                .ToListAsync();

            var model = favoriteDishes.Select(d => new DishViewModel
            {
                Id = d.DishId,
                Name = d.Name,
                Description = d.Recipe,
                CookingTime = d.PreparationTimeMinutes,
                Type = d.Type,
                ImageUrl = $"/images/{d.Name.ToLower().Replace(" ", "")}.jpg",
                IsFavorite = true
            }).ToList();

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToFavorites(int dishId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (!_context.FavoriteDishes.Any(f => f.UserID == userId && f.DishID == dishId))
            {
                _context.FavoriteDishes.Add(new FavoriteDish { UserID = userId, DishID = dishId });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Favorites");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RemoveFromFavorites(int dishId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var favorite = await _context.FavoriteDishes
                .FirstOrDefaultAsync(f => f.UserID == userId && f.DishID == dishId);

            if (favorite != null)
            {
                _context.FavoriteDishes.Remove(favorite);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Favorites");
        }

        [Authorize]
        public async Task<IActionResult> Ratings()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var ratings = await _context.Ratings
                .Where(r => r.UserID == userId)
                .Include(r => r.Dish)
                .ToListAsync();

            return View(ratings);
        }

        [Authorize]
        public async Task<IActionResult> Comments()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var comments = await _context.Comments
                .Where(c => c.UserID == userId)
                .Include(c => c.Dish)
                .ToListAsync();

            return View(comments);
        }

        private async Task SignInUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        private bool UserExists(int id) => _context.Users.Any(e => e.UserId == id);

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied() => View();

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // ------------------- CRUD ACTIONS ------------------------

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            user.RegistrationDate = DateTime.Now;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash); // expect plain text

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            return user == null ? NotFound() : View(user);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user == null ? NotFound() : View(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var existing = await _context.Users.FindAsync(user.UserId);
            if (existing == null)
                return NotFound();

            existing.Username = user.Username;
            existing.Email = user.Email;
            existing.Role = user.Role;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user == null ? NotFound() : View(user);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
