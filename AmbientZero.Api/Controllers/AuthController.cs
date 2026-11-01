using AmbientZero.Data.Entities;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Request;
using AmbientZero.Service.Models.Result;
using AmbientZero.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AmbientZero.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IApplicationSettingsService _applicationSettingsService;

        public AuthController(IAuthService authService,
            IApplicationSettingsService applicationSettingsService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _applicationSettingsService = applicationSettingsService ?? throw new ArgumentNullException(nameof(applicationSettingsService));
        }

        [HttpGet]
        [Route("GetProtectionMode")]
        [AllowAnonymous]
        public IActionResult GetProtectionMode()
        {
            var result = _applicationSettingsService.GetApplicationSecurity();
            if (result is null)
            {
                return NotFound();
            }
            else
            { 
                return Ok(result);
            }
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        [EnableRateLimiting("LoginLimit")]
        public IActionResult Login(LoginRequest login)
        {
            var result = _authService.AuthUser(login.Username, login.Password);

            if (result is not null)
            {
                return Ok(new LoginResponse() { Token = result });
            }
            else
            {
                return Forbid();
            }
        }
    }
}
