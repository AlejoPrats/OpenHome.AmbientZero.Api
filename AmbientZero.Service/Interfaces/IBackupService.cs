using AmbientZero.Service.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Interfaces
{
    public interface IBackupService
    {
        void GenerateBackup();
        BackupFileResponse? GetFileStream();
        bool GenerateBackupFile(string[]? deviceIds = null);
        bool BackupFileUpload(Stream file);
        BackupImportResponse ImportBackupFile();
    }
}
