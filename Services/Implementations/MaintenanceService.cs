using Microsoft.EntityFrameworkCore;
using WashFlow.Api.Data;
using WashFlow.Api.DTOs.Maintenance;
using WashFlow.Api.Enums;
using WashFlow.Api.Models;
using WashFlow.Api.Services.Interfaces;

namespace WashFlow.Api.Services.Implementations;

public class MaintenanceService : IMaintenanceService
{
    private readonly WashFlowDbContext _db;

    public MaintenanceService(WashFlowDbContext db)
    {
        _db = db;
    }

    public IEnumerable<MaintenanceDto> GetAll(int? stationId = null)
    {
        var q = _db.MaintenanceLogs.AsNoTracking().AsQueryable();

        if (stationId.HasValue)
            q = q.Where(m => m.StationId == stationId.Value);

        return q.OrderByDescending(m => m.OpenedAt)
            .Select(m => new MaintenanceDto(
                m.Id,
                m.StationId,
                m.Title,
                m.Description,
                m.OpenedAt,
                m.ClosedAt,
                m.IsOpen
            ))
            .ToList();
    }

    public MaintenanceDto Open(MaintenanceOpenDto dto)
    {
        if (dto.StationId <= 0)
            throw new ArgumentException("StationId invalid.");

        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new ArgumentException("Titlul mentenanței este obligatoriu.");

        if (string.IsNullOrWhiteSpace(dto.Description))
            throw new ArgumentException("Descrierea mentenanței este obligatorie.");

        var station = _db.Stations.FirstOrDefault(s => s.Id == dto.StationId);
        if (station == null || station.Status == StationStatus.Inactive)
            throw new ArgumentException("Stația nu există.");

        var alreadyOpen = _db.MaintenanceLogs.Any(m => m.StationId == dto.StationId && m.ClosedAt == null);
        if (alreadyOpen)
            throw new ArgumentException("Există deja o mentenanță deschisă pentru această stație.");

        var log = new MaintenanceLog
        {
            StationId = dto.StationId,
            Title = dto.Title.Trim(),
            Description = dto.Description.Trim(),
            OpenedAt = DateTime.UtcNow,
            ClosedAt = null
        };

        _db.MaintenanceLogs.Add(log);

        station.Status = StationStatus.Maintenance;

        _db.SaveChanges();

        return new MaintenanceDto(
            log.Id,
            log.StationId,
            log.Title,
            log.Description,
            log.OpenedAt,
            log.ClosedAt,
            log.IsOpen
        );
    }

    public DateTime? Close(int maintenanceId, MaintenanceCloseDto dto)
    {
    var log = _db.MaintenanceLogs.FirstOrDefault(m => m.Id == maintenanceId);
    if (log == null) return null;
    if (log.ClosedAt != null) return null;

    var station = _db.Stations.FirstOrDefault(s => s.Id == log.StationId);
    if (station == null) return null;

    log.ClosedAt = DateTime.UtcNow;

    if (station.Status != StationStatus.Inactive)
        station.Status = StationStatus.Active;

    _db.SaveChanges();

    return log.ClosedAt;
    }

}
