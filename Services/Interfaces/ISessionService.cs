using WashFlow.Api.DTOs.Session;

namespace WashFlow.Api.Services.Interfaces;

public interface ISessionService
{
    IEnumerable<SessionDto> GetAll();
    IEnumerable<SessionDto> GetActive();
    SessionDto Start(SessionStartDto dto);
    SessionEndResultDto? End(int sessionId);
    bool Cancel(int sessionId);
}
