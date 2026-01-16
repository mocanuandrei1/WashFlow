namespace WashFlow.Api.DTOs.Session;

public record SessionEndResultDto(
    int SessionId,
    int UsedMinutes,
    DateTime EndedAt
);
