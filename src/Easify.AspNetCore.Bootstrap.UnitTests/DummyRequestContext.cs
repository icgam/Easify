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
using System.Security.Principal;
using Easify.Http;

namespace Easify.AspNetCore.Bootstrap.UnitTests
{
    public sealed class DummyRequestContext : IRequestContext
    {
        public string CorrelationId { get; } = Guid.NewGuid().ToString();
        public string AuthorizationHeader { get; } = string.Empty;
        public IPrincipal User { get; } = new GenericPrincipal(new GenericIdentity("Test"), new string[] { });
    }
}