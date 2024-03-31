using AmbientZero.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Interfaces
{
    public interface IMeasurmentLogRepository
    {
        void AddMeasurmentLog(MeasurementLog measurementLog);
        IEnumerable<MeasurementLog> GetDailyMeasurements(DateTime dateTime);
        IEnumerable<MeasurementLog> GetLastFiveMeasurementsBySensor(string deviceId);
    }
}
