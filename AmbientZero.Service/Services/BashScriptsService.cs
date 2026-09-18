using AmbientZero.Data.Constants;
using AmbientZero.Data.Interfaces;
using AmbientZero.Data.Transactions;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Request;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AmbientZero.Service.Services
{
    public class BashScriptsService : IBashScriptsService
    {
        private readonly IApplicationSettingRepository _applicationSettingRepository;
        private readonly ITransactionScope _transactionScope;

        public BashScriptsService(IApplicationSettingRepository applicationSettingRepository,
            ITransactionScope transactionScope)
        {
            _applicationSettingRepository = applicationSettingRepository ??
                                            throw new ArgumentNullException(nameof(applicationSettingRepository));
            _transactionScope = transactionScope ?? throw new ArgumentNullException(nameof(transactionScope));
        }

        public async Task ConfigureAccessPoint(UpdateAccessPointRequest request)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "/usr/bin/sudo",
                ArgumentList =
                {
                    "/opt/api/Scripts/update_hostapd.sh",
                    request.Ssid,
                    request.Channel.ToString(),
                    request.Country,
                    request.Password,
                    request.ApSecurity.ToString()
                },
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            string stdout = await process.StandardOutput.ReadToEndAsync();
            string stderr = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (process.ExitCode == 0)
            {
                _transactionScope.Execute(() =>
                {
                    _applicationSettingRepository.UpdateApplicationSetting(ApplicationSettingsConstants.AccessPointName,
                        request.Ssid);
                    if (request.ApSecurity is not 0)
                    {
                        _applicationSettingRepository.UpdateApplicationSetting(
                            ApplicationSettingsConstants.AccessPointPassword, request.Password);
                    }

                    _applicationSettingRepository.UpdateApplicationSetting(
                        ApplicationSettingsConstants.AccessPointSecurity, request.ApSecurity.ToString());
                });
            }
        }

        public async void UpdateApi()
        {
            var psi = new ProcessStartInfo
            {
                FileName = "/usr/bin/sudo",
                ArgumentList =
                {
                    "/opt/api/Scripts/update_api.sh"
                },
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            string stdout = await process.StandardOutput.ReadToEndAsync();
            string stderr = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
        }


        public async void UpdateWeb()
        {
            var psi = new ProcessStartInfo
            {
                FileName = "/usr/bin/sudo",
                ArgumentList =
                {
                    "/opt/api/Scripts/update_web.sh"
                },
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            string stdout = await process.StandardOutput.ReadToEndAsync();
            string stderr = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
        }

        public async Task UpdateModule(string script, string remoteUrl, bool hasFile = false, int seconds = 10)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "/usr/bin/sudo",
                ArgumentList =
                {
                    $"/opt/api/Scripts/{script}",
                    remoteUrl,
                    hasFile.ToString(),
                    seconds.ToString()
                },
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            string stdout = await process.StandardOutput.ReadToEndAsync();
            string stderr = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
        }

        public async Task ScheduleReboot(long seconds)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "/usr/bin/sudo",
                ArgumentList =
                {
                    "/opt/api/Scripts/reboot.sh",
                    seconds.ToString()
                },
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            await process.WaitForExitAsync();
        }
    }
}