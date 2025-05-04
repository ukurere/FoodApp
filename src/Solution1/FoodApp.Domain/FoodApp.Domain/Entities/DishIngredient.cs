using FoodApp.Domain.Entities;

public class DishIngredient : Entity
{
    public int DishID { get; set; }
    public Dish Dish { get; set; }

    public int IngredientID { get; set; }
    public Ingredient Ingredient { get; set; }
    public int Quantity { get; set; }
    public string? CntIngredient { get; set; }
    public bool IsMandatory { get; set; }
}
