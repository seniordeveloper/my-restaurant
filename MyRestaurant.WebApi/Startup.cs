using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.HttpOverrides;
using MyRestaurant.Common;
using MyRestaurant.Core;
using MyRestaurant.Core.Configuration;
using MyRestaurant.Log;
using MyRestaurant.WebApi.Extensions;
using MyRestaurant.WebApi.Services;
using Serilog;
using Serilog.Exceptions;
using ILogger = Serilog.ILogger;

namespace MyRestaurant.WebApi
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private AppConfiguration _appConfiguration;
        private IConfiguration _configuration { get; }
        
        public Startup(IConfiguration config, IWebHostEnvironment env)
        {
            _configuration = config;
            _appConfiguration = config.GetSection(nameof(AppConfiguration)).Get<AppConfiguration>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
               .AddCustomMvc()
               .AddAutoMapperProfile()
               .AddDbContexts(_configuration);

            services
               .AddCoreServices()
               .AddCorsPolicy(_appConfiguration)
               .AddLogger()
               .AddTransientServices()
               .AddHttpContextAccessor()
               .AddSingleton<AppErrorDictionary>()
               .AddSingleton(ConfigureLogging())
               .AddSingleton(_appConfiguration);
            
            services.AddResponseCompression();
        }
        
        public void Configure(IApplicationBuilder app, AppInitializer appInitializer)
        {
            app.UseDeveloperExceptionPage();

            app.UseCors("AllowAll");
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseResponseCompression();
            app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseAuthentication();
            // app.UseAuthorization();

            appInitializer
               .InitializeAsync()
               .Wait();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        
        private ILogger ConfigureLogging()
        {
            var logConfiguration = new LoggerConfiguration()
                                  .Enrich.FromLogContext()
                                  .Enrich.WithExceptionDetails()
                                  .Enrich.WithMachineName()
                                  .Enrich.WithProperty("Environment", _appConfiguration.EnvironmentName)
                                  .Enrich.FromLogContext()
                                  .WriteTo.Debug()
                                  .ReadFrom.Configuration(_configuration);

            var logger = logConfiguration.CreateLogger();
            Serilog.Log.Verbose("Using {EnvironmentName} environment for logging", _appConfiguration.EnvironmentName);
            return logger;
        }
    }
}
