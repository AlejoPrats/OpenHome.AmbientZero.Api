using AmbientZero.Data.Entities;
using AmbientZero.Data.Interfaces;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Request;
using AmbientZero.Service.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Services
{
    public class MeasurmentService : IMeasurmentService
    {
        private readonly IMeasurmentLogRepository _measurmentRepository;
        private readonly IDeviceInformationRepository _deviceInformationRepository;
        private readonly ISensorSettingRepository _sensorSettingRepository;
        public MeasurmentService(IMeasurmentLogRepository measurmentLogRepository,
            IDeviceInformationRepository deviceInformationRepository,
            ISensorSettingRepository sensorSettingRepository)
        {
            _measurmentRepository = measurmentLogRepository;
            _deviceInformationRepository = deviceInformationRepository;
            _sensorSettingRepository = sensorSettingRepository;
        }

        public void AddNewMeasurement(MeasurementRequest measurementRequest)
        {
            var deviceInformation = new DeviceInformation()
            {
                DeviceId = measurementRequest.DeviceId,
                IsSignaling = measurementRequest.IsSignaling,
                ADCReading = measurementRequest.ADCReading
            };

            if (_deviceInformationRepository.SensorExists(measurementRequest.DeviceId))
            {
                _deviceInformationRepository.UpdateSensorSignal(deviceInformation);
            }
            else
            {
                _deviceInformationRepository.AddNewSensor(deviceInformation);
            }

            if(!_sensorSettingRepository.SensorSettingExist(measurementRequest.DeviceId)) 
            {
                _sensorSettingRepository.AddSensorSetting(new SensorSetting { DeviceId = measurementRequest.DeviceId, IsLightEnabled = true });
            }

            var measurementLog = new MeasurementLog()
            {
                DeviceId = measurementRequest.DeviceId,
                Date = DateTime.Now,
                Temperature = measurementRequest.Temperature
            };

            _measurmentRepository.AddMeasurmentLog(measurementLog);
        }

        public IEnumerable<MeasurementResult> GetDailyTemperature(DateTime dateTime)
        {
            return _measurmentRepository.GetDailyMeasurements(dateTime).Select(x => new MeasurementResult
            {
                DeviceId = x.DeviceId,
                Date = x.Date,
                Id = x.Id,
                Temperature = x.Temperature,
                SensorVirtualName = x.DeviceInformation?.DeviceVirtualName
            });
        }
    }
}
