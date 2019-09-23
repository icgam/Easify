using System;
using AutoMapper;
using EasyApi.AspNetCore.Bootstrap;
using EasyApi.AspNetCore.Bootstrap.Extensions;
using EasyApi.Sample.WebAPI.Core.Mappings;
using EasyApi.Sample.WebAPI.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EasyApi.Sample.WebAPI.IntegrationTests.Helpers
{
    public class StartupForAutomapper
    {
        public StartupForAutomapper(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.BootstrapApp<StartupForAutomapper>(Configuration,
                app => PipelineExtensions.ConfigureMappings(app
                        .HandleApplicationException<TemplateApiApplicationException>()
                        .UseDetailedErrors(), c =>
                    {
                        c.CreateMap<PersonEntity, PersonDO>();
                        c.CreateMap<AssetEntity, AssetDO>().ConvertUsing<AssetConverter>();
                    })
                    .AddServices((container, config) =>
                    {
                        ServiceCollectionServiceExtensions.AddTransient<IRateProvider, DummyRateProvider>(container);
                        ServiceCollectionServiceExtensions.AddTransient<AssetConverter, AssetConverter>(container);
                        ServiceCollectionServiceExtensions.AddTransient<ITypeConverter<AssetEntity, AssetDO>, AssetConverter>(container);
                    })
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDefaultApiPipeline(Configuration, env, loggerFactory);
        }
    }
}