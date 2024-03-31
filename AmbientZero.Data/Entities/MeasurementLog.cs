using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Entities
{
    public class MeasurementLog
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public float Temperature { get; set; }
        public DateTime Date { get; set; }
        public DeviceInformation DeviceInformation { get; set; }
    }
}
