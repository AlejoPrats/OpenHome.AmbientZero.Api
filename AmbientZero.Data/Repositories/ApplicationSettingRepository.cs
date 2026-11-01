using AmbientZero.Data.Constants;
using AmbientZero.Data.Entities;
using AmbientZero.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AmbientZero.Data.Repositories
{
    public class ApplicationSettingRepository : IApplicationSettingRepository
    {
        private readonly AmbientZeroContext _ambientZeroContext;
        public ApplicationSettingRepository(AmbientZeroContext ambientZeroContext)
        {
            _ambientZeroContext = ambientZeroContext ?? throw new ArgumentNullException(nameof(ambientZeroContext));
        }

        public IEnumerable<ApplicationSetting> GetApplicationSettings()
        {
            return _ambientZeroContext.ApplicationSettings.ToList();
        }

        public ApplicationSetting? GetApplicationSettingByKey(string key)
        {
            return _ambientZeroContext.ApplicationSettings.FirstOrDefault(x => x.SettingName == key);
        }

        public void AddApplicationSetting(ApplicationSetting applicationSettings)
        {
            _ambientZeroContext.Add(applicationSettings);
            _ambientZeroContext.SaveChanges();
        }

        public bool ApplicationSettingExists(string settingName)
        {
            return _ambientZeroContext.ApplicationSettings.Any(x => x.SettingName == settingName);
        }

        public IEnumerable<ApplicationSetting>? GetApplicationSettingsCollection(string[] applicationSettings)
        {
            return _ambientZeroContext
                .ApplicationSettings
                .Where(x => applicationSettings.Contains(x.SettingName))
                .ToList();
        }

        public int UpdateApplicationSetting(string settingName, string settingValue)
        {
            return _ambientZeroContext.ApplicationSettings.Where(x => x.SettingName == settingName)
                .ExecuteUpdate(x =>
                x.SetProperty(s => s.SettingValue, settingValue));
        }

        public IEnumerable<ApplicationSetting>? GetAdvancedApplicationSettings()
        {
            return _ambientZeroContext.ApplicationSettings
                .Where(x => ApplicationSettingsConstants.AdvancedSettings.Contains(x.SettingName))
                .ToList();
        }
    }
}
