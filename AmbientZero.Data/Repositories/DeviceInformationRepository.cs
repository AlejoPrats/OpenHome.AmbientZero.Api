using AmbientZero.Data.Entities;
using AmbientZero.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AmbientZero.Data.Repositories
{
    public class DeviceInformationRepository : IDeviceInformationRepository
    {
        private readonly AmbientZeroContext _ambientZeroContext;
        public DeviceInformationRepository(AmbientZeroContext ambientZeroContext)
        {
            _ambientZeroContext = ambientZeroContext ?? throw new ArgumentNullException(nameof(ambientZeroContext));
        }

        public void AddNewSensor(DeviceInformation deviceInformation)
        {
            _ambientZeroContext.Devices.Add(deviceInformation);
            _ambientZeroContext.SaveChanges();
        }

        public bool SensorExists(string deviceId)
        {
            return _ambientZeroContext.Devices.Any(x => x.DeviceId == deviceId);
        }

        public IEnumerable<DeviceInformation> GetAllSensors()
        {
            return _ambientZeroContext.Devices
                .Include(x => x.Settings)
                .ToList();
        }

        public DeviceInformation? GetSensorById(int id)
        {
            return _ambientZeroContext.Devices
                .Include(x => x.Settings)
                .FirstOrDefault(x => x.Id == id);
        }

        public void UpdateSensorSignal(DeviceInformation deviceInformation)
        {
            _ambientZeroContext.Devices
                .Where(x => x.DeviceId == deviceInformation.DeviceId)
                .ExecuteUpdate(x => x.SetProperty(d => d.IsSignaling, deviceInformation.IsSignaling)
                .SetProperty(d => d.ADCReading, deviceInformation.ADCReading)
                .SetProperty(d => d.Version, deviceInformation.Version));
        }

        public void UpdateSensorVirtualName(string deviceId, string virtualName)
        {
            _ambientZeroContext.Devices
                .Where(x => x.DeviceId == deviceId)
                .ExecuteUpdate(x => x.SetProperty(d => d.DeviceVirtualName, virtualName).SetProperty(d => d.IsSignaling, false));
        }

        public void DeleteSensor(int sensorId)
        {
            _ambientZeroContext.Devices
                .Where(x => x.Id == sensorId)
                .ExecuteDelete();
        }
    }
}
