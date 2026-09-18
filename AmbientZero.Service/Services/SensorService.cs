using AmbientZero.Data.Constants;
using AmbientZero.Data.Entities;
using AmbientZero.Data.Interfaces;
using AmbientZero.Service.Constants;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Request;
using AmbientZero.Service.Models.Result;
using AmbientZero.Service.Models.Options;
using Microsoft.Extensions.Options;

namespace AmbientZero.Service.Services
{
    public class SensorService : ISensorService
    {
        private readonly IMeasurmentLogRepository _measurmentRepository;
        private readonly IDeviceInformationRepository _deviceInformationRepository;
        private readonly ISensorSettingRepository _sensorSettingRepository;
        private readonly IApplicationSettingRepository _applicationSettingRepository;
        private readonly BatteryInfo _batteryInfo;

        public SensorService(IMeasurmentLogRepository measurmentLogRepository,
            IDeviceInformationRepository deviceInformationRepository,
            ISensorSettingRepository sensorSettingRepository,
            IApplicationSettingRepository applicationSettingRepository,
            IOptions<BatteryInfo> batteryInfo)
        {
            _measurmentRepository = measurmentLogRepository ?? throw new ArgumentNullException(nameof(measurmentLogRepository));
            _deviceInformationRepository = deviceInformationRepository ?? throw new ArgumentNullException(nameof(deviceInformationRepository));
            _sensorSettingRepository = sensorSettingRepository ?? throw new ArgumentNullException(nameof(sensorSettingRepository));
            _applicationSettingRepository = applicationSettingRepository ?? throw new ArgumentNullException(nameof(applicationSettingRepository));
            _batteryInfo = batteryInfo?.Value ?? throw new ArgumentNullException(nameof(batteryInfo));
        }

