using AmbientZero.Data.Constants;
using AmbientZero.Data.Entities;
using AmbientZero.Data.Interfaces;
using AmbientZero.Service.Constants;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Options;
using AmbientZero.Service.Models.Request;
using AmbientZero.Service.Models.Result;

namespace AmbientZero.Service.Services
{
    public class ApplicationSettingsService : IApplicationSettingsService
    {
        private readonly IApplicationSettingRepository _applicationSettingRepository;

        public ApplicationSettingsService(IApplicationSettingRepository applicationSettingRepository)
        {
            _applicationSettingRepository = applicationSettingRepository ?? throw new ArgumentNullException(nameof(applicationSettingRepository)); 
        }

        public IEnumerable<ApplicationSettingResult> GetApplicationSettings()
        {
            return _applicationSettingRepository.GetApplicationSettings()
                .Select(x => new ApplicationSettingResult() { ApplicationSettingName = x.SettingName, ApplicationSettingValue = x.SettingValue })
                .ToList();
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
        public void SaveApplicationSettings(ApplicationSettingsRequest[] applicationSettingsRequests)
        {
            foreach (var applicationSettingsRequest in applicationSettingsRequests)
            {
                if (_applicationSettingRepository.ApplicationSettingExists(applicationSettingsRequest.SettingName))
                {
                    _applicationSettingRepository.UpdateApplicationSetting(applicationSettingsRequest.SettingName, applicationSettingsRequest.SettingValue);
                }
                else
                {
                    _applicationSettingRepository.AddApplicationSetting(new ApplicationSetting()
                    {
                        SettingName = applicationSettingsRequest.SettingName,
                        SettingValue = applicationSettingsRequest.SettingValue
                    });
                }
            }
        }
    }
}
