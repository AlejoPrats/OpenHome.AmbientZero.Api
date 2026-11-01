using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Request;
using AmbientZero.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmbientZero.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicationSettingsController : ControllerBase
    {
        private readonly ILogger<ApplicationSettingsController> _logger;
        private readonly IApplicationSettingsService _applicationSettingsService;

        public ApplicationSettingsController(ILogger<ApplicationSettingsController> logger, IApplicationSettingsService applicationSettingsService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _applicationSettingsService = applicationSettingsService ?? throw new ArgumentNullException(nameof(applicationSettingsService));
        }

        [HttpGet]
        [Authorize(Policy = "Protected")]
        [Route("GetApplicationSettings")]
        public async Task<IActionResult> GetApplicationSettings()
        {
            return Ok(_applicationSettingsService.GetApplicationSettings());
        }

        [HttpPut]
        [Authorize(Policy = "Protected")]
        [Route("SaveApplicationSettings")]
        public async Task<IActionResult> SaveSettings(ApplicationSettingsRequest request)
        {
            if (_applicationSettingsService.SaveApplicationSettings(request))
            {
                return Accepted();
            }
            else
            {
                return Problem();
            }
        }

    }
}
