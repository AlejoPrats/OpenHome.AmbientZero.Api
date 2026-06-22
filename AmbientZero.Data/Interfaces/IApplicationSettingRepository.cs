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
        void UpdateApplicationSetting(string settingName, string settingValue);
        void UpdateApplicationSettingTransaction(string settingName, string settingValue);
        void UpdateApplicationSetting(ApplicationSetting applicationSetting);
        void UpdateApplicationSettingTransaction(ApplicationSetting applicationSetting);
    }
}
