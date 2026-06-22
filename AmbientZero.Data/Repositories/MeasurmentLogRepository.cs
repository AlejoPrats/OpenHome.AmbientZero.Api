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

        public IEnumerable<MeasurementLog> GetDailyMeasurements(DateTime dateTime)
        {
            var startDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
            var endDate = startDate.AddDays(1).AddTicks(-1);
            return _ambientZeroContext.Measurements.Where(x => x.Date >= startDate && x.Date <= endDate).Include(x => x.DeviceInformation).ToList();
        }

        public IEnumerable<MeasurementLog> GetLastFiveMeasurementsBySensor(string deviceId)
        {
            return _ambientZeroContext.Measurements
                .Where(x => x.DeviceId == deviceId)
                .OrderByDescending(x => x.Date)
                .Take(5)
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
