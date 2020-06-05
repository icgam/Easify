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

using System;
using Easify.Configurations;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Options;

namespace Easify.AspNetCore.Telemetry.AppInsights
{
    public sealed class ApplicationInfoTelemetryInitializer : ITelemetryInitializer
    {
        private readonly Application _application;

        public ApplicationInfoTelemetryInitializer(IOptions<Application> applicationAccessor)
        {
            _application = applicationAccessor?.Value ?? throw new ArgumentNullException(nameof(applicationAccessor));
        }
        
        public void Initialize(ITelemetry telemetry)
        {
            if (!string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName)) 
                return;
            
            var roleName = $"{_application.Name}-{_application.Environment.Name}";
                
            telemetry.Context.Cloud.RoleName = roleName;
            telemetry.Context.Cloud.RoleInstance = $"{roleName}-Instance";
        }
    }
}