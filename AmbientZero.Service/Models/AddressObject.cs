using System.Text.Json.Serialization;

namespace AmbientZero.Service.Models
{
    internal class AddressObject
    {
        [JsonPropertyName("ifname")]
        public string AdapterName { get; set; }
        [JsonPropertyName("operstate")]
        public string AdapterStatus { get; set; }
        [JsonPropertyName("addr_info")]
        public AddressInformation[] AddressInformation { get; set; }
    }
}
