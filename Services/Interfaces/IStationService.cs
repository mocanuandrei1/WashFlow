using WashFlow.Api.DTOs.Station;

namespace WashFlow.Api.Services.Interfaces;

public interface IStationService
{
    IEnumerable<StationDto> GetAll();
    StationDto? GetById(int id);
    StationDto Create(StationCreateDto dto);
    bool Update(int id, StationUpdateDto dto);
    bool Delete(int id); 
}
