using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
