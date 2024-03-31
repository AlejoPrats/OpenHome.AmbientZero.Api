using System.Text.Json.Serialization;

namespace AmbientZero.Service.Models.Result
{
    public class DeviceDescription
    {
        [JsonPropertyName("DeviceId")]
        public Guid DeviceId { get; set; }

        [JsonPropertyName("DeviceName")]
        public string DeviceName { get; set; }

        [JsonPropertyName("Ip")]
        public string Ip { get; set; }

        [JsonPropertyName("SendsNotification")]
        public bool SendsNotification { get; set; }

        [JsonPropertyName("Topic")]
        public string Topic { get; set; }

        [JsonPropertyName("MQTTPort")]
        public int MQTTPort { get; set; }
       
    }
}
