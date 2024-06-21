namespace MyRestaurant.Data.Entities
{
    /// <summary>
    /// An OR entity that is mapped to [QueuedClientsGroups] table.
    /// </summary>
    public class QueuedClientsGroupEntity
    {
        /// <summary>
        /// Gets or sets a primary key of this entity. 
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or sets a primary key of group of clients associated with this entity.
        /// </summary>
        public Guid ClientsGroupId { get; set; }

        /// <summary>
        /// Gets or sets an instance of <see cref="ClientsGroupEntity"/> associated with this entity.
        /// </summary>
        public ClientsGroupEntity ClientsGroup { get; set; }

        /// <summary>
        /// Gets or sets date and time when this group of clients were queued.
        /// </summary>
        public DateTime QueuedOn { get; set; }
    }
}
