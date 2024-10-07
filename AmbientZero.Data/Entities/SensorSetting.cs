namespace AmbientZero.Data.Entities
{
    public class SensorSetting
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public bool IsLightEnabled { get; set; }
        public bool IsLightOnSchedule { get; set; }
        public TimeOnly? DisableStartTime { get; set; }
        public TimeOnly? DisableEndTime { get; set; }
        public DeviceInformation Sensor { get; set; }
    }
}
