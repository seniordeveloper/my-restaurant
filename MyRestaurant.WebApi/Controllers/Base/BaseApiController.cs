using Microsoft.AspNetCore.Mvc;
using MyRestaurant.ApiModels.Wrappers;
using MyRestaurant.Common.Enums;
using MyRestaurant.Common.Exceptions;
using MyRestaurant.Core.Configuration;
using MyRestaurant.WebApi.Helpers;
using Newtonsoft.Json;
using ILogger = MyRestaurant.Log.ILogger;

namespace MyRestaurant.WebApi.Controllers.Base
{
    public class BaseApiController: ControllerBase
    {
        protected const string JsonContentType = "application/json";
        protected readonly ILogger _logger;
        protected readonly AppConfiguration _appConfiguration;
        
        public BaseApiController(ILogger logger, AppConfiguration appConfiguration)
        {
            _logger = logger;
            _appConfiguration = appConfiguration;
        }
        
        protected async Task<IActionResult> WrapResponseAsync<T>(Func<CancellationToken, Task<T>> responseBuilder,
            bool cache = false)
        {
            var token = HttpContext.RequestAborted;
            string response;

            try
            {
                var respValue = new ResponseWrapper
                {
                    Data = await responseBuilder(token),
                    Success = true
                };

                response = ToJson(respValue);
            }
            catch (MyRestaurantApplicationException ex)
            {
                response = ToJson(GetResponseFromException(ex));
            }
            catch (Exception ex)
            {
                response = ToJson(GetResponseFromException(ex));
            }

            return Content(response, JsonContentType);
        }
        
        protected async Task<IActionResult> WrapResponseAsync(Func<CancellationToken, Task> responseBuilder)
        {
            var token = HttpContext.RequestAborted;
            ResponseWrapper response;

            try
            {
                await responseBuilder(token);
                response = new ResponseWrapper
                {
                    Success = true
                };
            }
            catch (MyRestaurantApplicationException ex)
            {
                response = GetResponseFromException(ex);
            }
            catch (Exception ex)
            {
                response = GetResponseFromException(ex);
            }

            var json = ToJson(response);

            return Content(json, JsonContentType);
        }
        
        protected static string ToJson(object data) => JsonConvert.SerializeObject(data, JsonHelper.DefaultSerializerSettings);
        
        private ResponseWrapper GetResponseFromException(MyRestaurantApplicationException exception)
        {
            var response = new ResponseWrapper
            {
                Message = exception.Message,
                Success = false,
                ErrorStackTrace = _appConfiguration.EnableStackTrace ? exception.StackTrace : null,
                ErrorCode = exception.ErrorCode,
                Parameter = exception.Parameter
            };

            _logger.Error(exception);

            return response;
        }
        
        private ResponseWrapper GetResponseFromException(Exception exception)
        {
            var response = new ResponseWrapper
            {
                Message = exception.Message,
                Success = false,
                ErrorStackTrace = _appConfiguration.EnableStackTrace ? exception.StackTrace : null,
                ErrorCode = ErrorCode.UnhandledError
            };

            _logger.Error(exception);

            return response;
        }
    }
}
