using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Constants
{
    public class ApplicationSettingsConstants
    {
        public const string AdminUsername = "Admin";

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

        public const string RootProjectURL = "RootProjectURL";
        internal const string RootProjectURLValue = "https://openhomeproject.com";

        public const string VersionUpdateEndpoint = "UpdateEndpoint";
        internal const string VersionUpdateEndpointValue = "api/Version/GetVersionsByProductId?id=";

        public const string RemoteDeviceId = "RemoteDeviceId";
        internal const string RemoteDeviceIdValue = "419a522b-8c31-4b44-b9a8-980cb29f1492";

        public const string IsFirstRunSetup = "IsFirstRunSetup";
        public const string IsFirstRunSetupValue = "1";

        public const string DefaultLanguage = "DefaultLanguage";
        public const string DefaultLanguageValue = "en";

        public const string DefaultUnit = "DefaultUnit";
        public const string DefaultUnitValue = "1";

        public const string BootTime = "BootTime";
        public const string BootTimeDefaultValue = "0";

        public const string SshPassword = "SshPassword";
        public const string SshPasswordValue = "devpass";

        public static readonly string[] AdvancedSettings = [
            ApplicationSecurity,
            AccessPointName,
            AccessPointSecurity,
            CheckForUpdatesEnabled,
            AutomaticUpdatesEnabled,
            MaxBatteryAdcReading,
            MinBatteryAdcReading,
            SshPassword
            ];

        public static readonly string[] BatterySettings = [
            MaxBatteryAdcReading,
            MinBatteryAdcReading
            ];

        public static readonly string[] VersionUpdateSettings = [
            RootProjectURL,
            VersionUpdateEndpoint,
            RemoteDeviceId
            ];

    }
}
