using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Backup.ScriptGenerators
{
    public class MeasurementLogQueryGenerator
    {
        private readonly StringBuilder _sb;
        private const string _tableName = "measurementLog";
        private static string[] _columns = ["DeviceId", "Temperature", "Date"];
        private const string _sectionSeparator = "-- SECTION: {0} PAGE {1} {2}";
        private const string _queryType = "MEASUREMENT";
        private int _totalRows;
        private int _page;
        private int _activeRow = 1;

        private static string EscapeSingleQuotes(string value) => value.Replace("'", "''");

        public MeasurementLogQueryGenerator()
        {
            _sb = new StringBuilder();
        }

        public void CreateHeader(int totalRows, int page)
        {
            _page = page;
            _totalRows = totalRows;
            _sb.AppendLine(string.Format(_sectionSeparator, _queryType, _page + 1, "START"));
            _sb.AppendLine($"INSERT INTO {_tableName} ({string.Join(',', _columns)}) VALUES");
        }

        public void Append(string deviceId, float temperatrue, DateTime date)
        {
            _sb.Append($"('{deviceId}',{temperatrue},'{date.ToString("yyyy-MM-dd HH:mm:ss.FFFFFF")}')");

            if (_activeRow == _totalRows)
            {
                _sb.AppendLine(";");
                return;
            }
            else
            {
                _sb.AppendLine(",");

            }

            _activeRow++;
        }

        public string FinishSqlScript()
        {
            _sb.AppendLine(string.Format(_sectionSeparator, _queryType, _page + 1, "END"));
            var sql = _sb.ToString();
            _sb.Clear();
            _activeRow = 1;
            return sql;
        }
    }
}
