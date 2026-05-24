using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmbientZero.Data.Entities
{
    public class SensorSetting
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public bool IsLightEnabled { get; set; }
        public bool IsLightOnSchedule { get; set; }
        public TimeOnly? DisableStartTime { get; set; }
        public TimeOnly? DisableEndTime { get; set; }
        public DeviceInformation Sensor { get; set; }

        internal static void Map(EntityTypeBuilder<SensorSetting> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).ValueGeneratedOnAdd();
            entityBuilder.HasOne(x => x.Sensor).WithOne(x => x.Settings);
            entityBuilder.ToTable("sensorSetting");
        }
    }
}
