using WashFlow.Api.Enums;

namespace WashFlow.Api.Models;

public class Transaction
{
    public int Id { get; set; }

    public int StationId { get; set; }
    public WashStation Station { get; set; } = null!;

    public int? SessionId { get; set; }
    public WashSession? Session { get; set; }

    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
