using AmbientZero.Service.Constants;
using AmbientZero.Service.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Interfaces
{
    public interface ISystemService
    {
        Task<DeviceDescription> GetDeviceInformation();
        Task ScheduleReboot();
        Task<int> FreeRam(FreeRamMode freeRamMode);

    }
}
