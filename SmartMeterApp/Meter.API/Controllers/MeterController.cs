using MeterApi.DTOs;
using MeterApi.Models.Entities;
using MeterApi.Services.MeterService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace MeterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterController : ControllerBase
    {
        private readonly IMeterService _meterService;
        public MeterController(IMeterService meterService)
        {
            _meterService = meterService;
        }

        /// <summary>
        /// Belirtilen parametrelere göre sayaçları getirir.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("GetMeters")]
        public IActionResult GetMeters([FromQuery] MeterQueryParameters parameters)
        {
            GenericResult genericResult = new GenericResult();
            try
            {
                genericResult.Value = _meterService.GetMeters(parameters);
            }
            catch (Exception ex)
            {
                genericResult.AddError(ex.Message);
                return BadRequest(genericResult);
            }
            
            return Ok(genericResult);
        }

        /// <summary>
        /// Yeni bir sayaç okuma verisi ekler.
        /// </summary>
        /// <param name="meterReading"></param>
        /// <returns></returns>
        [HttpPost("AddMeterReading")]
        public IActionResult AddMeterReading([FromBody] MeterReadingDTO meterReading)
        {
            GenericResult genericResult = new GenericResult();
            try
            {
                _meterService.AddMeterReading(meterReading);
            }
            catch (Exception ex)
            {
                genericResult.AddError(ex.Message);
                return BadRequest(genericResult);
            }
            
            return Ok(genericResult);

        }

        /// <summary>
        /// Sayaç seçim listesini getirir.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMeterSelection")]
        public IActionResult GetMeterSelection()
        {
            GenericResult genericResult = new GenericResult();
            try
            {
                genericResult.Value = _meterService.GetMeterSelections();
            }
            catch (Exception ex)
            {
                genericResult.AddError(ex.Message);
                return BadRequest(genericResult);
            }
            
            return Ok(genericResult);
        }

        /// <summary>
        /// Belirtilen sayaç ID'sine ait tüm okuma verilerini getirir.
        /// </summary>
        /// <param name="meterId"></param>
        /// <returns></returns>
        [HttpGet("readings/{meterId}")]
        public IActionResult GetMeterReadings(Guid meterId)
        {
            GenericResult genericResult = new GenericResult();
            try
            {
                genericResult.Value = _meterService.GetMeterReadings(meterId);
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
