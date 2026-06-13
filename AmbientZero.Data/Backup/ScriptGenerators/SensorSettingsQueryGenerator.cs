using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Backup.ScriptGenerators
{
    public class SensorSettingsQueryGenerator
    {
        private readonly StringBuilder _sb;
        private const string _tableName = "sensorSetting";
        private static string[] _columns = ["DeviceId", "IsLightEnabled", "IsLightOnSchedule", "Color", "DisableStartTime", "DisableEndTime"];
        private const string _sectionSeparator = "-- SECTION: {0} {1}";
        private const string _queryType = "SENSOR SETTINGS";
        private readonly int _totalRows;
        private int activeRow = 1;
        private static string EscapeSingleQuotes(string value) => value.Replace("'", "''");

        public SensorSettingsQueryGenerator(int count)
        {
            _totalRows = count;
            _sb = new StringBuilder();
            _sb.AppendLine(string.Format(_sectionSeparator, _queryType, "START"));
            _sb.AppendLine($"INSERT INTO {_tableName} ({string.Join(',', _columns)}) VALUES");
        }

        public void Append(string deviceId, TimeOnly? disableEndTime, TimeOnly? disableStartTime, string? color, bool isLightEnabled, bool isScheduled)
        {
            var parsedIsLightEnabled = isLightEnabled ? 1 : 0;
            var parsedIsScheduled = isScheduled ? 1 : 0;
            var colorParsed = string.IsNullOrEmpty(color) ? "NULL" : $"'{color}'";
            var parsedDisableEndTime = disableEndTime.HasValue ? disableEndTime.Value.ToString("HH:mm:ss") : "NULL";
            var parsedDisableStartTime = disableStartTime.HasValue ? disableStartTime.Value.ToString("HH:mm:ss") : "NULL";
            
            _sb.Append($"('{deviceId}',{parsedIsLightEnabled},{parsedIsScheduled},{colorParsed},'{disableStartTime}','{disableEndTime}')");

            if (activeRow == _totalRows)
            {
                _sb.AppendLine(";");
                return;
            }
            else
            {
                _sb.AppendLine(",");
            }

            activeRow++;
        }

        public string FinishSqlScript()
        {
            _sb.AppendLine(string.Format(_sectionSeparator, _queryType, "END"));
            var sql = _sb.ToString();
            _sb.Clear();
            return sql;
        }
    }
}
