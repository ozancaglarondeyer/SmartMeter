using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReportApi.DTOs;
using ReportApi.Services.ReportService;
using Shared;

namespace ReportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {

        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Raporların listesini getirir.
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetReports")]
        public IActionResult GetReports()
        {
            GenericResult genericResult = new GenericResult();
            try
            {
                genericResult.Value = _reportService.GetReports();
            }
            catch (Exception ex)
            {
                genericResult.AddError(ex.Message);
                return BadRequest(genericResult);
            }

            return Ok(genericResult);
        }


        /// <summary>
        /// Belirtilen ID'ye sahip raporun detaylarını getirir.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public  IActionResult GetReportDetails(Guid id)
        {
            GenericResult genericResult = new GenericResult();
            try
            {
                genericResult.Value = _reportService.GetReportDetails(id);
            }
            catch (Exception ex)
            {
                genericResult.AddError(ex.Message);
                return BadRequest(genericResult);
            }

            return Ok(genericResult);
        }

        /// <summary>
        /// Yeni bir rapor oluşturur.
        /// </summary>
        /// <param name="createReportDto"></param>
        /// <returns></returns>
        [HttpPost("CreateReport")]
        public IActionResult CreateReport([FromBody] CreateRaportDTO createReport)
        {
            GenericResult genericResult = new GenericResult();
            try
            {
                _reportService.CreateReport(createReport);
            }
            catch (Exception ex)
            {
                genericResult.AddError(ex.Message);
                return BadRequest(genericResult);
            }

            return Ok(genericResult);
        }

    }
}
