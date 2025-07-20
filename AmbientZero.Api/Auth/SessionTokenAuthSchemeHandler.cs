using AmbientZero.Service.Constants;
using AmbientZero.Service.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace AmbientZero.Api.Auth
{
    public class SessionTokenAuthSchemeHandler : AuthenticationHandler<AuthSchemeOptions>
    {
        IApplicationSettingsService _applicationSettingsService;

        public SessionTokenAuthSchemeHandler(IApplicationSettingsService applicationSettingsService,
       IOptionsMonitor<AuthSchemeOptions> options,
       ILoggerFactory logger,
       UrlEncoder encoder) : base(options, logger, encoder)
        {
            _applicationSettingsService = applicationSettingsService;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Read the token from request headers/cookies
            // Check that it's a valid session, depending on your implementation
            /*if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.Fail("Authorization Header is not present in the request"));*/

            var applicationSettings = _applicationSettingsService.GetApplicationSettings();

            var securitySetting = (Security)int.Parse(applicationSettings.FirstOrDefault(x => x.ApplicationSettingName == ApplicationSettings.SecuritySetting)?.ApplicationSettingValue ?? "0");

            if (securitySetting == Security.None)
            {
                var autIdentity = new ClaimsIdentity(null, Scheme.Name);
                var authPrincipal = new ClaimsPrincipal(autIdentity);
                var authTicket = new AuthenticationTicket(authPrincipal, Scheme.Name);
                // Return the Authentication Ticket
                return Task.FromResult(AuthenticateResult.Success(authTicket));
            }


            return Task.FromResult(AuthenticateResult.Fail("Authorization Header is not present in the request"));

            // If the token is missing or the session is invalid, return failure:
            // return AuthenticateResult.Fail("Authentication failed");
        }
    }
}