        public IEnumerable<SensorInformationResult> GetSensorInformation()
        {
            var applicationSettings = _applicationSettingRepository.GetApplicationSettings();

            string timeZoneId = applicationSettings.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.TimeZone)?.SettingValue ?? string.Empty;
            int temperatureFormat = int.Parse(applicationSettings.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.TemperatureSetting)?.SettingValue ?? "0");
            int minBatteryValue = int.Parse(applicationSettings.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.MinBatteryAdcReading)?.SettingValue ?? "1");
            int maxBatteryValue = int.Parse(applicationSettings.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.MaxBatteryAdcReading)?.SettingValue ?? "1");

            List<SensorInformationResult> sensors = _deviceInformationRepository
                .GetAllSensors()
                .Select(x => new SensorInformationResult()
                {
                    DeviceId = x.DeviceId,
                    RawADCReading = x.ADCReading,
                    BatteryPercentage = GetBatteryPercentage(x.ADCReading, minBatteryValue, maxBatteryValue),
                    DeviceVirtualName = x.DeviceVirtualName,
                    Id = x.Id,
                    IsSignaling = x.IsSignaling,
                    SensorSetting = new SensorSettingResult
                    {
                        DeviceId = x.Settings!.DeviceId,
                        DisableEndTime = x.Settings?.DisableEndTime,
                        DisableStartTime = x.Settings?.DisableStartTime,
                        IsLightEnabled = x.Settings?.IsLightEnabled ?? false,
                        IsScheduled = x.Settings?.IsLightOnSchedule ?? false
                    }
                }).ToList();


            foreach (var sensor in sensors)
            {
                var lastFiveResults = _measurmentRepository.GetLastFiveMeasurementsBySensor(sensor.DeviceId).OrderBy(x => x.Date);
                sensor.LastReadingTime = GetTime(lastFiveResults.Last().Date, timeZoneId);
                sensor.LastReadingValue = GetTemperature(lastFiveResults.Last().Temperature, temperatureFormat);

                var sensorTemperatureDiffernce = lastFiveResults.First().Temperature - lastFiveResults.Last().Temperature;

                if (sensorTemperatureDiffernce > 0)
                {
                    sensor.TemperatureTendency = "Descending";
                }
                else if (sensorTemperatureDiffernce < 0)
                {
                    sensor.TemperatureTendency = "Rising";
                }
            }

            return sensors;
        }

        public SensorInformationResult? GetSensorInformationById(int id)
        {
            var applicationSettings = _applicationSettingRepository.GetApplicationSettings();

            string timeZoneId = applicationSettings.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.TimeZone)?.SettingValue ?? string.Empty;
            int temperatureFormat = int.Parse(applicationSettings.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.TemperatureSetting)?.SettingValue ?? "0");
            int minBatteryValue = int.Parse(applicationSettings.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.MinBatteryAdcReading)?.SettingValue ?? "1");
            int maxBatteryValue = int.Parse(applicationSettings.FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.MaxBatteryAdcReading)?.SettingValue ?? "1");

            DeviceInformation? sensor = _deviceInformationRepository.GetSensorById(id);

            if (sensor == null)
            {
                return null;
            }

            var sensorResult = new SensorInformationResult()
            {
                DeviceId = sensor.DeviceId,
                RawADCReading = sensor.ADCReading,
                BatteryPercentage = GetBatteryPercentage(sensor.ADCReading, minBatteryValue, maxBatteryValue),
                DeviceVirtualName = sensor.DeviceVirtualName,
                Id = sensor.Id,
                IsSignaling = sensor.IsSignaling,
                SensorSetting = new SensorSettingResult
                {
                    DeviceId = sensor.Settings!.DeviceId,
                    DisableEndTime = sensor.Settings?.DisableEndTime,
                    DisableStartTime = sensor.Settings?.DisableStartTime,
                    IsLightEnabled = sensor.Settings?.IsLightEnabled ?? false,
                    IsScheduled = sensor.Settings?.IsLightOnSchedule ?? false
                }
            };

            var lastFiveResults = _measurmentRepository.GetLastFiveMeasurementsBySensor(sensor.DeviceId).OrderBy(x => x.Date);
            sensorResult.LastReadingTime = GetTime(lastFiveResults.Last().Date, timeZoneId);
            sensorResult.LastReadingValue = GetTemperature(lastFiveResults.Last().Temperature, temperatureFormat);

            var sensorTemperatureDiffernce = lastFiveResults.First().Temperature - lastFiveResults.Last().Temperature;

            if (sensorTemperatureDiffernce > 0)
            {
                sensorResult.TemperatureTendency = "Descending";
            }
            else if (sensorTemperatureDiffernce < 0)
            {
                sensorResult.TemperatureTendency = "Rising";
            }

            return sensorResult;
        }

        public void UpdateSensorVirtualName(string deviceId, string virtualName)
        {
            _deviceInformationRepository.UpdateSensorVirtualName(deviceId, virtualName);
        }

        public void UpdateSensorSettings(UpdateSettingsRequest updateSettingsRequest)
        {
            var sensorSetting = new SensorSetting
            {
                DeviceId = updateSettingsRequest.DeviceId,
                IsLightEnabled = updateSettingsRequest.IsLightEnabled,
                DisableEndTime = updateSettingsRequest.DisableEndTime,
                DisableStartTime = updateSettingsRequest.DisableStartTime,
                IsLightOnSchedule = updateSettingsRequest.IsScheduled
            };

            if (updateSettingsRequest.IsLightEnabled)
            {
                if (updateSettingsRequest.IsScheduled &&
                    !(updateSettingsRequest.DisableStartTime.HasValue || updateSettingsRequest.DisableEndTime.HasValue))
                {
                    sensorSetting.IsLightOnSchedule = false;
                    sensorSetting.DisableStartTime = null;
                    sensorSetting.DisableEndTime = null;
                }
            }
            else
            {
                sensorSetting.IsLightOnSchedule = false;
                sensorSetting.DisableStartTime = null;
                sensorSetting.DisableEndTime = null;
            }

            _sensorSettingRepository.UpdaterSensorSetting(sensorSetting);
        }

        public bool ShouldTurnLightOn(string deviceId)
        {
            var settings = _sensorSettingRepository.GetSensorSettingForSensor(deviceId);
            string timeZoneId = _applicationSettingRepository
                .GetApplicationSettings()
                .FirstOrDefault(x => x.SettingName == ApplicationSettingsConstants.TimeZone)?.SettingValue ?? string.Empty;

            if (settings.IsLightEnabled)
            {
                if (settings.IsLightOnSchedule)
                {
                    var actualTime = TimeOnly.FromDateTime(GetTime(DateTime.UtcNow, timeZoneId));

                    if (settings.DisableStartTime > settings.DisableEndTime)
                    {
                        return settings.DisableStartTime >= actualTime && settings.DisableEndTime <= actualTime;
                    }

                    return !(settings.DisableStartTime < actualTime && actualTime < settings.DisableEndTime);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        private static int GetBatteryPercentage(int? batteryLevel, int minBatteryValue, int maxBatteryValue)
        {
            if (batteryLevel.HasValue)
            {
                return Convert.ToInt32(((batteryLevel.Value - minBatteryValue) * 100) / (maxBatteryValue - minBatteryValue));
            }

            return 0;
        }

        private static DateTime GetTime(DateTime date, string timeZoneId)
        {
            if (string.IsNullOrEmpty(timeZoneId))
            {
                return date;
            }

            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, TimeZoneInfo.Utc.Id, timeZoneId);
        }

        private static float GetTemperature(float temperature, int temperatureSetting) => (Temperature)temperatureSetting switch
        {
            Temperature.Celsius => temperature,
            Temperature.Fahrenheit => (temperature * 1.8f) + 32,
            Temperature.Kelvin => temperature + 273.15f,
            _ => temperature
        };

        public void DeleteSensor(int sensorId)
        {
            var sensor = _deviceInformationRepository.GetSensorById(sensorId);
            if (sensor != null)
            {
                _deviceInformationRepository.DeleteSensor(sensor.Id);
                _sensorSettingRepository.DeleteSensorSettings(sensor.Settings.Id);
                _measurmentRepository.DeleteSensorMeasurements(sensor.DeviceId);
            }
        }
    }

}
