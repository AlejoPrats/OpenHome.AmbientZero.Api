using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Services;
using Coravel.Invocable;
using System.Diagnostics;

namespace AmbientZero.Api.ScheduledTasks
{
    public class RebootJob : IInvocable
    {
        private readonly ISystemService _systemService;

        public RebootJob(ISystemService systemService)
        {
            _systemService = systemService ?? throw new ArgumentNullException(nameof(systemService));
        }

        public async Task Invoke()
        {
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = "/usr/bin/sudo",
                Arguments = "/opt/api/Scripts/reboot.sh",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(processStartInfo);
            await process.WaitForExitAsync();
        }
    }
}
