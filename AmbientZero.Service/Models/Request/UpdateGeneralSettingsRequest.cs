using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Models.Request
{
    public class UpdateGeneralSettingsRequest
    {
        public bool CheckForUpdatesEnabled { get; set; }
        public bool AutomaticUpdatesEnabled { get; set; }
        public int MaxBatteryValue { get; set; }
        public int MinBatteryValue { get; set; }
    }
}
