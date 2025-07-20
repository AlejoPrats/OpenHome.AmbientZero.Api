using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Services;

namespace AmbientZero.Api.Extensions
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IMeasurmentService, MeasurmentService>();
            services.AddTransient<ISensorService, SensorService>();
            services.AddTransient<IDeviceInformationService, DeviceInformatonService>();
            services.AddTransient<IApplicationSettingsService, ApplicationSettingsService>();
        }
    }
}
