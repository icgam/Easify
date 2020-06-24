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
using Microsoft.Extensions.Configuration;

namespace Easify.Hosting.Core.Configuration
{
    public sealed class HostingOptions
    {
        public HostingOptions(string pathToContentRoot, bool launchedAsService, IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrWhiteSpace(pathToContentRoot))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(pathToContentRoot));

            Name = configuration[ConfigurationKeys.AppNameKey];
            Version = configuration[ConfigurationKeys.AppVersionKey];
            BaseUrl = configuration.GetSection("ServiceHost").Get<ServiceHostConfig>().BaseUrl;
            PathToContentRoot = pathToContentRoot;
            LaunchedAsService = launchedAsService;
            Configuration = configuration;
        }

        public string Name { get; }
        public string Version { get; }
        public string BaseUrl { get; }
        public string PathToContentRoot { get; }
        public bool LaunchedAsService { get; }
        public IConfiguration Configuration { get; }
    }
}