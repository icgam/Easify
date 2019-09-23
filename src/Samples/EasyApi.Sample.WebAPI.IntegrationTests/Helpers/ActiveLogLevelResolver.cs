using Serilog;
using Serilog.Events;

namespace EasyApi.Sample.WebAPI.IntegrationTests.Helpers
{
    public static class LoggerExtensions
    {
        public static string GetActiveLogLevel(this ILogger logger)
        {
            if (Log.Logger.IsEnabled(LogEventLevel.Verbose))
                return LogEventLevel.Verbose.ToString();
            if (Log.Logger.IsEnabled(LogEventLevel.Debug))
                return LogEventLevel.Debug.ToString();
            if (Log.Logger.IsEnabled(LogEventLevel.Information))
                return LogEventLevel.Information.ToString();
            if (Log.Logger.IsEnabled(LogEventLevel.Warning))
                return LogEventLevel.Warning.ToString();
            if (Log.Logger.IsEnabled(LogEventLevel.Error))
                return LogEventLevel.Error.ToString();

            return LogEventLevel.Fatal.ToString();
        }
    }
}