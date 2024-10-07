using System.Text.Json.Serialization;

namespace AmbientZero.Service.Models.Result
{
    public class AddMeasurementResult
    {
        [JsonPropertyName("DeviceId")]
        public string DeviceId { get; set; }

        [JsonPropertyName("IsLightEnabled")]
        public bool IsLightEnabled { get; set; }
    }
}
