using Microsoft.EntityFrameworkCore;
using WashFlow.Api.Data;
using WashFlow.Api.DTOs.Station;
using WashFlow.Api.Enums;
using WashFlow.Api.Models;
using WashFlow.Api.Services.Interfaces;

namespace WashFlow.Api.Services.Implementations;

public class StationService : IStationService
{
    private readonly WashFlowDbContext _db;

    public StationService(WashFlowDbContext db)
    {
        _db = db;
    }

    public IEnumerable<StationDto> GetAll()
    {
        return _db.Stations
            .AsNoTracking()
            .Where(s => s.Status != StationStatus.Inactive)
            .OrderBy(s => s.Id)
            .Select(s => new StationDto(s.Id, s.Name, s.Location, s.Status))
            .ToList();
    }

    public StationDto? GetById(int id)
    {
        var s = _db.Stations.AsNoTracking().FirstOrDefault(x => x.Id == id);
        if (s == null || s.Status == StationStatus.Inactive) return null;

        return new StationDto(s.Id, s.Name, s.Location, s.Status);
    }

    public StationDto Create(StationCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Numele stației este obligatoriu.");

        var entity = new WashStation
        {
            Name = dto.Name.Trim(),
            Location = string.IsNullOrWhiteSpace(dto.Location) ? null : dto.Location.Trim(),
            Status = StationStatus.Active
        };

        _db.Stations.Add(entity);
        _db.SaveChanges();

        return new StationDto(entity.Id, entity.Name, entity.Location, entity.Status);
    }

    public bool Update(int id, StationUpdateDto dto)
    {
        var entity = _db.Stations.FirstOrDefault(x => x.Id == id);
        if (entity == null || entity.Status == StationStatus.Inactive) return false;

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Numele statiei este obligatoriu.");

        entity.Name = dto.Name.Trim();
        entity.Location = string.IsNullOrWhiteSpace(dto.Location) ? null : dto.Location.Trim();
        entity.Status = dto.Status;

        _db.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var entity = _db.Stations.FirstOrDefault(x => x.Id == id);
        if (entity == null || entity.Status == StationStatus.Inactive) return false;

        entity.Status = StationStatus.Inactive;
        _db.SaveChanges();
        return true;
    }
}
