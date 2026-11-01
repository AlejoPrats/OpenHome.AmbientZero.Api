using AmbientZero.Data.Constants;
using AmbientZero.Data.Entities;
using AmbientZero.Data.Interfaces;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Options;
using AmbientZero.Service.Models.Request;
using AmbientZero.Service.Models.Result;

namespace AmbientZero.Service.Services
{
    public class MeasurmentService : IMeasurmentService
    {
        private readonly IMeasurmentLogRepository _measurmentRepository;
        private readonly IDeviceInformationRepository _deviceInformationRepository;
        private readonly IApplicationSettingRepository _applicationSettingRepository;
        private readonly ISensorSettingRepository _sensorSettingRepository;
        private readonly ITimeZoneService _timeZoneService;

        private const float limitValuesDiferential = 0.2f;
        public MeasurmentService(IMeasurmentLogRepository measurmentLogRepository,
            IDeviceInformationRepository deviceInformationRepository,
            ISensorSettingRepository sensorSettingRepository,
            IApplicationSettingRepository applicationSettingRepository,
            ITimeZoneService timeZoneService)
        {
            _measurmentRepository = measurmentLogRepository ?? throw new ArgumentNullException(nameof(measurmentLogRepository));
            _deviceInformationRepository = deviceInformationRepository ?? throw new ArgumentNullException(nameof(deviceInformationRepository));
            _sensorSettingRepository = sensorSettingRepository ?? throw new ArgumentNullException(nameof(sensorSettingRepository));
            _applicationSettingRepository = applicationSettingRepository ?? throw new ArgumentNullException(nameof(applicationSettingRepository));
            _timeZoneService = timeZoneService ?? throw new ArgumentNullException(nameof(timeZoneService));
        }

        public AddMeasurementResult AddNewMeasurement(MeasurementRequest measurementRequest)
        {
            var result = new AddMeasurementResult();

            if (string.IsNullOrEmpty(measurementRequest.DeviceId))
            {
                measurementRequest.DeviceId = Guid.NewGuid().ToString();
                result.DeviceId = measurementRequest.DeviceId;
            }

            var deviceInformation = new DeviceInformation()
            {
                DeviceId = measurementRequest.DeviceId,
                IsSignaling = measurementRequest.IsSignaling,
                ADCReading = measurementRequest.ADCReading,
                Version = measurementRequest.Version
            };

            if (_deviceInformationRepository.SensorExists(measurementRequest.DeviceId))
            {
                _deviceInformationRepository.UpdateSensorSignal(deviceInformation);
            }
            else
            {
                _sensorSettingRepository.AddSensorSetting(new SensorSetting { DeviceId = measurementRequest.DeviceId, IsLightEnabled = true });
                _deviceInformationRepository.AddNewSensor(deviceInformation);
            }

            var measurementLog = new MeasurementLog()
            {
                DeviceId = measurementRequest.DeviceId,
                Date = DateTime.UtcNow,
                Temperature = measurementRequest.Temperature
            };

            _measurmentRepository.AddMeasurmentLog(measurementLog);

            return result;
        }

        public ChartResult GetDailyTemperature(DateTime dateTime)
        {
            var timeZoneSetting = _applicationSettingRepository.GetApplicationSettingByKey(ApplicationSettingsConstants.TimeZone);

            var dateTimeRange = _timeZoneService.GetSartTimezone(dateTime, timeZoneSetting.SettingValue ?? string.Empty);
            var temperatureReadings = _measurmentRepository.GetDailyMeasurements(dateTimeRange.DayStart, dateTimeRange.DayEnd);
            var group = temperatureReadings.GroupBy(x => x.DeviceId);
            var chartData = new ChartResult();
            if (temperatureReadings.Any())
            {
                chartData.MinTemperature = (temperatureReadings.Min(x => x.Temperature) - limitValuesDiferential);
                chartData.MaxTemperature = (temperatureReadings.Max(x => x.Temperature) + limitValuesDiferential);
            }

            var result = new List<MeasurementResult>();

            foreach (var deviceMeasurements in group)
            {
                var value = new MeasurementResult();

                value.DeviceId = deviceMeasurements.FirstOrDefault().DeviceId;
                value.SensorVirtualName = deviceMeasurements.FirstOrDefault().DeviceInformation.DeviceVirtualName ?? deviceMeasurements.FirstOrDefault().DeviceId;
                value.Color = deviceMeasurements.FirstOrDefault()?.DeviceInformation?.Settings?.Color ?? null;
                value.TemperatureReadings = deviceMeasurements.Select(x => new DeviceMeasurementResult()
                {
                    Id = x.Id,
                    Date = _timeZoneService.GetTimeFromTimezone(x.Date, timeZoneSetting.SettingValue ?? string.Empty),
                    Temperature = x.Temperature
                }).ToList();

                result.Add(value);
            }

            chartData.Measurements = result;

            return chartData;
        }
    }
}
