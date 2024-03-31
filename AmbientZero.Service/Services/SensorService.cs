using AmbientZero.Data.Entities;
using AmbientZero.Data.Interfaces;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Request;
using AmbientZero.Service.Models.Result;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Services
{
    public class SensorService : ISensorService
    {
        private readonly IMeasurmentLogRepository _measurmentRepository;
        private readonly IDeviceInformationRepository _deviceInformationRepository;
        private readonly ISensorSettingRepository _sensorSettingRepository;

        public SensorService(IMeasurmentLogRepository measurmentLogRepository,
            IDeviceInformationRepository deviceInformationRepository,
            ISensorSettingRepository sensorSettingRepository)
        {
            _measurmentRepository = measurmentLogRepository;
            _deviceInformationRepository = deviceInformationRepository;
            _sensorSettingRepository = sensorSettingRepository;
        }

        public IEnumerable<SensorInformationResult> GetSensorInformation()
        {
            List<SensorInformationResult> sensors = _deviceInformationRepository
                .GetAllSensors()
                .Select(x => new SensorInformationResult()
                {
                    DeviceId = x.DeviceId,
                    ADCReading = x.ADCReading,
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
                sensor.LastReadingTime = lastFiveResults.Last().Date;
                sensor.LastReadingValue = lastFiveResults.Last().Temperature;

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

            if (settings.IsLightEnabled)
            {
                if (settings.IsLightOnSchedule)
                {
                    var actualTime = TimeOnly.FromDateTime(DateTime.Now);

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
    }

}
