using Microsoft.EntityFrameworkCore;
using WashFlow.Api.Data;
using WashFlow.Api.DTOs.Reports;
using WashFlow.Api.Services.Interfaces;

namespace WashFlow.Api.Services.Implementations;

public class ReportService : IReportService
{
    private readonly WashFlowDbContext _db;

    public ReportService(WashFlowDbContext db)
    {
        _db = db;
    }

    public RevenueReportDto GetRevenue(int? stationId, DateTime? from, DateTime? to)
    {
        if (stationId.HasValue)
        {
            var stationExists = _db.Stations.AsNoTracking().Any(s => s.Id == stationId.Value);
            if (!stationExists)
                throw new ArgumentException("Statia specificata nu exista.");
        }

        var q = _db.Transactions.AsNoTracking().AsQueryable();

        if (stationId.HasValue)
            q = q.Where(t => t.StationId == stationId.Value);

        if (from.HasValue)
            q = q.Where(t => t.CreatedAt >= from.Value.ToUniversalTime());

        if (to.HasValue)
            q = q.Where(t => t.CreatedAt <= to.Value.ToUniversalTime());

        var count = q.Count();
        var total = q.Sum(t => (decimal?)t.Amount) ?? 0m;

        return new RevenueReportDto(stationId, from, to, count, total);
    }

    public IEnumerable<TopProgramDto> GetTopPrograms(DateTime? from, DateTime? to, int top = 5)
    {
        if (top < 1) top = 1;
        if (top > 20) top = 20;

        var sessions = _db.Sessions.AsNoTracking().AsQueryable();

        if (from.HasValue)
            sessions = sessions.Where(s => s.StartedAt >= from.Value.ToUniversalTime());

        if (to.HasValue)
            sessions = sessions.Where(s => s.StartedAt <= to.Value.ToUniversalTime());

        // Join cu Programs pentru nume
        var result = sessions
            .GroupBy(s => s.ProgramId)
            .Select(g => new
            {
                ProgramId = g.Key,
                SessionsCount = g.Count()
            })
            .OrderByDescending(x => x.SessionsCount)
            .Take(top)
            .Join(
                _db.Programs.AsNoTracking(),
                x => x.ProgramId,
                p => p.Id,
                (x, p) => new TopProgramDto(p.Id, p.Name, x.SessionsCount)
            )
            .ToList();

        return result;
    }
}
