namespace AmbientZero.Service.Models.Request
{
    public class MeasurementRequest
    {
        public float Temperature { get; set; }
        public string DeviceId { get; set; }
        public int ADCReading { get; set; }
        public bool IsSignaling { get; set; }
    }
}
