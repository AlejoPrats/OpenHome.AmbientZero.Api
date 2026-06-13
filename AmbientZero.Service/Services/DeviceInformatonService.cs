using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models;
using AmbientZero.Service.Models.Options;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AmbientZero.Service.Services
{
    public class DeviceInformatonService : IDeviceInformationService
    {
        private readonly ServicesDependencies _servicesDependencies;
        public DeviceInformatonService(IOptions<ServicesDependencies> servicesDependencies)
        {
            _servicesDependencies = servicesDependencies?.Value ?? throw new ArgumentNullException(nameof(servicesDependencies));
        }

        public string GetDeviceIpAdress()
        {
            string commandResult = string.Empty;
            ProcessStartInfo getNetCmd = new ProcessStartInfo() { FileName = "ip", Arguments = "-j -br address", RedirectStandardOutput = true };

            using (Process proc = new Process() { StartInfo = getNetCmd })
            {
                proc.Start();
                StreamReader sr = proc.StandardOutput;
                commandResult = sr.ReadToEnd();
                proc.WaitForExit();
            }

            var addressObject = JsonSerializer.Deserialize<List<AddressObject>>(commandResult);
            var ip = addressObject
                .FirstOrDefault(x => x.AdapterStatus == "UP" && x.AdapterName == "wlan1")
                .AddressInformation
                .FirstOrDefault(x => x.AddressType == Constants.IPAddressType.IPv4).IpAddress;
            return ip;
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

                result.Add(JsonSerializer.Deserialize<ServiceStatus>(commandResult));
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

        public string GetCpuTemperature()
        {
            ProcessStartInfo getTemperature = new ProcessStartInfo() { FileName = "tail", Arguments = "/sys/class/thermal/thermal_zone0/temp", RedirectStandardOutput = true };
            string temp = string.Empty;

            using (Process proc = new Process() { StartInfo = getTemperature })
            {
                string output = string.Empty;
                proc.Start();
                StreamReader sr = proc.StandardOutput;
                temp = sr.ReadToEnd();
                proc.WaitForExit();
            }

            return (int.Parse(temp) / 1000).ToString();
        }

        public string GetCpuUsage()
        {
            ProcessStartInfo top = new ProcessStartInfo() { FileName = "top", Arguments = "-b -n1", RedirectStandardOutput = true };
            string cpuUsage = string.Empty;

            using (Process proc = new Process() { StartInfo = top })
            {
                string output = string.Empty;
                proc.Start();
                StreamReader sr = proc.StandardOutput;
                var topReadout = sr.ReadToEnd();
                cpuUsage = topReadout.Substring(topReadout.IndexOf("id,") - 5, 4);
                proc.WaitForExit();
            }

            return cpuUsage;
        }

        public string GetRamUsage()
        {
            ProcessStartInfo free = new ProcessStartInfo() { FileName = "free", RedirectStandardOutput = true };
            string ramUsage = string.Empty;

            using (Process proc = new Process() { StartInfo = free })
            {
                string output = string.Empty;
                proc.Start();
                StreamReader sr = proc.StandardOutput;
                sr.ReadLine();
                output = sr.ReadLine()!;
                ramUsage = (int.Parse(Regex.Match(output, @"(\d+)(?!.*\d)").Value) * 100 / int.Parse(Regex.Match(output, @"\d+").Value)).ToString();
                proc.WaitForExit();
            }

            return ramUsage;
        }
    }
}
