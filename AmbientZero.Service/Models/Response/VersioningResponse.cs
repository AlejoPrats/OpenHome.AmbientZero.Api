using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Models.Response
{
    public class VersioningResponse
    {
        public string Name { get; set; }
        public string LocalVersion { get; set; }
        public string? RemoteVersion { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
