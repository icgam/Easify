using System;

namespace EasyApi.AspNetCore.RequestCorrelation.Core.OptionsBuilder
{
    public interface IExcludeRequests : ICorrelateRequests
    {
        IExcludeRequests ExcludeUrl(Func<ISetUrlFilter, ICheckUrl> filterProvider);
    }
}