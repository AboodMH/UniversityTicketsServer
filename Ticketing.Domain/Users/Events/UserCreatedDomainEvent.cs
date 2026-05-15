using Ticketing.Domain.Abstractions;

namespace Ticketing.Domain.Users.Events;

public sealed record UserCreatedDomainEvent(Guid UserId) : IDomainEvent;