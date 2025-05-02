namespace FoodAppMVC.WebMVC.Models
{
    public class AddDishViewModel
    {
        public int DishId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int PreparationTimeMinutes { get; set; }
        public string Recipe { get; set; }

        public int Calories { get; set; }
        public int Proteins { get; set; }
        public int Fats { get; set; }
        public int Carbohydrates { get; set; }

        public IFormFile? ImageFile { get; set; }

        public List<int>? SelectedIngredientIds { get; set; } = new();
    }
}