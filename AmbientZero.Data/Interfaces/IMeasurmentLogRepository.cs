using AmbientZero.Data.Entities;

namespace AmbientZero.Data.Interfaces
{
    public interface IMeasurmentLogRepository
    {
        void AddMeasurmentLog(MeasurementLog measurementLog);
        IEnumerable<MeasurementLog> GetDailyMeasurements(DateTime dateTime);
        IEnumerable<MeasurementLog> GetLastFiveMeasurementsBySensor(string deviceId);
    }
}
