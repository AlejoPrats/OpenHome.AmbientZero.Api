using AmbientZero.Service.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Models.Response
{
    public class ApplicationSecurityResponse
    {
        public Security ApplicationSecurity { get; set; }
        public string SshPassword { get; set; }
    }
}
