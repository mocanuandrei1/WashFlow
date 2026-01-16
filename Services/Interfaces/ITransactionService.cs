using WashFlow.Api.DTOs.Transaction;
using WashFlow.Api.Enums;

namespace WashFlow.Api.Services.Interfaces;

public interface ITransactionService
{
    IEnumerable<TransactionDto> GetAll(int? stationId, DateTime? from, DateTime? to, PaymentMethod? paymentMethod);
}
