// This software is part of the Easify framework
// Copyright (C) 2019 Intermediate Capital Group
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
