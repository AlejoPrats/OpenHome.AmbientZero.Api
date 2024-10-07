using AmbientZero.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmbientZero.Data.Mappings
{
    internal class SensorSettingMap
    {
        public static void Map(EntityTypeBuilder<SensorSetting> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).ValueGeneratedOnAdd();
            entityBuilder.HasOne(x => x.Sensor).WithOne(x => x.Settings);
            entityBuilder.ToTable("sensorSetting");
        }
    }
}
