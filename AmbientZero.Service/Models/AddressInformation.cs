using AmbientZero.Service.Constants;
using System.Text.Json.Serialization;

namespace AmbientZero.Service.Models
{
    public class AddressInformation
    {
        [JsonPropertyName("local")]
        public string IpAddress { get; set; }
        [JsonPropertyName("prefixlen")]
        public int PrefixLength { get; set; }
        public IPAddressType AddressType
        {
            get
            {
                if (PrefixLength is > 0 and <= 32)
                {
                    return IPAddressType.IPv4;
                }
                else if (PrefixLength is 64)
                {
                    return IPAddressType.IPv6;
                }
                else if (PrefixLength is 128)
                {
                    return IPAddressType.IPv6IaNa;
                }
                else
                {
                    return IPAddressType.Unknown;
                }
            }
        }
    }
}
