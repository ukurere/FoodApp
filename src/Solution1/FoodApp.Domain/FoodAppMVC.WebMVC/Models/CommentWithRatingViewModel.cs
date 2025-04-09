namespace FoodAppMVC.WebMVC.Models
{
    public class CommentWithRatingViewModel
    {
        public int DishId { get; set; }
        public string Text { get; set; }
        public string AuthorName { get; set; }
        public string? AuthorAvatarUrl { get; set; }
        public int RatingValue { get; set; }
    }
}
