using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models;
using AmbientZero.Service.Models.Options;
using AmbientZero.Service.Models.Result;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AmbientZero.Service.Services
{
    public class DeviceInformatonService : IDeviceInformationService
    {
        private readonly ServicesDependencies _servicesDependencies;
        private readonly IBashScriptsService _bashScriptService;
        private readonly ILogger<DeviceInformatonService> _logger;
        public DeviceInformatonService(IOptions<ServicesDependencies> servicesDependencies,
            IBashScriptsService bashScriptsService,
            ILogger<DeviceInformatonService> logger)
        {
            _servicesDependencies = servicesDependencies?.Value ?? throw new ArgumentNullException(nameof(servicesDependencies));
            _bashScriptService = bashScriptsService ?? throw new ArgumentNullException(nameof(bashScriptsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public List<ServiceStatus> GetDeviceServiceStatus()
        {
            var result = new List<ServiceStatus>();

            foreach (string serviceName in _servicesDependencies.ServicesNames)
            {
                ProcessStartInfo getSSID = new ProcessStartInfo() { FileName = "python3", Arguments = $"/home/dietpi/status.py {serviceName}", RedirectStandardOutput = true };
                string commandResult = string.Empty;

                using (Process proc = new Process() { StartInfo = getSSID })
                {
                    proc.Start();
                    StreamReader sr = proc.StandardOutput;
                    commandResult = sr.ReadToEnd();
                    proc.WaitForExit();
                }

                ServiceStatus serviceStatus;

                try
                {
                    serviceStatus = JsonSerializer.Deserialize<ServiceStatus>(commandResult)!;
                    result.Add(serviceStatus);
                }
                catch
                {
                    continue;
                }

            }

            return result;
        }

        public List<string> GetDeviceApplicationLogs(string serviceName)
        {
            ProcessStartInfo getAppLogs = new ProcessStartInfo() { FileName = "journalctl", Arguments = $"--unit={serviceName} --lines=30", RedirectStandardOutput = true };
            string commandResult = string.Empty;

            using (Process proc = new Process() { StartInfo = getAppLogs })
            {
                proc.Start();
                StreamReader sr = proc.StandardOutput;
                commandResult = sr.ReadToEnd();
                proc.WaitForExit();
            }

            var result = commandResult.Split(Environment.NewLine).ToList();

            return result;
        }

        public string GetSSIDName()
        {
            string ssidName = string.Empty;
            ProcessStartInfo getSSID = new ProcessStartInfo() { FileName = "iwgetid", Arguments = "-r", RedirectStandardOutput = true };

            using (Process proc = new Process() { StartInfo = getSSID })
            {
                proc.Start();
                StreamReader sr = proc.StandardOutput;
                ssidName = sr.ReadToEnd();
                proc.WaitForExit();
            }

            return ssidName;
        }

        public int GetMQTTPort()
        {
            string mqttPort = string.Empty;
            ProcessStartInfo getMQTTPort = new ProcessStartInfo() { FileName = "grep", Arguments = "listener /etc/mosquitto/mosquitto.conf", RedirectStandardOutput = true };

            using (Process proc = new Process() { StartInfo = getMQTTPort })
            {
                proc.Start();
                StreamReader sr = proc.StandardOutput;
                mqttPort = sr.ReadToEnd().Replace("listener", string.Empty).Trim();
                proc.WaitForExit();
            }

            return int.Parse(mqttPort);
        }

        public async Task<DeviceStatusResult?> GetDeviceStatus()
        {

            string systemInfoResponse = await _bashScriptService.GetSystemInfo();

            if (string.IsNullOrEmpty(systemInfoResponse))
            {
                return null;
            }
            else
            {
                return JsonSerializer.Deserialize<DeviceStatusResult>(systemInfoResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }
    }
}
