using Microsoft.Extensions.Configuration;

namespace EasyApi.Logging.SeriLog.Seq
{
    public static class SeqExtensions
    {
        public static ISetApiKey UseSeq(this ILoggerConfiguration configurationServices, string serverUrl)
        {
            return new FluentSeqSinkBuilder(configurationServices, serverUrl);
        }

        public static IBuildSink UseSeq(this ILoggerConfiguration configurationServices, IConfigurationSection config)
        {
            return new ConfigBasedSeqSinkBuilder(configurationServices, config);
        }
    }
}