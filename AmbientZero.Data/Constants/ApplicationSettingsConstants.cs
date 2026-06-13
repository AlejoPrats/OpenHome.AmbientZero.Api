using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Constants
{
    public class ApplicationSettingsConstants
    {
        public const string TimeZone = "TimeZone";
        internal const string TimeZoneValue = "UTC";

        public const string TemperatureSetting = "TemperatureSetting";
        internal const string TemperatureSettingValue = "1";

        public const string ApplicationSecurity = "ApplicationSecurity";
        internal const string ApplicationSecurityValue = "1";

        public const string IsFirstRun = "IsFirstRun";
        internal const string IsFirstRunValue = "1";

        public const string AccessPointName = "AccessPointName";
        internal const string AccessPointNameValue = "AmbientZero";

        public const string AccessPointSecurity = "AccessPointSecurity";
        internal const string AccessPointSecurityValue = "0";

        public const string AccessPointPassword = "AccessPointPassword";
        internal const string? AccessPointPasswordValue = null;

        public const string CheckForUpdatesEnabled = "CheckForUpdatesEnabled";
        internal const string CheckForUpdatesEnabledValue = "1";

        public const string AutomaticUpdatesEnabled = "AutomaticUpdatesEnabled";
        internal const string AutomaticUpdatesEnabledValue = "0";

        public const string MaxBatteryAdcReading = "MaxBatteryAdcReading";
        internal const string MaxBatteryAdcReadingValue = "36024";

        public const string MinBatteryAdcReading = "MinBatteryAdcReading";
        internal const string MinBatteryAdcReadingValue = "14915";

        public static string[] AdvancedSettings = [
            ApplicationSecurity,
            AccessPointName,
            AccessPointSecurity,
            CheckForUpdatesEnabled,
            AutomaticUpdatesEnabled,
            MaxBatteryAdcReading,
            MinBatteryAdcReading
            ];

    }
}
