using AmbientZero.Data.Entities;

namespace AmbientZero.Data.Interfaces
{
    public interface IApplicationSettingRepository
    {
        IEnumerable<ApplicationSetting> GetApplicationSettings();
        void AddApplicationSetting(ApplicationSetting applicationSettings);
        bool ApplicationSettingExists(string settingName);
        void UpdateApplicationSetting(string settingName, string settingValue);
        void UpdateApplicationSetting(ApplicationSetting applicationSetting);
    }
}
