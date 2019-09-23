using Serilog.Configuration;

namespace EasyApi.Logging.SeriLog
{
    public interface ILoggerConfiguration
    {
        LoggerSinkConfiguration SinkConfiguration { get; }

        string ApplicationName { get; }
        string EnvironmentName { get; }
    }
}