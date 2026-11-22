using AmbientZero.Data.Entities;
using AmbientZero.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AmbientZero.Data.Repositories
{
    public class SensorSettingRepository : ISensorSettingRepository
    {
        private readonly AmbientZeroContext _ambientZeroContext;
        public SensorSettingRepository(AmbientZeroContext ambientZeroContext)
        {
            _ambientZeroContext = ambientZeroContext ?? throw new ArgumentNullException(nameof(ambientZeroContext));
        }

        public bool SensorSettingExist(string deviceId)
        {
            return _ambientZeroContext.SensorSettings.Any(x => x.DeviceId == deviceId);
        }

        public SensorSetting? GetSensorSettingForSensor(string deviceId)
        {
            return _ambientZeroContext.SensorSettings
                .AsNoTracking()
                .FirstOrDefault(x => x.DeviceId == deviceId);
        }

        public void AddSensorSetting(SensorSetting sensorSetting)
        {
            _ambientZeroContext.SensorSettings.Add(sensorSetting);
            _ambientZeroContext.SaveChanges();
        }

        public void UpdaterSensorSetting(SensorSetting sensorSetting)
        {
            _ambientZeroContext.SensorSettings.Where(x => x.DeviceId == sensorSetting.DeviceId)
                .ExecuteUpdate(x =>
                x.SetProperty(s => s.IsLightEnabled, s => sensorSetting.IsLightEnabled.HasValue ? sensorSetting.IsLightEnabled : s.IsLightEnabled)
                    .SetProperty(s => s.IsLightOnSchedule, s => sensorSetting.IsLightOnSchedule.HasValue ? sensorSetting.IsLightOnSchedule : s.IsLightOnSchedule)
                    .SetProperty(s => s.DisableStartTime, s => sensorSetting.DisableStartTime ?? s.DisableStartTime)
                    .SetProperty(s => s.DisableEndTime, s => sensorSetting.DisableEndTime ?? s.DisableEndTime)
                    .SetProperty(s => s.Color, s => sensorSetting.Color ?? s.Color));
        }

        public void DeleteSensorSettings(int settingId)
        {
            _ambientZeroContext.SensorSettings.Where(x => x.Id == settingId).ExecuteDelete();
        }
    }
}
