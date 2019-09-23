using System;
using System.ServiceProcess;
using EasyApi.Hosting.Core.HostContainer;
using Microsoft.AspNetCore.Hosting;

namespace EasyApi.Hosting.WindowsService
{
    public sealed class WindowsServiceHostContainer : IServiceHost
    {
        private readonly IWebHost _host;

        public WindowsServiceHostContainer(IWebHost host)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
        }

        public void Run()
        {
            var webHostService = new InstrumentedWebHostService(_host);
            ServiceBase.Run(webHostService);
        }
    }
}
