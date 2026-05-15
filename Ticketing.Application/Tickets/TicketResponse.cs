using Ticketing.Domain.Tickets;

namespace Ticketing.Application.Tickets;

public sealed class TicketResponse
{
    public Guid Id { get; init; }

    public Guid StudentId { get; init; }

    public string Title { get; init; }

    public string Description { get; init; }

    public Status Status { get; init; }

    public DateTime CreatedAt { get; init; }

    public List<ReplyResponse> Replies { get; set; } = new();
}

