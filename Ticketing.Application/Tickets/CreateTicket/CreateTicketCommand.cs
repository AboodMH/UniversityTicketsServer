using Ticketing.Application.Abstractions.Messaging;

namespace Ticketing.Application.Tickets.CreateTicket;

public sealed record CreateTicketCommand(Guid StudentId, string Title, string Description) : ICommand<Guid>;