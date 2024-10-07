using AmbientZero.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmbientZero.Data.Mappings
{
    internal class DeviceInformationMap
    {
        public static void Map(EntityTypeBuilder<DeviceInformation> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).ValueGeneratedOnAdd();
            entityBuilder.HasOne(x => x.Settings)
                .WithOne(b => b.Sensor)
                .HasForeignKey<DeviceInformation>(x => x.DeviceId)
                .HasPrincipalKey<SensorSetting>(x => x.DeviceId);
            entityBuilder.ToTable("deviceInformation");
        }
    }
}
