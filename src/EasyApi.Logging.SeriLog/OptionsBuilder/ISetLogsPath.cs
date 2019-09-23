namespace EasyApi.Logging.SeriLog.OptionsBuilder
{
    public interface ISetLogsPath : IBuildSeriLogOptions
    {
        IBuildSeriLogOptions SaveLogsTo(string absoluteLogsPath);
    }
}