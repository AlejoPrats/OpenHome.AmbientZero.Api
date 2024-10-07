using System.Text.Json.Serialization;

namespace AmbientZero.Service.Models.Request
{
    public class UpdateNameRequest
    {
        [JsonPropertyName("DeviceId")]
        public string DeviceId { get; set; }

        [JsonPropertyName("VirtualName")]
        public string VirtualName { get; set; }
    }
}
