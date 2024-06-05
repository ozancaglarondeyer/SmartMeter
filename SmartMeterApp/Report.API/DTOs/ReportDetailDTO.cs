namespace ReportApi.DTOs
{
    public class ReportDetailDTO
    {
        public Guid Id { get; set; }
        public Guid ReportId { get; set; }
        public DateTime ReadingTime { get; set; }
        public decimal Voltage { get; set; }
        public decimal Power { get; set; }
        public decimal LastIndex { get; set; }
    }
}
