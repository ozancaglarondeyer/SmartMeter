namespace MeterApi.DTOs
{
    public class MeterQueryParameters
    {
        public string? SerialNumber { get; set; }
        public DateTime? LastReadingStartDate { get; set; }
        public DateTime? LastReadingEndDate { get; set; }
    }
}
