using AmbientZero.Data.Entities;

namespace AmbientZero.Data.Interfaces
{
    public interface IMeasurmentLogRepository
    {
        void AddMeasurmentLog(MeasurementLog measurementLog);
        IEnumerable<MeasurementLog> GetDailyMeasurements(DateTime startDateTime, DateTime endDateTime);
        IEnumerable<MeasurementLog> GetLastFiveMeasurementsBySensor(string deviceId);
        IEnumerable<MeasurementLog> GetLastReadingsFromAllSensors(DateTime cutoff);
        void MergeSensorsMeasurements(string originDeviceId, string destinyDeviceId);
        void DeleteSensorMeasurements(string deviceId);
    }
}
