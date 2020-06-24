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
using Easify.ExceptionHandling.ConfigurationBuilder;
using Easify.ExceptionHandling.ConfigurationBuilder.Fluent;
using Easify.ExceptionHandling.Formatter;
using Easify.ExceptionHandling.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Easify.ExceptionHandling
{
    public static class ExceptionHandlingExtensions
    {
        public static void AddGlobalExceptionHandler(this IServiceCollection services,
            Func<IHandleApplicationException, IBuildErrorProviderOptions> configurationBuilder)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configurationBuilder == null) throw new ArgumentNullException(nameof(configurationBuilder));

            var builder = new GlobalErrorHandlerConfigurationBuilder(services);
            var options = configurationBuilder(builder).Build();

            services.AddSingleton(options);
            services.AddSingleton<IErrorMessageFormatterOptions>(options);
            services.AddSingleton<IHttpStatusCodeProvider, DefaultHttpStatusCodeProvider>();
            services.AddTransient<IErrorProvider, ErrorProvider>();
            services.AddTransient<IErrorMessageFormatter, HierarchicalErrorMessageFormatter>();
            services.AddTransient<IErrorResponseProvider, ErrorResponseProvider>();
        }
    }
}