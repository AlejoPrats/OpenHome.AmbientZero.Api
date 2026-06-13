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
        BackupFileResponse? GenerateBackupFile(string[] deviceIds);
        BackupImportResponse ImportBackup(string name, Stream file);
    }
}
