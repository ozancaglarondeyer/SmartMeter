using ClosedXML.Excel;
using ReportApi.DTOs;

namespace ReportApi.Helpers
{
    public static class ExcelService
    {
        public static async Task<string> GenerateExcelReport(string meterSerialNumber, List<MeterReadingInformationDTO> readings)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", $"Report_{meterSerialNumber}_{DateTime.Now:yyyyMMddHHmmss}.xlsx");

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Meter Readings");

                // Başlıklar
                worksheet.Cell(1, 1).Value = "Meter Serial Number";
                worksheet.Cell(1, 2).Value = "Reading Time";
                worksheet.Cell(1, 3).Value = "Last Index";
                worksheet.Cell(1, 4).Value = "Voltage";
                worksheet.Cell(1, 5).Value = "Power";

                // Veriler
                for (int i = 0; i < readings.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = meterSerialNumber;
                    worksheet.Cell(i + 2, 2).Value = readings[i].ReadingTime;
                    worksheet.Cell(i + 2, 3).Value = readings[i].LastIndex;
                    worksheet.Cell(i + 2, 4).Value = readings[i].Voltage;
                    worksheet.Cell(i + 2, 5).Value = readings[i].Power;
                }

                // Dosya kaydetme
                workbook.SaveAs(filePath);
            }

            return await Task.FromResult(filePath);
        }


    }
}
