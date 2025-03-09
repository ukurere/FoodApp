using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FoodApp.Domain.Entities;

namespace FoodApp.Infrastructure.Configurations
{
    public class FavoriteDishEntityTypeConfiguration : IEntityTypeConfiguration<FavoriteDish>
    {
        public void Configure(EntityTypeBuilder<FavoriteDish> builder)
        {
            builder.HasKey(fd => fd.Id);
            builder.Property(fd => fd.DateAdded).HasDefaultValueSql("GETDATE()");

            builder.HasOne(fd => fd.User)
                .WithMany(u => u.FavoriteDishes)
                .HasForeignKey(fd => fd.UserID);

            builder.HasOne(fd => fd.Dish)
                .WithMany()
                .HasForeignKey(fd => fd.DishID);
        }
    }
}
