using AmbientZero.Service.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Models.Response
{
    public class AccessPointSettingsResponse
    {
        public string AccessPointName { get; set; }
        public AccessPointAuthMethod AccessPointSecurity { get; set; }
    }
}
