using AmbientZero.Data.Entities;
using AmbientZero.Service.Models.Request;
using AmbientZero.Service.Models.Result;

namespace AmbientZero.Service.Interfaces
{
    public interface IMeasurmentService
    {
        /// <summary>
        /// Adds a measurement to the database validating that the sensor and sensor settings exists to avoid data inconcistencies
        /// </summary>
        /// <param name="measurementRequest">Simple object recieved from the sensor to record the infomation in the DB</param>
        void AddNewMeasurement(MeasurementRequest measurementRequest);

        /// <summary>
        /// Gets the temperature for the specified day from all sensors
        /// </summary>
        /// <param name="dateTime">Day of which you will get the temperature measurements</param>
        /// <returns></returns>
        IEnumerable<MeasurementResult> GetDailyTemperature(DateTime dateTime);
    }
}
