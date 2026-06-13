using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Backup.ScriptGenerators
{
    public class MigrationReferenceQueryGenerator
    {
        private readonly StringBuilder _sb;
        private const string _tableName = "migrationReferenceId";
        private static string[] _columns = ["OriginalId", "NewId"];
        private const string _sectionSeparator = "-- SECTION: {0} {1}";
        private const string _queryType = "MIGRATION REFERENCE";
        private readonly int _totalRows;
        private int activeRow = 1;

        public MigrationReferenceQueryGenerator(int count)
        {
            _totalRows = count;
            _sb = new StringBuilder();
            _sb.AppendLine(string.Format(_sectionSeparator, _queryType, "START"));
            _sb.AppendLine($"INSERT INTO {_tableName} ({string.Join(',', _columns)}) VALUES");
        }

        public void Append(string originalId, string newId)
        {
            _sb.Append($"('{originalId}','{newId}')");

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
