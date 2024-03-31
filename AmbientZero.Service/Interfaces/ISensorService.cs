using AmbientZero.Data.Entities;
using AmbientZero.Service.Models.Request;
using AmbientZero.Service.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Interfaces
{
    public interface ISensorService
    {
        /// <summary>
        /// Gets the stored information about all sensors
        /// </summary>
        /// <returns>Sensor information collection</returns>
        IEnumerable<SensorInformationResult> GetSensorInformation();

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
    }
}
