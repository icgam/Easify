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