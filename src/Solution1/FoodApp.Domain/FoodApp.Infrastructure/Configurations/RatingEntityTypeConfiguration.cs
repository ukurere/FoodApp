using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FoodApp.Domain.Entities;

namespace FoodApp.Infrastructure.Configurations
{
    public class RatingEntityTypeConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.RatingValue).IsRequired();

            builder.HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserID);

            builder.HasOne(r => r.Dish)
                .WithMany(d => d.Ratings)
                .HasForeignKey(r => r.DishID);
        }
    }
}
