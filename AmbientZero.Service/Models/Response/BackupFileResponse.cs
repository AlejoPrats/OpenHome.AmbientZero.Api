using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Models.Response
{
    public class BackupFileResponse
    {
        public string Filename { get; set; }
        public Stream BackupStream { get; set; }
    }
}
