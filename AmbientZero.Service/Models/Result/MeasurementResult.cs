namespace AmbientZero.Service.Models.Result
{
    public class MeasurementResult
    {
        public string DeviceId { get; set; }
        public string SensorVirtualName { get; set; }
        public IEnumerable<DeviceMeasurementResult>? TemperatureReadings { get; set; }
    }
}
