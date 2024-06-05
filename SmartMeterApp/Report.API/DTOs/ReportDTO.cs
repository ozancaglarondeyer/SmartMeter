using ReportApi.Enums;

namespace ReportApi.DTOs
{
    public class ReportDTO
    {
        public Guid ReportId { get; set; }
        public Guid MeterId { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public DateTime RequestDate { get; set; }
        public EReportStatus Status { get; set; }
    }
}
