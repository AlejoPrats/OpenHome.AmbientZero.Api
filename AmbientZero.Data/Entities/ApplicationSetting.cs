using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmbientZero.Data.Entities
{
    public class ApplicationSetting
    {
        public int Id { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }

        internal static void Map(EntityTypeBuilder<ApplicationSetting> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).ValueGeneratedOnAdd();
            entityBuilder.ToTable("applicationSetting");
        }
    }
}
