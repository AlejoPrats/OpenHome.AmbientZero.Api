using AmbientZero.Data.Entities;

namespace AmbientZero.Data.Interfaces
{
    public interface IApplicationSettingRepository
    {
        IEnumerable<ApplicationSetting> GetApplicationSettings();
        void AddApplicationSetting(ApplicationSetting applicationSettings);
        bool ApplicationSettingExists(string settingName);
        IEnumerable<ApplicationSetting>? GetApplicationSettingsCollection(string[] applicationSettings);
        ApplicationSetting? GetApplicationSettingByKey(string key);
        IEnumerable<ApplicationSetting>? GetAdvancedApplicationSettings();
        int UpdateApplicationSetting(string settingName, string settingValue);
    }
}
