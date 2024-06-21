using MyRestaurant.ApiModels;

namespace MyRestaurant.Contracts.Services
{
    /// <summary>
    /// Provides an abstraction to manage customers.
    /// </summary>
    public interface ICustomerManager
    {
        /// <summary>
        /// Creates the specified <paramref name="customer" /> in the backing store with email or phone,
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="customer">The customer to create.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="CustomerModel" />
        /// of the operation.
        /// </returns>
        Task<CustomerModel> CreateAsync(CustomerModel customer, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Asynchronously process the group of clients on arrive.
        /// </summary>
        /// <param name="clientsGroup"><see cref="ClientsGroupModel"/></param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task" /> that contains the result the asynchronous operation.</returns>
        Task<ClientsGroupModel> CustomerArrivedAsync(ClientsGroupModel clientsGroup,  CancellationToken cancellationToken = default);
    }
}
