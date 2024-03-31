using AmbientZero.Data.Entities;
using AmbientZero.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Repositories
{
    public class SensorSettingRepository : ISensorSettingRepository
    {
        private readonly AmbientZeroContext _ambientZeroContext;
        public SensorSettingRepository(AmbientZeroContext ambientZeroContext)
        {
            _ambientZeroContext = ambientZeroContext;
        }

        public bool SensorSettingExist(string deviceId)
        { 
            return _ambientZeroContext.SensorSettings.Any(x => x.DeviceId == deviceId);
        }

        public SensorSetting GetSensorSettingForSensor(string deviceId)
        {
            return _ambientZeroContext.SensorSettings.FirstOrDefault(x => x.DeviceId == deviceId);
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
                x.SetProperty(s => s.IsLightEnabled, sensorSetting.IsLightEnabled)
                    .SetProperty(s => s.IsLightOnSchedule, sensorSetting.IsLightOnSchedule)
                    .SetProperty(s => s.DisableStartTime, sensorSetting.DisableStartTime)
                    .SetProperty(s => s.DisableEndTime, sensorSetting.DisableEndTime));
        }
    }
}
