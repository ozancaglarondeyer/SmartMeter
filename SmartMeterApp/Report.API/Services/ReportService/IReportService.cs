using Microsoft.EntityFrameworkCore.Storage;
using ReportApi.DTOs;

namespace ReportApi.Services.ReportService
{
    public interface IReportService
    {
        List<ReportDTO> GetReports();
        List<ReportDetailDTO> GetReportDetails(Guid reportId);
        void CreateReport(CreateRaportDTO createReport, IDbContextTransaction _transaction = null);
        void GenerateReport(Guid reportId);

    }
}
