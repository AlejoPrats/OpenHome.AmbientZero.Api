using AmbientZero.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Interfaces
{
    public interface IApplicationSettingRepository
    {
        IEnumerable<ApplicationSetting> GetApplicationSettings();
        void AddApplicationSetting(ApplicationSetting applicationSettings);
        void UpdateApplicationSetting(ApplicationSetting applicationSetting);
    }
}
