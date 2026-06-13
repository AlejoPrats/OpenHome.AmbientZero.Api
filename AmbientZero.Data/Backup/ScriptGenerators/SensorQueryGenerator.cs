using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Backup.ScriptGenerators
{
    public class SensorQueryGenerator
    {
        private readonly StringBuilder _sb;
        private const string _tableName = "deviceInformation";
        private static string[] _columns = ["DeviceId", "DeviceVirtualName", "IsMigrated", "Version"];
        private const string _sectionSeparator = "-- SECTION: {0} {1}";
        private const string _queryType = "SENSOR";
        private readonly int _totalRows;
        private int activeRow = 1;
        private static string EscapeSingleQuotes(string value) => value.Replace("'", "''");

        public SensorQueryGenerator(int count)
        {
            _totalRows = count;
            _sb = new StringBuilder();
            _sb.AppendLine(string.Format(_sectionSeparator, _queryType, "START"));
            _sb.AppendLine($"INSERT INTO {_tableName} ({string.Join(',', _columns)}) VALUES");
        }

        public void Append(string deviceId, string? deviceVirtualName, string version)
        {
            var parsedDeviceVirtualName = string.IsNullOrEmpty(deviceVirtualName) ? "NULL" : $"{EscapeSingleQuotes(deviceVirtualName)}";
            _sb.Append($"('{deviceId}','{parsedDeviceVirtualName}',1,'{version}')");

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
