using AmbientZero.Service.Models.Request;
using AmbientZero.Service.Models.Result;

namespace AmbientZero.Service.Interfaces
{
    public interface IApplicationSettingsService
    {
        IEnumerable<ApplicationSettingResult> GetApplicationSettings();
        void SaveApplicationSettings(ApplicationSettingsRequest[] applicationSettingsRequests);
    }
}
