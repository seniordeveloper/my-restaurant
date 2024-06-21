using MyRestaurant.ApiModels;

namespace MyRestaurant.Contracts.Services
{
    /// <summary>
    /// Provides an abstraction to manager seats.
    /// </summary>
    public interface IRestManager
    {
        /// <summary>
        /// Returns a table where a given client group is seated or null if it is still queueing or has already left as an asynchronous operation.
        /// </summary>
        /// <param name="clientsGroup"><see cref="ClientsGroupModel"/></param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task" /> that contains the result the asynchronous operation.</returns>
        Task<TableModel> LookupAsync(ClientsGroupModel clientsGroup,  CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously accommodates queued clients group.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task" /> that contains the result the asynchronous operation.</returns>
        Task AccommodatedQueuedClientsGroupAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously fetches accommodated clients group.
        /// </summary>
        /// <param name="requiredSize">Required size.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task" /> that contains the result the asynchronous operation.</returns>
        Task<List<AccommodatedClientsGroupModel>> GetAccommodatedClientsGroupAsync(int? requiredSize = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously fetches empty available seats.
        /// </summary>
        /// <param name="requiredSiez">Required size.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task" /> that contains the result the asynchronous operation.</returns>
        Task<List<TableModel>> GetEmptyTablesAsync(int? requiredSiez = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Processes customer leaving event.
        /// </summary>
        /// <param name="model">Payload.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task" /> that contains the result the asynchronous operation.</returns>
        Task CustomerLeavingAsync(ClientsGroupModel model, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Asynchronously fetches all available seats.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task" /> that contains the result the asynchronous operation.</returns>
        Task<IEnumerable<TableModel>> GetAvailableSeatsAsync(CancellationToken cancellationToken = default);
    }
}
