using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Models
{
    public class ProcessProgress
    {
        public Dictionary<string,string> Messages { get; set; }
        public bool IsSuccess { get; set; }
        public string Status { get; set; }
        public int Percentage { get; set; }
        public string[][] Message => Messages.Select(kvp => new string[] { kvp.Key, kvp.Value }).ToArray();
    }
}
