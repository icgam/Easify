using System;
using Easify.Bootstrap;
using Microsoft.Extensions.DependencyInjection;

namespace Easify.AspNetCore.Bootstrap
{
    public interface IConfigureHealthChecks : IExtendPipeline
    {
        IExtendPipeline ConfigureHealthChecks(Action<IHealthChecksBuilder> configure);
    }
}