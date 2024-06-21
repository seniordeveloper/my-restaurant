using MyRestaurant.Common;
using MyRestaurant.Common.Enums;
using MyRestaurant.Common.Exceptions;
using MyRestaurant.Contracts.Services;

namespace MyRestaurant.Core.Services
{
    /// <summary>
    /// A default implementation of <see cref="IErrorDescriber"/>.
    /// </summary>
    public class ErrorDescriber : IErrorDescriber
    {
        private readonly AppErrorDictionary _errorDictionary;
        
        public ErrorDescriber(AppErrorDictionary errorDictionary)
        {
            _errorDictionary = errorDictionary;
        }
        
        public void DescribeWithException(ErrorCode errorCode) =>
            throw new MyRestaurantApplicationException(errorCode, _errorDictionary[errorCode]);

        public void DescribeWithException(ErrorCode errorCode, string param) =>
            throw new MyRestaurantApplicationException(errorCode, string.Format(_errorDictionary[errorCode], param), param);
    }
}
