using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FoodApp.Domain.Entities;
using FoodApp.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodAppMVC.WebMVC.Models;

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

        [HttpGet]
        public IActionResult Create(int dishId)
        {
            var model = new AddCommentViewModel
            {
                DishId = dishId
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddCommentViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name);
            if (user == null) return Unauthorized();

            var comment = new Comment
            {
                DishID = model.DishId,
                UserID = user.UserId,
                Text = model.Text,
                DatePosted = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var rating = new Rating
            {
                DishID = model.DishId,
                UserID = user.UserId,
                RatingValue = model.RatingValue,
                DateRated = DateTime.UtcNow
            };

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Dishes", new { id = model.DishId });
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

            var commentExists = await _context.Comments.AnyAsync(c => c.CommentId == commentId);
            if (!commentExists)
                return NotFound("Comment does not exist.");

            var alreadyReported = await _context.CommentReports
                .AnyAsync(r => r.CommentId == commentId && r.ReporterId == userId);

            if (alreadyReported)
                return BadRequest("You already reported this comment.");

            var report = new CommentReport
            {
                CommentId = commentId,
                ReporterId = userId,
                ReportedAt = DateTime.UtcNow,
                Reason = reason
            };

            _context.CommentReports.Add(report);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
