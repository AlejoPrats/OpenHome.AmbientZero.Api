using AmbientZero.Data.Constants;
using AmbientZero.Data.Entities;
using AmbientZero.Data.Interfaces;
using AmbientZero.Service.Constants;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models;
using AmbientZero.Service.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AmbientZero.Service.Services
{
    public class SystemService : ISystemService
    {
        private readonly IMeasurmentLogRepository _measurmentLogRepository;
        private readonly IApplicationSettingRepository _advancedSettingsService;
        private readonly IBashScriptsService _bashScriptsService;

        public SystemService(IMeasurmentLogRepository measurmentLogRepository,
            IApplicationSettingRepository advancedSettingsService,
            IBashScriptsService bashScriptsService)
        {
            _measurmentLogRepository = measurmentLogRepository ?? throw new ArgumentNullException(nameof(measurmentLogRepository));
            _advancedSettingsService = advancedSettingsService ?? throw new ArgumentNullException(nameof(advancedSettingsService));
            _bashScriptsService = bashScriptsService ?? throw new ArgumentNullException(nameof(bashScriptsService));
        }

        public async Task ScheduleReboot()
        {
            var now = DateTime.UtcNow;
            var bootTime = TimeSpan.FromSeconds(int.Parse(_advancedSettingsService.GetApplicationSettingByKey(ApplicationSettingsConstants.BootTime).SettingValue!));
            var sensorLastReadings = _measurmentLogRepository.GetLastReadingsFromAllSensors(DateTime.UtcNow.AddMinutes(-25)).Select(x => x.Date).ToList();

            var buffer = TimeSpan.FromMinutes(2);
            var extraShift = TimeSpan.FromMinutes(5);
            var ticksResult = CalculateSafeRebootTicks(sensorLastReadings, TimeSpan.FromMinutes(20), bootTime, buffer);
            await _bashScriptsService.ScheduleReboot(TimeSpan.FromTicks(ticksResult).Seconds);
        }

        private long CalculateSafeRebootTicks(List<DateTime> sensorLastReadingsUtc, TimeSpan interval, TimeSpan bootTime, TimeSpan buffer)
        {
            long now = DateTime.UtcNow.Ticks;

            long intervalTicks = interval.Ticks;
            long bootTicks = bootTime.Ticks;
            long bufferTicks = buffer.Ticks;

            var nextReadings = sensorLastReadingsUtc
                .Select(r => r.Ticks + intervalTicks)
                .OrderBy(t => t)
                .ToList();

            if (nextReadings.Count == 0)
            {
                return now + bufferTicks;
            }

            long earliestNext = nextReadings.First();
            long safeStart = earliestNext - bootTicks - bufferTicks;

            if (safeStart > now)
            {
                return safeStart;
            }

            long candidate = now + bufferTicks + TimeSpan.FromMinutes(5).Ticks;

            foreach (var nextReading in nextReadings)
            {
                if (candidate + bootTicks >= nextReading)
                {
                    candidate = nextReading + bufferTicks;
                }
            }

            return candidate;
        }

        public async Task<DeviceDescription> GetDeviceInformation()
        {
            var deviceDescription = new DeviceDescription();

            deviceDescription.DeviceId = new Guid(_advancedSettingsService.GetApplicationSettingByKey(ApplicationSettingsConstants.RemoteDeviceId).SettingValue!);
            deviceDescription.DeviceName = _advancedSettingsService.GetApplicationSettingByKey(ApplicationSettingsConstants.DeviceName).SettingValue!;
            string deviceNetworkInformationJson = await _bashScriptsService.GetNetworkAdapterStatus("wlan1");
            deviceDescription.NetworkInformation = JsonSerializer.Deserialize<AddressObject>(deviceNetworkInformationJson)!;

            return deviceDescription;
        }

        public async Task<int> FreeRam(FreeRamMode freeRamMode)
        {
            if (freeRamMode == FreeRamMode.Cache)
            {
                await _bashScriptsService.FreeRam(freeRamMode);
                return 0;
            }
            else
            {
                List<DateTime> sensorLastReadings = _measurmentLogRepository.GetLastReadingsFromAllSensors(DateTime.UtcNow.AddMinutes(-25)).Select(x => x.Date).ToList();
                var extraShift = TimeSpan.FromMinutes(5);
                long ticksResult = CalculateSafeRebootTicks(sensorLastReadings, TimeSpan.FromMinutes(20), TimeSpan.Zero, TimeSpan.Zero);
                await _bashScriptsService.FreeRam(freeRamMode,ticksResult);
                return TimeSpan.FromTicks(ticksResult).Seconds;
            }
        }
    }
}
