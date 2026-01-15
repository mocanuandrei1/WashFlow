using WashFlow.Api.DTOs.Reports;

namespace WashFlow.Api.Services.Interfaces;

public interface IReportService
{
    RevenueReportDto GetRevenue(int? stationId, DateTime? from, DateTime? to);
    IEnumerable<TopProgramDto> GetTopPrograms(DateTime? from, DateTime? to, int top = 5);
}
