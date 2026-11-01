using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace AmbientZero.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdvancedApplicationSettingsController : ControllerBase
    {
        private readonly IAdvancedSettingsService _advancedSettingsService;
        public AdvancedApplicationSettingsController(IAdvancedSettingsService advancedSettingsService)
        {
            _advancedSettingsService = advancedSettingsService ?? throw new ArgumentNullException(nameof(advancedSettingsService));
        }

        [HttpGet]
        [Route("GetAdvancedSettings")]
        public IActionResult GetAdvancedSettings()
        {
            var result = _advancedSettingsService.GetAdvancedSettings();
            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPatch]
        [Route("UpdateGeneralSettings")]
        public IActionResult UpdateGeneralSettings(UpdateGeneralSettingsRequest request)
        {
            _advancedSettingsService.UpdateGeneralSettings(request);
            return Accepted();
        }

        [HttpPatch]
        [Route("UpdateNetworkSettings")]
        public async Task<IActionResult> UpdateNetworkSettings(UpdateAccessPointRequest request)
        {
            await _advancedSettingsService.UpdateNetworkSettings(request);
            return Ok();
        }
    }
}
