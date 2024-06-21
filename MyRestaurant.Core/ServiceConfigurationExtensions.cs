using Microsoft.Extensions.DependencyInjection;
using MyRestaurant.Contracts.Services;
using MyRestaurant.Core.Services;

namespace MyRestaurant.Core
{
    /// <summary>
    /// Contains extension methods to <see cref="IServiceCollection" /> for configuring services.
    /// </summary>
    public static class ServiceConfigurationExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services
               .AddTransientServices()
               .AddSingleton<IErrorDescriber, ErrorDescriber>();
            
            return services;
        }

        private static IServiceCollection AddTransientServices(this IServiceCollection services)
        {
            services
               .AddTransient<IRestManager, RestManager>()
               .AddTransient<ICustomerManager, CustomerManager>();
            
            return services;
        }
    }
}
