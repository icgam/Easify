using System;
using Microsoft.AspNetCore.Hosting;

namespace EasyApi.Hosting.Core.HostContainer
{
    public sealed class WebHostContainer : IServiceHost
    {
        private readonly IWebHost _host;

        public WebHostContainer(IWebHost host)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
        }
        
        public void Run()
        {
           _host.Run();
        }
    }
}
