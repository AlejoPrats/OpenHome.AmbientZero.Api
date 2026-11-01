using AmbientZero.Data;
using AmbientZero.Data.Backup;
using AmbientZero.Service.Constants;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models;
using AmbientZero.Service.Models.Result;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text.Json;

namespace AmbientZero.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SystemController : ControllerBase
    {
        private readonly IDeviceInformationService _deviceInformationService;
        private readonly ISystemService _systemService;
        public SystemController(IDeviceInformationService deviceInformationService,
            ISystemService systemService)
        {
            _deviceInformationService = deviceInformationService ?? throw new ArgumentNullException(nameof(deviceInformationService));
            _systemService = systemService ?? throw new ArgumentNullException(nameof(systemService));
        }

        [HttpGet]
        [Route("WhoAmI")]
        public async Task<IActionResult> DeviceDescription()
        {
            var result = await _systemService.GetDeviceInformation();
            return Ok(result);
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
        [Route("GetSystemInformation")]
        public async Task<IActionResult> GetDeviceStatus()
        {
            var result = await _deviceInformationService.GetDeviceStatus();

            if (result is null)
            {
                return Problem();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("RamCleanup")]
        public async Task<IActionResult> RamCleanup(FreeRamMode freeRamMode)
        {
            int result = await _systemService.FreeRam(freeRamMode);
            return Ok(result);
        }

        [HttpGet]
        [Route("Health")]
        public IActionResult Health()
        {
            return Ok();
        }
    }
}
