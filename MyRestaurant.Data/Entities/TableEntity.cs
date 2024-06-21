using System.Diagnostics.CodeAnalysis;

namespace MyRestaurant.Data.Entities
{
    /// <summary>
    /// An OR entity that is mapped to [Tables] table.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TableEntity
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
