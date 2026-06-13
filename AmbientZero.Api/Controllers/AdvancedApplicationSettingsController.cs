using AmbientZero.Service.Interfaces;
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
        public IActionResult GetAdvancedSettings()
        {
            var result = _advancedSettingsService.GetAdvancedSettings();
            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
