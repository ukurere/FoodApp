namespace FoodAppMVC.WebMVC.Models
{
    public class DishesFilterViewModel
    {
        public List<string> AllIngredients { get; set; } = new();
        public List<string> AllTimes { get; set; } = new(); // приклад: 10, 20, 30
        public List<string> AllTypes { get; set; } = new(); // приклад: Сніданок, Обід, Вечеря

        public List<string>? SelectedIngredients { get; set; }
        public string? SelectedTime { get; set; }
        public string? SelectedType { get; set; }

        public List<DishViewModel> FilteredDishes { get; set; } = new();
    }

}
