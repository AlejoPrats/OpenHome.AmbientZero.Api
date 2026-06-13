using System.Text.Json.Serialization;

namespace AmbientZero.Service.Models.Result
{
    public class ApplicationSettingResult
    {
        [JsonPropertyName("settingName")]
        public string ApplicationSettingName { get; set; }

        [JsonPropertyName("settingValue")]
        public string ApplicationSettingValue { get; set; }
    }
}
