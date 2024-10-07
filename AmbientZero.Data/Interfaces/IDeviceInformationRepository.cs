using AmbientZero.Data.Entities;

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
