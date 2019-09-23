using System;
using EasyApi.AspNetCore.RequestCorrelation.Domain;
using EasyApi.ExceptionHandling;
using EasyApi.ExceptionHandling.ConfigurationBuilder;
using EasyApi.RestEase;
using FluentValidation;
using RestEase;

namespace EasyApi.AspNetCore.ExceptionHandling
{
    public static class GlobalErrorHandlerConfigurationBuilderExtensions
    {
        public const string GenericErrorMessage = "Unknown Error Happened";
        public const string GenericErrorType = "UnknownException";

        public static GlobalErrorHandlerConfigurationBuilder UseDefault(
            this GlobalErrorHandlerConfigurationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder
                .AndHandle<ValidationException>()
                .AndHandle<NotCorrelatedRequestException>()
                .AndHandle<ApiException>(b => b.UseBuilderForApi(), f => f.ClientError())
                .UseGenericError(GenericErrorMessage, GenericErrorType);

            return builder;
        }
    }
}