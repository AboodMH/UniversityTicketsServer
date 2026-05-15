namespace Ticketing.Application.Tickets;

public sealed class ReplyResponse
{
    public Guid Id { get; init; }

    public Guid InstructorId { get; init; }

    public string Message { get; init; }

    public DateTime CreatedAt { get; init; }
}