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
using Easify.Bootstrap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Easify.AspNetCore.Bootstrap
{
    public static class StartupExtensions
    {
        public static void BootstrapApp<TStartup>(this IServiceCollection services,
            IConfiguration configuration,
            Func<IConfigureApplicationBootstrapper, IBootstrapApplication> appBootstrapperProvider
        )
            where TStartup : class
        {
            var bootstrapper = appBootstrapperProvider(new AppBootstrapper<TStartup>(services, configuration));
            bootstrapper.Bootstrap();
        }

        public static IBootstrapApplication
            AddServices(this IExtendPipeline bootstrapper,
                Action<IServiceCollection, IConfiguration> serviceConfigurationProvider)
        {
            if (bootstrapper == null) throw new ArgumentNullException(nameof(bootstrapper));
            if (serviceConfigurationProvider == null)
                throw new ArgumentNullException(nameof(serviceConfigurationProvider));

            var factory = new ServiceCollectionContainerFactory(serviceConfigurationProvider);
            return bootstrapper.UseContainer(factory);
        }

        public static IBootstrapApplication
            AddServices<TContainer>(
                this Func<Action<TContainer, IConfiguration>, IBootstrapApplication>
                    bootstrapperServicesConfigurator,
                Action<TContainer, IConfiguration> serviceConfigurationProvider)
        {
            if (bootstrapperServicesConfigurator == null)
                throw new ArgumentNullException(nameof(bootstrapperServicesConfigurator));
            if (serviceConfigurationProvider == null)
                throw new ArgumentNullException(nameof(serviceConfigurationProvider));

            return bootstrapperServicesConfigurator(serviceConfigurationProvider);
        }
    }
}