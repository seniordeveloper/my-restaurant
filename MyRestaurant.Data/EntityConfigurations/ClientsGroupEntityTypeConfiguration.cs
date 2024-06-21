using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRestaurant.Data.Entities;

namespace MyRestaurant.Data.EntityConfigurations
{
    /// <summary>
    /// An entity configuration for <see cref="ClientsGroupEntity"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    class ClientsGroupEntityTypeConfiguration : IEntityTypeConfiguration<ClientsGroupEntity>
    {
        public void Configure(EntityTypeBuilder<ClientsGroupEntity> builder)
        {
            builder.ToTable(nameof(RestaurantDbContext.ClientsGroups));

            builder.HasKey(x => x.Id);
            
            builder.HasOne(x => x.Customer)
                   .WithMany()
                   .HasForeignKey(x => x.CustomerId);
        }
    }
}
