namespace FoodAppMVC.WebMVC.Models
{
    public class DishViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int CookingTime { get; set; }
        public string Type { get; set; } = "";
        public string ImageUrl { get; set; } = "";
    }
}