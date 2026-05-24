using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Models.Result
{
    public class ChartResult
    {
        public float MaxTemperature { get; set; }
        public float MinTemperature { get; set; }
        public List<MeasurementResult> Measurements { get; set; }

    }
}
