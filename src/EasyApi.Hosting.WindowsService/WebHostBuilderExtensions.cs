using EasyApi.Hosting.Core.HostContainer;
using Microsoft.AspNetCore.Hosting;

namespace EasyApi.Hosting.WindowsService
{
    public static class WebHostBuilderExtensions
    {
        public static IServiceHost UseWindowsService(this IWebHost webHost)
        {
            return new WindowsServiceHostContainer(webHost);
        }
    }
}