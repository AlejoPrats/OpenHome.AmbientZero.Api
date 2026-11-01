using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AmbientZero.Service.Constants
{
    internal static class BackupSettings
    {
        public static string BackupFileName = "AmbientZero_Backup";
        public static string GeneratedBackupFileName = $"{BackupFileName}{FileExtension}";
        public static string ImportedBackupName = "AmbientZero_Import";
        public static string ImportedBackupFileName = $"{ImportedBackupName}{FileExtension}";
        public const string FileExtension = ".sql";
        public static string UploadBackupPath = Path.Combine(AppContext.BaseDirectory, "Data", "Import");
        public static string GeneratedBackupPath = Path.Combine(AppContext.BaseDirectory, "Data", "Backup");
    }
}
