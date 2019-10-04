// This software is part of the Easify framework
// Copyright (C) 2019 Intermediate Capital Group
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

 using System;
using AutoMapper;
 using Easify.RestEase;
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