using AmbientZero.Data.Entities;
using AmbientZero.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AmbientZero.Data.Repositories
{
    public class MeasurmentLogRepository : IMeasurmentLogRepository
    {
        private readonly AmbientZeroContext _ambientZeroContext;
        public MeasurmentLogRepository(AmbientZeroContext ambientZeroContext)
        {
            _ambientZeroContext = ambientZeroContext ?? throw new ArgumentNullException(nameof(ambientZeroContext));
        }

        public void AddMeasurmentLog(MeasurementLog measurementLog)
        {
            _ambientZeroContext.Measurements.Add(measurementLog);
            _ambientZeroContext.SaveChanges();
        }

        public IEnumerable<MeasurementLog> GetDailyMeasurements(DateTime startDateTime, DateTime endDateTime)
        {
            return _ambientZeroContext.Measurements.Where(x => x.Date >= startDateTime && x.Date <= endDateTime).Include(x => x.DeviceInformation).ToList();
        }

        public IEnumerable<MeasurementLog> GetLastFiveMeasurementsBySensor(string deviceId)
        {
            return _ambientZeroContext.Measurements
                .Where(x => x.DeviceId == deviceId)
                .OrderByDescending(x => x.Date)
                .Take(5)
                .ToList();
        }

        public IEnumerable<MeasurementLog> GetLastReadingsFromAllSensors(DateTime cutoff)
        {
            return _ambientZeroContext.Measurements
                .GroupBy(m => m.DeviceId)
                .Select(g => new MeasurementLog {
                    DeviceId = g.Key,
                    Date = g.Max(x => x.Date)
                })
                .Where(x => x.Date > cutoff)
                .ToList();
        }

        public void DeleteSensorMeasurements(string deviceId)
        {
            _ambientZeroContext.Measurements
                .Where(x => x.DeviceId == deviceId)
                .ExecuteDelete();
        }
    }
}
