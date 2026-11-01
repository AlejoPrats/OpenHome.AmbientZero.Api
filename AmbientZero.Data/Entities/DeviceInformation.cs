using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmbientZero.Data.Entities
{
    public class DeviceInformation
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public string? DeviceVirtualName { get; set; }
        public int? ADCReading { get; set; }
        public bool? IsSignaling { get; set; }
        public ICollection<MeasurementLog> Measurements { get; set; }
        public SensorSetting? Settings { get; set; }
        public string? Version { get; set; }
        public bool IsMigrated { get; set; }

        internal static void Map(EntityTypeBuilder<DeviceInformation> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).ValueGeneratedOnAdd();
            entityBuilder.HasOne(x => x.Settings)
                .WithOne(b => b.Sensor)
                .HasForeignKey<DeviceInformation>(x => x.DeviceId)
                .HasPrincipalKey<SensorSetting>(x => x.DeviceId);
            entityBuilder.Property(x => x.IsMigrated).HasDefaultValue(false);
            entityBuilder.ToTable("deviceInformation");
        }
    }
}
