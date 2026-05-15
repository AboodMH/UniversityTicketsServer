namespace Ticketing.Api.Controllers.Tickets;

public sealed record CreateTicketRequest(Guid StudentId, string Title, string Description);