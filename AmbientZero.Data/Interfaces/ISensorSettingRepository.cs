using AmbientZero.Data.Entities;

namespace AmbientZero.Data.Interfaces
{
    public interface ISensorSettingRepository
    {
        void AddSensorSetting(SensorSetting sensorSetting);
        void UpdaterSensorSetting(SensorSetting sensorSetting);
        bool SensorSettingExist(string deviceId);
        SensorSetting? GetSensorSettingForSensor(string deviceId);
        void DeleteSensorSettings(int settingId);
    }
}
