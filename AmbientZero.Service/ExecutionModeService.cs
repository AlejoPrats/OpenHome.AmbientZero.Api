using AmbientZero.Data;
using AmbientZero.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service
{
    public class ExecutionModeService : IExecutionModeService
    {
        public void GenerateBackup(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var backupService = scope.ServiceProvider.GetRequiredService<IBackupService>();
            backupService.GenerateBackupFile();
        }

        public void ImportBackup(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var backupService = scope.ServiceProvider.GetRequiredService<IBackupService>();
            backupService.ImportBackupFile();
        }
    }
}
