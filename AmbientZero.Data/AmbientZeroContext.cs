using AmbientZero.Data.Entities;
using AmbientZero.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace AmbientZero.Data
{
    public class AmbientZeroContext : DbContext
    {
        public DbSet<DeviceInformation> Devices { get; set; }
        public DbSet<MeasurementLog> Measurements { get; set; }
        public DbSet<SensorSetting> SensorSettings { get; set; }
        public AmbientZeroContext(DbContextOptions<AmbientZeroContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DeviceInformationMap.Map(modelBuilder.Entity<DeviceInformation>());
            MeasurementLogMap.Map(modelBuilder.Entity<MeasurementLog>());
            SensorSettingMap.Map(modelBuilder.Entity<SensorSetting>());

            base.OnModelCreating(modelBuilder);
        }
    }
}