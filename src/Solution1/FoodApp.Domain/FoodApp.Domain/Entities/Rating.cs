using FoodApp.Domain.Entities;

public class Rating : Entity
{
    public int UserID { get; set; }
    public User User { get; set; }

    public int DishID { get; set; }
    public Dish Dish { get; set; }

    public int RatingValue { get; set; }
}
