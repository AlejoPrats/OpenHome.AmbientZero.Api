using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Backup.Models
{
    public class ImportMetadata
    {
        public DateTime CreatedOn { get; set; }
        public int SensorsExported { get; set; }
        public int SensorSettingsExported { get; set; }
        public int TemperatureLogsExported { get; set; }
        public int TemperatureLogsPages { get; set; }
    }
}
