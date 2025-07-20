using AmbientZero.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmbientZero.Data.Mappings
{
    public static class ApplicationSettingMap
    {
        public static void Map(EntityTypeBuilder<ApplicationSetting> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).ValueGeneratedOnAdd();
            entityBuilder.ToTable("applicationSetting");
        }
    }
}
