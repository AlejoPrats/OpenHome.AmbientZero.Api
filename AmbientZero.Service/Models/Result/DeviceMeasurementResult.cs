namespace AmbientZero.Service.Models.Result
{
    public class DeviceMeasurementResult
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public DateTime Date { get; set; }
        public float Temperature { get; set; }
    }
}
