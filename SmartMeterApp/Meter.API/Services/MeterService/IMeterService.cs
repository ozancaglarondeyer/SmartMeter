using MeterApi.DTOs;
using Microsoft.EntityFrameworkCore.Storage;

namespace MeterApi.Services.MeterService
{
    public interface IMeterService
    {
        List<MetersDTO> GetMeters(MeterQueryParameters parameters);
        void AddMeterReading(MeterReadingDTO meterReadingDto, IDbContextTransaction _transaction = null);
        List<MeterSelectionDTO> GetMeterSelections();
        List<MeterReadingDTO> GetMeterReadings(Guid meterId);
    }
}
