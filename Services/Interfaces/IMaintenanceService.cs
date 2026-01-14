using WashFlow.Api.DTOs.Maintenance;

namespace WashFlow.Api.Services.Interfaces;

public interface IMaintenanceService
{
    IEnumerable<MaintenanceDto> GetAll(int? stationId = null);
    MaintenanceDto Open(MaintenanceOpenDto dto);
    DateTime? Close(int maintenanceId, MaintenanceCloseDto dto);
}

