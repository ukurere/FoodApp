using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodApp.Domain.Entities
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public int DishID { get; set; }
        public Dish Dish { get; set; }

        public string Text { get; set; }

        public DateTime DatePosted { get; set; }
    }
}
