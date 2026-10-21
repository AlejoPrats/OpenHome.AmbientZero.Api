using AmbientZero.Service.Constants;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Text.Json.Serialization;

namespace AmbientZero.Service.Models
{
    public class AddressObject
    {
        public string AdapterName { get; set; }
        public string AdapterStatus { get; set; }
        public string IpAddress { get; set; }
        public int Prefixlen { get; set; }
        public IPAddressType AddressType
        {
            get
            {
                return Prefixlen switch
                {
                    > 0 and <= 32 => IPAddressType.IPv4,
                    64 => IPAddressType.IPv6,
                    128 => IPAddressType.IPv6IaNa,
                    _ => IPAddressType.Unknown
                };
            }
        }
    }
}
