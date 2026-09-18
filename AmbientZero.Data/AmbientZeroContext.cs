using AmbientZero.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AmbientZero.Data
{
    public class AmbientZeroContext : DbContext
    {
        public DbSet<ApplicationSetting> ApplicationSettings { get; set; }
        public DbSet<ApplicationVersion> ApplicationVersions { get; set; }
        public DbSet<DeviceInformation> Devices { get; set; }
        public DbSet<MeasurementLog> Measurements { get; set; }
        public DbSet<SensorSetting> SensorSettings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MigrationReference> MigrationReferences { get; set; }
        public AmbientZeroContext(DbContextOptions<AmbientZeroContext> options) : base(options){ }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ApplicationSetting.Map(modelBuilder.Entity<ApplicationSetting>());
            ApplicationVersion.Map(modelBuilder.Entity<ApplicationVersion>());
            DeviceInformation.Map(modelBuilder.Entity<DeviceInformation>());
            MeasurementLog.Map(modelBuilder.Entity<MeasurementLog>());
            MigrationReference.Map(modelBuilder.Entity<MigrationReference>());
            SensorSetting.Map(modelBuilder.Entity<SensorSetting>());
            User.Map(modelBuilder.Entity<User>());

            ApplicationSetting.Seed(modelBuilder.Entity<ApplicationSetting>());
            ApplicationVersion.Seed(modelBuilder.Entity<ApplicationVersion>());
            User.Seed(modelBuilder.Entity<User>());

            base.OnModelCreating(modelBuilder);
        }
    }
}