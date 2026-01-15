namespace WashFlow.Api.DTOs.Program;

public record ProgramUpdateDto(string Name, decimal PricePerMinute, int MinMinutes, bool IsMaintenanceHeavy);
