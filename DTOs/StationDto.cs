using WashFlow.Api.Enums;

namespace WashFlow.Api.DTOs.Station;

public record StationDto(int Id, string Name, string? Location, StationStatus Status);
