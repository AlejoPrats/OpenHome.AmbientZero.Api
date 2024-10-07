namespace AmbientZero.Service.Models.Result
{
    public class SensorInformationResult
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public string? DeviceVirtualName { get; set; }
        public int? ADCReading { get; set; }
        public bool? IsSignaling { get; set; }
        public DateTime LastReadingTime { get; set; }
        public float? LastReadingValue { get; set; }
        public string TemperatureTendency { get; set; }
        public SensorSettingResult SensorSetting { get; set; }
    }
}
