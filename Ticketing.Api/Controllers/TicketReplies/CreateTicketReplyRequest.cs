namespace Ticketing.Api.Controllers.TicketReplies;

public sealed record CreateTicketReplyRequest(Guid TicketId, Guid InstructorId, string Message);