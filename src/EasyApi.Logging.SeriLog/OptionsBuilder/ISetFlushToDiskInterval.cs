namespace EasyApi.Logging.SeriLog.OptionsBuilder
{
    public interface ISetFlushToDiskInterval : ISetLogsPath, IBuildSeriLogOptions
    {
        ISetLogFilesRetention FlushToDiskEveryInMs(int intervalInMs);
    }
}