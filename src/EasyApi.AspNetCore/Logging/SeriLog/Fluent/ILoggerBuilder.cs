using System;
using EasyApi.Logging.SeriLog;
using EasyApi.Logging.SeriLog.OptionsBuilder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace EasyApi.AspNetCore.Logging.SeriLog.Fluent
{
    // TODO: The whole fluent part need to be moved to another library
    public interface ILoggerBuilder
    {
        IConfiguration Configuration { get; }
        IHostingEnvironment Environment { get; } // TODO: Need to be move out of the interface and be replaced by a provider

        IBuildLogger ConfigureLogger<TStartup>()
            where TStartup : class;

        IBuildLogger ConfigureLogger<TStartup>(Func<ILoggerConfiguration, IBuildSink> sinksProvider)
            where TStartup : class;

        IBuildLogger ConfigureLogger<TStartup>(Func<ISetFileSizeLimit, IBuildSeriLogOptions> optionsProvider,
            Func<ILoggerConfiguration, IBuildSink> sinksProvider)
            where TStartup : class;

        IBuildLogger ConfigureLogger<TStartup>(Func<ISetFileSizeLimit, IBuildSeriLogOptions> optionsProvider)
            where TStartup : class;
    }
}