namespace AmbientZero.Service.Models.Result
{
    public class MeasurementResult
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public float Temperature { get; set; }
        public DateTime Date { get; set; }
        public string SensorVirtualName { get; set; }
    }
}
