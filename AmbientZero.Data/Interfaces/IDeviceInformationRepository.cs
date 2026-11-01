using AmbientZero.Data.Entities;

namespace AmbientZero.Data.Interfaces
{
    public interface IDeviceInformationRepository
    {
        void AddNewSensor(DeviceInformation deviceInformation);
        bool SensorExists(string deviceId);
        IEnumerable<DeviceInformation> GetAllSensors();
        DeviceInformation? GetSensorById(int id);
        void UpdateSensorSignal(DeviceInformation deviceInformation);
        int UpdateSensorVirtualName(string deviceId, string virtualName);
        void DeleteSensor(int sensorId);
    }
}
