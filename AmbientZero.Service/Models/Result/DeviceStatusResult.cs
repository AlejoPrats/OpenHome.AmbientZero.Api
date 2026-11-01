using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Models.Result
{
    public class DeviceStatusResult
    {
        public MemoryInformation Memory { get; set; }
        public SwapInfo Swap { get; set; }
        public CpuInfo Cpu { get; set; }
    }

    public class MemoryInformation
    {
        public string Available { get; set; }
        public string Free { get; set; }
    }

    public class SwapInfo
    {
        public string Used { get; set; }
    }

    public class CpuInfo
    {
        public double Temperature { get; set; }
    }
}
