using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MyRestaurant.Data
{
    public class DesignTimeRestaurantDbContextFactory : IDesignTimeDbContextFactory<RestaurantDbContext>
    {
        public RestaurantDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                               .SetBasePath(Directory.GetCurrentDirectory())
                               .AddJsonFile("appsettings.json")
                               .Build();

            var builder = new DbContextOptionsBuilder<RestaurantDbContext>();
            var connectionString = configuration.GetConnectionString("MyRestaurantDbConnection");
            builder.UseNpgsql(connectionString);

            return new RestaurantDbContext(builder.Options);
        }
    }
}
