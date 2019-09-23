using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyApi.Bootstrap
{
    public interface IExtendPipeline : IConfigureContainer
    {
        IExtendPipeline Extend(
            Action<IServiceCollection, IConfiguration> pipelineExtender);
    }
}