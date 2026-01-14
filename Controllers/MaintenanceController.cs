using Microsoft.AspNetCore.Mvc;
using WashFlow.Api.DTOs.Maintenance;
using WashFlow.Api.Services.Interfaces;

namespace WashFlow.Api.Controllers;

[ApiController]
[Route("api/maintenance")]
public class MaintenanceController : ControllerBase
{
    private readonly IMaintenanceService _service;

    public MaintenanceController(IMaintenanceService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] int? stationId = null)
    {
        return Ok(_service.GetAll(stationId));
    }

    [HttpPost("open")]
    public IActionResult Open([FromBody] MaintenanceOpenDto dto)
    {
        try
        {
            var created = _service.Open(dto);

            return Created($"/api/maintenance/{created.Id}", new
            {
                message = "Mentenanța a fost deschisă cu succes. Stația a fost trecută în modul Maintenance.",
                maintenance = created
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }


    [HttpPost("{id:int}/close")]
    public IActionResult Close(int id, [FromBody] MaintenanceCloseDto dto)
    {
        var closedAt = _service.Close(id, dto);

        if (closedAt == null)
            return NotFound(new { error = "Mentenanța nu a fost găsită sau este deja închisă." });

        return Ok(new
        {
            message = "Mentenanța a fost închisă cu succes.",
            closedAt
        });
    }

}
