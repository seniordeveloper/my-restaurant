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
    [Route("api/customers")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class CustomerController : BaseApiController
    {
        private readonly ICustomerManager _customerManager;
        
        public CustomerController(
            ILogger logger,
            AppConfiguration appConfiguration,
            ICustomerManager customerManager) : base(logger, appConfiguration)
        {
            _customerManager = customerManager;
        }

        [HttpPost]
        [Route("arrive")]
        public Task<IActionResult> Arrive(ClientsGroupModel model)
        {
            return WrapResponseAsync(cancellationToken => _customerManager.CustomerArrivedAsync(model, cancellationToken));
        }
    }
}
