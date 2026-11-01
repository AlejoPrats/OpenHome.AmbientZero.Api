using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Models.RemoteResponse
{
    internal class VersionInfoRemoteResponse
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string? BinaryUrl { get; set; }
        public bool IsNewer { get; set; }
    }
}
