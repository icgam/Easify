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
using Easify.AspNetCore.Bootstrap;
using Easify.AspNetCore.Security.Fluent;
using Easify.RestEase;
using Easify.Sample.WebAPI.Core;
using Easify.Sample.WebAPI.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Easify.Sample.WebAPI.IntegrationTests.Helpers
{
    public abstract class StartupForHealth
    {
        protected StartupForHealth(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        protected virtual Action<ISetAuthenticationMode> AuthConfigure => o => o.WithNoAuth();

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
                return services.BootstrapApp<StartupForIntegration>(Configuration,
                    app => app.AddConfigSection<Clients>()
                        .AndSection<Section1>()
                        .AndSection<Section2>()
                        .HandleApplicationException<TemplateApiApplicationException>()
                        .AndHandle<ThirdPartyPluginException>()
                        .UseUserErrors()
                        .ConfigureAuthentication(AuthConfigure)
                        .AddServices((container, config) =>
                        {
                            container.AddRestClient<IValuesClient, Clients>(c => c.ProducerClientUrl, o => o.ExcludeAuthorizationHeader());
                            container.TryAddTransient<IMyService, MyService>();
                        })
                );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDefaultApiPipeline(Configuration, env, loggerFactory);
        }
    }
}