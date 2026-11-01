namespace AmbientZero.Service.Models.Options
{
    public class BatteryInfo
    {
        public int MaxBatteryValue { get; set; }
        public int MinBatteryValue { get; set; }
        public int MaxBatteryRelativeValue => MaxBatteryValue - MinBatteryValue;
    }
}
