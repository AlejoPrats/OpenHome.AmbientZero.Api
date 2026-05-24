using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Request;
using AmbientZero.Service.Models.Result;
using Microsoft.AspNetCore.Mvc;

namespace AmbientZero.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AmbientTemperatureController : ControllerBase
    {
        private readonly ILogger<AmbientTemperatureController> _logger;
        private readonly IMeasurmentService _measurmentService;
        private readonly ISensorService _sensorService;

        public AmbientTemperatureController(ILogger<AmbientTemperatureController> logger,
            IMeasurmentService measurmentService,
            ISensorService sensorService)
        {
            _logger = logger;
            _measurmentService = measurmentService;
            _sensorService = sensorService;
        }

        [HttpPut]
        public ActionResult<AddMeasurementResult> AddMeasurement(MeasurementRequest measurementRequest)
        {
            var result = _measurmentService.AddNewMeasurement(measurementRequest);
            result.IsLightEnabled = _sensorService.ShouldTurnLightOn(result.DeviceId ?? measurementRequest.DeviceId);
            return Accepted(result);
        }

        [HttpGet]
        public IActionResult GetDailyMeasurements(DateTime? dateTime)
        {
            dateTime ??= DateTime.Now;
            return Ok(_measurmentService.GetDailyTemperature(dateTime.Value));
        }
    }
}
