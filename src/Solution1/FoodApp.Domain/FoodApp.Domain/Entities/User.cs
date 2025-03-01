using System.Xml.Linq;

namespace FoodApp.Domain.Entities;

public class User : IAggregateRoot
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime RegistrationDate { get; set; }

    public List<FavoriteDish> FavoriteDishes { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
    public List<Rating> Ratings { get; set; } = new();
}
