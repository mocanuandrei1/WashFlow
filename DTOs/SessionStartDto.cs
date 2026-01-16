using WashFlow.Api.Enums;

namespace WashFlow.Api.DTOs.Session;

public record SessionStartDto(
    int StationId,
    int ProgramId,
    int PurchasedMinutes,
    PaymentMethod PaymentMethod
);
