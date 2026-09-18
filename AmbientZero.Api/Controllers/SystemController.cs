using AmbientZero.Data;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Result;
using AmbientZero.Service.Services;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace AmbientZero.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SystemController : ControllerBase
    {
        private readonly IDeviceInformationService _deviceInformationService;
        private readonly IBashScriptsService _bashScriptsService;
        private readonly AmbientZeroContext _context;
        public SystemController(IDeviceInformationService deviceInformationService, 
            AmbientZeroContext context,
            IBashScriptsService bashScriptsService)
        {
            _deviceInformationService = deviceInformationService ?? throw new ArgumentNullException(nameof(deviceInformationService));

            _context = context;
            _bashScriptsService = bashScriptsService;
        }

        [HttpGet]
        [Route("WhoAmI")]
        public DeviceDescription DeviceDescription()
        {
            return new DeviceDescription()
            {
                DeviceId = Guid.Parse("0f3da17e-91b7-4e3f-9c8a-43171cf4b007"),
                DeviceName = "AmbientZero",
                Ip = _deviceInformationService.GetDeviceIpAdress()
            };
        }

        [HttpGet]
        [Route("WhereAmI")]
        public string DeviceLocation()
        {
            return _deviceInformationService.GetSSIDName();
        }

        [HttpGet]
        [Route("GetTimeZones")]
        public IEnumerable<SystemTimeZone> GetTimeZones()
        {
            return TimeZoneInfo.GetSystemTimeZones().Select(x => new SystemTimeZone() { Id = x.Id, Name = x.DisplayName });
        }

        [HttpGet]
        [Route("TemporalSshPassword")]
        public IActionResult GetSshPassword()
        {
            return Ok(_context.ApplicationSettings.FirstOrDefault(x => x.SettingName == "SshPassword").SettingValue);
        }

        [HttpGet]
        [Route("ForceApiUpdate")]
        public IActionResult ForceApiUpdate()
        {
            _bashScriptsService.UpdateApi();
            return Ok();
        }

        [HttpGet]
        [Route("ForceWebUpdate")]
        public IActionResult ForceWebUpdate()
        {
            _bashScriptsService.UpdateWeb();
            return Ok();
        }

        [HttpGet]
        [Route("Health")]
        public IActionResult Health()
        {
            return Ok();
        }
    }
}
