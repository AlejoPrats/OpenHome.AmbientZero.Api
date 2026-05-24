using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Request;
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
        [Route("GetSensorData")]
        public async Task<IActionResult> GetSensorInformation()
        {
            return Ok(_sensorService.GetSensorInformation());
        }

        [HttpGet]
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
        [Route("UpdateSensorName")]
        public async Task<IActionResult> UpdateSensorName(UpdateNameRequest updateNameRequest)
        {
            _sensorService.UpdateSensorVirtualName(updateNameRequest.DeviceId, updateNameRequest.VirtualName);
            return Accepted();
        }

        [HttpPatch]
        [Route("UpdateSensorSettings")]
        public async Task<IActionResult> UpdateSensorSettings(UpdateSettingsRequest updateSettingsRequest)
        {
            _sensorService.UpdateSensorSettings(updateSettingsRequest);
            return Accepted();
        }

        [HttpDelete]
        [Route("DeleteSensor")]
        public IActionResult DeleteSensor(int sensorId)
        {
            _sensorService.DeleteSensor(sensorId);
            return NoContent();
        }

    }
}
