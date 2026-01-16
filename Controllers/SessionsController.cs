using Microsoft.AspNetCore.Mvc;
using WashFlow.Api.DTOs.Session;
using WashFlow.Api.Services.Interfaces;

namespace WashFlow.Api.Controllers;

[ApiController]
[Route("api/sessions")]
public class SessionsController : ControllerBase
{
    private readonly ISessionService _service;

    public SessionsController(ISessionService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_service.GetAll());

    [HttpGet("active")]
    public IActionResult GetActive() => Ok(_service.GetActive());

    [HttpPost("start")]
    public IActionResult Start([FromBody] SessionStartDto dto)
    {
        try
        {
            var created = _service.Start(dto);
            return Ok(new
            {
                message = "Sesiunea a fost pornita cu succes. Tranzactia a fost înregistrata.",
                session = created
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("{id:int}/end")]
    public IActionResult End(int id)
    {
        var result = _service.End(id);
        if (result == null)
            return NotFound(new { error = "Sesiunea nu a fost gasita sau nu este activa." });

        return Ok(new
        {
            message = "Sesiunea a fost închisa.",
            result
        });
    }

    [HttpPost("{id:int}/cancel")]
    public IActionResult Cancel(int id)
    {
        var ok = _service.Cancel(id);
        if (!ok)
            return NotFound(new { error = "Sesiunea nu a fost gasita sau nu este activa." });

        return Ok(new
        {
            message = "Sesiunea a fost anulata cu succes.",
            sessionId = id
        });
    }
}
