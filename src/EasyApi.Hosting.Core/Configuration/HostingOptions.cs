using System;
using EasyApi.Configurations;
using Microsoft.Extensions.Configuration;

namespace EasyApi.Hosting.Core.Configuration
{
    //TODO: This should be revised regarding to the new capabilities in v2.0
    public sealed class HostingOptions
    {
        public HostingOptions(string pathToContentRoot, bool launchedAsService, IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrWhiteSpace(pathToContentRoot))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(pathToContentRoot));

            Title = configuration[ConfigurationKeys.AppTitleKey];
            Version = configuration[ConfigurationKeys.AppVersionKey];
            BaseUrl = configuration.GetSection("ServiceHost").Get<ServiceHostConfig>().BaseUrl;
            PathToContentRoot = pathToContentRoot;
            LaunchedAsService = launchedAsService;
            Configuration = configuration;
        }

        public string Title { get; }
        public string Version { get; }
        public string BaseUrl { get; }
        public string PathToContentRoot { get; }
        public bool LaunchedAsService { get; }
        public IConfiguration Configuration { get; }
    }
}