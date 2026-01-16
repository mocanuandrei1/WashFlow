using Microsoft.AspNetCore.Mvc;
using WashFlow.Api.Enums;
using WashFlow.Api.Services.Interfaces;

namespace WashFlow.Api.Controllers;

[ApiController]
[Route("api/transactions")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _service;

    public TransactionsController(ITransactionService service)
    {
        _service = service;
    }

    // GET /api/transactions?stationId=1&from=2026-01-01&to=2026-01-31&paymentMethod=Card
    [HttpGet]
    public IActionResult GetAll([FromQuery] int? stationId, [FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] PaymentMethod? paymentMethod)
    {
        var items = _service.GetAll(stationId, from, to, paymentMethod);
        return Ok(new
        {
            message = "Lista tranzactiilor a fost generata cu succes.",
            count = items.Count(),
            items
        });
    }
}
