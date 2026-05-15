using Ticketing.Domain.Abstractions;

namespace Ticketing.Domain.Tickets.Events;

public sealed record TicketCreatedDomainEvent(Guid TicketId) : IDomainEvent;