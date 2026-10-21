using AmbientZero.Data.Constants;
using AmbientZero.Data.Interfaces;
using AmbientZero.Data.Transactions;
using AmbientZero.Service.Constants;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Options;
using AmbientZero.Service.Models.Request;
using AmbientZero.Service.Models.Response;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Services
{
    public class AdvancedSettingsService : IAdvancedSettingsService
    {
        private readonly IApplicationSettingRepository _applicationSettingRepository;
        private readonly ITransactionScope _transactionScope;
        private readonly ILogger<AdvancedSettingsService> _logger;
        private readonly IBashScriptsService _bashScriptsService;
        private readonly ITimeZoneService _timeZoneService;
        public AdvancedSettingsService(IApplicationSettingRepository applicationSettingRepository,
            ITransactionScope transactionScope,
            ILogger<AdvancedSettingsService> logger,
            IBashScriptsService bashScriptsService,
            ITimeZoneService timeZoneService)
        {
            _applicationSettingRepository = applicationSettingRepository ?? throw new ArgumentNullException(nameof(applicationSettingRepository));
            _transactionScope = transactionScope ?? throw new ArgumentNullException(nameof(transactionScope));
            _bashScriptsService = bashScriptsService ?? throw new ArgumentNullException(nameof(bashScriptsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _timeZoneService = timeZoneService ?? throw new ArgumentNullException(nameof(timeZoneService));
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
                    MinBatteryValue = int.Parse(advancedSettings.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.MinBatteryAdcReading).SettingValue!),
                    LastBackupDate = null/*_timeZoneService
                    .GetTimeFromTimezone(advancedSettings.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.LastBackupDate)?.SettingValue,
                    advancedSettings.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.TimeZone).SettingValue!)*/
                };

                result.ApplicationSecurity = new ApplicationSecurityResponse()
                {
                    ApplicationSecurity = (Security)int.Parse(advancedSettings.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.ApplicationSecurity).SettingValue!),
                    SshPassword = advancedSettings.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.SshPassword).SettingValue!
                };

                return result;
            }

            return null;
        }

        public void UpdateGeneralSettings(UpdateGeneralSettingsRequest request)
        {
            try
            {
                _transactionScope.Execute(() =>
                {
                    _applicationSettingRepository.UpdateApplicationSetting(ApplicationSettingsConstants.CheckForUpdatesEnabled, request.CheckForUpdatesEnabled ? "1" : "0");
                    _applicationSettingRepository.UpdateApplicationSetting(ApplicationSettingsConstants.AutomaticUpdatesEnabled, request.AutomaticUpdatesEnabled ? "1" : "0");
                    _applicationSettingRepository.UpdateApplicationSetting(ApplicationSettingsConstants.MaxBatteryAdcReading, request.MaxBatteryValue.ToString());
                    _applicationSettingRepository.UpdateApplicationSetting(ApplicationSettingsConstants.MinBatteryAdcReading, request.MinBatteryValue.ToString());
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Updating General Settings: {Message}", ex.Message);
            }
        }

        public async Task UpdateNetworkSettings(UpdateAccessPointRequest request)
        {
            await _bashScriptsService.ConfigureAccessPoint(request);
        }
    }
}
