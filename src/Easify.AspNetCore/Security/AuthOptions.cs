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
using Easify.AspNetCore.Bootstrap;

namespace Easify.AspNetCore.Security
{
    public sealed class AuthOptions : ISetAuthenticationMode, IConfigureWithAuthority
    {
        public AuthenticationMode AuthenticationMode { get; set; } = AuthenticationMode.None;
        public AuthenticationInfo Authentication { get; set; } = new AuthenticationInfo();
        public void WithNoAuth()
        {
            AuthenticationMode = AuthenticationMode.None;
        }

        public void WithImpersonation()
        {
            AuthenticationMode = AuthenticationMode.Impersonated;
        }

        public IConfigureWithAuthority WithOAuth2()
        {
            AuthenticationMode = AuthenticationMode.OAuth2;
            return this;
        }

        public void UseParameters(string authority, string audience)
        {
            if (authority == null) throw new ArgumentNullException(nameof(authority));
            if (audience == null) throw new ArgumentNullException(nameof(audience));

            Authentication.Audience = audience;
            Authentication.Authority = authority;
        }
    }
}