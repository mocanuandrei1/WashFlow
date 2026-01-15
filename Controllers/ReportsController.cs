using Microsoft.AspNetCore.Mvc;
using WashFlow.Api.Services.Interfaces;

namespace WashFlow.Api.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _service;

    public ReportsController(IReportService service)
    {
        _service = service;
    }

    [HttpGet("revenue")]
    public IActionResult Revenue([FromQuery] int? stationId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        try
        {
            var report = _service.GetRevenue(stationId, from, to);
            return Ok(new
            {
                message = "Raportul de venituri a fost generat cu succes.",
                report
            });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }


    [HttpGet("top-programs")]
    public IActionResult TopPrograms([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] int top = 5)
    {
        var result = _service.GetTopPrograms(from, to, top);
        return Ok(new
        {
            message = "Top programe a fost generat cu succes.",
            top,
            from,
            to,
            items = result
        });
    }
}
