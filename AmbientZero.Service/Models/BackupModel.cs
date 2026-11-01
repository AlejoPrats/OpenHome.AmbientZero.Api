using AmbientZero.Data.Entities;
using AmbientZero.Service.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Models
{
    public class BackupModel
    {
        public List<SensorInformationResult> Sensors { get; set; }
        public List<DeviceMeasurementResult> MeasurementLogs { get; set; }
        public List<SensorSettingResult> SensorSettings { get; set; }
        public IReadOnlyDictionary<string,string> MigrationIds { get; set; }
    }
}
