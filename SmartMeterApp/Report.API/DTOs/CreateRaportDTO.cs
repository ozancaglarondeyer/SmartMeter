namespace ReportApi.DTOs
{
    public class CreateRaportDTO
    {
        public Guid MeterId { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
    }
}
