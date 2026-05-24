using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmbientZero.Data.Entities
{
    public class MeasurementLog
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public float Temperature { get; set; }
        public DateTime Date { get; set; }
        public DeviceInformation DeviceInformation { get; set; }

        internal static void Map(EntityTypeBuilder<MeasurementLog> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).ValueGeneratedOnAdd();
            entityBuilder.HasOne(x => x.DeviceInformation)
                .WithMany(b => b.Measurements)
                .HasForeignKey(x => x.DeviceId)
                .HasPrincipalKey(b => b.DeviceId);
            entityBuilder.ToTable("measurementLog");
        }
    }
}
