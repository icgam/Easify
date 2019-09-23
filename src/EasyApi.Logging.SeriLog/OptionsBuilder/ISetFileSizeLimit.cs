namespace EasyApi.Logging.SeriLog.OptionsBuilder
{
    public interface ISetFileSizeLimit : ISetFlushToDiskInterval, IBuildSeriLogOptions
    {
        ISetLogFilesRetention SetMaxLogFileSizeInMbTo(int fileSizeInMegabytes);
    }
}