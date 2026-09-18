using AmbientZero.Data.Constants;
using AmbientZero.Data.Entities;
using AmbientZero.Data.Interfaces;
using AmbientZero.Data.Transactions;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Services
{
    public class OnboardingService : IOnboardingService
    {
        private readonly IApplicationSettingRepository _applicationSettingRepository;
        private readonly ISecurityService _securityService;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionScope _transactionScope;

        public OnboardingService(IApplicationSettingRepository applicationSettingRepository,
            ISecurityService securityService,
            IUserRepository userRepository,
            ITransactionScope transactionScope)
        {
            _applicationSettingRepository = applicationSettingRepository ?? throw new ArgumentNullException(nameof(applicationSettingRepository));
            _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _transactionScope = transactionScope ?? throw new ArgumentNullException(nameof(transactionScope));
        }

        public OnboardingValidationResult ValidateOnboardingStatus()
        {
            var result = new OnboardingValidationResult()
            {
                IsSetupPending = _applicationSettingRepository.GetApplicationSettingByKey(ApplicationSettingsConstants.IsFirstRunSetup).SettingValue == "1",
                IsApplicationTourPending = _applicationSettingRepository.GetApplicationSettingByKey(ApplicationSettingsConstants.IsFirstRun).SettingValue == "1"
            };

            return result;
        }

        public bool ValidateOnboardingSetup()
        {
            return _applicationSettingRepository.GetApplicationSettingByKey(ApplicationSettingsConstants.IsFirstRunSetup).SettingValue == "1";
        }

        public void SaveLanguage(string lang)
        {
            _applicationSettingRepository.UpdateApplicationSetting(ApplicationSettingsConstants.DefaultLanguage, lang);
        }

        public void SaveUnit(string unit)
        {
            _applicationSettingRepository.UpdateApplicationSetting(ApplicationSettingsConstants.DefaultUnit, unit);
        }

        public void SaveTimeZone(string timeZone)
        {
            _applicationSettingRepository.UpdateApplicationSetting(ApplicationSettingsConstants.TimeZone, timeZone);
        }

        public bool SaveAdminPassword(string password)
        {
            string? hash = _securityService.HashPassword(ApplicationSettingsConstants.AdminUsername, password);

            if (string.IsNullOrEmpty(hash))
            {
                return false;
            }

            try
            {
                _transactionScope.Execute(() =>
                {
                    _userRepository.UpdatePassword(ApplicationSettingsConstants.AdminUsername, hash);
                    _userRepository.ActivateUser(ApplicationSettingsConstants.AdminUsername);
                });

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool FinishOnboardingSetup()
        {
            int result = _applicationSettingRepository.UpdateApplicationSetting(ApplicationSettingsConstants.IsFirstRunSetup, "0");
            return result == 1;
        }

        public bool FinishOnboardingTour()
        {
            int result = _applicationSettingRepository.UpdateApplicationSetting(ApplicationSettingsConstants.IsFirstRun, "0");
            return result == 1;
        }
    }
}
