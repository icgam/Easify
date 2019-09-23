namespace EasyApi.Logging.SeriLog.OptionsBuilder
{
    public interface ISetLogFilesRetention : ISetLogsPath, IBuildSeriLogOptions
    {
        ISetLogsPath RetainNumberOfLogFiles(int numberOfLogFilesToRetain);
    }
}