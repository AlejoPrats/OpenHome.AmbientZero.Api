using AmbientZero.Data.Backup.Models;
using AmbientZero.Data.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Backup
{
    /// <summary>
    /// <para>
    /// Handles the full lifecycle of a backup file during the import process.
    /// This includes saving the uploaded file, opening a streaming reader,
    /// extracting metadata, reading SQL blocks sequentially, and finally
    /// cleaning up the temporary file.
    /// </para>
    /// <para>
    /// The class is designed for low‑memory environments (like the Pi Zero),
    /// ensuring that the backup file is processed in a streaming fashion
    /// without loading the entire file into RAM.
    /// </para>
    /// </summary>
    public class BackupFileProcessing :
        BackupFileProcessing.IOpenStage,
        BackupFileProcessing.IReadStage
    {
        public bool HasMoreBlocks => !_reader.EndOfStream;
        private StreamReader _reader;
        private readonly string _fileLocation;
        private readonly StringBuilder _queryStringBuilder = new StringBuilder();
        private ImportMetadata ImportMetadata = new ImportMetadata();
        public ImportMetadata Metadata => ImportMetadata;

        #region Fluent Api Interfaces

        private BackupFileProcessing(string fileLocation)
        {
            _fileLocation = fileLocation;
        }

        public interface IOpenStage
        {
            /// <summary>
            /// <para>
            /// Opens a StreamReader for the backup file and immediately extracts metadata
            /// from the beginning of the file. Metadata is read until the first empty line.
            /// </para>
            /// </summary>
            IReadStage Open();
        }

        public interface IReadStage
        {
            /// <summary>
            /// <para>
            /// Reads the next SQL block from the backup file. Lines starting with "--"
            /// are ignored as comments. Reading stops at the first empty line.
            /// </para>
            /// <para>
            /// Returns the accumulated SQL block as a string, or an empty string if no
            /// more content is available.
            /// </para>
            /// </summary>
            string? ReadNextBlock();
            /// <summary>
            /// <para>
            /// Closes the StreamReader and deletes the temporary backup file from disk.
            /// Should be called after the import process completes.
            /// </para>
            /// </summary>
            void CloseAndDelete();
            /// <summary>
            /// Indicates whether there is more SQL content to read from the backup file.
            /// </summary>
            bool HasMoreBlocks { get; }
            ImportMetadata Metadata { get; }
        }
        #endregion

        public static IOpenStage SetFileLocation(string fileLocation) => new BackupFileProcessing(fileLocation);

        /// <summary>
        /// <para>
        /// Initializes the processor with the directory and file name where the
        /// uploaded backup will be stored.
        /// </para>
        /// </summary>

        public IReadStage Open()
        {
            _reader = new StreamReader(_fileLocation);
            GetMetadata();
            return this;
        }

        public string? ReadNextBlock()
        {
            while (true)
            {
                var line = _reader.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }
                else if (line.StartsWith("--"))
                {
                    continue;
                }
                _queryStringBuilder.Append(line);
            }

            var sql = _queryStringBuilder.ToString();
            _queryStringBuilder.Clear();
            return sql;
        }

        public void CloseAndDelete()
        {
            _reader.Close();
            File.Delete(_fileLocation);
        }

        private void GetMetadata()
        {
            while (true)
            {
                var line = _reader.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                switch (line)
                {
                    case var _ when line.StartsWith(BackupMetadataConstants.CreatedUtc, StringComparison.Ordinal):
                        ImportMetadata.CreatedOn = DateTime.Parse(line.Substring(BackupMetadataConstants.CreatedUtcLength));
                        break;
                    case var _ when line.StartsWith(BackupMetadataConstants.SensorSettingsExported, StringComparison.Ordinal):
                        ImportMetadata.SensorSettingsExported = int.Parse(line.Substring(BackupMetadataConstants.SensorSettingsExportedLength));
                        break;
                    case var _ when line.StartsWith(BackupMetadataConstants.SensorsExported, StringComparison.Ordinal):
                        ImportMetadata.SensorsExported = int.Parse(line.Substring(BackupMetadataConstants.SensorsExportedLength));
                        break;
                    case var _ when line.StartsWith(BackupMetadataConstants.TemperatureLogsExported, StringComparison.Ordinal):
                        ImportMetadata.TemperatureLogsExported = int.Parse(line.Substring(BackupMetadataConstants.TemperatureLogsExportedLength));
                        break;
                    case var _ when line.StartsWith(BackupMetadataConstants.TemperatureLogsPages):
                        ImportMetadata.TemperatureLogsPages = int.Parse(line.Substring(BackupMetadataConstants.TemperatureLogsPagesLength));
                        break;
                }
            }


        }
    }
}