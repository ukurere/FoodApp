using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FoodApp.Domain.Entities;

namespace FoodApp.Infrastructure.Configurations
{
    public class DishIngredientEntityTypeConfiguration : IEntityTypeConfiguration<DishIngredient>
    {
        public void Configure(EntityTypeBuilder<DishIngredient> builder)
        {
            builder.HasKey(di => new { di.DishID, di.IngredientID });

            builder.HasOne(di => di.Dish)
                .WithMany(d => d.DishIngredients)
                .HasForeignKey(di => di.DishID);

            builder.HasOne(di => di.Ingredient)
                .WithMany()
                .HasForeignKey(di => di.IngredientID);

            builder.Property(di => di.CntIngredient).IsRequired();
            builder.Property(di => di.IsMandatory).HasDefaultValue(true);
        }
    }
}
