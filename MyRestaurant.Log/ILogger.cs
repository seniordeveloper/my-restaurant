using System.Runtime.CompilerServices;

namespace MyRestaurant.Log
{
    public interface ILogger
    {
        void CallInfo([CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "");
        void Info(string message, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "");
        void Warn(string message, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "");

        void Error(Exception exception, [CallerFilePath] string filePath = "",
            [CallerMemberName] string methodName = "");

        void Error(string message, Exception exception, [CallerFilePath] string filePath = "",
            [CallerMemberName] string methodName = "");
    }
}
