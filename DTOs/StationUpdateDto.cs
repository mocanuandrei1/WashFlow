using WashFlow.Api.Enums;

namespace WashFlow.Api.DTOs.Station;

public record StationUpdateDto(string Name, string? Location, StationStatus Status);
