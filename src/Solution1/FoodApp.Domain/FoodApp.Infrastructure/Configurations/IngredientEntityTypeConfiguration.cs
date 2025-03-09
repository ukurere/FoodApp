using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FoodApp.Domain.Entities;

namespace FoodApp.Infrastructure.Configurations
{
    public class IngredientEntityTypeConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.HasKey(i => i.IngredientId);
            builder.Property(i => i.Name).IsRequired().HasMaxLength(100);
        }
    }
}
