namespace EasyApi.Logging.SeriLog.Loggly
{
    public interface IConfigureLogBuffer
    {
        IControlLogLevel BufferLogsAt(string bufferBaseFilename);
    }
}