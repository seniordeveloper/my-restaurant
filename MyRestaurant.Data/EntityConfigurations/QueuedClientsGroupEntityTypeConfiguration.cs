using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRestaurant.Data.Entities;

namespace MyRestaurant.Data.EntityConfigurations
{
    public class QueuedClientsGroupEntityTypeConfiguration : IEntityTypeConfiguration<QueuedClientsGroupEntity>
    {
        public void Configure(EntityTypeBuilder<QueuedClientsGroupEntity> builder)
        {
            builder.ToTable(nameof(RestaurantDbContext.QueuedClientsGroups));
            
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();
            
            builder.HasOne(x => x.ClientsGroup)
                   .WithMany()
                   .HasForeignKey(x => x.ClientsGroupId);
        }
    }
}
