using AmbientZero.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Interfaces
{
    public interface ISensorSettingRepository
    {
        void AddSensorSetting(SensorSetting sensorSetting);
        void UpdaterSensorSetting(SensorSetting sensorSetting);
        bool SensorSettingExist(string deviceId);
        SensorSetting GetSensorSettingForSensor(string deviceId);
    }
}
