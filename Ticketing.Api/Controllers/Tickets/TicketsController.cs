using Ticketing.Application.Tickets.CreateTicket;
using Ticketing.Application.Tickets.GetTicket;
using Ticketing.Application.Tickets.GetTickets;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ticketing.Api.Controllers.Tickets;

[Authorize]
[ApiController]
[Route("api/tickets")]
public class TicketsController : Controller
{
    private readonly ISender _sender;

    public TicketsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicket(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetTicketQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpGet("student/{id}")]
    public async Task<IActionResult> GetTickets(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetTicketsQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateTicket(CreateTicketRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateTicketCommand(request.StudentId, request.Title, request.Description);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetTicket), new { id = result.Value }, result.Value);
    }
}