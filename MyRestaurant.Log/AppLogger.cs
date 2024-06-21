using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace MyRestaurant.Log
{
    [ExcludeFromCodeCoverage]
    public class AppLogger : ILogger
    {
        private readonly Serilog.ILogger _logger;
        private readonly string _envName;
        private readonly IHttpContextAccessor _contextAccessor;

        public AppLogger(IHostEnvironment environment, IHttpContextAccessor contextAccessor, Serilog.ILogger logger)
        {
            _envName = environment.EnvironmentName;
            _contextAccessor = contextAccessor;
            _logger = logger;
        }

        public void CallInfo([CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "")
        {
            _logger.Information($"{GetCallerPath(filePath, methodName)} was called.");
        }

        public void Info(string message, [CallerFilePath] string filePath = "",
            [CallerMemberName] string methodName = "")
        {
            _logger.Information($"{message}|{GetCallerPath(filePath, methodName)}");
        }

        public void Warn(string message, [CallerFilePath] string filePath = "",
            [CallerMemberName] string methodName = "")
        {
            _logger.Warning($"{message}|{GetCallerPath(filePath, methodName)}");
        }

        public void Error(Exception exception, [CallerFilePath] string filePath = "",
            [CallerMemberName] string methodName = "")
        {
            _logger.Error(exception, $"{exception.Message}|{GetCallerPath(filePath, methodName)}");
        }

        public void Error(string message, Exception exception, [CallerFilePath] string filePath = "",
            [CallerMemberName] string methodName = "")
        {
            _logger.Error(exception, $"{message}|{GetCallerPath(filePath, methodName)}");
        }

        private string GetCallerPath(string filePath, string methodName)
        {
            var ipAddress = _contextAccessor.HttpContext?.Connection.RemoteIpAddress;

            return $"client-ip:{ipAddress}|env:{_envName}|{Path.GetFileName(filePath)}.{methodName}";
        }
    }
}
