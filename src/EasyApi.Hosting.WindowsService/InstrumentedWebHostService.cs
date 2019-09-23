using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EasyApi.Hosting.WindowsService
{
    public sealed class InstrumentedWebHostService : WebHostService
    {
        private ILogger<InstrumentedWebHostService> Log { get; }
        
        public InstrumentedWebHostService(IWebHost host) : base(host)
        {
            Log = host.Services.GetRequiredService<ILogger<InstrumentedWebHostService>>();
        }
        
        protected override void OnStarting(string[] args)
        {
            Log.LogDebug("Services is starting ...");
            base.OnStarting(args);
        }

        protected override void OnStarted()
        {
            Log.LogDebug("Service has started successfully.");
            base.OnStarted();
        }

        protected override void OnStopping()
        {
            Log.LogDebug("Service is stopping ...");
            base.OnStopping();
        }

        protected override void OnStopped()
        {
            Log.LogDebug("Service has been stopped successfully.");
            base.OnStopped();
        }
    }
}
