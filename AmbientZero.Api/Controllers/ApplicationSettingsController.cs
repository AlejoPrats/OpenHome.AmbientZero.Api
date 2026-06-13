using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Request;
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
            _logger = logger;
            _applicationSettingsService = applicationSettingsService;
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
        public async Task<IActionResult> SaveSettings(ApplicationSettingsRequest[] applicationSetting)
        {
            _applicationSettingsService.SaveApplicationSettings(applicationSetting);
            return Accepted();
        }

    }
}
