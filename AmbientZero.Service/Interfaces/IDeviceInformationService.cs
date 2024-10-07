using AmbientZero.Service.Models;

namespace AmbientZero.Service.Interfaces
{
    public interface IDeviceInformationService
    {
        /// <summary>
        /// Gets the Local Ip of the device
        /// </summary>
        /// <returns>IP assinged by the router</returns>
        string GetDeviceIpAdress();

        /// <summary>
        /// Gets the MQTT port in case is set up reading the mosquito.conf file
        /// </summary>
        /// <returns>Port were the mosquitto service is listening</returns>
        int GetMQTTPort();

        /// <summary>
        /// Gets the actual temperature of the CPU executing tail on /sys/class/thermal/thermal_zone0/temp
        /// </summary>
        /// <returns>Temperature From Cpu</returns>
        string GetCpuTemperature();

        /// <summary>
        /// Gets CPU usage executing the top command and extracting the information available
        /// </summary>
        /// <returns>CPU usage percentage</returns>
        string GetCpuUsage();

        /// <summary>
        /// Gets the ram usage percentage executeing the free command
        /// </summary>
        /// <returns>Ram usage percentage</returns>
        string GetRamUsage();

        /// <summary>
        /// Gets the SSID name to which the device is connected
        /// </summary>
        /// <returns>SSID name</returns>
        string GetSSIDName();

        /// <summary>
        /// Gets the status of all the services that are specified inside the appsettings.json and returns a list with the information
        /// </summary>
        /// <returns>List of linux services especified</returns>
        List<ServiceStatus> GetDeviceServiceStatus();

        /// <summary>
        /// Gets the last 30 logs from the journalctl from the specified linux service
        /// </summary>
        /// <param name="serviceName">Service name to filter in the journalctl</param>
        /// <returns>last 30 logs of the specified service</returns>
        List<string> GetDeviceApplicationLogs(string serviceName);
    }
}
