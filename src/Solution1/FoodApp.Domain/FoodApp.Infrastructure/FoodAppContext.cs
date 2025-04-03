using Microsoft.EntityFrameworkCore;
using FoodApp.Domain.Entities;
using FoodApp.Infrastructure.Configurations;

namespace FoodApp.Infrastructure
{
    public class FoodAppContext : DbContext
    {
        public FoodAppContext(DbContextOptions<FoodAppContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<FavoriteDish> FavoriteDishes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<DishIngredient> DishIngredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DishEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new IngredientEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FavoriteDishEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CommentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RatingEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DishIngredientEntityTypeConfiguration());
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Role)
                    .HasColumnName("Role")
                    .HasMaxLength(20);
            });

        }
    }
}
