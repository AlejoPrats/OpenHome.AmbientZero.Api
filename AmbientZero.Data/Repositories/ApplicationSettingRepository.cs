using AmbientZero.Data.Entities;
using AmbientZero.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Repositories
{
    public class ApplicationSettingRepository : IApplicationSettingRepository
    {
        private readonly AmbientZeroContext _ambientZeroContext;
        public ApplicationSettingRepository(AmbientZeroContext ambientZeroContext)
        {
            _ambientZeroContext = ambientZeroContext;
        }

        public IEnumerable<ApplicationSetting> GetApplicationSettings()
        {
            return _ambientZeroContext.ApplicationSettings.ToList();
        }

        public void AddApplicationSetting(ApplicationSetting applicationSettings)
        {
            _ambientZeroContext.Add(applicationSettings);
            _ambientZeroContext.SaveChanges();
        }

        public void UpdateApplicationSetting(ApplicationSetting applicationSetting)
        {
            _ambientZeroContext.ApplicationSettings.Where(x => x.Id == applicationSetting.Id)
                .ExecuteUpdate(x =>
                x.SetProperty(s => s.SettingValue, applicationSetting.SettingValue));
        }
    }
}
