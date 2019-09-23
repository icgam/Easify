using System;
using AutoMapper;
using EasyApi.AspNetCore.Bootstrap;
using EasyApi.AspNetCore.Bootstrap.Extensions;
using EasyApi.AspNetCore.Logging.SeriLog;
using EasyApi.RestEase;
using EasyApi.Sample.WebAPI.Core;
using EasyApi.Sample.WebAPI.Core.Mappings;
using EasyApi.Sample.WebAPI.Domain;
using Foil;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EasyApi.Sample.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return ConfigureUsingServiceCollection(services);
        }

        private IServiceProvider ConfigureUsingServiceCollection(IServiceCollection services)
        {
            return services.BootstrapApp<Startup>(Configuration,
                app => PipelineExtensions.ConfigureMappings(app
                        .AddConfigSection<Clients>()
                        .AndSection<Section1>()
                        .AndSection<Section2>()
                        .HandleApplicationException<TemplateApiApplicationException>(), c =>
                    {
                        c.CreateMap<PersonEntity, PersonDO>();
                        c.CreateMap<AssetEntity, AssetDO>().ConvertUsing<AssetConverter>();
                    })
                    .AddServices((container, config) =>
                    {
                        RestClientExtensions.AddRestClient<IValuesClient, Clients>(container, c => c.ProducerClientUrl);
                        ServiceCollectionExtensions.AddTransientWithInterception<IMyService, MyService>(container, by =>
                            by.InterceptBy<LogInterceptor>());
                        ServiceCollectionExtensions.AddTransientWithInterception<IRateProvider, DummyRateProvider>(container, by =>
                            by.InterceptBy<LogInterceptor>());
                        ServiceCollectionServiceExtensions.AddSingleton<ITypeConverter<AssetEntity, AssetDO>, AssetConverter>(container);
                    })
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDefaultApiPipeline(Configuration, env, loggerFactory);
        }
    }
}