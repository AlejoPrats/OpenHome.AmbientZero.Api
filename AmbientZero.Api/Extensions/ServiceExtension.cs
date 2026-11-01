using AmbientZero.Data.Entities;
using AmbientZero.Service;
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
            services.AddTransient<IOnboardingService, OnboardingService>();
            services.AddTransient<ISecurityService, SecurityService>();
            services.AddTransient<ISensorService, SensorService>();
            services.AddTransient<ISystemService, SystemService>();
            services.AddTransient<IVersioningService, VersioningService>();
            services.AddTransient<IExecutionModeService, ExecutionModeService>();

            ////////////////////
            /// CrossCutting ///
            ////////////////////

            services.AddTransient<IBashScriptsService, BashScriptsService>();
            services.AddTransient<ITimeZoneService, TimeZoneService>();
            services.AddTransient(typeof(ITemporalJsonService<>),typeof(TemporalJsonService<>));

            ///////////////
            /// Modules ///
            ///////////////

            services.AddSingleton<IStartupModule, StartupModule>();

        }
    }
}
