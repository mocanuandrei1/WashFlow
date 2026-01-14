namespace WashFlow.Api.Models;

public class WashProgram
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal PricePerMinute { get; set; }
    public int MinMinutes { get; set; }
    public bool IsMaintenanceHeavy { get; set; }

    public ICollection<WashSession> Sessions { get; set; } = new List<WashSession>();
}
