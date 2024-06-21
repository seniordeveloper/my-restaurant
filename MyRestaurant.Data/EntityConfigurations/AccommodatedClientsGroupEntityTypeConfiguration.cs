using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRestaurant.Data.Entities;

namespace MyRestaurant.Data.EntityConfigurations
{
    /// <summary>
    /// An entity configuration for <see cref="AccommodatedClientsGroupEntity"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AccommodatedClientsGroupEntityTypeConfiguration : IEntityTypeConfiguration<AccommodatedClientsGroupEntity>
    {
        public void Configure(EntityTypeBuilder<AccommodatedClientsGroupEntity> builder)
        {
            builder.ToTable(nameof(RestaurantDbContext.AccommodatedClientsGroups));
            
            builder.HasKey(x => new { x.TableId, x.ClientsGroupId });
            
            builder.HasOne(x => x.Table)
                   .WithMany()
                   .HasForeignKey(x => x.TableId);
            
            builder.HasOne(x => x.ClientsGroup)
                   .WithMany()
                   .HasForeignKey(x => x.ClientsGroupId);
        }
    }
}
