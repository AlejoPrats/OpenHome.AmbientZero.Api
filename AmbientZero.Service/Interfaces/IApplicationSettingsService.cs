using AmbientZero.Service.Constants;
using AmbientZero.Service.Models.Request;
using AmbientZero.Service.Models.Result;

namespace AmbientZero.Service.Interfaces
{
    public interface IApplicationSettingsService
    {
        IEnumerable<ApplicationSettingResult> GetApplicationSettings();
        Security? GetApplicationSecurity();
        void SaveApplicationSettings(ApplicationSettingsRequest[] applicationSettingsRequests);
    }
}
