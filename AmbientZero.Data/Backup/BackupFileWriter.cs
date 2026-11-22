using AmbientZero.Data.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Backup
{
    public class BackupFileWriter
    {
        private readonly StreamWriter _writer;
        private readonly string _fileLocation;
        private bool _isDisposed = false;
        public Stream BackupStream => GetStream();

        public BackupFileWriter(string filePath, string fileName)
        {
            _fileLocation = Path.Combine(filePath, fileName);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            _writer = new StreamWriter(_fileLocation, false, Encoding.UTF8);
        }

        public void WriteMetadata(DateTime createdUtc, int sensorCount, int measurementsCount, int measurementPages)
        {
            _writer.WriteLine($"{BackupMetadataConstants.CreatedUtc} {createdUtc:O}");
            _writer.WriteLine($"{BackupMetadataConstants.SensorsExported} {sensorCount}");
            _writer.WriteLine($"{BackupMetadataConstants.SensorSettingsExported} {sensorCount}");
            _writer.WriteLine($"{BackupMetadataConstants.TemperatureLogsExported} {measurementsCount}");
            _writer.WriteLine($"{BackupMetadataConstants.TemperatureLogsPages} {measurementPages}");
            _writer.WriteLine();
        }

        public void WriteSection(string sqlBlock)
        {
            _writer.Write(sqlBlock);
            _writer.WriteLine();
        }

        public void Dispose()
        {
            _writer.Flush();
            _writer.Dispose();
            _isDisposed = true;
        }

        private Stream GetStream()
        {
            if (_isDisposed)
            {
                return new FileStream(_fileLocation, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            else
            {
                return _writer.BaseStream;
            }
        }
    }
}
