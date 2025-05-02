using FoodApp.Domain.Entities;
using System.Xml.Linq;

public class Dish : IAggregateRoot
{
    public int DishId { get; set; }
    public string Name { get; set; }
    public int PreparationTimeMinutes { get; set; }
    public string Type { get; set; }
    public string Recipe { get; set; }
    public double Proteins { get; set; }  // білки
    public double Fats { get; set; }      // жири
    public double Carbohydrates { get; set; }  // вуглеводи
    public double Calories { get; set; }

    public List<DishIngredient> DishIngredients { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
    public List<Rating> Ratings { get; set; } = new();
    public string? ImagePath { get; set; }

}
