using AmbientZero.Service.Models.Response;
using Microsoft.AspNetCore.Http;
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
        Task<bool> BackupFileUpload(IFormFile file);
        void ImportBackup();
        BackupImportResponse ImportBackupFile();
    }
}
