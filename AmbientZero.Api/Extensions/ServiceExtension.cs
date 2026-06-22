using AmbientZero.Data.Entities;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Modules;
using AmbientZero.Service.Services;

namespace AmbientZero.Api.Extensions
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IAdvancedSettingsService, AdvancedSettingsService>();
            services.AddTransient<IApplicationSettingsService, ApplicationSettingsService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IBackupService, BackupService>();
            services.AddTransient<IDeviceInformationService, DeviceInformatonService>();
            services.AddTransient<IMeasurmentService, MeasurmentService>();
            services.AddTransient<ISensorService, SensorService>();
            services.AddTransient<IVersioningService, VersioningService>();

            ///////////////
            /// Modules ///
            ///////////////
            
            services.AddSingleton<IStartupModule, StartupModule>();

        }
    }
}
