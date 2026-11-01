using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Request;
using AmbientZero.Service.Models.Result;
using AmbientZero.Service.Services;
using Microsoft.AspNetCore.Authorization;
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
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _measurmentService = measurmentService ?? throw new ArgumentNullException(nameof(measurmentService));
            _sensorService = sensorService ?? throw new ArgumentNullException(nameof(sensorService));
        }

        [HttpPut]
        [AllowAnonymous]
        public ActionResult<AddMeasurementResult> AddMeasurement(MeasurementRequest measurementRequest)
        {
            var result = _measurmentService.AddNewMeasurement(measurementRequest);
            result.IsLightEnabled = _sensorService.ShouldTurnLightOn(measurementRequest.DeviceId ?? result.DeviceId);
            return Accepted(result);
        }

        [HttpGet]
        [Authorize(Policy = "Protected")]
        public IActionResult GetDailyMeasurements(DateTime? dateTime)
        {
            dateTime ??= DateTime.Now;
            return Ok(_measurmentService.GetDailyTemperature(dateTime.Value));
        }
    }
}
