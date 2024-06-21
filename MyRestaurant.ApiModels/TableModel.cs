namespace MyRestaurant.ApiModels
{
    /// <summary>
    /// Represents a table model.
    /// </summary>
    public class TableModel
    {
        /// <summary>
        /// Gets or sets a unique identifier of this [Table].
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets number of seats of this table.
        /// </summary>
        public int Size { get; set; }
    }
}
