using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Constants
{
    public class BackupMetadataConstants
    {
        public const string CreatedUtc = "-- CREATED_UTC:";
        public static int CreatedUtcLength = CreatedUtc.Length;
        public const string SensorsExported = "-- SENSORS EXPORTED:";
        public static int SensorsExportedLength = SensorsExported.Length;
        public const string SensorSettingsExported = "-- SENSOR SETTINGS EXPORTED:";
        public static int SensorSettingsExportedLength = SensorSettingsExported.Length;
        public const string TemperatureLogsExported = "-- TEMPERATURE LOGS EXPORTED:";
        public static int TemperatureLogsExportedLength = TemperatureLogsExported.Length;
        public const string TemperatureLogsPages = "-- TEMPERATURE LOGS PAGES:";
        public static int TemperatureLogsPagesLength = TemperatureLogsPages.Length;
    }
}
