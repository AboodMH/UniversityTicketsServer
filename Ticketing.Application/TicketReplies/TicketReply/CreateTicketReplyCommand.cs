using Ticketing.Application.Abstractions.Messaging;

namespace Ticketing.Application.TicketReplies.TicketReply;

public sealed record CreateTicketReplyCommand(Guid TicketId, Guid InstructorId, string Message) : ICommand<Guid>;