using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using ReportApi.DTOs;
using ReportApi.Enums;
using ReportApi.Helpers;
using ReportApi.Models;
using ReportApi.Models.Entities;
using ReportApi.Services.RabbitMQService;
using Shared;
using System.Linq.Expressions;

namespace ReportApi.Services.ReportService
{
    public class ReportService : BaseService, IReportService
    {
        private readonly ReportDbContext _dbContext;
        private readonly IRabbitMQService _rabbitMQService;
        public ReportService(IOptions<MeterApiConfiguration> configurationService, ReportDbContext dbContext, IRabbitMQService rabbitMQService, IHttpContextAccessor httpContext) : base(configurationService, httpContext)
        {
            _dbContext = dbContext;
            _rabbitMQService = rabbitMQService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createReport"></param>
        /// <param name="_transaction"></param>
        /// <exception cref="Exception"></exception>
        public void CreateReport(CreateRaportDTO createReport, IDbContextTransaction _transaction = null)
        {
            // guid geçersiz ise
            if (createReport == null)
            {
                throw new Exception("The report data provided is null.");
            }
            IDbContextTransaction transaction = _transaction ?? _dbContext.Database.BeginTransaction();
            try
            {

                Report report = new Report
                {
                    Id = Guid.NewGuid(),
                    MeterId = createReport.MeterId,
                    SerialNumber = createReport.SerialNumber,
                    Name = createReport.Name,
                    RequestDate = DateTime.Now,
                    Status = EReportStatus.InProgress
                };
                _dbContext.Reports.Add(report);

                _dbContext.SaveChanges();

                if (_transaction == null)
                {
                    transaction.Commit();
                    transaction.Dispose();
                }

                _rabbitMQService.SendMessage(report.Id.ToString(), RabbitMQSettings.ReportQueue);

            }
            catch (Exception ex)
            {
                if (_transaction == null)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                }
                throw new Exception("An error occurred while creating the report." + " " + ex.Message);
            }
        }

        public List<ReportDetailDTO> GetReportDetails(Guid id)
        {
            try
            {
                List<ReportDetailDTO> reportDetails = _dbContext.ReportDetails.Where(r => r.ReportId == id).Select(r => new ReportDetailDTO
                {
                    Id = r.Id,
                    ReportId = r.ReportId,
                    ReadingTime = r.ReadingTime,
                    LastIndex = r.LastIndex,
                    Voltage = r.Voltage,
                    Power = r.Power

                }).ToList();

                return reportDetails;

            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while getting report details." + " " + ex.Message);
            }
        }

        public List<ReportDTO> GetReports()
        {

            try
            {
                List<ReportDTO> reports = _dbContext.Reports.Select(r => new ReportDTO
                {
                    ReportId = r.Id,
                    SerialNumber = r.SerialNumber,
                    MeterId = r.MeterId,
                    Name = r.Name,
                    RequestDate = r.RequestDate,
                    Status = r.Status

                }).ToList();

                return reports;
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while getting the reports." + " " + ex.Message);
            }
        }


        public void GenerateReport(Guid reportId)
        {
            Report report = _dbContext.Reports.FirstOrDefault(r => r.Id == reportId);
            if (report == null)
            {
                throw new Exception("The report could not be found.");
            }

            try
            {   
                GenericResult apiResult = Get("api/Meter/readings/" + report.MeterId);
                // sayaca bağlı okuma bilgilerini MeterApi den getir
                List<MeterReadingInformationDTO> meterReadingInformations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MeterReadingInformationDTO>>(apiResult.Value + "");

                foreach (var meterReadingInformation in meterReadingInformations)
                {
                    ReportDetail reportDetail = new ReportDetail
                    {
                        Id = Guid.NewGuid(),
                        ReportId = report.Id,
                        ReadingTime = meterReadingInformation.ReadingTime,
                        LastIndex = meterReadingInformation.LastIndex,
                        Voltage = meterReadingInformation.Voltage,
                    };
                    _dbContext.ReportDetails.Add(reportDetail);
                }
                

                // Bu veriler ile excel oluştur
                var filePath = ExcelService.GenerateExcelReport(report.SerialNumber, meterReadingInformations);

                report.Status = EReportStatus.Completed;
                report.CreationDate = DateTime.Now;
                _dbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                report.Status = EReportStatus.FailedToGenerate;
            }   
        }

    }
}
