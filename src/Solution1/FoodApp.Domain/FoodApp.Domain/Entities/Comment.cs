using FoodApp.Domain.Entities;

public class Comment : Entity
{
    public int UserID { get; set; }
    public User User { get; set; }

    public int DishID { get; set; }
    public Dish Dish { get; set; }

    public string Text { get; set; }
    public DateTime DatePosted { get; set; }
}
