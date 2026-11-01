using AmbientZero.Service.Constants;
using AmbientZero.Service.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Interfaces
{
    public interface IBashScriptsService
    {
        Task ConfigureAccessPoint(UpdateAccessPointRequest request);
        void UpdateApi();
        void UpdateWeb();
        Task<string> GetSystemInfo();
        Task FreeRam(FreeRamMode freeRamMode, long delay = 0);
        Task<string> GetNetworkAdapterStatus(string adapterName);
        Task UpdateModule(string script, string remoteUrl, bool hasFile = false, int seconds = 10);
        Task ScheduleReboot(long seconds);
        void OfflineExecution(string executionType);
    }
}
