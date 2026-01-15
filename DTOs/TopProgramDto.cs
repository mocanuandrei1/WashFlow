namespace WashFlow.Api.DTOs.Reports;

public record TopProgramDto(
    int ProgramId,
    string ProgramName,
    int SessionsCount
);
