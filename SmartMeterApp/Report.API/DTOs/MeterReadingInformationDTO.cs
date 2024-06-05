namespace ReportApi.DTOs
{
    public class MeterReadingInformationDTO
    {
        public Guid MeterId { get; set; }
        public DateTime ReadingTime { get; set; }
        public decimal Voltage { get; set; }
        public decimal LastIndex { get; set; }
        public decimal Power { get; set; }
    }
}
