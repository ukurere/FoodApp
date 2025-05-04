using System.ComponentModel.DataAnnotations;

namespace FoodAppMVC.WebMVC.Models
{
    public class AddCommentViewModel
    {
        public int DishId { get; set; }

        [Required(ErrorMessage = "Comment text is required.")]
        public string Text { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int RatingValue { get; set; }
    }
}
