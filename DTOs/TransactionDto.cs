using WashFlow.Api.Enums;

namespace WashFlow.Api.DTOs.Transaction;

public record TransactionDto(
    int Id,
    int StationId,
    int? SessionId,
    decimal Amount,
    PaymentMethod PaymentMethod,
    DateTime CreatedAt
);
