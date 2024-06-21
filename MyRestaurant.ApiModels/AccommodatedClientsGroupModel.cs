namespace MyRestaurant.ApiModels
{
    /// <summary>
    /// Represents an accommodated group of clients.
    /// </summary>
    public class AccommodatedClientsGroupModel
    {
        /// <summary>
        /// Gets or sets primary key of table.
        /// </summary>
        public int TableId { get; set; }
        
        /// <summary>
        /// Gets or sets table size.
        /// </summary>
        public int TableSize { get; set; }
        
        /// <summary>
        /// Gets or sets available seats.
        /// </summary>
        public int AvailableSeats { get; set; }
    }
}
