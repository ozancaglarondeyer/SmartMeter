namespace MeterApi.Models.Entities
{
    public class MeterReading
    {
        public Guid Id { get; set; }
        public DateTime ReadingTime { get; set; }
        public double Voltage { get; set; }
        public double LastIndex { get; set; }
        public double Power { get; set; }
        public Guid MeterId { get; set; }
        public Meter Meter { get; set; }
    }
}
