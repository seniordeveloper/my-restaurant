using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace MyRestaurant.Log
{
    [ExcludeFromCodeCoverage]
    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection AddLogger(this IServiceCollection services)
        {
            services.AddSingleton<ILogger, AppLogger>();

            return services;
        }
    }
}
