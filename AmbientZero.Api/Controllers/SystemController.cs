using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Result;
using Microsoft.AspNetCore.Mvc;

namespace AmbientZero.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SystemController : ControllerBase
    {
        private readonly IDeviceInformationService _deviceInformationService;
        public SystemController(IDeviceInformationService deviceInformationService)
        {
            _deviceInformationService = deviceInformationService;
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
    }
}
