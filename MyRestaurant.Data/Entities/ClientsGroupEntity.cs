using System.Diagnostics.CodeAnalysis;

namespace MyRestaurant.Data.Entities
{
    /// <summary>
    /// An OR entity that is mapped to ClientsGroups table.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ClientsGroupEntity
    {
        /// <summary>
        /// Gets or sets a primary key of this group of clients.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a number of people of this client group.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets a primary key of customer associated with this group of clients.
        /// </summary>
        public int? CustomerId { get; set; }

        /// <summary>
        /// Gets or sets an instance of <see cref="CustomerEntity"/> associated with this group of clients.
        /// </summary>
        public CustomerEntity Customer { get; set; }
    }
}
