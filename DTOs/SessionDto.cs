using WashFlow.Api.Enums;

namespace WashFlow.Api.DTOs.Session;

public record SessionDto(
    int Id,
    int StationId,
    int ProgramId,
    DateTime StartedAt,
    DateTime? EndedAt,
    int PurchasedMinutes,
    int UsedMinutes,
    SessionStatus Status,
    decimal? Amount
);
