using WashFlow.Api.Enums;

namespace WashFlow.Api.Models;

public class WashSession
{
    public int Id { get; set; }

    public int StationId { get; set; }
    public WashStation Station { get; set; } = null!;

    public int ProgramId { get; set; }
    public WashProgram Program { get; set; } = null!;

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EndedAt { get; set; }

    public int PurchasedMinutes { get; set; }
    public int UsedMinutes { get; set; }

    public SessionStatus Status { get; set; } = SessionStatus.Active;

    public Transaction? Transaction { get; set; }
}
