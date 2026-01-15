using WashFlow.Api.DTOs.Program;

namespace WashFlow.Api.Services.Interfaces;

public interface IProgramService
{
    IEnumerable<ProgramDto> GetAll();
    ProgramDto? GetById(int id);
    ProgramDto Create(ProgramCreateDto dto);
    bool Update(int id, ProgramUpdateDto dto);
    bool Delete(int id);
}
