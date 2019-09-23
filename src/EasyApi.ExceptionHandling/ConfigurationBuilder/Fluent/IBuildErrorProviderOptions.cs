using EasyApi.ExceptionHandling.Domain;

namespace EasyApi.ExceptionHandling.ConfigurationBuilder.Fluent
{
    public interface IBuildErrorProviderOptions
    {
        IErrorResponseProviderOptions Build();
    }
}