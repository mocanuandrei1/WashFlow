using Microsoft.AspNetCore.Mvc;
using WashFlow.Api.DTOs.Program;
using WashFlow.Api.Services.Interfaces;

namespace WashFlow.Api.Controllers;

[ApiController]
[Route("api/programs")]
public class ProgramsController : ControllerBase
{
    private readonly IProgramService _service;

    public ProgramsController(IProgramService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_service.GetAll());

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var p = _service.GetById(id);
        return p == null ? NotFound(new { error = "Programul nu a fost găsit." }) : Ok(p);
    }

    [HttpPost]
    public IActionResult Create([FromBody] ProgramCreateDto dto)
    {
        try
        {
            var created = _service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new
            {
                message = "Programul a fost creat cu succes.",
                program = created
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] ProgramUpdateDto dto)
    {
        try
        {
            var ok = _service.Update(id, dto);
            if (!ok) return NotFound(new { error = "Programul nu a fost găsit." });

            return Ok(new { message = "Programul a fost actualizat cu succes.", programId = id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        try
        {
            var ok = _service.Delete(id);
            if (!ok) return NotFound(new { error = "Programul nu a fost găsit." });

            return Ok(new { message = "Programul a fost șters cu succes.", programId = id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
