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
using Easify.Logging.SeriLog;
using Easify.Logging.SeriLog.OptionsBuilder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Easify.AspNetCore.Logging.SeriLog.Fluent
{
    // TODO: The whole fluent part need to be moved to another library
    public interface ILoggerBuilder
    {
        IConfiguration Configuration { get; }

        IHostEnvironment
            Environment { get; } // TODO: Need to be move out of the interface and be replaced by a provider

        IBuildLogger ConfigureLogger<TStartup>()
            where TStartup : class;

        IBuildLogger ConfigureLogger<TStartup>(Func<ISinkBuilderContext, ISinkBuilderContext> sinksProvider)
            where TStartup : class;

        IBuildLogger ConfigureLogger<TStartup>(Func<ISetFileSizeLimit, IBuildSeriLogOptions> optionsProvider,
            Func<ISinkBuilderContext, ISinkBuilderContext> sinksProvider)
            where TStartup : class;

        IBuildLogger ConfigureLogger<TStartup>(Func<ISetFileSizeLimit, IBuildSeriLogOptions> optionsProvider)
            where TStartup : class;
    }
}