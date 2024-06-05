using MeterApi.DTOs;
using MeterApi.Models;
using MeterApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;

namespace MeterApi.Services.MeterService
{
    public class MeterService : IMeterService
    {
        private readonly MeterDbContext _dbContext;

        public MeterService(MeterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Belirtilen parametrelere göre sayaçları getirir.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<MetersDTO> GetMeters(MeterQueryParameters parameters)
        {
            try
            {
                bool isEmptyAd = string.IsNullOrWhiteSpace(parameters.SerialNumber);

                DateTime lastReadingStartDate = parameters.LastReadingStartDate.HasValue ? parameters.LastReadingStartDate.Value.Date : DateTime.MinValue;
                DateTime lastReadingEndDate = parameters.LastReadingEndDate.HasValue ? parameters.LastReadingEndDate.Value.Date.AddDays(1).AddSeconds(-1) : DateTime.MaxValue;

                List<MetersDTO> meterList = _dbContext.Meters.Include(m => m.MeterReadings)
                                             .Where(m => isEmptyAd || m.SerialNumber.Contains(parameters.SerialNumber))
                                             .Select(m => new MetersDTO
                                             {
                                                 MeterId = m.Id,
                                                 SerialNumber = m.SerialNumber,
                                                 LastReadingTime = m.MeterReadings
                                                     .OrderByDescending(r => r.ReadingTime)
                                                     .Select(r => (DateTime?)r.ReadingTime)
                                                     .FirstOrDefault(),
                                                 LastReadingVoltage = m.MeterReadings
                                                     .OrderByDescending(r => r.ReadingTime)
                                                     .Select(r => (double?)r.Voltage)
                                                     .FirstOrDefault(),
                                                 LastIndex = m.MeterReadings
                                                     .OrderByDescending(r => r.ReadingTime)
                                                     .Select(r => (double?)r.LastIndex)
                                                     .FirstOrDefault(),
                                                 LastReadingPower = m.MeterReadings
                                                     .OrderByDescending(r => r.ReadingTime)
                                                     .Select(r => (double?)r.Power)
                                                     .FirstOrDefault()
                                             })
                                             .Where(m => m.LastReadingTime >= lastReadingStartDate && m.LastReadingTime <= lastReadingEndDate).ToList();

                return meterList;

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving meters. " + ex.Message);
            }
        }

        /// <summary>
        /// Bir sayaç okuma verisini ekler.
        /// </summary>
        /// <param name="meterReadingDto"></param>
        /// <param name="_transaction"></param>
        /// <exception cref="Exception"></exception>
        public void AddMeterReading(MeterReadingDTO meterReadingDto, IDbContextTransaction _transaction = null)
        {
            if (meterReadingDto == null)
            {
                throw new Exception("Meter is empty.");
            }

            // Mevcut bir transaction yoksa yeni bir transaction başlat
            IDbContextTransaction transaction = _transaction ?? _dbContext.Database.BeginTransaction();
            try
            {
                Meter meter = _dbContext.Meters.FirstOrDefault(m => m.Id == meterReadingDto.MeterId);
                if (meter == null)
                {
                    throw new Exception("Meter not found.");
                }
                MeterReading meterReading = new MeterReading
                {
                    Id = Guid.NewGuid(),
                    MeterId = meterReadingDto.MeterId,
                    ReadingTime = meterReadingDto.ReadingTime,
                    Voltage = meterReadingDto.Voltage,
                    LastIndex = meterReadingDto.LastIndex,
                    Power = meterReadingDto.Power
                };
                _dbContext.MeterReadings.Add(meterReading);
                _dbContext.SaveChanges();

                // transaction bu metodda başlatıldıysa transaction onayla ve sonlandır
                if (_transaction == null)
                {
                    transaction.Commit();
                    transaction.Dispose();
                }
            }
            catch (Exception ex)
            {
                // transaction bu metodda başlatıldıysa transaction'ı geri al ve sonlandır
                if (_transaction == null)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                }
                throw new Exception("An error occurred while adding meter reading. " + ex.Message);
            }

        }

        /// <summary>
        /// Sayaç seçim listesini getirir.
        /// </summary>
        /// <returns></returns>
        public List<MeterSelectionDTO> GetMeterSelections()
        {
            Meter[] meters = _dbContext.Meters.ToArray();
            return meters.Select(m => new MeterSelectionDTO
            {
                Id = m.Id,
                SerialNumber = m.SerialNumber
            }).ToList();
        }

        /// <summary>
        /// Verilen sayaç ID'sine ait tüm sayaç okuma verilerini getirir.
        /// </summary>
        /// <param name="meterId"></param>
        /// <returns></returns>
        public List<MeterReadingDTO> GetMeterReadings(Guid meterId)
        {
            var meter = _dbContext.Meters.Include(m => m.MeterReadings).FirstOrDefault(m => m.Id == meterId);

            if (meter == null)
            {
                return new List<MeterReadingDTO>();
            }

            List<MeterReadingDTO> meterReadings = meter.MeterReadings.Select(mr => new MeterReadingDTO
            {
                MeterId = mr.MeterId,
                ReadingTime = mr.ReadingTime,
                Voltage = mr.Voltage,
                LastIndex = mr.LastIndex,
                Power = mr.Power
            }).ToList();

            return meterReadings;
        }

    }
}
