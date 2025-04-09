using System.ComponentModel.DataAnnotations;

namespace FoodAppMVC.WebMVC.Models
{
    public class AddCommentViewModel
    {
        [Required]
        public int DishId { get; set; }

        [Required(ErrorMessage = "Поле коментар обов'язкове.")]
        [StringLength(500)]
        public string Text { get; set; }

        [Required]
        [Range(1, 5)]
        public int RatingValue { get; set; }
    }
}
