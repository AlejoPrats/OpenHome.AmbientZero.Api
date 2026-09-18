using AmbientZero.Data.Constants;
using AmbientZero.Service.Constants;
using AmbientZero.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AmbientZero.Api.AuthHandler
{
    public class ProtectionHandler : AuthorizationHandler<ProtectionRequirement>
    {
        private readonly IApplicationSettingsService _settings;

        public ProtectionHandler(IApplicationSettingsService settings)
        {
            _settings = settings;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProtectionRequirement requirement)
        {
            var http = (context.Resource as DefaultHttpContext)!;

            var requestProtectionMode = http.Request.Headers["X-Protection-Mode"].FirstOrDefault();
            var applicationProtectionMode = _settings.GetApplicationSecurity();

            if (applicationProtectionMode == Security.None)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (context.User.Identity?.IsAuthenticated == true)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (int.TryParse(requestProtectionMode, out var fe) && (int)applicationProtectionMode < fe)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }
    }
}