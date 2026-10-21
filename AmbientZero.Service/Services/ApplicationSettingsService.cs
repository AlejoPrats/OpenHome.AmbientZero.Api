using AmbientZero.Data.Constants;
using AmbientZero.Data.Entities;
using AmbientZero.Data.Interfaces;
using AmbientZero.Data.Transactions;
using AmbientZero.Service.Constants;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Options;
using AmbientZero.Service.Models.Request;
using AmbientZero.Service.Models.Result;
using Microsoft.Extensions.Logging;

namespace AmbientZero.Service.Services
{
    public class ApplicationSettingsService : IApplicationSettingsService
    {
        private readonly IApplicationSettingRepository _applicationSettingRepository;
        private readonly ITransactionScope _transactionScope;
        private readonly ILogger<ApplicationSettingsService> _logger;

        public ApplicationSettingsService(IApplicationSettingRepository applicationSettingRepository,
            ITransactionScope transactionScope,
            ILogger<ApplicationSettingsService> logger)
        {
            _applicationSettingRepository = applicationSettingRepository ?? throw new ArgumentNullException(nameof(applicationSettingRepository));
            _transactionScope = transactionScope ?? throw new ArgumentNullException(nameof(transactionScope));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ApplicationSettingResult GetApplicationSettings()
        {
            var result = _applicationSettingRepository.GetApplicationSettings();
            return new ApplicationSettingResult()
            {
                TimeZone = result.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.TimeZone).SettingValue!,
                DisplayUnit = int.Parse(result.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.DefaultUnit).SettingValue!)
            };
        }

        public Security? GetApplicationSecurity()
        {
            var result = _applicationSettingRepository.GetApplicationSettingByKey(ApplicationSettingsConstants.ApplicationSecurity);
            if (result is null)
            {
                return null;
            }
            else
            {
                return (Security)int.Parse(result.SettingValue!);
            }
        }

        public bool SaveApplicationSettings(ApplicationSettingsRequest applicationSettingsRequests)
        {
            try
            {
                _transactionScope.Execute(() =>
                {
                    if (applicationSettingsRequests.TimeZone != null)
                    {
                        _applicationSettingRepository.UpdateApplicationSetting(ApplicationSettingsConstants.TimeZone, applicationSettingsRequests.TimeZone);
                    }

                    if (applicationSettingsRequests.DisplayUnit.HasValue)
                    {
                        _applicationSettingRepository.UpdateApplicationSetting(ApplicationSettingsConstants.DefaultUnit, applicationSettingsRequests.DisplayUnit.ToString()!);
                    }
                });
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
