using Ticketing.Domain.Abstractions;

namespace Ticketing.Domain.Tickets;

public static class TicketError
{
    public static Error NotFound = new(
        "Ticket.Found",
        "The ticket with the specified identifier was not found");

    public static Error TicketClosed = new(
        "Ticket.Closed",
        "The ticket is closed and cannot be replied to");
}