using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Models.Response
{
    public class AdvancedSettingsResponse
    {
        public GeneralSettingsResponse GeneralSettings { get; set; }
        public AccessPointSettingsResponse AccessPointSettings { get; set; }
        public ApplicationSecurityResponse ApplicationSecurity { get; set; }
    }
}
