using AmbientZero.Data.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace AmbientZero.Data.Entities
{
    public class ApplicationSetting
    {
        public int Id { get; set; }
        public string SettingName { get; set; }
        public string? SettingValue { get; set; }

        internal static void Map(EntityTypeBuilder<ApplicationSetting> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).ValueGeneratedOnAdd();
            entityBuilder.ToTable("applicationSetting");
        }

        internal static void Seed(EntityTypeBuilder<ApplicationSetting> entityBuilder)
        {
            entityBuilder.HasData(
                new ApplicationSetting { Id = 1, SettingName = ApplicationSettingsConstants.TimeZone, SettingValue = ApplicationSettingsConstants.TimeZoneValue },
                new ApplicationSetting { Id = 2, SettingName = ApplicationSettingsConstants.TemperatureSetting, SettingValue = ApplicationSettingsConstants.TemperatureSettingValue },
                new ApplicationSetting { Id = 3, SettingName = ApplicationSettingsConstants.ApplicationSecurity, SettingValue = ApplicationSettingsConstants.ApplicationSecurityValue },
                new ApplicationSetting { Id = 4, SettingName = ApplicationSettingsConstants.IsFirstRun, SettingValue = ApplicationSettingsConstants.IsFirstRunValue },
                new ApplicationSetting { Id = 5, SettingName = ApplicationSettingsConstants.AccessPointName, SettingValue = ApplicationSettingsConstants.AccessPointNameValue },
                new ApplicationSetting { Id = 6, SettingName = ApplicationSettingsConstants.AccessPointSecurity, SettingValue = ApplicationSettingsConstants.AccessPointSecurityValue },
                new ApplicationSetting { Id = 7, SettingName = ApplicationSettingsConstants.AccessPointPassword, SettingValue = ApplicationSettingsConstants.AccessPointPasswordValue },
                new ApplicationSetting { Id = 8, SettingName = ApplicationSettingsConstants.CheckForUpdatesEnabled, SettingValue = ApplicationSettingsConstants.CheckForUpdatesEnabledValue },
                new ApplicationSetting { Id = 9, SettingName = ApplicationSettingsConstants.AutomaticUpdatesEnabled, SettingValue = ApplicationSettingsConstants.AutomaticUpdatesEnabledValue },
                new ApplicationSetting { Id = 10, SettingName = ApplicationSettingsConstants.MaxBatteryAdcReading, SettingValue = ApplicationSettingsConstants.MaxBatteryAdcReadingValue },
                new ApplicationSetting { Id = 11, SettingName = ApplicationSettingsConstants.MinBatteryAdcReading, SettingValue = ApplicationSettingsConstants.MinBatteryAdcReadingValue }
                );
        }
    }
}
