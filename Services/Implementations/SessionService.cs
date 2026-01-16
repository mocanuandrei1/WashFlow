using Microsoft.EntityFrameworkCore;
using WashFlow.Api.Data;
using WashFlow.Api.DTOs.Session;
using WashFlow.Api.Enums;
using WashFlow.Api.Models;
using WashFlow.Api.Services.Interfaces;

namespace WashFlow.Api.Services.Implementations;

public class SessionService : ISessionService
{
    private readonly WashFlowDbContext _db;

    public SessionService(WashFlowDbContext db)
    {
        _db = db;
    }

    public IEnumerable<SessionDto> GetAll()
    {
        return _db.Sessions
            .AsNoTracking()
            .OrderByDescending(s => s.StartedAt)
            .Select(s => new SessionDto(
                s.Id,
                s.StationId,
                s.ProgramId,
                s.StartedAt,
                s.EndedAt,
                s.PurchasedMinutes,
                s.UsedMinutes,
                s.Status,
                s.Transaction != null ? s.Transaction.Amount : null
            ))
            .ToList();
    }

    public IEnumerable<SessionDto> GetActive()
    {
        return _db.Sessions
            .AsNoTracking()
            .Where(s => s.Status == SessionStatus.Active)
            .OrderByDescending(s => s.StartedAt)
            .Select(s => new SessionDto(
                s.Id,
                s.StationId,
                s.ProgramId,
                s.StartedAt,
                s.EndedAt,
                s.PurchasedMinutes,
                s.UsedMinutes,
                s.Status,
                s.Transaction != null ? s.Transaction.Amount : null
            ))
            .ToList();
    }

    public SessionDto Start(SessionStartDto dto)
    {
        if (dto.StationId <= 0) throw new ArgumentException("StationId invalid.");
        if (dto.ProgramId <= 0) throw new ArgumentException("ProgramId invalid.");
        if (dto.PurchasedMinutes <= 0) throw new ArgumentException("PurchasedMinutes trebuie sa fie > 0.");

        var station = _db.Stations.FirstOrDefault(s => s.Id == dto.StationId);
        if (station == null) throw new ArgumentException("Statia nu exista.");
        if (station.Status != StationStatus.Active)
            throw new ArgumentException("Statia nu este disponibila (trebuie sa fie Active).");

        var program = _db.Programs.FirstOrDefault(p => p.Id == dto.ProgramId);
        if (program == null) throw new ArgumentException("Programul nu exista.");

        if (dto.PurchasedMinutes < program.MinMinutes)
            throw new ArgumentException($"PurchasedMinutes trebuie sa fie >= {program.MinMinutes} pentru acest program.");

        var hasActive = _db.Sessions.Any(s => s.StationId == dto.StationId && s.Status == SessionStatus.Active);
        if (hasActive)
            throw new ArgumentException("Exista deja o sesiune activa pe aceasta statie.");

        var now = DateTime.UtcNow;
        var amount = program.PricePerMinute * dto.PurchasedMinutes;

        var session = new WashSession
        {
            StationId = dto.StationId,
            ProgramId = dto.ProgramId,
            StartedAt = now,
            PurchasedMinutes = dto.PurchasedMinutes,
            UsedMinutes = 0,
            Status = SessionStatus.Active
        };

        _db.Sessions.Add(session);
        _db.SaveChanges(); // ca să obținem SessionId

        var tx = new Transaction
        {
            StationId = dto.StationId,
            SessionId = session.Id,
            Amount = amount,
            PaymentMethod = dto.PaymentMethod,
            CreatedAt = now
        };

        _db.Transactions.Add(tx);
        _db.SaveChanges();

        return new SessionDto(
            session.Id,
            session.StationId,
            session.ProgramId,
            session.StartedAt,
            session.EndedAt,
            session.PurchasedMinutes,
            session.UsedMinutes,
            session.Status,
            amount
        );
    }

    public SessionEndResultDto? End(int sessionId)
    {
        var session = _db.Sessions
            .Include(s => s.Program)
            .FirstOrDefault(s => s.Id == sessionId);

        if (session == null) return null;
        if (session.Status != SessionStatus.Active) return null;

        var now = DateTime.UtcNow;

        var elapsedMinutes = (int)Math.Ceiling((now - session.StartedAt).TotalMinutes);
        if (elapsedMinutes < 0) elapsedMinutes = 0;
        var used = Math.Min(elapsedMinutes, session.PurchasedMinutes);

        session.UsedMinutes = used;
        session.EndedAt = now;
        session.Status = SessionStatus.Finished;

        _db.SaveChanges();

        return new SessionEndResultDto(session.Id, session.UsedMinutes, session.EndedAt.Value);
    }

    public bool Cancel(int sessionId)
    {
        var session = _db.Sessions.FirstOrDefault(s => s.Id == sessionId);
        if (session == null) return false;
        if (session.Status != SessionStatus.Active) return false;

        session.Status = SessionStatus.Cancelled;
        session.EndedAt = DateTime.UtcNow;

        _db.SaveChanges();
        return true;
    }
}
