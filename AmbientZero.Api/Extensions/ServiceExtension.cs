using AmbientZero.Data.Entities;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Services;

namespace AmbientZero.Api.Extensions
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IApplicationSettingsService, ApplicationSettingsService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IDeviceInformationService, DeviceInformatonService>();
            services.AddTransient<IMeasurmentService, MeasurmentService>();
            services.AddTransient<ISensorService, SensorService>();
            services.AddTransient<IAdvancedSettingsService, AdvancedSettingsService>();
            services.AddTransient<IBackupService, BackupService>();
        }
    }
}
