namespace EasyApi.Logging.SeriLog
{
    public interface IControlLogLevel : IBuildSink
    {
        IBuildSink EnableLogLevelControl();
    }
}