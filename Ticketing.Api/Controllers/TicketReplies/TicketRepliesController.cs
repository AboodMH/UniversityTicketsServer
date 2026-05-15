using Ticketing.Application.TicketReplies.TicketReply;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.Tickets.GetTicketReplies;

namespace Ticketing.Api.Controllers.TicketReplies;

[Authorize]
[ApiController]
[Route("api/ticket/replies")]
public class TicketRepliesController : Controller
{
    private readonly ISender _sender;

    public TicketRepliesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetTickets(CancellationToken cancellationToken)
    {
        var query = new GetTicketRepliesQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateTicketReply(CreateTicketReplyRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateTicketReplyCommand(request.TicketId, request.InstructorId, request.Message);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(request.TicketId);
    }
}