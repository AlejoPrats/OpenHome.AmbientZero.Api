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
        AddMeasurementResult AddNewMeasurement(MeasurementRequest measurementRequest);

        /// <summary>
        /// Gets the temperature for the specified day from all sensors
        /// </summary>
        /// <param name="dateTime">Day of which you will get the temperature measurements</param>
        /// <returns></returns>
        ChartResult GetDailyTemperature(DateTime dateTime);
    }
}
