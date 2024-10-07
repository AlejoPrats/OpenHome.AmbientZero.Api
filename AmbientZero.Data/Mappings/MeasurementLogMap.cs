using AmbientZero.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmbientZero.Data.Mappings
{
    internal class MeasurementLogMap
    {
        public static void Map(EntityTypeBuilder<MeasurementLog> entityBuilder)
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
