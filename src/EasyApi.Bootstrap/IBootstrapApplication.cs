using System;

namespace EasyApi.Bootstrap
{
    public interface IBootstrapApplication 
    {
        IServiceProvider Bootstrap();
    }
}