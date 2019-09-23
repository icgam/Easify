using Serilog;

namespace EasyApi.Logging.SeriLog
{
    public interface IBuildSink
    {
        LoggerConfiguration Build();
    }
}