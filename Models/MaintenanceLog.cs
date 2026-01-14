namespace WashFlow.Api.Models;

public class MaintenanceLog
{
    public int Id { get; set; }

    public int StationId { get; set; }
    public WashStation Station { get; set; } = null!;

    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;

    public DateTime OpenedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ClosedAt { get; set; }

    public bool IsOpen => ClosedAt == null;
}
