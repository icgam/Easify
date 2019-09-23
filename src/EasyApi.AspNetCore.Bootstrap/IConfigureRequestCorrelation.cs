using System;
using EasyApi.AspNetCore.RequestCorrelation.Core.OptionsBuilder;
using EasyApi.Bootstrap;

namespace EasyApi.AspNetCore.Bootstrap
{
    public interface IConfigureRequestCorrelation : IExtendPipeline
    {
        IExtendPipeline ConfigureCorrelation(Func<IExcludeRequests, IBuildOptions> optionsProvider);
        IExtendPipeline ConfigureCorrelation(Func<IExcludeRequests, ICorrelateRequests> optionsProvider);
    }
}