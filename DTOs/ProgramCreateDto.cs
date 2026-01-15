namespace WashFlow.Api.DTOs.Program;

public record ProgramCreateDto(string Name, decimal PricePerMinute, int MinMinutes, bool IsMaintenanceHeavy);
