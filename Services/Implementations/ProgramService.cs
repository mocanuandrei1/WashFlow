using Microsoft.EntityFrameworkCore;
using WashFlow.Api.Data;
using WashFlow.Api.DTOs.Program;
using WashFlow.Api.Models;
using WashFlow.Api.Services.Interfaces;

namespace WashFlow.Api.Services.Implementations;

public class ProgramService : IProgramService
{
    private readonly WashFlowDbContext _db;

    public ProgramService(WashFlowDbContext db)
    {
        _db = db;
    }

    public IEnumerable<ProgramDto> GetAll()
    {
        return _db.Programs
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .Select(p => new ProgramDto(p.Id, p.Name, p.PricePerMinute, p.MinMinutes, p.IsMaintenanceHeavy))
            .ToList();
    }

    public ProgramDto? GetById(int id)
    {
        var p = _db.Programs.AsNoTracking().FirstOrDefault(x => x.Id == id);
        return p == null ? null : new ProgramDto(p.Id, p.Name, p.PricePerMinute, p.MinMinutes, p.IsMaintenanceHeavy);
    }

    public ProgramDto Create(ProgramCreateDto dto)
    {
        Validate(dto.Name, dto.PricePerMinute, dto.MinMinutes);

        var entity = new WashProgram
        {
            Name = dto.Name.Trim(),
            PricePerMinute = dto.PricePerMinute,
            MinMinutes = dto.MinMinutes,
            IsMaintenanceHeavy = dto.IsMaintenanceHeavy
        };

        _db.Programs.Add(entity);
        _db.SaveChanges();

        return new ProgramDto(entity.Id, entity.Name, entity.PricePerMinute, entity.MinMinutes, entity.IsMaintenanceHeavy);
    }

    public bool Update(int id, ProgramUpdateDto dto)
    {
        var entity = _db.Programs.FirstOrDefault(x => x.Id == id);
        if (entity == null) return false;

        Validate(dto.Name, dto.PricePerMinute, dto.MinMinutes);

        entity.Name = dto.Name.Trim();
        entity.PricePerMinute = dto.PricePerMinute;
        entity.MinMinutes = dto.MinMinutes;
        entity.IsMaintenanceHeavy = dto.IsMaintenanceHeavy;

        _db.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var entity = _db.Programs.FirstOrDefault(x => x.Id == id);
        if (entity == null) return false;

        var hasSessions = _db.Sessions.Any(s => s.ProgramId == id);
        if (hasSessions)
            throw new ArgumentException("Nu poti sterge un program care are sesiuni asociate.");

        _db.Programs.Remove(entity);
        _db.SaveChanges();
        return true;
    }

    private static void Validate(string name, decimal pricePerMinute, int minMinutes)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Numele programului este obligatoriu.");

        if (pricePerMinute <= 0)
            throw new ArgumentException("Pretul pe minut trebuie sa fie > 0.");

        if (minMinutes < 1)
            throw new ArgumentException("MinMinutes trebuie sa fie >= 1.");
    }
}
