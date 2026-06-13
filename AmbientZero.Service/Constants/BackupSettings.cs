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
        public static string FileName = $"AmbientZero_Backup_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm")}{FileExtension}";
        public const string FileExtension = ".sql";
        public static string UploadBackupPath = Path.Combine(AppContext.BaseDirectory, "Data", "UploadedBackup");
        public static string GeneratedBackupPath = Path.Combine(AppContext.BaseDirectory, "Data", "GeneratedBackup");
    }
}
