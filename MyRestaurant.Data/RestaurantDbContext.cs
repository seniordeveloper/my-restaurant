using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MyRestaurant.Data.Entities;
using MyRestaurant.Data.EntityConfigurations;

namespace MyRestaurant.Data
{
    /// <summary>
    /// A class for the Entity Framework database context used for the app.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RestaurantDbContext : DbContext
    {
        private readonly DbContextOptions<RestaurantDbContext> _dbContextOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestaurantDbContext" />.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext" />.</param>
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
            _dbContextOptions = options;
        }
        
        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}" /> of [Tables].
        /// </summary>
        public DbSet<TableEntity> Tables { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}" /> of [ClientsGroups].
        /// </summary>
        public DbSet<ClientsGroupEntity> ClientsGroups { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}" /> of [Customers].
        /// </summary>
        public DbSet<CustomerEntity> Customers { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}" /> of [AccommodatedClientsGroups].
        /// </summary>
        public DbSet<AccommodatedClientsGroupEntity> AccommodatedClientsGroups { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}" /> of [QueuedClientsGroups].
        /// </summary>
        public DbSet<QueuedClientsGroupEntity> QueuedClientsGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
               .ApplyConfiguration(new TableEntityTypeConfiguration())
               .ApplyConfiguration(new ClientsGroupEntityTypeConfiguration())
               .ApplyConfiguration(new CustomerEntityTypeConfiguration())
               .ApplyConfiguration(new AccommodatedClientsGroupEntityTypeConfiguration())
               .ApplyConfiguration(new QueuedClientsGroupEntityTypeConfiguration());
        }
    }
}
