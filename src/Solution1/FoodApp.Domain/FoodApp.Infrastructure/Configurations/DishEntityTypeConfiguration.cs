using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FoodApp.Domain.Entities;

namespace FoodApp.Infrastructure.Configurations
{
    public class DishEntityTypeConfiguration : IEntityTypeConfiguration<Dish>
    {
        public void Configure(EntityTypeBuilder<Dish> builder)
        {
            builder.HasKey(d => d.DishId);
            builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
            builder.Property(d => d.PreparationTimeMinutes).IsRequired();
            builder.Property(d => d.Type).HasMaxLength(50);
            builder.Property(d => d.Recipe).IsRequired();
        }
    }
}
