using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Entities
{
    public class DeviceInformation
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public string? DeviceVirtualName { get; set; }
        public int? ADCReading { get; set; }
        public bool? IsSignaling { get; set; }
        public ICollection<MeasurementLog> Measurements { get; set; }
        public SensorSetting? Settings { get; set; }
    }
}
