namespace MyRestaurant.ApiModels
{
    /// <summary>
    /// Represents a group of clients.
    /// </summary>
    public class ClientsGroupModel
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
        /// Gets or sets an instance of <see cref="CustomerModel"/> associated with this group of clients.
        /// </summary>
        public CustomerModel Customer { get; set; }
    }
}
