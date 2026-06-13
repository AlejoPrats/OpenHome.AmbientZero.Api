using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmbientZero.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SensorController : ControllerBase
    {
        private readonly ILogger<SensorController> _logger;
        private readonly ISensorService _sensorService;

        public SensorController(ILogger<SensorController> logger,
    ISensorService sensorService)
        {
            _logger = logger;
            _sensorService = sensorService;
        }

        [HttpGet]
        [Authorize(Policy = "Protected")]
        [Route("GetSensorData")]
        public async Task<IActionResult> GetSensorInformation()
        {
            return Ok(_sensorService.GetSensorInformation());
        }

        [HttpGet]
        [Authorize(Policy = "Protected")]
        [Route("GetSensorById")]
        public async Task<IActionResult> GetSensorById(int sensorId)
        {
            var result = _sensorService.GetSensorInformationById(sensorId);

            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }

        }

        [HttpPatch]
        [Authorize(Policy = "Protected")]
        [Route("UpdateSensorName")]
        public async Task<IActionResult> UpdateSensorName(UpdateNameRequest updateNameRequest)
        {
            _sensorService.UpdateSensorVirtualName(updateNameRequest.DeviceId, updateNameRequest.VirtualName);
            return Accepted();
        }

        [HttpPatch]
        [Authorize(Policy = "Protected")]
        [Route("UpdateSensorSettings")]
        public async Task<IActionResult> UpdateSensorSettings(UpdateSettingsRequest updateSettingsRequest)
        {
            _sensorService.UpdateSensorSettings(updateSettingsRequest);
            return Accepted();
        }

        [HttpDelete]
        [Authorize(Policy = "Protected")]
        [Route("DeleteSensor")]
        public IActionResult DeleteSensor(int sensorId)
        {
            _sensorService.DeleteSensor(sensorId);
            return NoContent();
        }

    }
}
