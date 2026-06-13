using AmbientZero.Data.Interfaces;
using AmbientZero.Data.Repositories;
using AmbientZero.Data.Transactions;

namespace AmbientZero.Api.Extensions
{
    public static class RepositoryExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IApplicationSettingRepository, ApplicationSettingRepository>();
            services.AddScoped<IBackupRepository, BackupRepository>();
            services.AddScoped<IDeviceInformationRepository, DeviceInformationRepository>();
            services.AddScoped<IMeasurmentLogRepository, MeasurmentLogRepository>();
            services.AddScoped<ISensorSettingRepository, SensorSettingRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            //////////////////
            /// Repo Tools ///
            //////////////////
            
            services.AddScoped<ITransactionScope, TransactionScope>();
        }
    }
}
