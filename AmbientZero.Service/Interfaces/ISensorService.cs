using AmbientZero.Service.Models.Request;
using AmbientZero.Service.Models.Result;

namespace AmbientZero.Service.Interfaces
{
    public interface ISensorService
    {
        /// <summary>
        /// Gets the stored information about all sensors
        /// </summary>
        /// <returns>Sensor information collection</returns>
        IEnumerable<SensorInformationResult> GetSensorInformation();

        SensorInformationResult? GetSensorInformationById(int id);

        /// <summary>
        /// Updates the sensor virtual name
        /// </summary>
        /// <param name="deviceId">GUID as string</param>
        /// <param name="virtualName">New name for the sensor</param>
        void UpdateSensorVirtualName(string deviceId, string virtualName);

        /// <summary>
        /// Updates the settings for the sensor
        /// </summary>
        /// <param name="updateSettingsRequest">Object containing settings regarding the notification light behavior</param>
        void UpdateSensorSettings(UpdateSettingsRequest updateSettingsRequest);

        /// <summary>
        /// Evaluates if the notification light should turn on or not based on the settings
        /// </summary>
        /// <param name="deviceId">GUID as string</param>
        /// <returns>boolean indicating the light behavior</returns>
        bool ShouldTurnLightOn(string deviceId);


        /// <summary>
        /// Delete Sensor and all the related information (Settings And Measurements)
        /// </summary>
        /// <param name="sensorId">Sensor PK</param>
        void DeleteSensor(int sensorId);
    }
}
