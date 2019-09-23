using System;
using EasyApi.AspNetCore.Bootstrap;
using EasyApi.RestEase;
using EasyApi.Sample.WebAPI.Core;
using EasyApi.Sample.WebAPI.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace EasyApi.Sample.WebAPI.IntegrationTests.Helpers
{
    public class StartupForIntegration
    {
        public StartupForIntegration(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }


        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.BootstrapApp<StartupForIntegration>(Configuration,
                app => app.AddConfigSection<Clients>()
                    .AndSection<Section1>()
                    .AndSection<Section2>()
                    .HandleApplicationException<TemplateApiApplicationException>()
                    .AndHandle<ThirdPartyPluginException>()
                    .UseUserErrors()
                    .AddServices((container, config) =>
                    {
                        RestClientExtensions.AddRestClient<IValuesClient, Clients>(container, c => c.ProducerClientUrl);
                        ServiceCollectionDescriptorExtensions.TryAddTransient<IMyService, MyService>(container);
                    })
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDefaultApiPipeline(Configuration, env, loggerFactory);
        }
    }
}