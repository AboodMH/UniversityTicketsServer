using Ticketing.Application.Abstractions.Messaging;

namespace Ticketing.Application.Tickets.GetTicket;

public sealed record GetTicketQuery(Guid TicketId) : IQuery<TicketResponse>;