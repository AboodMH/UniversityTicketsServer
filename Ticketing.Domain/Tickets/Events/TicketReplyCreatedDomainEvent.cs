using Ticketing.Domain.Abstractions;

namespace Ticketing.Domain.Tickets.Events;

public sealed record TicketReplyCreatedDomainEvent(Guid TicketReplyId) : IDomainEvent;