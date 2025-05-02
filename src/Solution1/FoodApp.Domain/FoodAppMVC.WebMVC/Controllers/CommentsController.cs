using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FoodApp.Domain.Entities;
using FoodApp.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodAppMVC.WebMVC.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly FoodAppContext _context;

        public CommentsController(FoodAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var foodAppContext = _context.Comments
                .Include(c => c.Dish)
                .Include(c => c.User);
            return View(await foodAppContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var comment = await _context.Comments
                .Include(c => c.Dish)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CommentId == id);

            return comment == null ? NotFound() : View(comment);
        }

        public IActionResult Create()
        {
            ViewData["DishID"] = new SelectList(_context.Dishes, "DishId", "Name");
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,UserID,DishID,Text,DatePosted,Id")] Comment comment)
        {
            if (!ModelState.IsValid)
                return View(comment);

            _context.Add(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return NotFound();

            ViewData["DishID"] = new SelectList(_context.Dishes, "DishId", "Name", comment.DishID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "Email", comment.UserID);
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentId,UserID,DishID,Text,DatePosted,Id")] Comment comment)
        {
            if (id != comment.CommentId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(comment);

            try
            {
                _context.Update(comment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(comment.CommentId))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var comment = await _context.Comments
                .Include(c => c.Dish)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CommentId == id);

            return comment == null ? NotFound() : View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
                _context.Comments.Remove(comment);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.CommentId == id);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOwn(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.CommentId == id && c.UserID == userId);

            if (comment == null)
                return Unauthorized();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Report(int commentId, string reason)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            // Перевірка, чи коментар існує
            var commentExists = await _context.Comments.AnyAsync(c => c.CommentId == commentId);
            if (!commentExists)
                return NotFound("Comment does not exist.");

            // Перевірка, чи вже є скарга від цього користувача
            var alreadyReported = await _context.CommentReports
                .AnyAsync(r => r.CommentId == commentId && r.ReporterId == userId);

            if (alreadyReported)
                return BadRequest("You already reported this comment.");

            // Створення скарги
            var report = new CommentReport
            {
                CommentId = commentId,
                ReporterId = userId,
                ReportedAt = DateTime.UtcNow,
                Reason = reason
            };

            try
            {
                _context.CommentReports.Add(report);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Database update error: " + ex.InnerException?.Message);
            }

            return Ok();
        }
    }
}
