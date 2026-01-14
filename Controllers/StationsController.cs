using Microsoft.AspNetCore.Mvc;
using WashFlow.Api.DTOs.Station;
using WashFlow.Api.Services.Interfaces;

namespace WashFlow.Api.Controllers;

[ApiController]
[Route("api/stations")]
public class StationsController : ControllerBase
{
    private readonly IStationService _service;

    public StationsController(IStationService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_service.GetAll());
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var station = _service.GetById(id);
        return station == null ? NotFound() : Ok(station);
    }

    [HttpPost]
    public IActionResult Create([FromBody] StationCreateDto dto)
    {
        try
        {
            var created = _service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] StationUpdateDto dto)
    {
        try
        {
            var ok = _service.Update(id, dto);

            if (!ok)
                return NotFound(new { error = "Statia nu a fost gasita." });

            return Ok(new
            {
                message = "Statia a fost actualizata cu succes.",
                stationId = id
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }


    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var ok = _service.Delete(id);

        if (!ok)
            return NotFound(new { error = "Statia nu a fost gasita." });

        return Ok(new
        {
            message = "Statia a fost dezactivata cu succes (soft delete).",
            stationId = id
        });
    }
}
