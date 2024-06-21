using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRestaurant.ApiModels.Wrappers;
using MyRestaurant.Core.Configuration;
using MyRestaurant.Data;
using MyRestaurant.WebApi.AutoMapper;
using MyRestaurant.WebApi.Helpers;
using MyRestaurant.WebApi.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyRestaurant.WebApi.Extensions
{
    /// <summary>
    /// Contains extension methods to <see cref="IServiceCollection" /> for configuring services.
    /// </summary>
    public static class ServiceConfigurationExtensions
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services
               .AddControllersWithViews(action => { action.ReturnHttpNotAcceptable = true; })
               .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
               .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Type = "http://api.sotaman.io/",
                            Title = "One or more model validation errors occured.",
                            Status = StatusCodes.Status422UnprocessableEntity,
                            Detail = "See the errors property for details.",
                            Instance = context.HttpContext.Request.Path
                        };

                        problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

                        return new ContentResult
                        {
                            Content = JsonConvert.SerializeObject(new ResponseWrapper
                            {
                                Data = problemDetails
                            }, JsonHelper.DefaultSerializerSettings),
                            ContentType = "application/problem+json"
                        };
                    };
                });

            return services;
        }
        
        public static IServiceCollection AddAutoMapperProfile(this IServiceCollection services)
        {
            services.AddSingleton(
                new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); }).CreateMapper());

            return services;
        }
        
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RestaurantDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("MyRestaurantDbConnection")));
                // options.UseInMemoryDatabase($"{nameof(MyRestaurant)}InMemoryDb"));

            return services;
        }
        
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, AppConfiguration appConfiguration)
        {
            services.AddCors(options =>
                options.AddPolicy("AllowAll", p => p
                                                    .AllowAnyOrigin()
                                                    .AllowAnyMethod()
                                                    .AllowAnyHeader()));
            return services;
        }
        
        public static IServiceCollection AddTransientServices(this IServiceCollection services)
        {
            services
               .AddTransient<AppInitializer>();

            return services;
        }
    }
}
