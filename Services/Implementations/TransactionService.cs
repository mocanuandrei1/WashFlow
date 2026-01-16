using Microsoft.EntityFrameworkCore;
using WashFlow.Api.Data;
using WashFlow.Api.DTOs.Transaction;
using WashFlow.Api.Enums;
using WashFlow.Api.Services.Interfaces;

namespace WashFlow.Api.Services.Implementations;

public class TransactionService : ITransactionService
{
    private readonly WashFlowDbContext _db;

    public TransactionService(WashFlowDbContext db)
    {
        _db = db;
    }

    public IEnumerable<TransactionDto> GetAll(int? stationId, DateTime? from, DateTime? to, PaymentMethod? paymentMethod)
    {
        var q = _db.Transactions.AsNoTracking().AsQueryable();

        if (stationId.HasValue)
            q = q.Where(t => t.StationId == stationId.Value);

        if (paymentMethod.HasValue)
            q = q.Where(t => t.PaymentMethod == paymentMethod.Value);

        if (from.HasValue)
            q = q.Where(t => t.CreatedAt >= from.Value.ToUniversalTime());

        if (to.HasValue)
            q = q.Where(t => t.CreatedAt <= to.Value.ToUniversalTime());

        return q.OrderByDescending(t => t.CreatedAt)
            .Select(t => new TransactionDto(
                t.Id,
                t.StationId,
                t.SessionId,
                t.Amount,
                t.PaymentMethod,
                t.CreatedAt
            ))
            .ToList();
    }
}
