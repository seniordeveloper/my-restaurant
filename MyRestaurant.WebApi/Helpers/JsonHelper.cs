using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyRestaurant.WebApi.Helpers
{
    /// <summary>
    /// Contains useful methods to <see cref="Newtonsoft.Json.Serialization" />.
    /// </summary>
    public class JsonHelper
    {
        public static JsonSerializerSettings DefaultSerializerSettings { get; } = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };
    }
}
