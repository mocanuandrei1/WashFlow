namespace WashFlow.Api.DTOs.Reports;

public record RevenueReportDto(
    int? StationId,
    DateTime? From,
    DateTime? To,
    int TransactionsCount,
    decimal TotalRevenue
);
