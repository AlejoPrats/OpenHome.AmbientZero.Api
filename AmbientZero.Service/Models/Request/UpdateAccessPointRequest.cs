namespace AmbientZero.Service.Models.Request
{
    public class UpdateAccessPointRequest
    {
        public string Ssid { get; set; }
        public string Password { get; set; }
        public int Channel {  get; set; }
        public string Country { get; set; }
        public int ApSecurity { get; set; }
    }
}