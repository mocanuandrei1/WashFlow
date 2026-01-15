namespace WashFlow.Api.DTOs.Program;

public record ProgramDto(int Id, string Name, decimal PricePerMinute, int MinMinutes, bool IsMaintenanceHeavy);
