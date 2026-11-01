using AmbientZero.Service.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Interfaces
{
    public interface IOnboardingService
    {
        OnboardingValidationResult ValidateOnboardingStatus();
        bool ValidateOnboardingSetup();
        void SaveLanguage(string lang);
        void SaveUnit(string unit);
        void SaveTimeZone(string timeZone);
        bool SaveAdminPassword(string password);
        bool FinishOnboardingSetup();
        bool FinishOnboardingTour();
    }
}
