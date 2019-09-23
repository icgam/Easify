namespace EasyApi.Logging.SeriLog.LogEntries
{
    public interface IProvideTemplate : IBuildSink
    {
        IBuildSink WithTemplate(string template);
    }
}