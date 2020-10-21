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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ISystemClock = Microsoft.AspNetCore.Authentication.ISystemClock;

namespace Easify.AspNetCore.Security.Impersonation
{
    public sealed class ImpersonationAuthenticationHandler : AuthenticationHandler<ImpersonationBearerOptions>
    {
        public ImpersonationAuthenticationHandler(IOptionsMonitor<ImpersonationBearerOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            AuthenticateResult authenticateResult;
            try
            {
                var (tokenValue, hasError, error) = ExtractTokenValue();
                if (hasError)
                {
                    authenticateResult = AuthenticateResult.Fail(error);
                    return Task.FromResult(authenticateResult);
                }

                var identity = CreateClaimsIdentity(tokenValue);
                var principal = new ClaimsPrincipal(identity);
          
                authenticateResult = AuthenticateResult.Success(new AuthenticationTicket(principal, ImpersonationBearerDefaults.AuthenticationScheme));
                return Task.FromResult(authenticateResult);
            }
            catch (Exception e)
            {
                authenticateResult = AuthenticateResult.Fail(e);
                return Task.FromResult(authenticateResult);
            }
        }

        private ClaimsIdentity CreateClaimsIdentity(string tokenValue)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadJwtToken(tokenValue);

            var claimsIdentity = new ClaimsIdentity("AuthenticationTypes.Federation", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
            claimsIdentity.AddClaims(securityToken.Claims);
            
            return claimsIdentity;
        }

        private (string TokenValue, bool HasError, string Error) ExtractTokenValue()
        {
            var authorizationHeader = Context.Request.Headers["Authorization"];
            if (authorizationHeader.Count != 1)
                return (string.Empty, true, "No Authorization header");

            var headerValueSections = authorizationHeader[0].Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            if (headerValueSections.Length != 2)
                return (string.Empty, true, "Invalid Authorization header");

            return headerValueSections[0] != ImpersonationBearerDefaults.AuthenticationScheme ? (string.Empty, true, $"Invalid Authorization header. Scheme {headerValueSections[0]} is invalid, should be {ImpersonationBearerDefaults.AuthenticationScheme} ") : (headerValueSections[1], false, string.Empty);
        }
    }
}