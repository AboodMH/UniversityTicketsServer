using Ticketing.Application.Abstractions.Messaging;

namespace Ticketing.Application.Tickets.GetTickets;

public sealed record GetTicketsQuery(Guid StudentId) : IQuery<IReadOnlyList<TicketResponse>>;