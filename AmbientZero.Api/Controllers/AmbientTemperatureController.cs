using AmbientZero.Data.Entities;
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

        public AmbientTemperatureController(ILogger<AmbientTemperatureController> logger,
            IMeasurmentService measurmentService)
        {
            _logger = logger;
            _measurmentService = measurmentService;
        }

        [HttpPut]
        public ActionResult AddMeasurement(MeasurementRequest measurementRequest)
        {
            _measurmentService.AddNewMeasurement(measurementRequest);
            return Accepted();
        }

        [HttpGet]
        public ActionResult<IEnumerable<MeasurementResult>> GetDailyMeasurements(DateTime? dateTime)
        {
            dateTime ??= DateTime.Now;
            return Ok(_measurmentService.GetDailyTemperature(dateTime.Value));
        }
    }
}
