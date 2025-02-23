using FoodApp.Domain.Entities;

public class FavoriteDish : Entity
{
    public int UserID { get; set; }
    public User User { get; set; }

    public int DishID { get; set; }
    public Dish Dish { get; set; }

    public DateTime DateAdded { get; set; }
}
