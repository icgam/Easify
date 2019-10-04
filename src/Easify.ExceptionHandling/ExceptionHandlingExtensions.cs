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
using EasyApi.ExceptionHandling.ConfigurationBuilder;
using EasyApi.ExceptionHandling.ConfigurationBuilder.Fluent;
using EasyApi.ExceptionHandling.Domain;
using EasyApi.ExceptionHandling.ErrorBuilder;
using EasyApi.ExceptionHandling.ErrorBuilder.Fluent;
using EasyApi.ExceptionHandling.Formatter;
using EasyApi.ExceptionHandling.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RestEase;

namespace EasyApi.ExceptionHandling
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

            services.AddSingleton<IErrorResponseProviderOptions>(options);
            services.AddSingleton<IErrorMessageFormatterOptions>(options);
            services.AddSingleton<IHttpStatusCodeProvider, DefaultHttpStatusCodeProvider>();
            services.AddTransient<IErrorProvider, ErrorProvider>();
            services.AddTransient<IErrorMessageFormatter, HierarchicalErrorMessageFormatter>();
            services.AddTransient<IErrorResponseProvider, ErrorResponseProvider>();
        }

        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            app.UseMiddleware<ErrorHandlingMiddleware>();
            return app;
        }

        public static IProvideErrorBuilder<ApiException> UseBuilderForApi(
            this ISetErrorBuilder<ApiException> builderProvider)
        {
            return builderProvider.Use(new ErrorBuilderForApiException());
        }
    }
}