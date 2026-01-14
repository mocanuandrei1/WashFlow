using WashFlow.Api.Enums;

namespace WashFlow.Api.Models;

public class WashStation
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Location { get; set; }
    public StationStatus Status { get; set; } = StationStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<WashSession> Sessions { get; set; } = new List<WashSession>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<MaintenanceLog> MaintenanceLogs { get; set; } = new List<MaintenanceLog>();
}
