using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodApp.Domain.Entities;
using FoodApp.Infrastructure;
using FoodAppMVC.WebMVC.Models; // для ViewModels
using System.Security.Cryptography;
using System.Text;

namespace FoodAppMVC.WebMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly FoodAppContext _context;

        public UsersController(FoodAppContext context)
        {
            _context = context;
        }

        // === LOGIN ===
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var hashedPassword = HashPassword(model.Password);
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.PasswordHash == hashedPassword);

            if (user == null)
            {
                ModelState.AddModelError("", "Неправильна пошта або пароль");
                return View(model);
            }

            // TODO: зберегти логін сесію або кукі
            TempData["Message"] = $"Вітаємо, {user.Username}!";
            return RedirectToAction("Index", "Home");
        }

        // === REGISTER ===
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Користувач з такою поштою вже існує.");
                return View(model);
            }

            var newUser = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = HashPassword(model.Password),
                RegistrationDate = DateTime.Now
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }

        // === SIMPLE HASHING ===
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        // решта CRUD-методів залиш без змін...

        private bool UserExists(int id) => _context.Users.Any(e => e.UserId == id);
    }
}
