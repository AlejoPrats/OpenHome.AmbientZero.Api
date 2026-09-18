using AmbientZero.Service.Constants;
using AmbientZero.Service.Models.Request;
using AmbientZero.Service.Models.Result;

namespace AmbientZero.Service.Interfaces
{
    public interface IApplicationSettingsService
    {
        ApplicationSettingResult GetApplicationSettings();
        Security? GetApplicationSecurity();
        bool SaveApplicationSettings(ApplicationSettingsRequest applicationSettingsRequests);
    }
}
