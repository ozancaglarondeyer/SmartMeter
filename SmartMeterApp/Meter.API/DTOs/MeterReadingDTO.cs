namespace MeterApi.DTOs
{
    public class MeterReadingDTO
    {
        public Guid MeterId { get; set; }
        public DateTime ReadingTime { get; set; }
        public double Voltage { get; set; }
        public double LastIndex { get; set; }
        public double Power { get; set; }
    }
}
