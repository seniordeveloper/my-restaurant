using System.Diagnostics.CodeAnalysis;
using MyRestaurant.Common.Enums;

namespace MyRestaurant.ApiModels
{
    [ExcludeFromCodeCoverage]
    public class AppErrorModel
    {
        /// <summary>
        /// Gets or sets code of this error.
        /// </summary>
        public ErrorCode ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets a short description of this error.
        /// </summary>
        public string Description { get; set; }
    }
}
