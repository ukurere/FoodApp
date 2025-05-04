using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodAppMVC.WebMVC.Models
{
    public class EditDishViewModel
    {
        public int DishId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Display(Name = "Preparation Time (min)")]
        public int PreparationTimeMinutes { get; set; }

        [Required]
        public string Recipe { get; set; }

        public double Calories { get; set; }
        public double Proteins { get; set; }
        public double Fats { get; set; }
        public double Carbohydrates { get; set; }

        public List<int> SelectedIngredientIds { get; set; } = new List<int>();

        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }

        public string? ExistingImagePath { get; set; }
    }
}