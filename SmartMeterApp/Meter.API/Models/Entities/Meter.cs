namespace MeterApi.Models.Entities
{
    public class Meter
    {
        public Guid Id { get; set; }
        public string SerialNumber { get; set; }
        public List<MeterReading> MeterReadings { get; set; }
    }
}
