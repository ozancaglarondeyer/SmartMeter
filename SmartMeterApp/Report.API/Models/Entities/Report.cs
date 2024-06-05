using ReportApi.Enums;

namespace ReportApi.Models.Entities
{
    public class Report
    {
        public Guid Id { get; set; }
        public Guid MeterId { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public EReportStatus Status { get; set; }

        public ICollection<ReportDetail> ReportDetails { get; set; }

    }
}
