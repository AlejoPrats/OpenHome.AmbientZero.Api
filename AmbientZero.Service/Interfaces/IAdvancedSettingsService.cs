using AmbientZero.Service.Models.Request;
using AmbientZero.Service.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Interfaces
{
    public interface IAdvancedSettingsService
    {
        AdvancedSettingsResponse? GetAdvancedSettings();
        void UpdateGeneralSettings(UpdateGeneralSettingsRequest request);
    }
}
