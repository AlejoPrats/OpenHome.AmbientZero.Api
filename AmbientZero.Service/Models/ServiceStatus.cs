using System.Text.Json.Serialization;

namespace AmbientZero.Service.Models
{
    public class ServiceStatus
    {
        [JsonPropertyName("service")]
        public string ServiceName { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("since")]
        public string DateStarted { get; set; }
        [JsonPropertyName("uptime")]
        public string Uptime { get; set; }

    }
}
