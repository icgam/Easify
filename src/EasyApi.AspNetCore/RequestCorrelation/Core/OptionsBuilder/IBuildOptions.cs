using EasyApi.AspNetCore.RequestCorrelation.Domain;

namespace EasyApi.AspNetCore.RequestCorrelation.Core.OptionsBuilder
{
    public interface IBuildOptions
    {
        RequestCorrelationOptions Build();
    }
}