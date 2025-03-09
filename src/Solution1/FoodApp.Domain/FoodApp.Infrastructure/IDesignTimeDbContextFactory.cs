using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FoodApp.Infrastructure
{
    public class FoodAppContextFactory : IDesignTimeDbContextFactory<FoodAppContext>
    {
        public FoodAppContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<FoodAppContext>();
            var connectionString = config.GetConnectionString("FoodAppContext");

            optionsBuilder.UseSqlServer(connectionString);

            return new FoodAppContext(optionsBuilder.Options);
        }
    }
}
