namespace MyRestaurant.ApiModels
{
    /// <summary>
    /// Represents a customer model.
    /// </summary>
    public class CustomerModel
    {
        /// <summary>
        /// Gets or sets a primary key of this customer.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name of this customer.
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        /// Gets or sets a last name of this customer.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets email address of this customer. 
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Gets or sets phone number of this customer.
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
