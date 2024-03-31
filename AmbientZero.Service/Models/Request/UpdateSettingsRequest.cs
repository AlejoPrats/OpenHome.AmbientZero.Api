using System.Text.Json.Serialization;

namespace AmbientZero.Service.Models.Request
{
    public class UpdateSettingsRequest
    {
        [JsonPropertyName("DeviceId")]
        public string DeviceId { get; set; }

        [JsonPropertyName("IsLightEnabled")]
        public bool IsLightEnabled { get; set; }

        [JsonPropertyName("IsScheduled")]
        public bool IsScheduled { get; set; }

        [JsonPropertyName("DisableStartTime")]
        public TimeOnly? DisableStartTime { get; set; }

        [JsonPropertyName("DisableEndTime")]
        public TimeOnly? DisableEndTime { get; set; }
    }
}
