namespace AmbientZero.Data.Entities
{
    public class MeasurementLog
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public float Temperature { get; set; }
        public DateTime Date { get; set; }
        public DeviceInformation DeviceInformation { get; set; }
    }
}
