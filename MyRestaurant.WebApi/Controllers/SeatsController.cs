using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyRestaurant.ApiModels;
using MyRestaurant.Contracts.Services;
using MyRestaurant.Core.Configuration;
using MyRestaurant.WebApi.Controllers.Base;
using ILogger = MyRestaurant.Log.ILogger;

namespace MyRestaurant.WebApi.Controllers
{
    [Route("api/seats")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class SeatsController : BaseApiController
    {
        private readonly IRestManager _restManager;
        
        public SeatsController(
            ILogger logger,
            AppConfiguration appConfiguration,
            IRestManager restManager) : base(logger, appConfiguration)
        {
            _restManager = restManager;
        }
        
        [HttpPost]
        [Route("lookup")]
        public Task<IActionResult> LookUp(ClientsGroupModel model)
        {
            return WrapResponseAsync(cancellationToken => _restManager.LookupAsync(model, cancellationToken));
        }
        
        [HttpPost]
        [Route("leave")]
        public Task<IActionResult> Leave(ClientsGroupModel model)
        {
            return WrapResponseAsync(cancellationToken => _restManager.CustomerLeavingAsync(model, cancellationToken));
        }
        
        [HttpGet]
        [Route("accommodated-clients-group")]
        public Task<IActionResult> AccommodatedClientsGroup()
        {
            return WrapResponseAsync(cancellationToken => _restManager.GetAccommodatedClientsGroupAsync(cancellationToken: cancellationToken));
        }
        
        [HttpGet]
        [Route("empty-tables")]
        public Task<IActionResult> EmptyTables()
        {
            return WrapResponseAsync(cancellationToken => _restManager.GetEmptyTablesAsync(cancellationToken: cancellationToken));
        }
        
        [HttpGet]
        [Route("available-seats")]
        public Task<IActionResult> AvailableSeats()
        {
            return WrapResponseAsync(cancellationToken => _restManager.GetAvailableSeatsAsync(cancellationToken: cancellationToken));
        }
    }
}
