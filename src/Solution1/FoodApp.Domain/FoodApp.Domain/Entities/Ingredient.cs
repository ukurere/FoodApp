namespace FoodApp.Domain.Entities
{
    public class Ingredient : IAggregateRoot
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
    }
}
