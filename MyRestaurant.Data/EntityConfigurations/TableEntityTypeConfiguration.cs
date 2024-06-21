using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRestaurant.Data.Entities;

namespace MyRestaurant.Data.EntityConfigurations
{
    class TableEntityTypeConfiguration: IEntityTypeConfiguration<TableEntity>
    {
        public void Configure(EntityTypeBuilder<TableEntity> builder)
        {
            builder.ToTable(nameof(RestaurantDbContext.Tables));

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
