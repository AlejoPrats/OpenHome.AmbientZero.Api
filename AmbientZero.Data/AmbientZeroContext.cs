using AmbientZero.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AmbientZero.Data
{
    public class AmbientZeroContext : DbContext
    {
        public DbSet<DeviceInformation> Devices { get; set; }
        public DbSet<MeasurementLog> Measurements { get; set; }
        public DbSet<SensorSetting> SensorSettings { get; set; }
        public DbSet<ApplicationSetting> ApplicationSettings { get; set; }
        public AmbientZeroContext(DbContextOptions<AmbientZeroContext> options) : base(options)
        {
            if (Database.GetPendingMigrations().Any())
            {
                Database.Migrate();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DeviceInformation.Map(modelBuilder.Entity<DeviceInformation>());
            MeasurementLog.Map(modelBuilder.Entity<MeasurementLog>());
            SensorSetting.Map(modelBuilder.Entity<SensorSetting>());
            ApplicationSetting.Map(modelBuilder.Entity<ApplicationSetting>());

            base.OnModelCreating(modelBuilder);
        }
    }
}