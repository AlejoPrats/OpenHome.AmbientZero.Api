using AmbientZero.Data.Constants;
using AmbientZero.Data.Interfaces;
using AmbientZero.Service.Constants;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Options;
using AmbientZero.Service.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Services
{
    public class AdvancedSettingsService : IAdvancedSettingsService
    {
        private readonly IApplicationSettingRepository _applicationSettingRepository;
        public AdvancedSettingsService(IApplicationSettingRepository applicationSettingRepository)
        {
            _applicationSettingRepository = applicationSettingRepository ?? throw new ArgumentNullException(nameof(applicationSettingRepository)); ;
        }

        public AdvancedSettingsResponse? GetAdvancedSettings()
        {
            var advancedSettings = _applicationSettingRepository.GetAdvancedApplicationSettings();
            
            if (advancedSettings is not null)
            {
                var result = new AdvancedSettingsResponse();
                result.AccessPointSettings = new AccessPointSettingsResponse()
                {
                    AccessPointName = advancedSettings.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.AccessPointName).SettingValue!,
                    AccessPointSecurity = (AccessPointAuthMethod)int.Parse(advancedSettings.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.AccessPointSecurity).SettingValue!),
                };

                result.GeneralSettings = new GeneralSettingsResponse()
                {
                    CheckForUpdatesEnabled = advancedSettings.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.CheckForUpdatesEnabled).SettingValue! != "0",
                    AutomaticUpdatesEnabled = advancedSettings.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.AutomaticUpdatesEnabled).SettingValue! != "0",
                    MaxBatteryValue = int.Parse(advancedSettings.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.MaxBatteryAdcReading).SettingValue!),
                    MinBatteryValue = int.Parse(advancedSettings.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.MinBatteryAdcReading).SettingValue!)
                };

                result.ApplicationSecurity = new ApplicationSecurityResponse()
                {
                    ApplicationSecurity = (Security)int.Parse(advancedSettings.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.ApplicationSecurity).SettingValue!)
                };

                return result;
            }

            return null;
        }
    }
}
