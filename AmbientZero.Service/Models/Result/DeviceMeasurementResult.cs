using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Models.Result
{
    public class DeviceMeasurementResult
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public float Temperature { get; set; }
    }
}
