using AmbientZero.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Interfaces
{
    public interface IDeviceInformationRepository
    {
        void AddNewSensor(DeviceInformation deviceInformation);
        bool SensorExists(string deviceId);
        IEnumerable<DeviceInformation> GetAllSensors();
        void UpdateSensorSignal(DeviceInformation deviceInformation);
        void UpdateSensorVirtualName(string deviceId, string virtualName);
    }
}
