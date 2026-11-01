namespace AmbientZero.Service.Models.Result
{
    public class SensorSettingResult
    {
        public string DeviceId { get; set; }
        public bool IsLightEnabled { get; set; }
        public bool IsScheduled { get; set; }
        public string? Color { get; set; }
        public TimeOnly? DisableStartTime { get; set; }
        public TimeOnly? DisableEndTime { get; set; }
    }
}
