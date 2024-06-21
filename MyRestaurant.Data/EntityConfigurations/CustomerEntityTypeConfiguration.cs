using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRestaurant.Data.Entities;

namespace MyRestaurant.Data.EntityConfigurations
{
    /// <summary>
    /// An entity configuration for <see cref="CustomerEntity"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    class CustomerEntityTypeConfiguration: IEntityTypeConfiguration<CustomerEntity>
    {
        public void Configure(EntityTypeBuilder<CustomerEntity> builder)
        {
            builder.ToTable(nameof(RestaurantDbContext.Customers));

            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();
        }
    }
}
