using Ticketing.Application.Abstractions.Messaging;

namespace Ticketing.Application.Tickets.GetTicketReplies;

public sealed record GetTicketRepliesQuery : IQuery<IReadOnlyList<TicketResponse>>;