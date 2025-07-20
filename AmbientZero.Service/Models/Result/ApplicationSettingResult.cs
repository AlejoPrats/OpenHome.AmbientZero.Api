using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AmbientZero.Service.Models.Result
{
    public class ApplicationSettingResult
    {
        [JsonPropertyName("settingName")]
        public string ApplicationSettingName { get; set; }

        [JsonPropertyName("settingValue")]
        public string ApplicationSettingValue { get; set;}
    }
}
