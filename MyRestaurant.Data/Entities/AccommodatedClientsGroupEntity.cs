using System.Diagnostics.CodeAnalysis;

namespace MyRestaurant.Data.Entities
{
    /// <summary>
    /// An OR entity that is mapped to AccommodatedClientsGroups table.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AccommodatedClientsGroupEntity
    {
        /// <summary>
        /// Gets or sets primary key of table associated with this entity.
        /// </summary>
        public int TableId { get; set; }

        /// <summary>
        /// Gets or sets an instance of <see cref="TableEntity"/> associated with this entity.
        /// </summary>
        public TableEntity Table { get; set; }

        /// <summary>
        /// Gets or sets primary key of a group of clients associated with this entity.
        /// </summary>
        public Guid ClientsGroupId { get; set; }

        /// <summary>
        /// Gets or sets an instance of <see cref="ClientsGroupEntity"/> associated with this entity.
        /// </summary>
        public ClientsGroupEntity ClientsGroup { get; set; }
    }
}
