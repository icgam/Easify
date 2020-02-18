using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Easify.AspNetCore.Security
{
    public enum AuthenticationMode
    {
        None,
        Impersonated,
        OAuth2
    }

    public sealed class AuthOptions
    {
        public AuthenticationMode AuthenticationMode { get; set; } = AuthenticationMode.None;
        public AuthenticationInfo Authentication { get; set; } = new AuthenticationInfo();
    }

    public sealed class AuthenticationInfo
    {
        public string Authority { get; set; }
        public string Audience { get; set; }
        public string AudienceSecret { get; set; }
        public string Description { get; set; }
    }

    public static class ConfigurationExtensions
    {
        public static AuthOptions GetAuthOptions(this IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var options = new AuthOptions();
            configuration.GetSection(nameof(AuthOptions)).Bind(options);

            // TODO: Validate the options 

            return options;
        }
    }

    public static class SecurityExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, AuthOptions options)
        {
            switch (options.AuthenticationMode)
            {
                case AuthenticationMode.None:
                    break;
                case AuthenticationMode.Impersonated:
                    services.AddAuthentication(ImpersonationBearerDefaults.AuthenticationScheme)
                        .AddImpersonationBearer();
                    break;
                case AuthenticationMode.OAuth2:
                    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(opt =>
                        {
                            opt.Authority = options.Authentication.Authority;
                            opt.Audience = options.Authentication.Audience;
                        });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return services;
        }


        private static AuthenticationBuilder AddImpersonationBearer(this AuthenticationBuilder builder,
            Action<ImpersonationBearerOptions> configOptions = null)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddScheme<ImpersonationBearerOptions, ImpersonationAuthenticationHandler>(ImpersonationBearerDefaults.AuthenticationScheme,
                configOptions);

            return builder;
        }
    }
    
    public class ImpersonationBearerDefaults
    {
        public const string AuthenticationScheme = "ImpersonationBearer";
    }

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

            var headerValueSections = authorizationHeader[0].Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            if (headerValueSections.Length != 2)
                return (string.Empty, true, "Invalid Authorization header");

            return headerValueSections[0] != ImpersonationBearerDefaults.AuthenticationScheme ? (string.Empty, true, $"Invalid Authorization header. Scheme {headerValueSections[0]} is invalid, should be {ImpersonationBearerDefaults.AuthenticationScheme} ") : (headerValueSections[1], false, string.Empty);
        }
    }

    public sealed class ImpersonationBearerOptions : AuthenticationSchemeOptions
    {
        public string Authority { get; set; }
    }
}