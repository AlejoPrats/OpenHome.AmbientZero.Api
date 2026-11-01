using System.Text.Json.Serialization;

namespace AmbientZero.Service.Models.Result
{
    public class DeviceDescription
    {
        public Guid DeviceId { get; set; }

        public string DeviceName { get; set; }

        public AddressObject NetworkInformation { get; set; }

        public bool SendsNotification { get; set; }

        public string Topic { get; set; }

        public int MQTTPort { get; set; }

    }
}
