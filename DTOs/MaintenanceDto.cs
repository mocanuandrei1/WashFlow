namespace WashFlow.Api.DTOs.Maintenance;

public record MaintenanceDto(
    int Id,
    int StationId,
    string Title,
    string Description,
    DateTime OpenedAt,
    DateTime? ClosedAt,
    bool IsOpen
);
