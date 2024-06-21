using System.Reflection;
using MyRestaurant.Core.Configuration;
using MyRestaurant.Core.Configuration;
using Serilog;
using Serilog.Exceptions;

namespace MyRestaurant.WebApi
{
    public static class Program
    {
        private static string _environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static IConfiguration _config;
        private static AppConfiguration _appConfiguration;

        public static async Task Main(string[] args)
        {
            var builder = CreateHostBuilder(args);
            var host = builder.Build();

            await host.PrepareAndRunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                                      .AddJsonFile("appsettings.json",
                                           optional: false,
                                           reloadOnChange: true)
                                      .AddJsonFile($"appsettings.{_environmentName}.json",
                                           optional: true,
                                           reloadOnChange: true)
                                      .AddEnvironmentVariables();

            if (args.Length != 0)
            {
                configurationBuilder = configurationBuilder.AddCommandLine(args);
            }

            _config = configurationBuilder.Build();
            _appConfiguration = _config.GetSection(nameof(AppConfiguration)).Get<AppConfiguration>();

            var hostBuilder = Host.CreateDefaultBuilder(args)
                                  .UseEnvironment(_environmentName)
                                  .ConfigureAppConfiguration(builder => builder.AddConfiguration(_config))
                                  .ConfigureWebHostDefaults(webBuilder =>
                                   {
                                       webBuilder
                                          .UseConfiguration(_config)
                                          .UseStartup<Startup>();
                                   })
                                  .ConfigureLogging(log => { log.SetMinimumLevel(LogLevel.Debug); })
                                  .UseSerilog();

            return hostBuilder;
        }

        private static async Task PrepareAndRunAsync(this IHost host, CancellationToken token = default)
        {
            try
            {
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var configuration = services.GetRequiredService<IConfiguration>();

                    ConfigureLogging(configuration);

                    Serilog.Log.Information("Using {EnvironmentName} environment for app", _environmentName);
                    Serilog.Log.Information("Application is starting on {DomainUrl}", _appConfiguration.DomainUrl);
                }

                await host.RunAsync(token);
            }
            catch (Exception ex)
            {
                Serilog.Log.Fatal(ex, "Failed to start {AssemblyName}", GetExecutingAssemblyName());
                throw;
            }
            finally
            {
                Serilog.Log.CloseAndFlush();
            }
        }

        private static LoggerConfiguration ConfigureLogging(IConfiguration configuration)
        {
            var logConfiguration = new LoggerConfiguration()
                                  .Enrich.FromLogContext()
                                  .Enrich.WithExceptionDetails()
                                  .Enrich.WithMachineName()
                                  .Enrich.WithProperty("Environment", _environmentName)
                                  .Enrich.FromLogContext()
                                  .WriteTo.Debug()
                                  .ReadFrom.Configuration(configuration);

            Serilog.Log.Logger = logConfiguration.CreateLogger();

            Serilog.Log.Verbose("Using {EnvironmentName} environment for logging", _environmentName);
            return logConfiguration;
        }

        private static string GetExecutingAssemblyName() => Assembly.GetExecutingAssembly().GetName().Name;
    }
}
