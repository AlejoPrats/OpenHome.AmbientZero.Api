using AmbientZero.Data.Interfaces;
using AmbientZero.Data.Repositories;

namespace AmbientZero.Api.Extensions
{
    public static class RepositoryExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IMeasurmentLogRepository, MeasurmentLogRepository>();
            services.AddScoped<IDeviceInformationRepository, DeviceInformationRepository>();
            services.AddScoped<ISensorSettingRepository, SensorSettingRepository>();
            services.AddScoped<IApplicationSettingRepository, ApplicationSettingRepository>();
        }
    }
}
