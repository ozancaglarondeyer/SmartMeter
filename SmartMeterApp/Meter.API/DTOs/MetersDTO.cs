namespace MeterApi.DTOs
{
    public class MetersDTO
    {
        public Guid MeterId { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? LastReadingTime { get; set; }
        public double? LastReadingVoltage { get; set; }
        public double? LastIndex { get; set; }
        public double? LastReadingPower { get; set; }
    }
}
