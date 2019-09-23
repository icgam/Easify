namespace EasyApi.AspNetCore.Logging.SeriLog.Fluent
{
    public interface IBuildLogger
    {
        void Build<TStartup>()
            where TStartup : class;
    }
}