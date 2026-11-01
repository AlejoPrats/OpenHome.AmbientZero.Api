using System.Text.Json.Serialization;

namespace AmbientZero.Service.Models.Result
{
    public class ApplicationSettingResult
    {
        public string TimeZone { get; set; }
        public int DisplayUnit { get; set; }
    }
}
