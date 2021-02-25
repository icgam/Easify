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
using Microsoft.Extensions.Hosting;

namespace Easify.AspNetCore
{
    public sealed class ConfigurationOptions
    {
        public ConfigurationOptions(string basePath, string environment, string appEntry, string[] args)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            AppEntry = appEntry ?? throw new ArgumentNullException(nameof(appEntry));
            Args = args ?? throw new ArgumentNullException(nameof(args));
            BasePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
        }
        public string BasePath { get; }
        public string Environment { get; }
        public string AppEntry { get; }
        public string[] Args { get; }
        public bool IsDevelopment => Environment.Equals(Environments.Development);
    }
}