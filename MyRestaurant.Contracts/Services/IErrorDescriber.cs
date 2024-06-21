using MyRestaurant.Common.Enums;

namespace MyRestaurant.Contracts.Services
{
    /// <summary>
    /// Provides an abstraction for describing errors.
    /// </summary>
    public interface IErrorDescriber
    {
        /// <summary>
        ///  Throws an excepction <see cref="MyRestaurant.Common.Exceptions.MyRestaurantApplicationException"/> by its code.
        /// </summary>
        /// <param name="errorCode">Error code.</param>
        void DescribeWithException(ErrorCode errorCode);

        /// <summary>
        ///  Throws a formatted excepction <see cref="MyRestaurant.Common.Exceptions.MyRestaurantApplicationException"/> by its code.
        /// </summary>
        /// <param name="errorCode">Error code.</param>
        /// <param name="param">String format param.</param>
        void DescribeWithException(ErrorCode errorCode, string param);
    }
}
