using System.Diagnostics.CodeAnalysis;
using MyRestaurant.Common.Enums;

namespace MyRestaurant.ApiModels.Wrappers
{
    [ExcludeFromCodeCoverage]
    public class ResponseWrapper
    {
        public object Data { get; set; }

        public bool Success { get; set; }

        public ErrorCode? ErrorCode { get; set; }

        public string Message { get; set; }

        public string ErrorStackTrace { get; set; }

        public string Parameter { get; set; }
    }
}
